using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace QuanLyQuanCaPhe.Services.UI;

internal static class DataGridViewSelectionHelper
{
    public static void RebindData<T>(DataGridView dataGridView, IReadOnlyList<T> data)
    {
        dataGridView.DataSource = null;
        dataGridView.DataSource = data;
        ClearSelection(dataGridView);
    }

    public static void ClearSelection(DataGridView dataGridView)
    {
        dataGridView.ClearSelection();
        dataGridView.CurrentCell = null;
    }

    public static bool TrySelectNearestRow(DataGridView dataGridView, int requestedIndex)
    {
        if (dataGridView.Rows.Count == 0)
        {
            ClearSelection(dataGridView);
            return false;
        }

        var safeIndex = Math.Max(0, Math.Min(requestedIndex, dataGridView.Rows.Count - 1));
        var row = dataGridView.Rows[safeIndex];
        if (row.Cells.Count == 0)
        {
            ClearSelection(dataGridView);
            return false;
        }

        dataGridView.ClearSelection();
        row.Selected = true;
        dataGridView.CurrentCell = row.Cells[0];
        return true;
    }

    public static bool TryGetSelectedItem<T>(DataGridView dataGridView, out T? item, out int rowIndex)
        where T : class
    {
        foreach (DataGridViewRow row in dataGridView.SelectedRows)
        {
            if (row.DataBoundItem is not T typedItem)
            {
                continue;
            }

            item = typedItem;
            rowIndex = row.Index;
            return true;
        }

        foreach (DataGridViewRow row in dataGridView.Rows)
        {
            if (!row.Selected || row.DataBoundItem is not T typedItem)
            {
                continue;
            }

            item = typedItem;
            rowIndex = row.Index;
            return true;
        }

        item = null;
        rowIndex = -1;
        return false;
    }
}