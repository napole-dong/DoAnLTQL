using System.Globalization;
using System.Text;
using QuanLyQuanCaPhe.BUS;
using QuanLyQuanCaPhe.DTO;

namespace QuanLyQuanCaPhe.Forms
{
    public partial class frmBanHang : Form
    {
        private readonly BanBUS _banBUS = new();
        private readonly MonBUS _monBUS = new();
        private readonly BanHangBUS _banHangBUS = new();
        private readonly Dictionary<int, List<BanHangOrderItemDTO>> _gioTamTheoBan = new();
        private int? _banDangChonId;
        private string? _boLocLoaiMon;

        public frmBanHang()
        {
            InitializeComponent();

            dgvOrder.AutoGenerateColumns = false;
            colTenMon.DataPropertyName = nameof(BanHangOrderItemDTO.TenMon);
            colSoLuong.DataPropertyName = nameof(BanHangOrderItemDTO.SoLuong);
            colDonGia.DataPropertyName = nameof(BanHangOrderItemDTO.DonGiaHienThi);
            colThanhTien.DataPropertyName = nameof(BanHangOrderItemDTO.ThanhTienHienThi);

            Load += frmBanHang_Load;
            txtSearch.TextChanged += txtSearch_TextChanged;

            btnTatCa.Click += (_, _) => ChonBoLocLoaiMon(null, btnTatCa);
            btnCafe.Click += (_, _) => ChonBoLocLoaiMon("cafe", btnCafe);
            btnDaXay.Click += (_, _) => ChonBoLocLoaiMon("da xay", btnDaXay);
            btnTra.Click += (_, _) => ChonBoLocLoaiMon("tra", btnTra);

            btnGoiMon.Click += btnGoiMon_Click;
            btnTamTinh.Click += btnTamTinh_Click;
            btnThanhToan.Click += btnThanhToan_Click;
            btnChuyenBan.Click += (_, _) => ThucHienChuyenHoacGopBan(true);
            btnGopBan.Click += (_, _) => ThucHienChuyenHoacGopBan(false);
        }

        private void frmBanHang_Load(object? sender, EventArgs e)
        {
            ChonBoLocLoaiMon(null, btnTatCa);
            TaiSoDoBan();
            TaiDanhSachMon();
            CapNhatPhieuDangChon();
        }

        private void txtSearch_TextChanged(object? sender, EventArgs e)
        {
            TaiSoDoBan();
            TaiDanhSachMon();
            CapNhatPhieuDangChon();
        }

        private void TaiSoDoBan()
        {
            var tuKhoa = txtSearch.Text.Trim();
            var dsBan = _banBUS.LayDanhSachBan(null, null, tuKhoa);
            if (!string.IsNullOrWhiteSpace(tuKhoa) && dsBan.Count == 0)
            {
                dsBan = _banBUS.LayDanhSachBan(null, null, null);
            }

            if (_banDangChonId.HasValue && dsBan.All(x => x.ID != _banDangChonId.Value))
            {
                _banDangChonId = dsBan.FirstOrDefault()?.ID;
            }

            _banDangChonId ??= dsBan.FirstOrDefault()?.ID;

            flowBan.SuspendLayout();
            flowBan.Controls.Clear();

            foreach (var ban in dsBan)
            {
                flowBan.Controls.Add(TaoNutBan(ban));
            }

            if (dsBan.Count == 0)
            {
                flowBan.Controls.Add(TaoLabelRong("Chưa có dữ liệu bàn."));
            }

            flowBan.ResumeLayout();
        }

        private Button TaoNutBan(BanDTO ban)
        {
            var dangChon = _banDangChonId == ban.ID;
            var trangThai = ChuyenTrangThaiBan(ban.TrangThai);

            var btn = new Button
            {
                Width = 95,
                Height = 80,
                Margin = new Padding(8),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", dangChon ? 10F : 9.5F, dangChon ? FontStyle.Bold : FontStyle.Regular),
                ForeColor = Color.FromArgb(79, 56, 43),
                BackColor = dangChon
                    ? Color.FromArgb(255, 241, 230)
                    : ban.TrangThai == 1
                        ? Color.FromArgb(255, 248, 234)
                        : Color.FromArgb(248, 245, 241),
                Text = $"{ban.TenBan}\r\n{(dangChon ? "Đang chọn" : trangThai)}",
                Tag = ban.ID,
                UseVisualStyleBackColor = false
            };

            btn.FlatAppearance.BorderColor = Color.FromArgb(224, 214, 203);
            btn.Click += BtnBan_Click;
            return btn;
        }

        private void BtnBan_Click(object? sender, EventArgs e)
        {
            if (sender is not Button { Tag: int banId })
            {
                return;
            }

            _banDangChonId = banId;
            TaiSoDoBan();
            CapNhatPhieuDangChon();
        }

        private void TaiDanhSachMon()
        {
            var tuKhoa = txtSearch.Text.Trim();
            var dsMon = _monBUS.LayDanhSachMon(tuKhoa, null)
                .Where(x => x.TrangThai.Equals("Đang kinh doanh", StringComparison.OrdinalIgnoreCase))
                .Where(PhuHopBoLocLoaiMon)
                .OrderBy(x => x.TenLoaiMon)
                .ThenBy(x => x.TenMon)
                .ToList();

            if (!string.IsNullOrWhiteSpace(tuKhoa) && dsMon.Count == 0)
            {
                dsMon = _monBUS.LayDanhSachMon(null, null)
                    .Where(x => x.TrangThai.Equals("Đang kinh doanh", StringComparison.OrdinalIgnoreCase))
                    .Where(PhuHopBoLocLoaiMon)
                    .OrderBy(x => x.TenLoaiMon)
                    .ThenBy(x => x.TenMon)
                    .ToList();
            }

            flowMon.SuspendLayout();
            flowMon.Controls.Clear();

            foreach (var mon in dsMon)
            {
                flowMon.Controls.Add(TaoTheMon(mon));
            }

            if (dsMon.Count == 0)
            {
                flowMon.Controls.Add(TaoLabelRong("Không có món phù hợp."));
            }

            flowMon.ResumeLayout();
        }

        private Control TaoTheMon(MonDTO mon)
        {
            var panel = new Panel
            {
                BackColor = Color.FromArgb(250, 247, 243),
                Size = new Size(200, 120),
                Margin = new Padding(6)
            };

            var lblTen = new Label
            {
                AutoSize = false,
                Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold),
                ForeColor = Color.FromArgb(63, 45, 35),
                Location = new Point(14, 14),
                Size = new Size(170, 42),
                Text = mon.TenMon
            };

            var lblGia = new Label
            {
                AutoSize = true,
                Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold),
                ForeColor = Color.FromArgb(119, 63, 27),
                Location = new Point(14, 68),
                Text = $"{mon.DonGia:N0}đ"
            };

            var btnThem = new Button
            {
                BackColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.FromArgb(79, 56, 43),
                Location = new Point(100, 76),
                Size = new Size(86, 32),
                Text = "+ Thêm",
                Tag = mon,
                UseVisualStyleBackColor = false
            };

            btnThem.FlatAppearance.BorderColor = Color.FromArgb(230, 220, 210);
            btnThem.Click += BtnThemMon_Click;

            panel.Controls.Add(lblTen);
            panel.Controls.Add(lblGia);
            panel.Controls.Add(btnThem);
            return panel;
        }

        private static Label TaoLabelRong(string text)
        {
            return new Label
            {
                AutoSize = true,
                ForeColor = Color.FromArgb(130, 112, 96),
                Font = new Font("Segoe UI", 9.5F),
                Margin = new Padding(8),
                Text = text
            };
        }

        private void BtnThemMon_Click(object? sender, EventArgs e)
        {
            if (!_banDangChonId.HasValue)
            {
                MessageBox.Show("Vui lòng chọn bàn trước khi thêm món.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (sender is not Button { Tag: MonDTO mon })
            {
                return;
            }

            var gioTam = LayGioTamTheoBan(_banDangChonId.Value);
            var dongMon = gioTam.FirstOrDefault(x => x.MonID == mon.ID && x.DonGia == mon.DonGia);

            if (dongMon == null)
            {
                gioTam.Add(new BanHangOrderItemDTO
                {
                    MonID = mon.ID,
                    TenMon = mon.TenMon,
                    SoLuong = 1,
                    DonGia = mon.DonGia
                });
            }
            else
            {
                dongMon.SoLuong = (short)Math.Clamp(dongMon.SoLuong + 1, 1, short.MaxValue);
            }

            CapNhatPhieuDangChon();
        }

        private List<BanHangOrderItemDTO> LayGioTamTheoBan(int banId)
        {
            if (_gioTamTheoBan.TryGetValue(banId, out var gioTam))
            {
                return gioTam;
            }

            gioTam = new List<BanHangOrderItemDTO>();
            _gioTamTheoBan[banId] = gioTam;
            return gioTam;
        }

        private void CapNhatPhieuDangChon()
        {
            if (!_banDangChonId.HasValue)
            {
                dgvOrder.DataSource = null;
                lblOrderMeta.Text = "Chưa chọn bàn";
                lblThongTinBan.Text = "Chọn bàn để xem món đang phục vụ";
                lblTamTinhValue.Text = DinhDangTien(0);
                lblGiamGiaValue.Text = DinhDangTien(0);
                lblTongThanhToanValue.Text = DinhDangTien(0);
                return;
            }

            var banId = _banDangChonId.Value;
            var phieuDb = _banHangBUS.LayPhieuTheoBan(banId);
            var dsTam = _gioTamTheoBan.TryGetValue(banId, out var gioTam)
                ? gioTam
                : new List<BanHangOrderItemDTO>();

            var dsHienThi = TongHopChiTiet(phieuDb.ChiTiet.Concat(dsTam));

            dgvOrder.DataSource = null;
            dgvOrder.DataSource = dsHienThi;

            var tenBan = string.IsNullOrWhiteSpace(phieuDb.TenBan) ? $"Bàn {banId:D2}" : phieuDb.TenBan;
            var trangThaiBan = ChuyenTrangThaiBan(phieuDb.TrangThaiBan);
            var soMonTam = dsTam.Sum(x => x.SoLuong);
            var tongTien = dsHienThi.Sum(x => x.ThanhTien);
            var tongMon = dsHienThi.Sum(x => x.SoLuong);

            lblOrderMeta.Text = soMonTam > 0
                ? $"{tenBan} • {trangThaiBan} • {soMonTam} món chờ gọi"
                : $"{tenBan} • {trangThaiBan}";

            lblThongTinBan.Text = tongMon > 0
                ? $"{tenBan} đang có {tongMon} món phục vụ"
                : "Chọn bàn để xem món đang phục vụ";

            lblTamTinhValue.Text = DinhDangTien(tongTien);
            lblGiamGiaValue.Text = DinhDangTien(0);
            lblTongThanhToanValue.Text = DinhDangTien(tongTien);
        }

        private static List<BanHangOrderItemDTO> TongHopChiTiet(IEnumerable<BanHangOrderItemDTO> dsChiTiet)
        {
            return dsChiTiet
                .GroupBy(x => new { x.MonID, x.TenMon, x.DonGia })
                .Select(g => new BanHangOrderItemDTO
                {
                    MonID = g.Key.MonID,
                    TenMon = g.Key.TenMon,
                    DonGia = g.Key.DonGia,
                    SoLuong = (short)Math.Clamp(g.Sum(x => (int)x.SoLuong), 1, short.MaxValue)
                })
                .OrderBy(x => x.TenMon)
                .ToList();
        }

        private void btnGoiMon_Click(object? sender, EventArgs e)
        {
            if (!_banDangChonId.HasValue)
            {
                MessageBox.Show("Vui lòng chọn bàn trước khi gọi món.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var banId = _banDangChonId.Value;
            var gioTam = LayGioTamTheoBan(banId);

            if (gioTam.Count == 0)
            {
                MessageBox.Show("Không có món chờ gọi cho bàn này.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var result = _banHangBUS.GoiMon(
                banId,
                gioTam.Select(x => new BanHangThemMonDTO { MonID = x.MonID, SoLuong = x.SoLuong }));

            if (!result.ThanhCong)
            {
                MessageBox.Show(result.ThongBao, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            _gioTamTheoBan.Remove(banId);
            TaiSoDoBan();
            CapNhatPhieuDangChon();

            MessageBox.Show(result.ThongBao, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnTamTinh_Click(object? sender, EventArgs e)
        {
            if (dgvOrder.DataSource is not List<BanHangOrderItemDTO> dsHienThi || dsHienThi.Count == 0)
            {
                MessageBox.Show("Chưa có món để tạm tính.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var tongMon = dsHienThi.Sum(x => x.SoLuong);
            var tongTien = dsHienThi.Sum(x => x.ThanhTien);

            MessageBox.Show(
                $"Tổng số món: {tongMon}\nTạm tính: {DinhDangTien(tongTien)}",
                "Tạm tính",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private void btnThanhToan_Click(object? sender, EventArgs e)
        {
            if (!_banDangChonId.HasValue)
            {
                MessageBox.Show("Vui lòng chọn bàn trước khi thanh toán.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var banId = _banDangChonId.Value;
            var gioTam = LayGioTamTheoBan(banId);
            if (gioTam.Count > 0)
            {
                var xacNhanGoiMon = MessageBox.Show(
                    "Bàn đang có món chờ gọi. Bạn có muốn gọi món trước khi thanh toán không?",
                    "Xác nhận",
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Question);

                if (xacNhanGoiMon == DialogResult.Cancel)
                {
                    return;
                }

                if (xacNhanGoiMon == DialogResult.Yes)
                {
                    var resultGoiMon = _banHangBUS.GoiMon(
                        banId,
                        gioTam.Select(x => new BanHangThemMonDTO { MonID = x.MonID, SoLuong = x.SoLuong }));

                    if (!resultGoiMon.ThanhCong)
                    {
                        MessageBox.Show(resultGoiMon.ThongBao, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    _gioTamTheoBan.Remove(banId);
                }
            }

            var xacNhanThanhToan = MessageBox.Show(
                "Xác nhận thanh toán bàn đang chọn?",
                "Xác nhận",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (xacNhanThanhToan != DialogResult.Yes)
            {
                return;
            }

            var result = _banHangBUS.ThanhToan(banId);
            if (!result.ThanhCong)
            {
                MessageBox.Show(result.ThongBao, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            _gioTamTheoBan.Remove(banId);
            TaiSoDoBan();
            CapNhatPhieuDangChon();

            MessageBox.Show(result.ThongBao, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ThucHienChuyenHoacGopBan(bool laChuyenBan)
        {
            if (!_banDangChonId.HasValue)
            {
                MessageBox.Show("Vui lòng chọn bàn nguồn.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var banNguonId = _banDangChonId.Value;
            var gioTamNguon = LayGioTamTheoBan(banNguonId);
            if (gioTamNguon.Count > 0)
            {
                MessageBox.Show("Bàn nguồn còn món chờ gọi. Vui lòng gọi món trước khi chuyển/gộp.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var dsBanDich = _banBUS
                .LayDanhSachBanDich(banNguonId)
                .Where(x => laChuyenBan ? x.TrangThai == 0 : x.TrangThai == 1)
                .ToList();

            if (dsBanDich.Count == 0)
            {
                MessageBox.Show(
                    laChuyenBan ? "Không có bàn trống để chuyển." : "Không có bàn đang phục vụ để gộp.",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            using var dialog = new Form
            {
                Text = laChuyenBan ? "Chuyển bàn" : "Gộp bàn",
                StartPosition = FormStartPosition.CenterParent,
                Width = 390,
                Height = 180,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false
            };

            var lblBanDich = new Label { Left = 16, Top = 22, Width = 90, Text = "Bàn đích" };
            var cboBanDich = new ComboBox
            {
                Left = 106,
                Top = 18,
                Width = 252,
                DropDownStyle = ComboBoxStyle.DropDownList,
                DataSource = dsBanDich,
                DisplayMember = nameof(BanDTO.TenBan),
                ValueMember = nameof(BanDTO.ID)
            };

            var btnLuu = new Button
            {
                Left = 202,
                Top = 78,
                Width = 75,
                Text = "Lưu",
                DialogResult = DialogResult.OK
            };

            var btnHuy = new Button
            {
                Left = 283,
                Top = 78,
                Width = 75,
                Text = "Hủy",
                DialogResult = DialogResult.Cancel
            };

            dialog.Controls.AddRange(new Control[] { lblBanDich, cboBanDich, btnLuu, btnHuy });
            dialog.AcceptButton = btnLuu;
            dialog.CancelButton = btnHuy;

            if (dialog.ShowDialog(this) != DialogResult.OK || cboBanDich.SelectedValue is not int banDichId)
            {
                return;
            }

            var result = _banBUS.ChuyenHoacGopBan(new BanChuyenGopRequestDTO
            {
                BanNguonId = banNguonId,
                BanDichId = banDichId,
                LaChuyenBan = laChuyenBan
            });

            if (!result.ThanhCong)
            {
                MessageBox.Show(result.ThongBao, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            _banDangChonId = banDichId;
            TaiSoDoBan();
            CapNhatPhieuDangChon();

            MessageBox.Show(result.ThongBao, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ChonBoLocLoaiMon(string? boLoc, Button nutDuocChon)
        {
            _boLocLoaiMon = boLoc;
            ToMauNutLocMon(nutDuocChon);
            TaiDanhSachMon();
        }

        private void ToMauNutLocMon(Button nutDuocChon)
        {
            var dsNut = new[] { btnCafe, btnDaXay, btnTra, btnTatCa };
            foreach (var btn in dsNut)
            {
                var laNutDangChon = btn == nutDuocChon;
                btn.BackColor = laNutDangChon ? Color.FromArgb(94, 64, 47) : Color.FromArgb(248, 245, 241);
                btn.ForeColor = laNutDangChon ? Color.White : Color.FromArgb(79, 56, 43);
                btn.FlatAppearance.BorderSize = laNutDangChon ? 0 : 1;
                btn.FlatAppearance.BorderColor = Color.FromArgb(224, 214, 203);
            }
        }

        private bool PhuHopBoLocLoaiMon(MonDTO mon)
        {
            if (string.IsNullOrWhiteSpace(_boLocLoaiMon))
            {
                return true;
            }

            var tenLoai = ChuanHoaKhongDau(mon.TenLoaiMon);
            return _boLocLoaiMon switch
            {
                "cafe" => tenLoai.Contains("ca phe") || tenLoai.Contains("cafe"),
                "da xay" => tenLoai.Contains("da xay"),
                "tra" => tenLoai.Contains("tra"),
                _ => true
            };
        }

        private static string ChuyenTrangThaiBan(int trangThai)
        {
            return trangThai switch
            {
                1 => "Đang phục vụ",
                2 => "Đặt trước",
                _ => "Trống"
            };
        }

        private static string DinhDangTien(int soTien)
        {
            return $"{soTien:N0}đ";
        }

        private static string ChuanHoaKhongDau(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return string.Empty;
            }

            var normalized = text.Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder();
            foreach (var c in normalized)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(c);
                }
            }

            return sb
                .ToString()
                .Normalize(NormalizationForm.FormC)
                .ToLowerInvariant();
        }
    }
}
