USE [master]
GO
/****** Object:  Database [FEHRISTDB]    Script Date: 6/5/2023 10:06:37 PM ******/
CREATE DATABASE [FEHRISTDB]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'FEHRISTDB', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.SQLEXPRESS\MSSQL\DATA\FEHRISTDB.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'FEHRISTDB_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.SQLEXPRESS\MSSQL\DATA\FEHRISTDB_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [FEHRISTDB] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [FEHRISTDB].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [FEHRISTDB] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [FEHRISTDB] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [FEHRISTDB] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [FEHRISTDB] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [FEHRISTDB] SET ARITHABORT OFF 
GO
ALTER DATABASE [FEHRISTDB] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [FEHRISTDB] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [FEHRISTDB] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [FEHRISTDB] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [FEHRISTDB] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [FEHRISTDB] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [FEHRISTDB] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [FEHRISTDB] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [FEHRISTDB] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [FEHRISTDB] SET  ENABLE_BROKER 
GO
ALTER DATABASE [FEHRISTDB] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [FEHRISTDB] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [FEHRISTDB] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [FEHRISTDB] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [FEHRISTDB] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [FEHRISTDB] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [FEHRISTDB] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [FEHRISTDB] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [FEHRISTDB] SET  MULTI_USER 
GO
ALTER DATABASE [FEHRISTDB] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [FEHRISTDB] SET DB_CHAINING OFF 
GO
ALTER DATABASE [FEHRISTDB] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [FEHRISTDB] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [FEHRISTDB] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [FEHRISTDB] SET QUERY_STORE = OFF
GO
USE [FEHRISTDB]
GO
/****** Object:  Table [dbo].[__MigrationHistory]    Script Date: 6/5/2023 10:06:42 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[__MigrationHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ContextKey] [nvarchar](300) NOT NULL,
	[Model] [varbinary](max) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK_dbo.__MigrationHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC,
	[ContextKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ACCOUNTS]    Script Date: 6/5/2023 10:06:42 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ACCOUNTS](
	[ACCOUNTID] [int] IDENTITY(1,1) NOT NULL,
	[ROLEID] [int] NULL,
	[NAME] [varchar](50) NOT NULL,
	[EMAIL] [varchar](50) NOT NULL,
	[PASS] [text] NOT NULL,
	[AC_STATUS] [varchar](50) NULL,
	[CREATED_BY] [varchar](50) NULL,
	[DATE_CREATED] [datetime] NULL,
	[UPDATE_BY] [varchar](50) NULL,
	[DATE_UPDATED] [datetime] NULL,
 CONSTRAINT [PK_dbo.ACCOUNTS] PRIMARY KEY CLUSTERED 
(
	[ACCOUNTID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CHECKLIST]    Script Date: 6/5/2023 10:06:42 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CHECKLIST](
	[CHECKID] [int] IDENTITY(1,1) NOT NULL,
	[CL_DESCRIPTION] [text] NULL,
	[TASKID] [int] NULL,
	[CREATED_BY] [varchar](50) NULL,
	[DATE_CREATED] [datetime] NULL,
	[UPDATE_BY] [varchar](50) NULL,
	[DATE_UPDATED] [datetime] NULL,
 CONSTRAINT [PK_dbo.CHECKLIST] PRIMARY KEY CLUSTERED 
(
	[CHECKID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ROLES]    Script Date: 6/5/2023 10:06:42 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ROLES](
	[ROLEID] [int] IDENTITY(1,1) NOT NULL,
	[NAME] [varchar](50) NOT NULL,
	[CREATED_BY] [varchar](50) NULL,
	[DATE_CREATED] [datetime] NULL,
	[UPDATE_BY] [varchar](50) NULL,
	[DATE_UPDATED] [datetime] NULL,
 CONSTRAINT [PK_dbo.ROLES] PRIMARY KEY CLUSTERED 
(
	[ROLEID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TASK_IMAGES]    Script Date: 6/5/2023 10:06:42 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TASK_IMAGES](
	[IMAGEID] [int] IDENTITY(1,1) NOT NULL,
	[TI_PATH] [text] NULL,
	[TASKID] [int] NULL,
	[CREATED_BY] [varchar](50) NULL,
	[DATE_CREATED] [datetime] NULL,
	[UPDATE_BY] [varchar](50) NULL,
	[DATE_UPDATED] [datetime] NULL,
 CONSTRAINT [PK_dbo.TASK_IMAGES] PRIMARY KEY CLUSTERED 
(
	[IMAGEID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TASKS]    Script Date: 6/5/2023 10:06:42 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TASKS](
	[TASKID] [int] IDENTITY(1,1) NOT NULL,
	[ACCOUNTID] [int] NULL,
	[T_TITLE] [text] NULL,
	[T_DESC] [text] NULL,
	[T_STATUS] [varchar](50) NULL,
	[T_COLOR] [varchar](50) NULL,
	[T_DUE_DATE_TIME] [varchar](50) NULL,
	[T_ADDED_DATE_TIME] [varchar](50) NULL,
	[CREATED_BY] [varchar](50) NULL,
	[DATE_CREATED] [datetime] NULL,
	[UPDATE_BY] [varchar](50) NULL,
	[DATE_UPDATED] [datetime] NULL,
 CONSTRAINT [PK_dbo.TASKS] PRIMARY KEY CLUSTERED 
(
	[TASKID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
INSERT [dbo].[__MigrationHistory] ([MigrationId], [ContextKey], [Model], [ProductVersion]) VALUES (N'202306011638344_create-database', N'fehrist.Migrations.Configuration', 0x1F8B0800000000000400ED1CD96EEBB8F5BD40FF41D06391B1925C14E804F60C7C6567AE719338889441FB643032ED08D5E24A7490A0E897F5A19FD45F28B5515CB5DA8A3B63DC971B9267E1E1D9489DE3FFFEFB3FE39FDF7D4F7B8351EC86C144BF1A5DEA1A0C9C70ED06DB89BE479B1FFEA2FFFCD31FFF309EAFFD77EDD762DD97641D860CE289FE8AD0EEC63062E715FA201EF9AE138571B8412327F40DB00E8DEBCBCB1F8DAB2B0362143AC6A569E3A77D805C1FA67FE03FCD3070E00EED81771FAEA117E7E378C64AB16A0FC087F10E3870A26FE06BE4C66894ADD4B5A9E702CC8505BD8DAE812008114098C79BE7185A280A83ADB5C303C0B33F7610AFDB002F8639EF37E5F2A6DBB8BC4EB6619480052A671FA3D06F89F0EA4B2E178307EF245D9DC80D4B6E8E258C3E925DA7D29BE853D35C3E3FD8BAC6D3BA31BD2859C7CB7694435C68F9F8055100AC27C9BF0BCDDC7B681FC14900F72802DE85F6B87FF15CE73BFCB0C3BFC36012EC3D8F660B3386E798013CF418853B18A18F27B861995DCC74CD60C10D1E9E408BA0D9AE1601FA72AD6B0F9815F0E241A20394042C1446F01718C00820B87E0408C1081FE1620D53290A4C70249F967773815E35C8C3F47E5E00603DC5E6A66BF7E0FD0E065BF43AD1FF8CEDEBD67D87EB6220E7F93970B171967B1036554D757E3F5DDC0D4FF6716A591554F17F8F43F6DBF2E11042AEA6323557963DB59FAB7678184AE6D37C6ACF67ABAF7F3B3AA91926B4CAE915C466D8366CECB5EB609F1F53E8A1B8CCC8D573F900DEDC6D6AEE12EBD5B527E8A593F1ABBBCBC2C9289958E5EE049FEE6D14FA4FA197439089950DA22D44987E289BB5C27DE4B4E0C69E5ADF2D293BC9CC8A78F1921B7A5C60869994F13236CA4851193F3231350E1EC9F2CF881C852F6E1B36A43EFC8831E37302C0D9851CC585944EA2BB1B298C53EE460ABBEE64BA89136861BAC9F2CF30DD846E17D32DE086325D5586590D857DF3C2BEABB2F9C679501DA1D9DC3287A03350DE63AFCCE5DDF269003AB3E7F92AF508F6E220DEB98EDE7436C3CE78388A67F77F4CF7DF3D6BE37DBF34A56BCA8DF96D6E7EBF5B58762C65884CAFB2B050B2C4CE08A92437DD25B15D2DEEA7BFCC2BD2DB6C5EE08C9F93A7B9F4825EA92ED9698BA049603E2372A6C4BB844E023854EC34EFD2E8F4B478B417CB870182942C37383BC94F7092994DF77148BC9754F8ABCE2932F14EAD32E51CEA33CC3EA5DCC5EC09E050666F2F568F53FBDBD9DECFF6DE2ED04B33235926D0C4EAA7711C3A6ECA0FF5EA53DECDD99DCD83B5567951CF4EA1B8E2E383C066EEEEB06163D213FD7234BA12A4A5C249F2991227C91959B47F1270627F00A3C40C81676241630FE30648741E6EE0B83BE0556D89036AF1BE96C89C90E06766700783C451546DBD1F6D4282738475B2191B944E54AB0A93C8AB4E559ED597879AE97BDD892AF0355712A9EE75D21319034D8E4AF901AF95A6C8C4D99BFA00BAC2A534AAD355E537E5F9523791E64AA3BAC755ABE1E174464EBFC9B9C95F005BA98C5CA6FD880FE55DE8A058E911A411923D5E92CCB67436D25BF6509AA3E26020DD51C9F6C4B4274B6E300CC210302AB2B6AFE9B80B63C905E63986F91D269EE828DA4341BF12941644C2078D3293E2438EA0472C8A2458CBE0B35CA90638FF0A2A0067EA57034C3F800918288FDA80076245524EC82C87893A37561ED467206A8DF43B11AF4675D928E19E485ED0C4BAE4934251F2C95F2DD9BD35D837FB002A6E5B9D57D5675614C7B9C6546C5A9A4B1D67CFFC9386B8EBAA0CA1498E40F14DAB7BC5F61559419D08BB9E3873C1539CBA32DE358B781CEB0A6B6C16E27A88A1B86D12874CE6C64656BF980F8C0D45A1E3F81EEC76F82E4F153EE6239A95553D9A3F58ED4B02FD0C87E1C492CA40C22DA184C2086C21378B49634E6FDD28463380C00B489E0FCCB52F2CA3C28FC2AD1684B808231E56E1690B80E4FF1990BC4051129F73D05BBC233F89EFE9F39568EB22A496D49C020F44EA6FBE66E8EDFDA0F6A2A1C6555C636944AAABAD1A4B564042E3C8469A63C8AB016914F950731C59691F8D221B698121ABD263506443CD71503578ECE990E1E6B8E8E7401A193DDE1C1BFBE247E363679A63A4DE016974D4704BEEC84B9FC01D9911318E0DCEC484B457B063CE9FF27EA191D7C8529B1E2E23CD3EDBFB0B39D8A91BF859954F5895B334A3872ACBD2A606AA2C075389B5B8D6D202555D75D5580E193D491D15C35231D8064F5626C5A2C9C6DA6091051EBB43DC21554E2CA27CB0D5BED82A266E83EC641BBC42B5128B59983E3BAADF84A3A2EE973DBC55F9EED2DE6555C02A35AA286C61D4495E26538987AB5761D071732D6CE9205EF56C35276C35F48B44CF20AF40D330D62BA155A226C521B49415A526557848CD07A3E5C5E0D9587E77C6223C94F14B0875F260C63D8C8DF347AAFA3661E1D52A5BA26B58546FEE3A79B1B23E6204FD51B26064FDC3333D17EFB75C700F02770363949568E9D79757D75CB7F1E974FE1A71BCF6248F7C92F65FF6B8066FC6751311D71693B5EC6A627BB75212C217B045B086EF13FD9F29C88DB6F8EB2A83BAD096113EED1BED52FB174D38FD48D6A287EB0D44CE2B888472AC3E1DBA07C249B7DF22F88EFA35D2D633D5447442D7EC61D08AB57C87C12B2BDC5B63E54569495C3B5C4221DF0159E4AAF69AB1D8AE05B48303394C4FE6515CC7114CF8AC83C7D4414971CC80CD8547D14179906C10C108609F20C6F525F201A2190ABAE3B01B86634403AE71F05048A55D828742AE68093C87C7D3774D8A67AA615BB88EE2A1E45D5B9D0C5D74A70D1C5D06D5C7CB9D35FFD84159FACE346C1FD351749F6B5D3A2BFDEF54E9FB340FF56D1622E59383F6070DD50FA42EE3FA7F6C016A715635A79ED54EB6EE1EEA74DE6DCEE070BD3DBFA5669E46A75573E254717AB726A0CE5D1783B7E79C5A5345CF969CBE27CF1430776DE539F9D3AFFE9A79824D3562F1337F80D2BE99AAB699EC4314CE465E427CD25916A26C3D9035D5287B6A6498A5BD20D2561765B78D0CADBC465ED98853DD8723237094361DD54E3EAF8F47E8DBA9EC5EE0B48BA9643F99469D164C4ABC285FA87852BD388D58540574693DD34936DBB4DBA2C4FEEA5A720ED34C23D60260F74FFDB0388E3EB1BB2D51243F331E408771FC64CD22D88445FCE1382A967037CC7B8800BE51826984DC0D70109E76601CA73FC9F12BF0F678C9DC7F81EB45B0DCA3DD1EE12D43FFC5637E2A25896355F4D38E2196E7F17297FE9EC621B680D974934BF132F8BA77BD35E1FB567C1751A1480264FED8929C254A1E5DB61F04D343183444948B8FC4751BFA3B0F238B978105DE6017DE9E637807B7C0F9284A3AD448EA0F8215FB78E6826D04FC38C751C2E33FB10EAFFDF79FFE07CF26ADC05F5F0000, N'6.1.3-40302')
INSERT [dbo].[__MigrationHistory] ([MigrationId], [ContextKey], [Model], [ProductVersion]) VALUES (N'202306011641287_remove-phone-column-from-accounts', N'fehrist.Migrations.Configuration', 0x1F8B0800000000000400ED1CD96EEBB8F5BD40FF41D06391B1925C14E804F60C7C6567C6B8491C44CA60E6C96064DA11AAC595E82041D12FEB433FA9BF506AA3B86AB51577C6B82F3724CFC2C3B3913AC7FFFDF77FC63FBEFB9EF606A3D80D83897E35BAD4351838E1DA0DB6137D8F36DFFD4DFFF1873FFF693C5FFBEFDA2FC5BA2FC93A0C19C413FD15A1DD8D61C4CE2BF4413CF25D270AE37083464EE81B601D1AD79797DF1B575706C428748C4BD3C64FFB00B93E4CFFC07F9A61E0C01DDA03EF3E5C432FCEC7F18C9562D51E800FE31D70E044DFC0D7C88DD1285BA96B53CF05980B0B7A1B5D0341102280308F37CF31B45014065B6B870780677FEC205EB7015E0C73DE6FCAE54DB771799D6CC328010B54CE3E46A1DF12E1D5975C2E060FDE49BA3A911B96DC1C4B187D24BB4EA537D1A7A6B97C7EB0758DA775637A51B28E97ED2887B8D0F2F10BA200584F927F179AB9F7D03E829300EE5104BC0BED71FFE2B9CE37F861877F87C124D87B1ECD16660CCF310378E8310A7730421F4F70C332BB98E99AC1821B3C3C811641B35D2D02F4E55AD71E302BE0C5834407280958288CE04F3080114070FD081082113EC2C51AA6521498E0483E2DEFE602BD6A9087E9FDBC00C07A8ACD4DD7EEC1FB1D0CB6E875A2FF15DBD7ADFB0ED7C540CEF373E062E32CF7206CAA9AEAFC7EBAB81B9EECE3D4B22AA8E2FF1E85ECD45C59F6D47EAEA2DD74C7D594CCA7F9D49ECF565F7F3B3AA91926B4CAE915C466586B6DEC4FEB609F1F53E8A1B8CCC8D573F900DEDC6D6A8812BBD2B527E8A593F1ABBBCB1CFD289958E5868E4FF7360AFDA7D0CB21C8C4CA06D116224C3F94CD5AE13E725A70634FAD6F96949D646645FC6BC90D3D2E30C34CCA78191BA50FAFF4EC99981ABBF564F967F8F4C24BB675E852EF7A446FFE39AEF9EC428EE2424A27D1DD8D14C6297723855D7732DDC409B430DD64F967986E42B78BE916704399AE2AF7AB86C2BE7961DF55D97CE30CA58ED06C6E9943D01928EFB157E6F26EF934009DD9F37C957A047B7110EF5C476F3A9B61673C1CC5B3FB3FA6FBEF9EB5F1BE5F9AD235E5C6FC796E7EBB5B58762C65884CAFB2B050B2C4CE08A92437DD25B15D2DEEA73FCD2BD2DB6C5EE08C9F93A7B9F4825EA92ED9698BA049603E2372A6C4BB844E023854EC34EFD2E8F4B478B417CB870182942C37383BC94F7092994DF77148BC9754F8ABCE2932F14EAD32E51CEA33CC3EA5DCC5EC09E050666F2F568F53FBE7B3BD9FEDBD5DA0976646B24CA089D54FE33874DC941FEAD5A7BC9BB33B9B076BADF2A29E9D4271C5C70781CDDCDD61C3C6A427FAE5687425484B8593E433254E9233B268FF22E0C4FE0046891902CFC482C61EC60D90E83CDCC07177C0ABDA1207D4E27D2D913921C1CFCCE00E0689A3A8DA7A3FDA8404E708EB643336289DA856152691579DAA3CAB2F0F35D3F7BA1355E06BAE2452DDEBA42732069A1C95F2D35A2B4D9189B337F50174854B6954A7ABCA6FCAF3A56E22CD9546758FAB56C3C3E98C9C7E937393BF00B65219B94CFB111FCABBD041B1D2234823247BBC24996DE96CA4B7ECA13447C5C140BAA392ED89694F96DC601884216054646D5FD37117C6920BCC730CF33B4C3CD151B487827E25282D88840F1A6526C5871C418F581449B096C167B9520D70FE155400CED4AF06987E001330501EB5010FC48AA49C90590E13756EAC3CA8CF40D41AE977225E8DEAB251C23D91BCA08975C92785A2E493BF5AB27B6BB06FF60154DCB63AAFAACFAC288E738DA9D8B434973ACE9EF9270D71D7551942931C81E29B56F78AED2BB2823A11763D71E682A7387565BC6B16F138D615D6D82CC4F5104371DB240E99CC8D8DACB2301F181B8A12C4F13DD8EDF05D9E2A49CC47342BAB4734BFB3DA17EBF9190EC38925357B845B42098511D8426E1693C69CDEBA518C66008117903C1F986B5F5846851F855B2D087111463CACC2D31600C9FF332079E9A0243EE7A0B778477E12DFD3E72BD1D645482DA906051E88D4DF7CCDD0DBFB41ED45438DABB8C6D28854575B3596AC8084C6918D34C790D7E9D128F2A1E638B2A23B1A4536D21C03553FC74A960C37C7453FE5D1C8E8F1E6D8D8D73A1A1F3BD31C23F58647A3A3865B72475EE904EEC88C88716C70E621A4AC820D72BE90B7E946169FA5253DCC3DCD1CDBDBBA1CECD48DF3ACCA27ACCA598AD0439565294F03559683A9C45A5C496981AAAEA96A2C878C7CA4068A61A9186C83272B7162D164636DB0C8028FDD21EE900A2516513ED86A5F6C0512B74176B20D5EA1D288C52C4C9F1DD5EFC2515177C31EDEAA7C3369EFB22A60951A5514A530EA242F71A9C4C3D59A30E8B8B916B67410AF7AB69A13B61AFA35A1679057A06918EB95D02A5193C20E5ACA8A32912A3CA45E83D1F262F06C2C7F3863111EB9F825843A79ECE21EB5C6F903537DF3ADF0E2942DD1352CAA37779DBC36591F3182FE285930B2FEE1999E8BF75B2EB80781BB8131CACAABF4EBCBAB6BAE87F774FA698D385E7B92073A49532D7B5C83B7B8BA89886B0BC15A7624B17D572909E1EBD52258C3F789FECF14E4465BFCBACAA02EB465844FFB46BBD4FE45134E3F70B5E8BF7A0391F30A22A194AA4FDFEB8170D24DAD08BEA3FEEDA9F58C35119F58347718BCB20AB935D63494D69EB5C32554CC1D9045AE3CAE198BED7A2D3B58FB619A1F8F62E747B0B7B30E1E53072555280376F11D4507E511AD41B821807D220ED700C87BF36628E8D6BE6E188E110DB80EBD432195B6E31D0AB9A2F7EE1C1E4FDF3529DE9486ED953A8A8792B747753274D19D36707419541F2F77D6FC630765E9A3D0B00D4347D17DAE47E8ACF47F50A5EFD3A5D3B72B87D4290EDA883354E38DBA5EEAFFB1D7A6C559D59C7A56A4D8BA4DA7D379B73983C335D1FC9EBA661A9D56CD895355E0DDBA6D3AB7370CDE07736ADD0B3D7B5FFA9E3C5329DCB567E6E44FBFFAD3E30976AF8855C6FC014A1B54AAFA53B2AF46381B7909F149675988B2C65FD6BDA26C5E916196365D487B4A946D2D32B4F2627465C74B75C38B8CC051FA61543BF9BC8619A141A6B24D80D32EA664FC643A625A3029F1A27C55E14935BD34625115D0A5C54727D9D5D26E8B12FBABEB7D394CD78AF8E11EBB7FEAB7B571F489DD6D8922F9A5ED003A8CE3276B16C1262CE20FC751B184BB61DE4304F08D124C23E46E8083F0B403E338FDED8B5F80B7C74BE6FE0B5C2F82E51EEDF6086F19FA2F1EF39B24491CABA29FB6E6B03C8F97BBF4872B0EB105CCA69B5C8A97C1D7BDEBAD09DFB7E2BB880A451220F3C796E42C51F2E8B2FD20981EC2A021A25C7C24AEDBD0DF791859BC0C2CF006BBF0F61CC33BB805CE47517FA146527F10ACD8C733176C23E0C7398E121EFF897578EDBFFFF03FF496D39E625E0000, N'6.1.3-40302')
SET IDENTITY_INSERT [dbo].[ACCOUNTS] ON 

INSERT [dbo].[ACCOUNTS] ([ACCOUNTID], [ROLEID], [NAME], [EMAIL], [PASS], [AC_STATUS], [CREATED_BY], [DATE_CREATED], [UPDATE_BY], [DATE_UPDATED]) VALUES (1, 2, N'Hafsa', N'hafsanusrat5@gmail.com', N'AH5KX4jkJ+FdxY9ApQ65SCfV0n8mJOxQ5N6bwzcE0r4o9vIO9Oco+vZp4gjJ7kEOAg==', N'ACTIVE', NULL, NULL, NULL, NULL)
INSERT [dbo].[ACCOUNTS] ([ACCOUNTID], [ROLEID], [NAME], [EMAIL], [PASS], [AC_STATUS], [CREATED_BY], [DATE_CREATED], [UPDATE_BY], [DATE_UPDATED]) VALUES (2, 2, N'Noor Ahmed', N'noor.ahmed03@hotmail.com', N'AFk1XtD69nH+I4SWSdASWfzyvHerWROKn9Oh5N/7qxc2SPzUS6uFvfY53j6Gj9C3bQ==', N'ACTIVE', NULL, NULL, NULL, NULL)
INSERT [dbo].[ACCOUNTS] ([ACCOUNTID], [ROLEID], [NAME], [EMAIL], [PASS], [AC_STATUS], [CREATED_BY], [DATE_CREATED], [UPDATE_BY], [DATE_UPDATED]) VALUES (3, 2, N'TEST', N'test@d.com', N'AFRhkicFDuFBEzR3FMsJPqtsnzaCwV3rVn0vzvhDvH7R25O0UcRzrXJIy+l8N7Lyew==', N'ACTIVE', NULL, NULL, NULL, NULL)
INSERT [dbo].[ACCOUNTS] ([ACCOUNTID], [ROLEID], [NAME], [EMAIL], [PASS], [AC_STATUS], [CREATED_BY], [DATE_CREATED], [UPDATE_BY], [DATE_UPDATED]) VALUES (7, 2, N'TestName', N'test21@gmail.com', N'AGyLGcv8NAzTUd9vfeuWTOlCtGwr0W35sZdntiBUrvR0vfLNlrwBRv1+6B6k99rBUQ==', N'ACTIVE', NULL, NULL, NULL, NULL)
SET IDENTITY_INSERT [dbo].[ACCOUNTS] OFF
SET IDENTITY_INSERT [dbo].[CHECKLIST] ON 

INSERT [dbo].[CHECKLIST] ([CHECKID], [CL_DESCRIPTION], [TASKID], [CREATED_BY], [DATE_CREATED], [UPDATE_BY], [DATE_UPDATED]) VALUES (4, N'123', 4, N'1', CAST(N'2023-06-01T21:57:33.473' AS DateTime), NULL, NULL)
INSERT [dbo].[CHECKLIST] ([CHECKID], [CL_DESCRIPTION], [TASKID], [CREATED_BY], [DATE_CREATED], [UPDATE_BY], [DATE_UPDATED]) VALUES (6, N'', 6, N'2', CAST(N'2023-06-05T21:58:23.933' AS DateTime), NULL, NULL)
INSERT [dbo].[CHECKLIST] ([CHECKID], [CL_DESCRIPTION], [TASKID], [CREATED_BY], [DATE_CREATED], [UPDATE_BY], [DATE_UPDATED]) VALUES (8, N'', 8, N'2', CAST(N'2023-06-05T21:58:47.813' AS DateTime), NULL, NULL)
INSERT [dbo].[CHECKLIST] ([CHECKID], [CL_DESCRIPTION], [TASKID], [CREATED_BY], [DATE_CREATED], [UPDATE_BY], [DATE_UPDATED]) VALUES (10, N'12', 10, N'2', CAST(N'2023-06-05T21:59:07.687' AS DateTime), NULL, NULL)
INSERT [dbo].[CHECKLIST] ([CHECKID], [CL_DESCRIPTION], [TASKID], [CREATED_BY], [DATE_CREATED], [UPDATE_BY], [DATE_UPDATED]) VALUES (11, N'23', 10, N'2', CAST(N'2023-06-05T21:59:07.703' AS DateTime), NULL, NULL)
INSERT [dbo].[CHECKLIST] ([CHECKID], [CL_DESCRIPTION], [TASKID], [CREATED_BY], [DATE_CREATED], [UPDATE_BY], [DATE_UPDATED]) VALUES (12, N'', 11, N'2', CAST(N'2023-06-05T21:59:16.953' AS DateTime), NULL, NULL)
INSERT [dbo].[CHECKLIST] ([CHECKID], [CL_DESCRIPTION], [TASKID], [CREATED_BY], [DATE_CREATED], [UPDATE_BY], [DATE_UPDATED]) VALUES (13, N'', 12, N'2', CAST(N'2023-06-05T21:59:28.870' AS DateTime), NULL, NULL)
INSERT [dbo].[CHECKLIST] ([CHECKID], [CL_DESCRIPTION], [TASKID], [CREATED_BY], [DATE_CREATED], [UPDATE_BY], [DATE_UPDATED]) VALUES (18, N'123123', 9, N'2', CAST(N'2023-06-05T22:01:13.600' AS DateTime), NULL, NULL)
SET IDENTITY_INSERT [dbo].[CHECKLIST] OFF
SET IDENTITY_INSERT [dbo].[ROLES] ON 

INSERT [dbo].[ROLES] ([ROLEID], [NAME], [CREATED_BY], [DATE_CREATED], [UPDATE_BY], [DATE_UPDATED]) VALUES (1, N'ADMIN', NULL, NULL, NULL, NULL)
INSERT [dbo].[ROLES] ([ROLEID], [NAME], [CREATED_BY], [DATE_CREATED], [UPDATE_BY], [DATE_UPDATED]) VALUES (2, N'USER', NULL, NULL, NULL, NULL)
SET IDENTITY_INSERT [dbo].[ROLES] OFF
SET IDENTITY_INSERT [dbo].[TASK_IMAGES] ON 

INSERT [dbo].[TASK_IMAGES] ([IMAGEID], [TI_PATH], [TASKID], [CREATED_BY], [DATE_CREATED], [UPDATE_BY], [DATE_UPDATED]) VALUES (2, N'/storage/images/1/2.jpeg', 4, NULL, NULL, NULL, NULL)
INSERT [dbo].[TASK_IMAGES] ([IMAGEID], [TI_PATH], [TASKID], [CREATED_BY], [DATE_CREATED], [UPDATE_BY], [DATE_UPDATED]) VALUES (3, N'/storage/images/2/1.jpeg', 11, NULL, NULL, NULL, NULL)
INSERT [dbo].[TASK_IMAGES] ([IMAGEID], [TI_PATH], [TASKID], [CREATED_BY], [DATE_CREATED], [UPDATE_BY], [DATE_UPDATED]) VALUES (4, N'/storage/images/2/3.jpeg', 12, NULL, NULL, NULL, NULL)
INSERT [dbo].[TASK_IMAGES] ([IMAGEID], [TI_PATH], [TASKID], [CREATED_BY], [DATE_CREATED], [UPDATE_BY], [DATE_UPDATED]) VALUES (5, N'/storage/images/2/5.jpeg', 12, NULL, NULL, NULL, NULL)
INSERT [dbo].[TASK_IMAGES] ([IMAGEID], [TI_PATH], [TASKID], [CREATED_BY], [DATE_CREATED], [UPDATE_BY], [DATE_UPDATED]) VALUES (7, N'/storage/images/2/3.jpeg', NULL, NULL, NULL, NULL, NULL)
SET IDENTITY_INSERT [dbo].[TASK_IMAGES] OFF
SET IDENTITY_INSERT [dbo].[TASKS] ON 

INSERT [dbo].[TASKS] ([TASKID], [ACCOUNTID], [T_TITLE], [T_DESC], [T_STATUS], [T_COLOR], [T_DUE_DATE_TIME], [T_ADDED_DATE_TIME], [CREATED_BY], [DATE_CREATED], [UPDATE_BY], [DATE_UPDATED]) VALUES (4, 1, N'12', N'213', N'DELETED', N'#A7FFEB', N'2023-06-01T13:57', N'2023-06-01T16:57', NULL, NULL, NULL, NULL)
INSERT [dbo].[TASKS] ([TASKID], [ACCOUNTID], [T_TITLE], [T_DESC], [T_STATUS], [T_COLOR], [T_DUE_DATE_TIME], [T_ADDED_DATE_TIME], [CREATED_BY], [DATE_CREATED], [UPDATE_BY], [DATE_UPDATED]) VALUES (6, 2, N'Title', N'', N'ADDED', N'#E6C9A8', N'', N'2023-06-05T16:58', NULL, NULL, NULL, NULL)
INSERT [dbo].[TASKS] ([TASKID], [ACCOUNTID], [T_TITLE], [T_DESC], [T_STATUS], [T_COLOR], [T_DUE_DATE_TIME], [T_ADDED_DATE_TIME], [CREATED_BY], [DATE_CREATED], [UPDATE_BY], [DATE_UPDATED]) VALUES (8, 2, N'', N'', N'COMPLETED', N'#F28B82', N'', N'2023-06-05T16:58', NULL, NULL, NULL, NULL)
INSERT [dbo].[TASKS] ([TASKID], [ACCOUNTID], [T_TITLE], [T_DESC], [T_STATUS], [T_COLOR], [T_DUE_DATE_TIME], [T_ADDED_DATE_TIME], [CREATED_BY], [DATE_CREATED], [UPDATE_BY], [DATE_UPDATED]) VALUES (9, 2, N'kj', N'mh', N'ADDED', N'0', N'', N'2023-06-05T16:58', NULL, NULL, NULL, NULL)
INSERT [dbo].[TASKS] ([TASKID], [ACCOUNTID], [T_TITLE], [T_DESC], [T_STATUS], [T_COLOR], [T_DUE_DATE_TIME], [T_ADDED_DATE_TIME], [CREATED_BY], [DATE_CREATED], [UPDATE_BY], [DATE_UPDATED]) VALUES (10, 2, N'', N'', N'ADDED', N'#D7AEFB', N'', N'2023-06-05T16:59', NULL, NULL, NULL, NULL)
INSERT [dbo].[TASKS] ([TASKID], [ACCOUNTID], [T_TITLE], [T_DESC], [T_STATUS], [T_COLOR], [T_DUE_DATE_TIME], [T_ADDED_DATE_TIME], [CREATED_BY], [DATE_CREATED], [UPDATE_BY], [DATE_UPDATED]) VALUES (11, 2, N'', N'', N'ADDED', N'#FFF475', N'', N'2023-06-05T16:59', NULL, NULL, NULL, NULL)
INSERT [dbo].[TASKS] ([TASKID], [ACCOUNTID], [T_TITLE], [T_DESC], [T_STATUS], [T_COLOR], [T_DUE_DATE_TIME], [T_ADDED_DATE_TIME], [CREATED_BY], [DATE_CREATED], [UPDATE_BY], [DATE_UPDATED]) VALUES (12, 2, N'', N'', N'ADDED', N'#FDCFE8', N'', N'2023-06-05T16:59', NULL, NULL, NULL, NULL)
SET IDENTITY_INSERT [dbo].[TASKS] OFF
/****** Object:  Index [IX_ROLEID]    Script Date: 6/5/2023 10:06:43 PM ******/
CREATE NONCLUSTERED INDEX [IX_ROLEID] ON [dbo].[ACCOUNTS]
(
	[ROLEID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_TASKID]    Script Date: 6/5/2023 10:06:43 PM ******/
CREATE NONCLUSTERED INDEX [IX_TASKID] ON [dbo].[CHECKLIST]
(
	[TASKID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_TASKID]    Script Date: 6/5/2023 10:06:43 PM ******/
CREATE NONCLUSTERED INDEX [IX_TASKID] ON [dbo].[TASK_IMAGES]
(
	[TASKID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_ACCOUNTID]    Script Date: 6/5/2023 10:06:43 PM ******/
CREATE NONCLUSTERED INDEX [IX_ACCOUNTID] ON [dbo].[TASKS]
(
	[ACCOUNTID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[ACCOUNTS]  WITH CHECK ADD  CONSTRAINT [FK_dbo.ACCOUNTS_dbo.ROLES_ROLEID] FOREIGN KEY([ROLEID])
REFERENCES [dbo].[ROLES] ([ROLEID])
GO
ALTER TABLE [dbo].[ACCOUNTS] CHECK CONSTRAINT [FK_dbo.ACCOUNTS_dbo.ROLES_ROLEID]
GO
ALTER TABLE [dbo].[CHECKLIST]  WITH CHECK ADD  CONSTRAINT [FK_dbo.CHECKLIST_dbo.TASKS_TASKID] FOREIGN KEY([TASKID])
REFERENCES [dbo].[TASKS] ([TASKID])
GO
ALTER TABLE [dbo].[CHECKLIST] CHECK CONSTRAINT [FK_dbo.CHECKLIST_dbo.TASKS_TASKID]
GO
ALTER TABLE [dbo].[TASK_IMAGES]  WITH CHECK ADD  CONSTRAINT [FK_dbo.TASK_IMAGES_dbo.TASKS_TASKID] FOREIGN KEY([TASKID])
REFERENCES [dbo].[TASKS] ([TASKID])
GO
ALTER TABLE [dbo].[TASK_IMAGES] CHECK CONSTRAINT [FK_dbo.TASK_IMAGES_dbo.TASKS_TASKID]
GO
ALTER TABLE [dbo].[TASKS]  WITH CHECK ADD  CONSTRAINT [FK_dbo.TASKS_dbo.ACCOUNTS_ACCOUNTID] FOREIGN KEY([ACCOUNTID])
REFERENCES [dbo].[ACCOUNTS] ([ACCOUNTID])
GO
ALTER TABLE [dbo].[TASKS] CHECK CONSTRAINT [FK_dbo.TASKS_dbo.ACCOUNTS_ACCOUNTID]
GO
USE [master]
GO
ALTER DATABASE [FEHRISTDB] SET  READ_WRITE 
GO
