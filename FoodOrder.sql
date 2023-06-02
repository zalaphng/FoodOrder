USE master
DROP DATABASE FoodOrderDB
GO

CREATE DATABASE FoodOrderDB
GO
USE FoodOrderDB
GO

CREATE TABLE AdminLogins
(
    [adminid] NVARCHAR (3) NOT NULL,
    [Email] NVARCHAR (50) NOT NULL,
    [Password] NVARCHAR (100) NOT NULL,
    CONSTRAINT [PK_dbo.AdminLogins] PRIMARY KEY CLUSTERED ([adminid] ASC)
)

CREATE TABLE ContactModels
(
    [contactId] INT IDENTITY (1, 1) NOT NULL,
    [Name] NVARCHAR (MAX) NOT NULL,
    [Email] NVARCHAR (50) NOT NULL,
    [Subject] NVARCHAR (MAX) NOT NULL,
    [Message] NVARCHAR (MAX) NOT NULL,
    CONSTRAINT [PK_dbo.ContactModels] PRIMARY KEY CLUSTERED ([contactId] ASC)
);

CREATE TABLE [ProductTypes]
(
    [id] INT IDENTITY (1, 1) NOT NULL,
    [ProductTypeName] NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_dbo.ProductsTypes] PRIMARY KEY CLUSTERED ([id] ASC)
);

CREATE TABLE [Products]
(
    [id] INT IDENTITY (1, 1) NOT NULL,
    [ProductName] NVARCHAR (MAX) NULL,
    [ProductPrice] INT NOT NULL,
    [ProductPicture] NVARCHAR (MAX) NULL,
    [FKProductType] int not null,
    CONSTRAINT [PK_dbo.Products] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [FK_dbo.Products_dbo.ProductTypes] FOREIGN KEY ([FKProductType]) REFERENCES [ProductTypes] ([id])
);

CREATE TABLE [Users]
(
    [userid] NVARCHAR(6) NOT NULL,
    [Name] NVARCHAR (50) NOT NULL,
    [Email] NVARCHAR (50) NOT NULL,
    [Password] NVARCHAR (100) NOT NULL,
    [ConfirmPassword] NVARCHAR (50) NOT NULL,
    [Address] NVARCHAR (50) NOT NULL,
    [PhoneNumber] NVARCHAR (100) NOT NULL,
    CONSTRAINT [PK_dbo.Users] PRIMARY KEY CLUSTERED ([userid])
);


CREATE TABLE [InvoiceModels]
(
    [InvoiceID] NVARCHAR(6) NOT NULL,
    [DateInvoice] DATETIME NULL,
    [Total_Bill] REAL NOT NULL,
    [Status] TINYINT NOT NULL,
    [FKUserID] NVARCHAR(6) NULL,
    CONSTRAINT [PK_dbo.InvoiceModels] PRIMARY KEY ([InvoiceID]),
    CONSTRAINT [FK_dbo.InvoiceModels_dbo.Users_FKUserID] FOREIGN KEY ([FKUserID]) REFERENCES [Users] ([userid])
);
GO

CREATE TABLE [Orders]
(
    [Id] INT IDENTITY (1, 1) NOT NULL,
    [Qty] INT NOT NULL,
    [Unit_Price] INT NOT NULL,
    [Order_Bill] REAL NOT NULL,
    [Order_Date] DATETIME NULL,
    [FkProdId] INT NULL,
    [FkInvoiceID] NVARCHAR(6) NULL,
    CONSTRAINT [PK_dbo.Orders] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.Orders_dbo.InvoiceModels_FkInvoiceID] FOREIGN KEY ([FkInvoiceID]) REFERENCES [InvoiceModels] ([InvoiceID]),
    CONSTRAINT [FK_dbo.Orders_dbo.Products_FkProdId] FOREIGN KEY ([FkProdId]) REFERENCES [Products] ([id])
);
GO

-- Admin

INSERT INTO [AdminLogins]
    ([adminid], [Email], [Password])
VALUES
    ('A01', N'admin@gmail.com', '21232f297a57a5a743894a0e4a801fc3')

-- Liên hệ

SET IDENTITY_INSERT [ContactModels] ON
INSERT INTO [ContactModels]
    ([contactId], [Name], [Email], [Subject], [Message])
VALUES
    (1, N'Test', N'test.test@yahoo.com', N'Test', N'Where is Food Web')
SET IDENTITY_INSERT [ContactModels] OFF

-- Loại hàng hóa

SET IDENTITY_INSERT [ProductTypes] ON
INSERT INTO [ProductTypes]
    ([id], [ProductTypeName])
VALUES
    (1, N'Fruits')
INSERT INTO [ProductTypes]
    ([id], [ProductTypeName])
VALUES
    (2, N'Meats')
SET IDENTITY_INSERT [ProductTypes] OFF

-- Hàng hóa

SET IDENTITY_INSERT [Products] ON
INSERT INTO [Products]
    ([id], [ProductName], [ProductPrice], [ProductPicture], [FKProductType])
VALUES
    (2, N'Green Apple', 100, N'Images/product-home-1-img-6.jpg', 1)
INSERT INTO [Products]
    ([id], [ProductName], [ProductPrice], [ProductPicture], [FKProductType])
VALUES
    (4, N'Pomegranate', 300, N'Images/product-home-1-img-3.jpg', 1)
INSERT INTO [Products]
    ([id], [ProductName], [ProductPrice], [ProductPicture], [FKProductType])
VALUES
    (28, N'Fresh river fish', 25, N'Images/fish.jpg', 2)
INSERT INTO [Products]
    ([id], [ProductName], [ProductPrice], [ProductPicture], [FKProductType])
VALUES
    (5, N'Fresh river fish', 25, N'Images/fish.jpg', 2)
INSERT INTO [Products]
    ([id], [ProductName], [ProductPrice], [ProductPicture], [FKProductType])
VALUES
    (29, N'Cabbage vegetables', 50, N'Images/cabbage.jpg', 2)
INSERT INTO [Products]
    ([id], [ProductName], [ProductPrice], [ProductPicture], [FKProductType])
VALUES
    (30, N'Fresh red meat', 55, N'Images/redmeat.jpg', 2)
INSERT INTO [Products]
    ([id], [ProductName], [ProductPrice], [ProductPicture], [FKProductType])
VALUES
    (31, N'Fresh orange', 19, N'Images/orange.jpg', 1)
INSERT INTO [Products]
    ([id], [ProductName], [ProductPrice], [ProductPicture], [FKProductType])
VALUES
    (33, N'Ripe grapes', 39, N'Images/graps.jpg', 1)
INSERT INTO [Products]
    ([id], [ProductName], [ProductPrice], [ProductPicture], [FKProductType])
VALUES
    (34, N'Red Tomato', 10, N'Images/tomato.jpg', 1)
INSERT INTO [Products]
    ([id], [ProductName], [ProductPrice], [ProductPicture], [FKProductType])
VALUES
    (35, N'Cow fresh milk', 55, N'Images/milk.jpg', 2)
INSERT INTO [Products]
    ([id], [ProductName], [ProductPrice], [ProductPicture], [FKProductType])
VALUES
    (37, N'Fresh green vegetable', 20, N'Images/green.jpg', 1)
SET IDENTITY_INSERT [Products] OFF

-- Khách hàng
-- Mật khẩu: testuser

INSERT INTO [Users]
    ([userid], [Name], [Email], [Password], [ConfirmPassword], [Address], [PhoneNumber])
VALUES
    ('U00001', N'User 1', N'user@gmail.com', N'testuser', N'5d9c68c6c50ed3d02a2fcf54f63993b6', 'ABC', '0219481249')
INSERT INTO [Users]
    ([userid], [Name], [Email], [Password], [ConfirmPassword], [Address], [PhoneNumber])
VALUES
    ('U00002', N'User 2', N'user2@gmail.com', N'testuser', N'5d9c68c6c50ed3d02a2fcf54f63993b6', 'ABC', '0219481249')
INSERT INTO [Users]
    ([userid], [Name], [Email], [Password], [ConfirmPassword], [Address], [PhoneNumber])
VALUES
    ('U00003', N'User 3', N'user3@gmail.com', N'testuser', N'5d9c68c6c50ed3d02a2fcf54f63993b6', 'ABC', '0219481249')


-- Hóa Đơn

INSERT INTO [InvoiceModels]
    ([InvoiceID], [DateInvoice], [Total_Bill], [Status], [FKUserID])
VALUES
    ('I00001', N'2020-12-12 18:05:04', 100, 0, 'U00001')
INSERT INTO [InvoiceModels]
    ([InvoiceID], [DateInvoice], [Total_Bill], [Status], [FKUserID])
VALUES
    ('I00002', N'2020-12-29 14:59:38', 70, 1, 'U00001')
INSERT INTO [InvoiceModels]
    ([InvoiceID], [DateInvoice], [Total_Bill], [Status], [FKUserID])
VALUES
    ('I00003', N'2020-12-31 10:04:47', 110, 2, 'U00001')
INSERT INTO [InvoiceModels]
    ([InvoiceID], [DateInvoice], [Total_Bill], [Status], [FKUserID])
VALUES
    ('I00004', N'2020-12-31 19:15:53', 1635, 0, 'U00001')

-- Chi Tiết Hóa Đơn

SET IDENTITY_INSERT [Orders] ON
INSERT INTO [Orders]
    ([Id], [Qty], [Unit_Price], [Order_Bill], [Order_Date], [FkProdId], [FkInvoiceID])
VALUES
    (1, 4, 10, 40, N'2020-12-12 18:05:04', 2, 'I00001')
INSERT INTO [Orders]
    ([Id], [Qty], [Unit_Price], [Order_Bill], [Order_Date], [FkProdId], [FkInvoiceID])
VALUES
    (2, 2, 30, 60, N'2020-12-12 18:05:05', 4, 'I00001')
INSERT INTO [Orders]
    ([Id], [Qty], [Unit_Price], [Order_Bill], [Order_Date], [FkProdId], [FkInvoiceID])
VALUES
    (3, 2, 30, 60, N'2020-12-29 14:59:41', 4, 'I00002')
INSERT INTO [Orders]
    ([Id], [Qty], [Unit_Price], [Order_Bill], [Order_Date], [FkProdId], [FkInvoiceID])
VALUES
    (4, 1, 10, 10, N'2020-12-29 14:59:46', 2, 'I00002')
INSERT INTO [Orders]
    ([Id], [Qty], [Unit_Price], [Order_Bill], [Order_Date], [FkProdId], [FkInvoiceID])
VALUES
    (5, 2, 10, 20, N'2020-12-31 10:04:47', 2, 'I00003')
INSERT INTO [Orders]
    ([Id], [Qty], [Unit_Price], [Order_Bill], [Order_Date], [FkProdId], [FkInvoiceID])
VALUES
    (6, 3, 30, 90, N'2020-12-31 10:04:47', 4, 'I00003')
INSERT INTO [Orders]
    ([Id], [Qty], [Unit_Price], [Order_Bill], [Order_Date], [FkProdId], [FkInvoiceID])
VALUES
    (7, 3, 25, 75, N'2020-12-31 19:15:53', 28, 'I00004')
INSERT INTO [Orders]
    ([Id], [Qty], [Unit_Price], [Order_Bill], [Order_Date], [FkProdId], [FkInvoiceID])
VALUES
    (8, 2, 55, 110, N'2020-12-31 19:15:53', 30, 'I00004')
INSERT INTO [Orders]
    ([Id], [Qty], [Unit_Price], [Order_Bill], [Order_Date], [FkProdId], [FkInvoiceID])
VALUES
    (9, 5, 50, 250, N'2020-12-31 19:15:53', 29, 'I00004')
INSERT INTO [Orders]
    ([Id], [Qty], [Unit_Price], [Order_Bill], [Order_Date], [FkProdId], [FkInvoiceID])
VALUES
    (10, 4, 300, 1200, N'2020-12-31 19:15:54', 4, 'I00004')
SET IDENTITY_INSERT [Orders] OFF
