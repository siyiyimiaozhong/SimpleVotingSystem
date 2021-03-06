USE [Test]
GO
/****** Object:  Table [dbo].[option]    Script Date: 2020/9/25 0:13:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[option](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[vid] [int] NULL,
	[choice] [char](2) NULL,
	[msg] [nvarchar](25) NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[poll]    Script Date: 2020/9/25 0:13:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[poll](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[uid] [int] NULL,
	[vid] [int] NULL,
	[oid] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[userInfo]    Script Date: 2020/9/25 0:13:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[userInfo](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[username] [varchar](50) NULL,
	[password] [varchar](50) NULL,
	[name] [nvarchar](20) NULL,
	[sex] [nvarchar](5) NULL,
	[age] [int] NULL,
	[address] [nvarchar](32) NULL,
	[phone] [varchar](50) NULL,
	[email] [varchar](50) NULL,
	[type] [tinyint] NULL,
	[createTime] [datetime] NULL,
 CONSTRAINT [PK__userInfo__3213E83F3DC0F37A] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UQ__userInfo__F3DBC5721392A914] UNIQUE NONCLUSTERED 
(
	[username] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[votingTheme]    Script Date: 2020/9/25 0:13:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[votingTheme](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[title] [nvarchar](50) NULL,
	[type] [tinyint] NULL,
	[startTime] [datetime] NULL,
	[endTime] [datetime] NULL,
	[createTime] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[userInfo] ADD  CONSTRAINT [DF_userInfo_type]  DEFAULT ((0)) FOR [type]
GO
ALTER TABLE [dbo].[userInfo] ADD  CONSTRAINT [DF__userInfo__create__4316F928]  DEFAULT (getdate()) FOR [createTime]
GO
ALTER TABLE [dbo].[votingTheme] ADD  DEFAULT (getdate()) FOR [createTime]
GO
