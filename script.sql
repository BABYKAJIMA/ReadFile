USE [Data_db]
GO
/****** Object:  Table [dbo].[__EFMigrationsHistory]    Script Date: 6/21/2025 7:56:51 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[__EFMigrationsHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DeviceRecords]    Script Date: 6/21/2025 7:56:51 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DeviceRecords](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[STT] [nvarchar](max) NOT NULL,
	[LoaiKetNoi] [nvarchar](max) NOT NULL,
	[TenThietBi] [nvarchar](max) NOT NULL,
	[IP] [nvarchar](max) NOT NULL,
	[ChannelZone] [nvarchar](max) NOT NULL,
	[VLAN] [nvarchar](max) NOT NULL,
	[Port] [nvarchar](max) NOT NULL,
	[TuRack] [nvarchar](max) NOT NULL,
	[ODFPatchPannel] [nvarchar](max) NOT NULL,
	[SoSoiPort] [nvarchar](max) NOT NULL,
	[TenNhanDan] [nvarchar](max) NOT NULL,
	[ODF1] [nvarchar](max) NOT NULL,
	[SoSoi1] [nvarchar](max) NOT NULL,
	[TenNhanDan2] [nvarchar](max) NOT NULL,
	[ODF2] [nvarchar](max) NOT NULL,
	[SoSoi2] [nvarchar](max) NOT NULL,
	[DiemDauCuoi] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_DeviceRecords] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 6/21/2025 7:56:51 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Username] [nvarchar](max) NOT NULL,
	[Password] [nvarchar](max) NOT NULL,
	[Role] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
Insert into Users (Username, Password, Role) 
values ('admin@hoaphat.com.vn', '123456', 'Admin'),
		('User@hoaphat.com.vn', '123456', 'User'),
		('admin2@hoaphat.com.vn', '123456', 'Admin);