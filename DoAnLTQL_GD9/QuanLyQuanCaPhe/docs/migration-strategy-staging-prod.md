## Migration Strategy: Staging -> Production

This document defines a safe EF Core migration flow for GD7.

### 1) Preconditions

- Backup database before every release migration.
- Keep app version and migration version in the same release ticket.
- Run migration only from release artifact source (never from a dev dirty workspace).
- Ensure no pending model drift:
	- `dotnet ef migrations list`
	- `dotnet ef migrations has-pending-model-changes`

### 2) Data Precheck Queries

Run in staging and production before applying migration:

```sql
-- Ban.TrangThai must be in (0,1,2)
SELECT ID, TrangThai FROM Ban WHERE TrangThai NOT IN (0,1,2);

-- HoaDon.TrangThai must be in (0,1,2)
SELECT ID, TrangThai FROM HoaDon WHERE TrangThai NOT IN (0,1,2);

-- No duplicate open invoices per table
SELECT BanID, COUNT(*) AS SoHoaDonMo
FROM HoaDon
WHERE TrangThai = 0
GROUP BY BanID
HAVING COUNT(*) > 1;
```

If any query returns rows, stop migration and clean data first.

### 3) Apply Migration (Staging)

```powershell
dotnet ef database update
```

Post-check:

```sql
SELECT i.name, i.is_unique, i.filter_definition
FROM sys.indexes i
INNER JOIN sys.tables t ON t.object_id = i.object_id
WHERE t.name = 'HoaDon'
	AND i.name = 'UX_HoaDon_Ban_Mo';
```

### 4) Smoke Test After Migration

- Create invoice on an empty table -> success.
- Create second open invoice on same table -> blocked.
- Merge tables and verify source invoice becomes canceled state.
- Run invoice filter by date and status to confirm index-backed flow.

### 5) Production Rollout

1. Enable maintenance mode (or block write-heavy actions).
2. Run DB backup.
3. Run precheck queries.
4. Apply migration.
5. Run smoke tests.
6. Disable maintenance mode.

### 6) Rollback Plan

- If migration failed before commit: fix cause and re-run.
- If migration applied but app regression occurs:
	- Deploy previous app version only if schema-compatible.
	- Otherwise run `dotnet ef database update <previous_migration>` after impact analysis.
- Always keep backup restore path available for severe incidents.

### 7) CI/CD Gate Recommendation

- Add pipeline step to run:
	- `dotnet build`
	- `dotnet ef migrations has-pending-model-changes`
- Fail pipeline if model changes exist but no migration file is included.

### 8) Runtime Connection Configuration (P3)

Application now resolves DB connection string in this priority order:

1. Environment variable (env-specific first, then generic):
	- `CAPHE_CONNECTION_STRING__<ENV>`
	- `CAPHE_CONNECTION_STRING_<ENV>`
	- `CAPHE_CONNECTION_STRING`
2. Secret file path variable (env-specific first, then generic):
	- `CAPHE_CONNECTION_STRING_FILE__<ENV>`
	- `CAPHE_CONNECTION_STRING_FILE_<ENV>`
	- `CAPHE_CONNECTION_STRING_FILE`
3. App.config connection string by environment:
	- `CaPheConnection.<ENV>`
	- `CaPheConnection`

Environment name resolution order:

- `CAPHE_ENVIRONMENT`
- `DOTNET_ENVIRONMENT`
- `ASPNETCORE_ENVIRONMENT`
- `App.config` key `CaPheEnvironment`

Suggested secure deployment (staging/prod):

- Store full connection string in secret manager or CI/CD secure variable.
- Inject as environment variable at runtime (`CAPHE_CONNECTION_STRING`).
- Avoid committing production credentials into `App.config`.

PowerShell example:

```powershell
$env:CAPHE_ENVIRONMENT = "Staging"
$env:CAPHE_CONNECTION_STRING__STAGING = "Server=sql-stg;Database=QuanLyQuanCaPhe;User Id=...;Password=...;TrustServerCertificate=True"
```

