using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace BilliardsManagement
{
    public partial class Form1 : Form
    {
        // Thay đổi chuỗi kết nối nếu bạn dùng SQL Express hoặc tên Server khác
        private string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=BilliardsDB;Integrated Security=True;";

        // Lưu thông tin bàn & hóa đơn đang chọn
        private int selectedMaBan = -1;
        private string selectedTenBan = "";
        private string selectedLoaiBan = "";
        private decimal selectedGiaTheoGio = 0;
        private string selectedTrangThai = "";

        private int currentMaHD = -1;
        private DateTime currentGioBatDau;

        // Disconnected Model cho ChiTietHoaDon
        private SqlDataAdapter daChiTiet;
        private DataTable dtChiTiet;
        private DataTable dtMenuDichVu;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadMenuDichVu();
            LoadSoDoBan();
        }

        #region 1. SƠ ĐỒ BÀN BIDA TRỰC QUAN
        private void LoadSoDoBan()
        {
            flpBanBida.Controls.Clear();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT MaBan, TenBan, LoaiBan, GiaTheoGio, TrangThai FROM BanBida";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                foreach (DataRow row in dt.Rows)
                {
                    int maBan = Convert.ToInt32(row["MaBan"]);
                    string tenBan = row["TenBan"].ToString();
                    string loaiBan = row["LoaiBan"].ToString();
                    decimal giaGio = Convert.ToDecimal(row["GiaTheoGio"]);
                    string trangThai = row["TrangThai"].ToString();

                    Button btn = new Button();
                    btn.Width = 130;
                    btn.Height = 100;
                    btn.Margin = new Padding(8);
                    btn.FlatStyle = FlatStyle.Flat;
                    btn.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
                    btn.ForeColor = Color.White;
                    btn.Text = $"{tenBan}\n({loaiBan})\n[{trangThai}]";
                    btn.Tag = row; // Lưu DataRow vào Tag để dùng khi Click

                    // Bàn Trống: Xanh lá | Đang chơi: Cam đỏ
                    if (trangThai == "Trống")
                        btn.BackColor = Color.SeaGreen;
                    else
                        btn.BackColor = Color.OrangeRed;

                    btn.Click += BtnBan_Click;
                    flpBanBida.Controls.Add(btn);
                }
            }
        }

        private void BtnBan_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            DataRow row = btn.Tag as DataRow;

            selectedMaBan = Convert.ToInt32(row["MaBan"]);
            selectedTenBan = row["TenBan"].ToString();
            selectedLoaiBan = row["LoaiBan"].ToString();
            selectedGiaTheoGio = Convert.ToDecimal(row["GiaTheoGio"]);
            selectedTrangThai = row["TrangThai"].ToString();

            HienThiThongTinBan();
        }

        private void HienThiThongTinBan()
        {
            lblTenBan.Text = $"Bàn: {selectedTenBan}";
            lblLoaiBan.Text = $"Loại bàn: {selectedLoaiBan}";
            lblGiaGio.Text = $"Giá/giờ: {selectedGiaTheoGio:N0} VNĐ";
            lblTrangThai.Text = $"Trạng thái: {selectedTrangThai}";

            if (selectedTrangThai == "Trống")
            {
                lblTrangThai.ForeColor = Color.SeaGreen;
                lblGioBatDau.Text = "Giờ bắt đầu: ---";
                lblThoiGianChoi.Text = "Thời gian chơi: 00:00:00";

                btnMoBan.Enabled = true;
                btnThemMon.Enabled = false;
                btnThanhToan.Enabled = false;
                tmrTinhGio.Stop();

                currentMaHD = -1;
                dgvChiTietHD.DataSource = null;
                CapNhatTongTienTamTinh(0, 0);
            }
            else // Đang chơi
            {
                lblTrangThai.ForeColor = Color.OrangeRed;
                btnMoBan.Enabled = false;
                btnThemMon.Enabled = true;
                btnThanhToan.Enabled = true;

                LoadHoaDonHienTai(selectedMaBan);
                LoadChiTietDichVu(currentMaHD);
                tmrTinhGio.Start();
                CapNhatThoiGianVaTienGio();
            }
        }
        #endregion

        #region 2. MỞ BÀN & TÍNH GIỜ TỰ ĐỘNG
        private void btnMoBan_Click(object sender, EventArgs e)
        {
            if (selectedMaBan == -1 || selectedTrangThai != "Trống") return;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlTransaction trans = conn.BeginTransaction();
                try
                {
                    // Tạo Hóa đơn mới với GioBatDau = GETDATE()
                    string insertHD = "INSERT INTO HoaDon (MaBan, GioBatDau, TrangThai) VALUES (@MaBan, GETDATE(), 0)";
                    SqlCommand cmdHD = new SqlCommand(insertHD, conn, trans);
                    cmdHD.Parameters.AddWithValue("@MaBan", selectedMaBan);
                    cmdHD.ExecuteNonQuery();

                    // Cập nhật trạng thái bàn sang Đang chơi
                    string updateBan = "UPDATE BanBida SET TrangThai = N'Đang chơi' WHERE MaBan = @MaBan";
                    SqlCommand cmdBan = new SqlCommand(updateBan, conn, trans);
                    cmdBan.Parameters.AddWithValue("@MaBan", selectedMaBan);
                    cmdBan.ExecuteNonQuery();

                    trans.Commit();

                    selectedTrangThai = "Đang chơi";
                    LoadSoDoBan();
                    HienThiThongTinBan();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    MessageBox.Show("Lỗi khi mở bàn: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void LoadHoaDonHienTai(int maBan)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT TOP 1 MaHD, GioBatDau FROM HoaDon WHERE MaBan = @MaBan AND TrangThai = 0 ORDER BY MaHD DESC";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaBan", maBan);
                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        currentMaHD = Convert.ToInt32(reader["MaHD"]);
                        currentGioBatDau = Convert.ToDateTime(reader["GioBatDau"]);
                        lblGioBatDau.Text = $"Giờ bắt đầu: {currentGioBatDau:HH:mm:ss dd/MM}";
                    }
                }
            }
        }

        private void tmrTinhGio_Tick(object sender, EventArgs e)
        {
            if (currentMaHD != -1 && selectedTrangThai == "Đang chơi")
            {
                CapNhatThoiGianVaTienGio();
            }
        }

        private decimal TinhTienGioChoi(DateTime gioBatDau, DateTime gioKetThuc, decimal giaTheoGio)
        {
            TimeSpan thoiGian = gioKetThuc - gioBatDau;
            double tongPhut = thoiGian.TotalMinutes;
            if (tongPhut <= 0) tongPhut = 1;

            // Làm tròn theo block 15 phút (tối thiểu 1 block = 0.25 giờ)
            double soBlock15Phut = Math.Ceiling(tongPhut / 15.0);
            decimal soGio = (decimal)(soBlock15Phut * 0.25);

            // Nếu muốn tính chính xác từng phút thì dùng: decimal soGio = (decimal)(tongPhut / 60.0);
            return soGio * giaTheoGio;
        }

        private void CapNhatThoiGianVaTienGio()
        {
            DateTime now = DateTime.Now;
            TimeSpan ts = now - currentGioBatDau;
            if (ts.TotalSeconds < 0) ts = TimeSpan.Zero;

            lblThoiGianChoi.Text = $"Thời gian chơi: {ts.Hours:D2}:{ts.Minutes:D2}:{ts.Seconds:D2}";

            decimal tienGio = TinhTienGioChoi(currentGioBatDau, now, selectedGiaTheoGio);
            decimal tienDV = TinhTongTienDichVu();
            CapNhatTongTienTamTinh(tienGio, tienDV);
        }
        #endregion

        #region 3. GỌI ĐỒ UỐNG / THUÊ PHỤ KIỆN (DISCONNECTED MODEL - DATATABLE)
        private void LoadMenuDichVu()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT MaDV, TenDV, DonGia, TonKho FROM MenuDichVu";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                dtMenuDichVu = new DataTable();
                da.Fill(dtMenuDichVu);

                cboDichVu.DataSource = dtMenuDichVu;
                cboDichVu.DisplayMember = "TenDV";
                cboDichVu.ValueMember = "MaDV";
            }
        }

        private void LoadChiTietDichVu(int maHD)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            string selectQuery = @"
                SELECT ct.MaHD, ct.MaDV, m.TenDV, ct.SoLuong, ct.DonGia
                FROM ChiTietHoaDon ct
                INNER JOIN MenuDichVu m ON ct.MaDV = m.MaDV
                WHERE ct.MaHD = @MaHD";

            daChiTiet = new SqlDataAdapter(selectQuery, conn);
            daChiTiet.SelectCommand.Parameters.AddWithValue("@MaHD", maHD);

            // Cấu hình InsertCommand, UpdateCommand, DeleteCommand cho SqlDataAdapter (Disconnected Model)
            daChiTiet.InsertCommand = new SqlCommand(
                "INSERT INTO ChiTietHoaDon (MaHD, MaDV, SoLuong, DonGia) VALUES (@MaHD, @MaDV, @SoLuong, @DonGia)", conn);
            daChiTiet.InsertCommand.Parameters.Add("@MaHD", SqlDbType.Int, 4, "MaHD");
            daChiTiet.InsertCommand.Parameters.Add("@MaDV", SqlDbType.Int, 4, "MaDV");
            daChiTiet.InsertCommand.Parameters.Add("@SoLuong", SqlDbType.Int, 4, "SoLuong");
            daChiTiet.InsertCommand.Parameters.Add("@DonGia", SqlDbType.Decimal, 18, "DonGia");

            daChiTiet.UpdateCommand = new SqlCommand(
                "UPDATE ChiTietHoaDon SET SoLuong = @SoLuong WHERE MaHD = @MaHD AND MaDV = @MaDV", conn);
            daChiTiet.UpdateCommand.Parameters.Add("@SoLuong", SqlDbType.Int, 4, "SoLuong");
            daChiTiet.UpdateCommand.Parameters.Add("@MaHD", SqlDbType.Int, 4, "MaHD");
            daChiTiet.UpdateCommand.Parameters.Add("@MaDV", SqlDbType.Int, 4, "MaDV");

            daChiTiet.DeleteCommand = new SqlCommand(
                "DELETE FROM ChiTietHoaDon WHERE MaHD = @MaHD AND MaDV = @MaDV", conn);
            daChiTiet.DeleteCommand.Parameters.Add("@MaHD", SqlDbType.Int, 4, "MaHD");
            daChiTiet.DeleteCommand.Parameters.Add("@MaDV", SqlDbType.Int, 4, "MaDV");

            dtChiTiet = new DataTable();
            daChiTiet.Fill(dtChiTiet);

            // Thêm cột tính toán Thành Tiền trên DataTable
            DataColumn colThanhTien = new DataColumn("ThanhTien", typeof(decimal));
            colThanhTien.Expression = "SoLuong * DonGia";
            dtChiTiet.Columns.Add(colThanhTien);

            dgvChiTietHD.DataSource = dtChiTiet;

            // Định dạng cột hiển thị trên DataGridView
            dgvChiTietHD.Columns["MaHD"].Visible = false;
            dgvChiTietHD.Columns["MaDV"].Visible = false;
            dgvChiTietHD.Columns["TenDV"].HeaderText = "Tên dịch vụ";
            dgvChiTietHD.Columns["TenDV"].ReadOnly = true;
            dgvChiTietHD.Columns["SoLuong"].HeaderText = "Số lượng";
            dgvChiTietHD.Columns["SoLuong"].ReadOnly = false; // Cho phép sửa trực tiếp số lượng
            dgvChiTietHD.Columns["DonGia"].HeaderText = "Đơn giá";
            dgvChiTietHD.Columns["DonGia"].ReadOnly = true;
            dgvChiTietHD.Columns["DonGia"].DefaultCellStyle.Format = "N0";
            dgvChiTietHD.Columns["ThanhTien"].HeaderText = "Thành tiền";
            dgvChiTietHD.Columns["ThanhTien"].ReadOnly = true;
            dgvChiTietHD.Columns["ThanhTien"].DefaultCellStyle.Format = "N0";
        }

        private void btnThemMon_Click(object sender, EventArgs e)
        {
            if (currentMaHD == -1 || cboDichVu.SelectedValue == null) return;

            int maDV = Convert.ToInt32(cboDichVu.SelectedValue);
            DataRowView drv = cboDichVu.SelectedItem as DataRowView;
            string tenDV = drv["TenDV"].ToString();
            decimal donGia = Convert.ToDecimal(drv["DonGia"]);
            int tonKho = Convert.ToInt32(drv["TonKho"]);
            int soLuongThem = (int)nudSoLuong.Value;

            // Kiểm tra món đã có trong DataTable chưa
            DataRow existingRow = null;
            foreach (DataRow r in dtChiTiet.Rows)
            {
                if (r.RowState != DataRowState.Deleted && Convert.ToInt32(r["MaDV"]) == maDV)
                {
                    existingRow = r;
                    break;
                }
            }

            int tongSoLuongDuKien = soLuongThem + (existingRow != null ? Convert.ToInt32(existingRow["SoLuong"]) : 0);
            if (!tenDV.ToLower().Contains("thuê") && tongSoLuongDuKien > tonKho)
            {
                MessageBox.Show($"Số lượng vượt quá tồn kho hiện tại ({tonKho})!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (existingRow != null)
            {
                existingRow["SoLuong"] = tongSoLuongDuKien;
            }
            else
            {
                DataRow newRow = dtChiTiet.NewRow();
                newRow["MaHD"] = currentMaHD;
                newRow["MaDV"] = maDV;
                newRow["TenDV"] = tenDV;
                newRow["SoLuong"] = soLuongThem;
                newRow["DonGia"] = donGia;
                dtChiTiet.Rows.Add(newRow);
            }

            // Đồng bộ DataTable xuống SQL Server bằng SqlDataAdapter
            daChiTiet.Update(dtChiTiet);
            CapNhatThoiGianVaTienGio();
        }

        // Cho phép sửa trực tiếp số lượng trên DataGridView
        private void dgvChiTietHD_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvChiTietHD.Columns[e.ColumnIndex].Name == "SoLuong")
            {
                try
                {
                    int newQty = Convert.ToInt32(dgvChiTietHD.Rows[e.RowIndex].Cells["SoLuong"].Value);
                    if (newQty <= 0)
                    {
                        if (MessageBox.Show("Số lượng <= 0. Bạn muốn xóa món này khỏi hóa đơn?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            dtChiTiet.Rows[e.RowIndex].Delete();
                        }
                        else
                        {
                            dtChiTiet.RejectChanges();
                            return;
                        }
                    }

                    daChiTiet.Update(dtChiTiet);
                    CapNhatThoiGianVaTienGio();
                }
                catch
                {
                    MessageBox.Show("Vui lòng nhập số lượng hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    dtChiTiet.RejectChanges();
                }
            }
        }

        private decimal TinhTongTienDichVu()
        {
            decimal tong = 0;
            if (dtChiTiet != null)
            {
                foreach (DataRow row in dtChiTiet.Rows)
                {
                    if (row.RowState != DataRowState.Deleted)
                    {
                        tong += Convert.ToDecimal(row["ThanhTien"]);
                    }
                }
            }
            return tong;
        }

        private void CapNhatTongTienTamTinh(decimal tienGio, decimal tienDV)
        {
            lblTienGio.Text = $"Tiền giờ chơi (tạm tính): {tienGio:N0} VNĐ";
            lblTienDichVu.Text = $"Tiền dịch vụ: {tienDV:N0} VNĐ";
            lblTongTien.Text = $"TỔNG CỘNG: {(tienGio + tienDV):N0} VNĐ";
        }
        #endregion

        #region 4. THANH TOÁN HÓA ĐƠN (SQLTRANSACTION)
        private void btnThanhToan_Click(object sender, EventArgs e)
        {
            if (currentMaHD == -1 || selectedMaBan == -1) return;

            DateTime gioKetThuc = DateTime.Now;
            decimal tienGio = TinhTienGioChoi(currentGioBatDau, gioKetThuc, selectedGiaTheoGio);
            decimal tienDV = TinhTongTienDichVu();
            decimal tongTien = tienGio + tienDV;

            DialogResult confirm = MessageBox.Show(
                $"Xác nhận thanh toán cho {selectedTenBan}?\n" +
                $"- Tiền giờ chơi: {tienGio:N0} VNĐ\n" +
                $"- Tiền dịch vụ: {tienDV:N0} VNĐ\n" +
                $"==> TỔNG THANH TOÁN: {tongTien:N0} VNĐ",
                "Xác nhận thanh toán", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes) return;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlTransaction trans = conn.BeginTransaction();

                try
                {
                    // 1. Cập nhật GioKetThuc, TienGioChoi, TienDichVu, TrangThai = 1 cho HoaDon
                    string updateHD = @"
                        UPDATE HoaDon 
                        SET GioKetThuc = @GioKetThuc, 
                            TienGioChoi = @TienGioChoi, 
                            TienDichVu = @TienDichVu, 
                            TrangThai = 1 
                        WHERE MaHD = @MaHD";
                    SqlCommand cmdHD = new SqlCommand(updateHD, conn, trans);
                    cmdHD.Parameters.AddWithValue("@GioKetThuc", gioKetThuc);
                    cmdHD.Parameters.AddWithValue("@TienGioChoi", tienGio);
                    cmdHD.Parameters.AddWithValue("@TienDichVu", tienDV);
                    cmdHD.Parameters.AddWithValue("@MaHD", currentMaHD);
                    cmdHD.ExecuteNonQuery();

                    // 2. Trừ TonKho tương ứng trong MenuDichVu (Bỏ qua dịch vụ Thuê gậy)
                    foreach (DataRow row in dtChiTiet.Rows)
                    {
                        if (row.RowState == DataRowState.Deleted) continue;

                        string tenDV = row["TenDV"].ToString();
                        if (!tenDV.ToLower().Contains("thuê"))
                        {
                            int maDV = Convert.ToInt32(row["MaDV"]);
                            int soLuong = Convert.ToInt32(row["SoLuong"]);

                            string updateKho = "UPDATE MenuDichVu SET TonKho = TonKho - @SoLuong WHERE MaDV = @MaDV";
                            SqlCommand cmdKho = new SqlCommand(updateKho, conn, trans);
                            cmdKho.Parameters.AddWithValue("@SoLuong", soLuong);
                            cmdKho.Parameters.AddWithValue("@MaDV", maDV);
                            cmdKho.ExecuteNonQuery();
                        }
                    }

                    // 3. Cập nhật trạng thái BanBida về "Trống"
                    string updateBan = "UPDATE BanBida SET TrangThai = N'Trống' WHERE MaBan = @MaBan";
                    SqlCommand cmdBan = new SqlCommand(updateBan, conn, trans);
                    cmdBan.Parameters.AddWithValue("@MaBan", selectedMaBan);
                    cmdBan.ExecuteNonQuery();

                    // 4. Commit Transaction
                    trans.Commit();

                    tmrTinhGio.Stop();
                    MessageBox.Show($"Thanh toán thành công hóa đơn #{currentMaHD}!\nTổng tiền: {tongTien:N0} VNĐ",
                        "Hoàn tất", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Làm mới giao diện
                    selectedTrangThai = "Trống";
                    LoadMenuDichVu(); // Cập nhật lại tồn kho mới vào ComboBox
                    LoadSoDoBan();
                    HienThiThongTinBan();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    MessageBox.Show("Lỗi khi thanh toán (đã Rollback): " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        #endregion
    }
}