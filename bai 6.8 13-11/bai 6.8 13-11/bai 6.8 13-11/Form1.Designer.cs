using System.Drawing;
using System.Windows.Forms;

namespace BilliardsManagement
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        // Khai báo các Control
        private FlowLayoutPanel flpBanBida;
        private GroupBox gbSoDoBan;
        private GroupBox gbThongTinBan;
        private Label lblTenBan;
        private Label lblLoaiBan;
        private Label lblGiaGio;
        private Label lblTrangThai;
        private Label lblGioBatDau;
        private Label lblThoiGianChoi;
        private Button btnMoBan;

        private GroupBox gbDichVu;
        private ComboBox cboDichVu;
        private NumericUpDown nudSoLuong;
        private Button btnThemMon;
        private DataGridView dgvChiTietHD;

        private GroupBox gbThanhToan;
        private Label lblTienGio;
        private Label lblTienDichVu;
        private Label lblTongTien;
        private Button btnThanhToan;

        private System.Windows.Forms.Timer tmrTinhGio;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.gbSoDoBan = new GroupBox();
            this.flpBanBida = new FlowLayoutPanel();
            this.gbThongTinBan = new GroupBox();
            this.btnMoBan = new Button();
            this.lblThoiGianChoi = new Label();
            this.lblGioBatDau = new Label();
            this.lblTrangThai = new Label();
            this.lblGiaGio = new Label();
            this.lblLoaiBan = new Label();
            this.lblTenBan = new Label();
            this.gbDichVu = new GroupBox();
            this.dgvChiTietHD = new DataGridView();
            this.btnThemMon = new Button();
            this.nudSoLuong = new NumericUpDown();
            this.cboDichVu = new ComboBox();
            this.gbThanhToan = new GroupBox();
            this.btnThanhToan = new Button();
            this.lblTongTien = new Label();
            this.lblTienDichVu = new Label();
            this.lblTienGio = new Label();
            this.tmrTinhGio = new System.Windows.Forms.Timer(this.components);

            this.gbSoDoBan.SuspendLayout();
            this.gbThongTinBan.SuspendLayout();
            this.gbDichVu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvChiTietHD)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSoLuong)).BeginInit();
            this.gbThanhToan.SuspendLayout();
            this.SuspendLayout();

            // 
            // gbSoDoBan (Trái)
            // 
            this.gbSoDoBan.Controls.Add(this.flpBanBida);
            this.gbSoDoBan.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.gbSoDoBan.Location = new Point(12, 12);
            this.gbSoDoBan.Name = "gbSoDoBan";
            this.gbSoDoBan.Size = new Size(460, 590);
            this.gbSoDoBan.TabIndex = 0;
            this.gbSoDoBan.TabStop = false;
            this.gbSoDoBan.Text = "SƠ ĐỒ BÀN BIDA";

            // 
            // flpBanBida
            // 
            this.flpBanBida.AutoScroll = true;
            this.flpBanBida.Dock = DockStyle.Fill;
            this.flpBanBida.Location = new Point(3, 25);
            this.flpBanBida.Name = "flpBanBida";
            this.flpBanBida.Padding = new Padding(10);
            this.flpBanBida.Size = new Size(454, 562);
            this.flpBanBida.TabIndex = 0;

            // 
            // gbThongTinBan (Phải - Trên)
            // 
            this.gbThongTinBan.Controls.Add(this.btnMoBan);
            this.gbThongTinBan.Controls.Add(this.lblThoiGianChoi);
            this.gbThongTinBan.Controls.Add(this.lblGioBatDau);
            this.gbThongTinBan.Controls.Add(this.lblTrangThai);
            this.gbThongTinBan.Controls.Add(this.lblGiaGio);
            this.gbThongTinBan.Controls.Add(this.lblLoaiBan);
            this.gbThongTinBan.Controls.Add(this.lblTenBan);
            this.gbThongTinBan.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.gbThongTinBan.Location = new Point(485, 12);
            this.gbThongTinBan.Name = "gbThongTinBan";
            this.gbThongTinBan.Size = new Size(510, 155);
            this.gbThongTinBan.TabIndex = 1;
            this.gbThongTinBan.TabStop = false;
            this.gbThongTinBan.Text = "THÔNG TIN BÀN ĐANG CHỌN";

            // Labels thông tin bàn
            this.lblTenBan.AutoSize = true;
            this.lblTenBan.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            this.lblTenBan.ForeColor = Color.DarkBlue;
            this.lblTenBan.Location = new Point(15, 30);
            this.lblTenBan.Text = "Bàn: (Chưa chọn)";

            this.lblLoaiBan.AutoSize = true;
            this.lblLoaiBan.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular);
            this.lblLoaiBan.Location = new Point(15, 62);
            this.lblLoaiBan.Text = "Loại bàn: ---";

            this.lblGiaGio.AutoSize = true;
            this.lblGiaGio.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular);
            this.lblGiaGio.Location = new Point(15, 90);
            this.lblGiaGio.Text = "Giá/giờ: 0 VNĐ";

            this.lblTrangThai.AutoSize = true;
            this.lblTrangThai.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            this.lblTrangThai.Location = new Point(15, 118);
            this.lblTrangThai.Text = "Trạng thái: ---";

            this.lblGioBatDau.AutoSize = true;
            this.lblGioBatDau.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular);
            this.lblGioBatDau.Location = new Point(230, 32);
            this.lblGioBatDau.Text = "Giờ bắt đầu: ---";

            this.lblThoiGianChoi.AutoSize = true;
            this.lblThoiGianChoi.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.lblThoiGianChoi.ForeColor = Color.DarkRed;
            this.lblThoiGianChoi.Location = new Point(230, 62);
            this.lblThoiGianChoi.Text = "Thời gian chơi: 00:00:00";

            // 
            // btnMoBan
            // 
            this.btnMoBan.BackColor = Color.SeaGreen;
            this.btnMoBan.Enabled = false;
            this.btnMoBan.FlatStyle = FlatStyle.Flat;
            this.btnMoBan.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.btnMoBan.ForeColor = Color.White;
            this.btnMoBan.Location = new Point(233, 98);
            this.btnMoBan.Name = "btnMoBan";
            this.btnMoBan.Size = new Size(255, 40);
            this.btnMoBan.Text = "MỞ BÀN";
            this.btnMoBan.UseVisualStyleBackColor = false;
            this.btnMoBan.Click += new System.EventHandler(this.btnMoBan_Click);

            // 
            // gbDichVu (Phải - Giữa)
            // 
            this.gbDichVu.Controls.Add(this.dgvChiTietHD);
            this.gbDichVu.Controls.Add(this.btnThemMon);
            this.gbDichVu.Controls.Add(this.nudSoLuong);
            this.gbDichVu.Controls.Add(this.cboDichVu);
            this.gbDichVu.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.gbDichVu.Location = new Point(485, 175);
            this.gbDichVu.Name = "gbDichVu";
            this.gbDichVu.Size = new Size(510, 290);
            this.gbDichVu.TabIndex = 2;
            this.gbDichVu.TabStop = false;
            this.gbDichVu.Text = "GỌI ĐỒ UỐNG / THUÊ PHỤ KIỆN";

            // cboDichVu
            this.cboDichVu.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cboDichVu.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular);
            this.cboDichVu.Location = new Point(15, 32);
            this.cboDichVu.Name = "cboDichVu";
            this.cboDichVu.Size = new Size(270, 29);

            // nudSoLuong
            this.nudSoLuong.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular);
            this.nudSoLuong.Location = new Point(295, 32);
            this.nudSoLuong.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            this.nudSoLuong.Name = "nudSoLuong";
            this.nudSoLuong.Size = new Size(65, 29);
            this.nudSoLuong.Value = new decimal(new int[] { 1, 0, 0, 0 });

            // btnThemMon
            this.btnThemMon.BackColor = Color.SteelBlue;
            this.btnThemMon.Enabled = false;
            this.btnThemMon.FlatStyle = FlatStyle.Flat;
            this.btnThemMon.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            this.btnThemMon.ForeColor = Color.White;
            this.btnThemMon.Location = new Point(370, 30);
            this.btnThemMon.Name = "btnThemMon";
            this.btnThemMon.Size = new Size(125, 32);
            this.btnThemMon.Text = "Thêm món";
            this.btnThemMon.UseVisualStyleBackColor = false;
            this.btnThemMon.Click += new System.EventHandler(this.btnThemMon_Click);

            // dgvChiTietHD
            this.dgvChiTietHD.AllowUserToAddRows = false;
            this.dgvChiTietHD.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvChiTietHD.BackgroundColor = Color.White;
            this.dgvChiTietHD.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            this.dgvChiTietHD.Location = new Point(15, 72);
            this.dgvChiTietHD.Name = "dgvChiTietHD";
            this.dgvChiTietHD.RowHeadersVisible = false;
            this.dgvChiTietHD.Size = new Size(480, 205);
            this.dgvChiTietHD.CellEndEdit += new DataGridViewCellEventHandler(this.dgvChiTietHD_CellEndEdit);

            // 
            // gbThanhToan (Phải - Dưới)
            // 
            this.gbThanhToan.Controls.Add(this.btnThanhToan);
            this.gbThanhToan.Controls.Add(this.lblTongTien);
            this.gbThanhToan.Controls.Add(this.lblTienDichVu);
            this.gbThanhToan.Controls.Add(this.lblTienGio);
            this.gbThanhToan.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.gbThanhToan.Location = new Point(485, 472);
            this.gbThanhToan.Name = "gbThanhToan";
            this.gbThanhToan.Size = new Size(510, 130);
            this.gbThanhToan.TabIndex = 3;
            this.gbThanhToan.TabStop = false;
            this.gbThanhToan.Text = "THANH TOÁN HÓA ĐƠN";

            this.lblTienGio.AutoSize = true;
            this.lblTienGio.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular);
            this.lblTienGio.Location = new Point(15, 30);
            this.lblTienGio.Text = "Tiền giờ chơi (tạm tính): 0 VNĐ";

            this.lblTienDichVu.AutoSize = true;
            this.lblTienDichVu.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular);
            this.lblTienDichVu.Location = new Point(15, 58);
            this.lblTienDichVu.Text = "Tiền dịch vụ: 0 VNĐ";

            this.lblTongTien.AutoSize = true;
            this.lblTongTien.Font = new Font("Segoe UI", 11.5F, FontStyle.Bold);
            this.lblTongTien.ForeColor = Color.Crimson;
            this.lblTongTien.Location = new Point(15, 88);
            this.lblTongTien.Text = "TỔNG CỘNG: 0 VNĐ";

            // btnThanhToan
            this.btnThanhToan.BackColor = Color.DarkOrange;
            this.btnThanhToan.Enabled = false;
            this.btnThanhToan.FlatStyle = FlatStyle.Flat;
            this.btnThanhToan.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            this.btnThanhToan.ForeColor = Color.White;
            this.btnThanhToan.Location = new Point(315, 38);
            this.btnThanhToan.Name = "btnThanhToan";
            this.btnThanhToan.Size = new Size(180, 65);
            this.btnThanhToan.Text = "THANH TOÁN";
            this.btnThanhToan.UseVisualStyleBackColor = false;
            this.btnThanhToan.Click += new System.EventHandler(this.btnThanhToan_Click);

            // tmrTinhGio
            this.tmrTinhGio.Interval = 1000;
            this.tmrTinhGio.Tick += new System.EventHandler(this.tmrTinhGio_Tick);

            // Form1
            this.ClientSize = new Size(1010, 615);
            this.Controls.Add(this.gbThanhToan);
            this.Controls.Add(this.gbDichVu);
            this.Controls.Add(this.gbThongTinBan);
            this.Controls.Add(this.gbSoDoBan);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Hệ Thống Quản Lý Bàn Bida & Dịch Vụ Giải Trí";
            this.Load += new System.EventHandler(this.Form1_Load);

            this.gbSoDoBan.ResumeLayout(false);
            this.gbThongTinBan.ResumeLayout(false);
            this.gbThongTinBan.PerformLayout();
            this.gbDichVu.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvChiTietHD)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSoLuong)).EndInit();
            this.gbThanhToan.ResumeLayout(false);
            this.gbThanhToan.PerformLayout();
            this.ResumeLayout(false);
        }
    }
}