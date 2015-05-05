USE [resim]
GO

/****** Object:  Table [dbo].[Rsm_Hst_Eq]    Script Date: 05/05/2015 23:36:04 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[Rsm_Hst_Eq](
	[IntCode] [int] IDENTITY(1,1) NOT NULL,
	[S] [tinyint] NULL,
	[O] [tinyint] NULL,
	[A] [tinyint] NULL,
	[R] [tinyint] NULL,
	[G] [tinyint] NULL,
	[B] [tinyint] NULL,
	[K] [int] NULL,
	[PX] [int] NULL,
	[NAME] [varchar](250) NULL,
	[SESSION] [varchar](50) NULL
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO
USE [resim]
GO

/****** Object:  Table [dbo].[Rsm_Src_Eq]    Script Date: 05/05/2015 23:36:19 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[Rsm_Src_Eq](
	[IntCode] [int] IDENTITY(1,1) NOT NULL,
	[S] [tinyint] NULL,
	[O] [tinyint] NULL,
	[A] [tinyint] NULL,
	[R] [tinyint] NULL,
	[G] [tinyint] NULL,
	[B] [tinyint] NULL,
	[K] [int] NULL,
	[PX] [int] NULL,
	[NAME] [varchar](250) NULL,
	[SESSION] [varchar](50) NULL
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO
USE [resim]
GO

/****** Object:  Table [dbo].[Rsm_Tbl_Eq]    Script Date: 05/05/2015 23:36:34 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[Rsm_Tbl_Eq](
	[IntCode] [int] IDENTITY(1,1) NOT NULL,
	[S] [tinyint] NULL,
	[O] [tinyint] NULL,
	[A] [tinyint] NULL,
	[R] [tinyint] NULL,
	[G] [tinyint] NULL,
	[B] [tinyint] NULL,
	[K] [int] NULL,
	[PX] [int] NULL,
	[NAME] [varchar](250) NULL,
	[SESSION] [varchar](50) NULL
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

USE [resim]
GO

/****** Object:  StoredProcedure [dbo].[Rsm_Sp_Eq]    Script Date: 05/05/2015 23:37:38 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE proc [dbo].[Rsm_Sp_Eq] as

select   NAME,COUNT(*) adet, AVG(K) K from Rsm_Tbl_Eq with(nolock) group by name having AVG(K) =0-- order by k desc-- px/(k+1),O,R,G,B,K
/* declare @session varchar(50);

set @session='0025090192819';
select  B.NAME,COUNT(*) ADET from Rsm_Tbl_Eq a with(nolock) cross join  Rsm_Tbl_Eq b with(nolock)
WHERE /*(a.S=b.S) 
and*/ (a.O =b.O or a.O between b.O-35 and b.O+35)
and (a.R =b.R or a.R between b.R-15 and b.R+15)
and (a.G =b.G or a.G between b.G-15 and b.G+15)
and (a.B =b.B or a.B between b.B-15 and b.B+15)
and (a.K =b.K or a.K between b.K-100 and b.K+100)
AND a.SESSION =@session
GROUP BY B.NAME
order by ADET DESC*/
GO

USE [resim]
GO

/****** Object:  StoredProcedure [dbo].[Rsm_Sp_EqRslt]    Script Date: 05/05/2015 23:37:51 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE proc [dbo].[Rsm_Sp_EqRslt] (@session varchar(50)) as
--SELECT * FROM Rsm_Src_Eq
/*

Rsm_Sp_EqRslt '0496723601459';

*/

select  B.NAME,COUNT(*) ADET from Rsm_Src_Eq a with(nolock) cross join  Rsm_Tbl_Eq b with(nolock)
WHERE /*(a.S=b.S or ) 
and*/ (a.O =b.O or a.O between b.O-30 and b.O+25)
and (a.R =b.R or a.R between b.R-10 and b.R+10)
and (a.G =b.G or a.G between b.G-10 and b.G+10)
and (a.B =b.B or a.B between b.B-10 and b.B+10)
--and (a.K =b.K or a.K between b.K-200 and b.K+200)
AND a.SESSION =@session
GROUP BY B.NAME
HAVING COUNT(*)>1
order by ADET DESC
GO



