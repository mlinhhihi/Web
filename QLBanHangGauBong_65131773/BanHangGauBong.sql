USE master;
GO

IF EXISTS (SELECT * FROM sys.databases WHERE name = 'QuanLyBanHangGauBong')
BEGIN
    ALTER DATABASE QuanLyBanHangGauBong SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE QuanLyBanHangGauBong;
END
GO

CREATE DATABASE QuanLyBanHangGauBong;
GO

USE QuanLyBanHangGauBong;
GO
CREATE TABLE LOAISP (
    MaLoaiSP INT IDENTITY PRIMARY KEY,
    TenLoaiSP NVARCHAR(50) NOT NULL,
    ChieuRongCM INT CHECK (ChieuRongCM > 0),
    ChieuDaiCM INT CHECK (ChieuDaiCM > 0),
    KichThuoc AS (
        CAST(ChieuRongCM AS NVARCHAR(10)) + 'x' +
        CAST(ChieuDaiCM AS NVARCHAR(10))
    ) PERSISTED
);
GO
CREATE TABLE SANPHAM (
    MaSP INT IDENTITY PRIMARY KEY,
    MaLoaiSP INT NOT NULL,
    TenSP NVARCHAR(100) NOT NULL,
    AnhSP NVARCHAR(255),
    DonGia DECIMAL(18,2) CHECK (DonGia > 0),
    GiaNhap DECIMAL(18,2) CHECK (GiaNhap >= 0),
    SoLuongTon INT DEFAULT 0 CHECK (SoLuongTon >= 0),

    CONSTRAINT FK_SP_LOAI FOREIGN KEY (MaLoaiSP) REFERENCES LOAISP(MaLoaiSP)
);
GO
CREATE TABLE QUYEN (
    MaQuyen INT IDENTITY PRIMARY KEY,
    TenQuyen NVARCHAR(50) NOT NULL
);
GO
CREATE TABLE NHANVIEN (
    MaNV INT IDENTITY PRIMARY KEY,
    HoTenNV NVARCHAR(100) NOT NULL,
    SDT NVARCHAR(15),
    TenTK NVARCHAR(50) UNIQUE NOT NULL,
    MatKhau VARBINARY(64) NOT NULL,
    MaQuyen INT NOT NULL,
    LoaiNV NVARCHAR(30),

    CONSTRAINT FK_NV_QUYEN FOREIGN KEY (MaQuyen) REFERENCES QUYEN(MaQuyen)
);
GO
CREATE TABLE KHACHHANG (
    MaKH INT IDENTITY PRIMARY KEY,
    HoTen NVARCHAR(100),
    TenTK NVARCHAR(50) UNIQUE NOT NULL,
    MatKhau VARBINARY(64),
    Email NVARCHAR(100),
    SDT NVARCHAR(20),
    GioiTinh NVARCHAR(10)
        CHECK (GioiTinh IN (N'Nam', N'Nữ', N'Khác')),
    DiaChi NVARCHAR(255),
    NgayTaoTK DATETIME DEFAULT GETDATE()
);
GO
CREATE TABLE DONHANG (
    SoHD INT IDENTITY PRIMARY KEY,
    NgayDatHang DATETIME DEFAULT GETDATE(),
    NgayGiaoHang DATETIME,
    MaKH INT NOT NULL,
    MaNV INT,
    MaNV_GiaoHang INT,
    DiaChiGiao NVARCHAR(255) NOT NULL,
    TinhTrangDonHang NVARCHAR(50) DEFAULT N'Chưa giao',
    HinhThucTT NVARCHAR(30),
    TongTien DECIMAL(18,2) DEFAULT 0,

    CONSTRAINT FK_DH_KH FOREIGN KEY (MaKH) REFERENCES KHACHHANG(MaKH),
    CONSTRAINT FK_DH_NV FOREIGN KEY (MaNV) REFERENCES NHANVIEN(MaNV),
    CONSTRAINT FK_DH_SHIP FOREIGN KEY (MaNV_GiaoHang) REFERENCES NHANVIEN(MaNV)
);
GO
CREATE TABLE CHITIET_DONHANG (
    SoHD INT,
    MaSP INT,
    SoLuong INT CHECK (SoLuong > 0),
    DonGiaBan DECIMAL(18,2),
    ThanhTien AS (SoLuong * DonGiaBan) PERSISTED,

    PRIMARY KEY (SoHD, MaSP),
    FOREIGN KEY (SoHD) REFERENCES DONHANG(SoHD) ON DELETE CASCADE,
    FOREIGN KEY (MaSP) REFERENCES SANPHAM(MaSP)
);
GO
CREATE TABLE GIOHANG (
    MaKH INT,
    MaSP INT,
    SoLuong INT CHECK (SoLuong > 0),
    NgayThem DATETIME DEFAULT GETDATE(),

    PRIMARY KEY (MaKH, MaSP),
    FOREIGN KEY (MaKH) REFERENCES KHACHHANG(MaKH) ON DELETE CASCADE,
    FOREIGN KEY (MaSP) REFERENCES SANPHAM(MaSP) ON DELETE CASCADE
);

INSERT INTO QUYEN VALUES (N'Admin'), (N'Nhân viên'), (N'Giao hàng');

INSERT INTO NHANVIEN (HoTenNV, SDT, TenTK, MatKhau, MaQuyen, LoaiNV)
VALUES
(N'Admin', '0901', 'admin', HASHBYTES('SHA2_256','admin'), 1, N'Quản lý'),
(N'Thu ngân', '0902', 'nv1', HASHBYTES('SHA2_256','123'), 2, N'Thu ngân'),
(N'Shipper', '0903', 'ship', HASHBYTES('SHA2_256','123'), 3, N'Giao hàng');
GO

INSERT INTO KHACHHANG
(HoTen, TenTK, MatKhau, Email, SDT, GioiTinh, DiaChi)
VALUES
(N'Trần Minh', 'kh2', HASHBYTES('SHA2_256','123'), 'kh2@gmail.com', '0901', N'Nam', N'HCM'),
(N'Lê Hoa', 'kh3', HASHBYTES('SHA2_256','123'), 'kh3@gmail.com', '0902', N'Nữ', N'Hà Nội'),
(N'Phạm Long', 'kh4', HASHBYTES('SHA2_256','123'), 'kh4@gmail.com', '0903', N'Nam', N'Đà Nẵng'),
(N'Nguyễn Mai', 'kh5', HASHBYTES('SHA2_256','123'), 'kh5@gmail.com', '0904', N'Nữ', N'Cần Thơ'),
(N'Võ An', 'kh6', HASHBYTES('SHA2_256','123'), 'kh6@gmail.com', '0905', N'Nam', N'Bình Dương'),
(N'Hoàng Lan', 'kh7', HASHBYTES('SHA2_256','123'), 'kh7@gmail.com', '0906', N'Nữ', N'Hải Phòng'),
(N'Đỗ Quân', 'kh8', HASHBYTES('SHA2_256','123'), 'kh8@gmail.com', '0907', N'Nam', N'Nghệ An'),
(N'Bùi Thảo', 'kh9', HASHBYTES('SHA2_256','123'), 'kh9@gmail.com', '0908', N'Nữ', N'Huế'),
(N'Ngô Khang', 'kh10', HASHBYTES('SHA2_256','123'), 'kh10@gmail.com', '0909', N'Nam', N'Quảng Nam'),
(N'Phan Vy', 'kh11', HASHBYTES('SHA2_256','123'), 'kh11@gmail.com', '0910', N'Nữ', N'HCM');


INSERT INTO LOAISP VALUES
(N'Nhỏ', 20, 30),
(N'Trung bình', 40, 60),
(N'Lớn', 60, 100);

INSERT INTO SANPHAM (MaLoaiSP, TenSP, AnhSP, DonGia, GiaNhap, SoLuongTon)
VALUES
(1, N'Gấu Teddy Mini', 'nho1.jpg', 89000, 55000, 60),
(1, N'Gấu Brown Nhỏ', 'nho2.jpg', 99000, 65000, 55),
(1, N'Gấu Mini Để Bàn', 'nho3.jpg', 79000, 50000, 80),
(1, N'Gấu Emoji', 'nho4.jpg', 85000, 52000, 70),
(1, N'Gấu Móc Khoá', 'nho5.jpg', 59000, 35000, 120),
(1, N'Gấu Panda Mini', 'nho6.jpg', 95000, 60000, 50),
(1, N'Gấu Heo Nhỏ', 'nho7.jpg', 88000, 54000, 65),
(1, N'Gấu Thỏ Mini', 'nho8.jpg', 92000, 58000, 60),
(1, N'Gấu Noel Mini', 'nho9.jpg', 105000, 70000, 40),
(1, N'Gấu Valentine Mini', 'nho10.jpg', 110000, 75000, 35),
(1, N'Gấu Capybara Mini', 'nho11.jpg', 115000, 78000, 30),
(1, N'Gấu Chó Shiba Mini', 'nho12.jpg', 108000, 72000, 35),
(1, N'Gấu Mèo Mini', 'nho13.jpg', 98000, 65000, 45),
(1, N'Gấu Cáo Mini', 'nho14.jpg', 102000, 68000, 40),
(1, N'Gấu Khủng Long Mini', 'nho15.jpg', 120000, 80000, 25),
(1, N'Gấu Unicorn Mini', 'nho16.jpg', 125000, 85000, 22),
(1, N'Gấu Mặt Cười Mini', 'nho17.jpg', 90000, 58000, 50),
(2, N'Gấu Teddy Trung', 'tb1.jpg', 180000, 120000, 40),
(2, N'Gấu Brown Trung', 'tb2.jpg', 195000, 130000, 38),
(2, N'Gấu Panda Trung', 'tb3.jpg', 210000, 145000, 35),
(2, N'Gấu Stitch', 'tb4.jpg', 250000, 180000, 30),
(2, N'Gấu Pooh', 'tb5.jpg', 270000, 190000, 28),
(2, N'Gấu Doraemon', 'tb6.jpg', 260000, 185000, 32),
(2, N'Gấu Totoro', 'tb7.jpg', 290000, 210000, 25),
(2, N'Gấu Thỏ Hồng', 'tb8.jpg', 230000, 160000, 33),
(2, N'Gấu Heo Hồng', 'tb9.jpg', 240000, 170000, 30),
(2, N'Gấu Tình Yêu', 'tb10.jpg', 260000, 185000, 27),
(2, N'Gấu Capybara Trung', 'tb11.jpg', 280000, 200000, 24),
(2, N'Gấu Chó Shiba Trung', 'tb12.jpg', 295000, 215000, 22),
(2, N'Gấu Mèo Trung', 'tb13.jpg', 265000, 190000, 28),
(2, N'Gấu Unicorn Trung', 'tb14.jpg', 310000, 235000, 20),
(2, N'Gấu Cáo Trung', 'tb15.jpg', 275000, 205000, 23),
(2, N'Gấu Khủng Long Trung', 'tb16.jpg', 320000, 245000, 18),
(2, N'Gấu Hoạt Hình Trung', 'tb17.jpg', 290000, 210000, 21),
(3, N'Gấu Teddy Lớn', 'lon1.jpg', 380000, 270000, 20),
(3, N'Gấu Brown Lớn', 'lon2.jpg', 420000, 300000, 18),
(3, N'Gấu Panda Lớn', 'lon3.jpg', 450000, 330000, 15),
(3, N'Gấu Pooh Lớn', 'lon4.jpg', 480000, 350000, 14),
(3, N'Gấu Ôm Ngủ', 'lon5.jpg', 520000, 390000, 12),
(3, N'Gấu Body Pillow', 'lon6.jpg', 550000, 420000, 10),
(3, N'Gấu Khổng Lồ', 'lon7.jpg', 680000, 520000, 8),
(3, N'Gấu Noel Lớn', 'lon8.jpg', 600000, 460000, 9),
(3, N'Gấu Valentine Lớn', 'lon9.jpg', 590000, 450000, 11),
(3, N'Gấu Cao Cấp', 'lon10.jpg', 750000, 580000, 6),
(3, N'Gấu Capybara Lớn', 'lon11.jpg', 580000, 450000, 9),
(3, N'Gấu Chó Shiba Lớn', 'lon12.jpg', 620000, 480000, 8),
(3, N'Gấu Unicorn Lớn', 'lon13.jpg', 650000, 500000, 7),
(3, N'Gấu Khủng Long Lớn', 'lon14.jpg', 700000, 540000, 6),
(3, N'Gấu Hoạt Hình Lớn', 'lon15.jpg', 630000, 490000, 7),
(3, N'Gấu Siêu Mềm Cao Cấp', 'lon16.jpg', 820000, 650000, 5);
GO

CREATE OR ALTER TRIGGER trg_CheckTK_KhachHang
ON KHACHHANG
AFTER INSERT, UPDATE
AS
BEGIN
    IF EXISTS (
        SELECT 1
        FROM KHACHHANG kh
        JOIN inserted i ON kh.TenTK = i.TenTK
        WHERE kh.MaKH <> i.MaKH
    )
    BEGIN
        ROLLBACK TRANSACTION;
        RAISERROR (N'Tên tài khoản khách hàng đã tồn tại', 16, 1);
    END
END
GO

CREATE OR ALTER TRIGGER trg_Kho_SanPham
ON CHITIET_DONHANG
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    -- DELETE
    UPDATE sp
    SET sp.SoLuongTon = sp.SoLuongTon + d.SoLuong
    FROM SANPHAM sp
    JOIN deleted d ON sp.MaSP = d.MaSP
    LEFT JOIN inserted i ON d.SoHD = i.SoHD AND d.MaSP = i.MaSP
    WHERE i.MaSP IS NULL;

    -- INSERT
    UPDATE sp
    SET sp.SoLuongTon = sp.SoLuongTon - i.SoLuong
    FROM SANPHAM sp
    JOIN inserted i ON sp.MaSP = i.MaSP
    LEFT JOIN deleted d ON i.SoHD = d.SoHD AND i.MaSP = d.MaSP
    WHERE d.MaSP IS NULL;

    -- UPDATE (chênh lệch)
    UPDATE sp
    SET sp.SoLuongTon = sp.SoLuongTon + d.SoLuong - i.SoLuong
    FROM SANPHAM sp
    JOIN inserted i ON sp.MaSP = i.MaSP
    JOIN deleted d ON i.SoHD = d.SoHD AND i.MaSP = d.MaSP;

    IF EXISTS (SELECT 1 FROM SANPHAM WHERE SoLuongTon < 0)
    BEGIN
        ROLLBACK TRANSACTION;
        RAISERROR (N'Số lượng tồn kho không đủ', 16, 1);
    END
END
GO

CREATE OR ALTER TRIGGER trg_TongTien
ON CHITIET_DONHANG
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    UPDATE dh
    SET TongTien = ISNULL((
        SELECT SUM(ThanhTien)
        FROM CHITIET_DONHANG ct
        WHERE ct.SoHD = dh.SoHD
    ), 0)
    FROM DONHANG dh
    WHERE dh.SoHD IN (
        SELECT SoHD FROM inserted
        UNION
        SELECT SoHD FROM deleted
    );
END
GO

CREATE OR ALTER PROCEDURE sp_DangNhapNhanVien
    @TenTK NVARCHAR(50),
    @MatKhau NVARCHAR(100)
AS
BEGIN
    SELECT MaNV, HoTenNV, MaQuyen, LoaiNV
    FROM NHANVIEN
    WHERE TenTK = @TenTK
      AND MatKhau = HASHBYTES('SHA2_256', @MatKhau)
END
GO

CREATE OR ALTER PROCEDURE sp_DangKyKhachHang
    @HoTen NVARCHAR(100),
    @TenTK NVARCHAR(50),
    @MatKhau NVARCHAR(100),
    @Email NVARCHAR(100),
    @SDT NVARCHAR(20),
    @GioiTinh NVARCHAR(10),
    @DiaChi NVARCHAR(255)
AS
BEGIN
    IF EXISTS (SELECT 1 FROM KHACHHANG WHERE TenTK = @TenTK)
    BEGIN
        RAISERROR (N'Tên tài khoản đã tồn tại', 16, 1);
        RETURN;
    END

    INSERT INTO KHACHHANG
    (
        HoTen, TenTK, MatKhau, Email, SDT, GioiTinh, DiaChi
    )
    VALUES
    (
        @HoTen,
        @TenTK,
        HASHBYTES('SHA2_256', @MatKhau),
        @Email,
        @SDT,
        @GioiTinh,
        @DiaChi
    );
END
GO

CREATE OR ALTER PROCEDURE sp_DangNhapKhachHang
    @TenTK NVARCHAR(50),
    @MatKhau NVARCHAR(100)
AS
BEGIN
    SELECT MaKH, HoTen, Email, SDT
    FROM KHACHHANG
    WHERE TenTK = @TenTK
      AND MatKhau = HASHBYTES('SHA2_256', @MatKhau);
END
GO

CREATE OR ALTER PROCEDURE sp_ThemChiTietDonHang
    @SoHD INT,
    @MaSP INT,
    @SoLuong INT
AS
BEGIN
    DECLARE @DonGia DECIMAL(18,2);
    DECLARE @TonKho INT;

    SELECT 
        @DonGia = DonGia,
        @TonKho = SoLuongTon
    FROM SANPHAM
    WHERE MaSP = @MaSP;

    IF (@SoLuong > @TonKho)
    BEGIN
        RAISERROR (N'Không đủ hàng trong kho', 16, 1);
        RETURN;
    END

    IF EXISTS (
        SELECT 1 
        FROM CHITIET_DONHANG 
        WHERE SoHD = @SoHD AND MaSP = @MaSP
    )
    BEGIN
        UPDATE CHITIET_DONHANG
        SET SoLuong = SoLuong + @SoLuong
        WHERE SoHD = @SoHD AND MaSP = @MaSP;
    END
    ELSE
    BEGIN
        INSERT INTO CHITIET_DONHANG(SoHD, MaSP, SoLuong, DonGiaBan)
        VALUES (@SoHD, @MaSP, @SoLuong, @DonGia);
    END
END
GO


CREATE OR ALTER PROCEDURE sp_TaoDonHang
    @MaKH INT,
    @DiaChiGiao NVARCHAR(255),
    @HinhThucTT NVARCHAR(30)
AS
BEGIN
    BEGIN TRAN;

    INSERT INTO DONHANG(MaKH, DiaChiGiao, HinhThucTT)
    VALUES (@MaKH, @DiaChiGiao, @HinhThucTT);

    DECLARE @SoHD INT = SCOPE_IDENTITY();

    COMMIT TRAN;

    SELECT @SoHD AS SoHD;
END
GO


CREATE VIEW v_DanhSachSanPham
AS
SELECT 
    sp.MaSP,
    sp.TenSP,
    lsp.TenLoaiSP,
    lsp.KichThuoc,
    sp.DonGia,
    sp.SoLuongTon,
    sp.AnhSP
FROM SANPHAM sp
JOIN LOAISP lsp ON sp.MaLoaiSP = lsp.MaLoaiSP
GO

CREATE PROCEDURE sp_XemGioHang
    @MaKH INT
AS
BEGIN
    SELECT 
        gh.MaSP,
        sp.TenSP,
        gh.SoLuong,
        sp.DonGia,
        gh.SoLuong * sp.DonGia AS ThanhTien
    FROM GIOHANG gh
    JOIN SANPHAM sp ON gh.MaSP = sp.MaSP
    WHERE gh.MaKH = @MaKH;
END
GO

CREATE VIEW v_ChiTietDonHang
AS
SELECT 
    dh.SoHD,
    dh.NgayDatHang,
    kh.HoTen AS TenKhachHang,
    sp.TenSP,
    ct.SoLuong,
    ct.DonGiaBan,
    ct.ThanhTien,
    dh.TongTien
FROM DONHANG dh
JOIN KHACHHANG kh ON dh.MaKH = kh.MaKH
JOIN CHITIET_DONHANG ct ON dh.SoHD = ct.SoHD
JOIN SANPHAM sp ON ct.MaSP = sp.MaSP
GO
CREATE ROLE role_Admin;
CREATE ROLE role_NhanVien;
CREATE ROLE role_KhachHang;
GO

CREATE OR ALTER VIEW v_DoanhThuTheoNgay
AS
SELECT 
    ROW_NUMBER() OVER (ORDER BY CAST(NgayDatHang AS DATE)) AS ID, 
    CAST(NgayDatHang AS DATE) AS Ngay,
    SUM(TongTien) AS TongDoanhThu
FROM DONHANG
GROUP BY CAST(NgayDatHang AS DATE);
GO


-- Admin toàn quyền
GRANT CONTROL ON DATABASE::QuanLyBanHangGauBong TO role_Admin;

-- Nhân viên
GRANT SELECT, INSERT, UPDATE ON DONHANG TO role_NhanVien;
GRANT SELECT, INSERT ON CHITIET_DONHANG TO role_NhanVien;

-- Khách hàng
GRANT EXECUTE ON sp_TaoDonHang TO role_KhachHang;

