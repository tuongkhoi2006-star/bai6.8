USE BilliardsDB;
GO

-- 1. Bảng BanBida
CREATE TABLE BanBida (
    MaBan INT PRIMARY KEY IDENTITY(1,1),
    TenBan NVARCHAR(50) NOT NULL,
    LoaiBan NVARCHAR(30),
    GiaTheoGio DECIMAL(18,2) NOT NULL,
    TrangThai NVARCHAR(20) DEFAULT N'Trống'
);

-- 2. Bảng MenuDichVu
CREATE TABLE MenuDichVu (
    MaDV INT PRIMARY KEY IDENTITY(1,1),
    TenDV NVARCHAR(100) NOT NULL,
    DonGia DECIMAL(18,2) NOT NULL,
    TonKho INT NOT NULL
);

-- 3. Bảng HoaDon
CREATE TABLE HoaDon (
    MaHD INT PRIMARY KEY IDENTITY(1,1),
    MaBan INT FOREIGN KEY REFERENCES BanBida(MaBan),
    GioBatDau DATETIME NOT NULL,
    GioKetThuc DATETIME NULL,
    TienGioChoi DECIMAL(18,2) DEFAULT 0,
    TienDichVu DECIMAL(18,2) DEFAULT 0,
    TrangThai BIT DEFAULT 0 -- 0: Chưa thanh toán, 1: Đã thanh toán
);

-- 4. Bảng ChiTietHoaDon
CREATE TABLE ChiTietHoaDon (
    MaHD INT FOREIGN KEY REFERENCES HoaDon(MaHD),
    MaDV INT FOREIGN KEY REFERENCES MenuDichVu(MaDV),
    SoLuong INT CHECK (SoLuong > 0),
    DonGia DECIMAL(18,2) NOT NULL,
    PRIMARY KEY (MaHD, MaDV)
);
GO

-- Thêm dữ liệu mẫu cho BanBida
INSERT INTO BanBida (TenBan, LoaiBan, GiaTheoGio, TrangThai) VALUES
(N'Bàn Lỗ 01', N'Pool (Lỗ)', 60000, N'Trống'),
(N'Bàn Lỗ 02', N'Pool (Lỗ)', 60000, N'Trống'),
(N'Bàn Lỗ 03', N'Pool (Lỗ)', 65000, N'Trống'),
(N'Bàn Carom 01', N'Carom (Phăng)', 70000, N'Trống'),
(N'Bàn Carom 02', N'Carom (Phăng)', 70000, N'Trống'),
(N'Bàn Snooker VIP', N'Snooker', 100000, N'Trống');

-- Thêm dữ liệu mẫu cho MenuDichVu
INSERT INTO MenuDichVu (TenDV, DonGia, TonKho) VALUES
(N'Nước tăng lực Sting', 15000, 50),
(N'Nước suối Aquafina', 10000, 100),
(N'Cà phê sữa đá', 25000, 40),
(N'Mì xào bò', 45000, 20),
(N'Khoai tây chiên', 35000, 30),
(N'Thuê gậy Carbon', 30000, 10),
(N'Thuê gậy Break', 20000, 10);
GO