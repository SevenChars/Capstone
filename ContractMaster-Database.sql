
USE [master]
GO

IF EXISTS (SELECT "name"
		   FROM Sysdatabases
		   WHERE "name" = 'ContractMaster')
   BEGIN
    ALTER DATABASE [ContractMaster] 
	SET SINGLE_USER
	WITH ROLLBACK IMMEDIATE;
	DROP DATABASE [ContractMaster];
   END
GO

CREATE DATABASE [ContractMaster]
GO

USE [ContractMaster]
GO

Create table [dbo].[Company](
[CompanyId] VARCHAR(50) CONSTRAINT PK_company PRIMARY KEY,
[CompanyName] VARCHAR(50),
[AccountStatus] VARCHAR(50),
[Address] VARCHAR(50),
[City] VARCHAR(50),
[Province] VARCHAR(50),
[PositalCode] VARCHAR(50),
[ContactPerson] VARCHAR(50))
GO

Create table [dbo].[Contractor](
[ContractorId] VARCHAR(50) CONSTRAINT PK_constractor PRIMARY KEY,
[ProjectPrincipal] VARCHAR(50),
[ProjectCoordinator] VARCHAR(50),

CONSTRAINT FK_company_contractor
FOREIGN KEY ([contractorId])
REFERENCES [dbo].[Company] ([CompanyId])
ON DELETE CASCADE
ON UPDATE CASCADE)
GO

Create table [dbo].[ContactPerson](
[Id] VARCHAR(50) CONSTRAINT PK_contactperson PRIMARY KEY,
[Name] VARCHAR(50),
[CompanyId] VARCHAR(50),
[Email] VARCHAR(50),
[Phone] VARCHAR(50),

CONSTRAINT FK_company_contact
FOREIGN KEY ([companyId])
REFERENCES [dbo].[company] ([companyId])
ON DELETE CASCADE
ON UPDATE CASCADE)
GO

Create table [dbo].[Client](
[ClientId] VARCHAR(50) CONSTRAINT PK_Client PRIMARY KEY,

CONSTRAINT FK_company_client
FOREIGN KEY ([clientId])
REFERENCES [dbo].[company] ([companyId])
ON DELETE CASCADE
ON UPDATE CASCADE)
GO

CREATE TABLE [dbo].[Buyer](
[BuyerId] VARCHAR(50) CONSTRAINT PK_buyer PRIMARY KEY,
[BuyerName] VARCHAR(50),
[ClientId] VARCHAR(50),
[BuyerEmail] VARCHAR(50),

CONSTRAINT FK_client_buyer
FOREIGN KEY ([clientId])
REFERENCES [dbo].[client] ([clientId])
ON DELETE CASCADE
ON UPDATE CASCADE)
GO

CREATE TABLE [dbo].[MainCategory](
[CategoryId] VARCHAR(50) CONSTRAINT PK_MainCategory PRIMARY KEY,
[CategoryName] VARCHAR(50) )
GO

CREATE TABLE [dbo].[SubCategory](
[SubcategoryId] VARCHAR(50) CONSTRAINT PK_SubCategory PRIMARY KEY,
[CategoryId] VARCHAR(50),
[SubcategoryName] VARCHAR(50),

CONSTRAINT FK_MainCategory_SubCategory
FOREIGN KEY ([CategoryId])
REFERENCES [dbo].[MainCategory] ([CategoryId])
ON DELETE CASCADE
ON UPDATE CASCADE)
GO

CREATE TABLE [dbo].[Contract](
[ContractId] VARCHAR(50) CONSTRAINT PK_Contract PRIMARY KEY,
[StartDate] DATE,
[EndDate] DATE,
[AddDate] DATE )
GO

CREATE TABLE [dbo].[ContractDetail](
[ContractId] VARCHAR(50) CONSTRAINT PK_ContractDetail PRIMARY KEY,
[ContractName] VARCHAR(50),
[CategoryId] VARCHAR(50),
[SubCategoryId] VARCHAR(50),
[ClientId] VARCHAR(50),
[ContractorId] VARCHAR(50),
[BuyerId] VARCHAR(50),
[Description] VARCHAR(200),
[BidDate] DATE,
[BidNumber] VARCHAR(50),

CONSTRAINT FK_Contract_ContractDetail
FOREIGN KEY ([ContractId])
REFERENCES [dbo].[Contract] ([ContractId])
ON DELETE CASCADE
ON UPDATE CASCADE,

CONSTRAINT FK_MainCategory_ContractDetail
FOREIGN KEY ([CategoryId])
REFERENCES [dbo].[MainCategory] ([CategoryId])
ON DELETE CASCADE
ON UPDATE CASCADE,

CONSTRAINT FK_SubCategory_ContractDetail
FOREIGN KEY ([SubcategoryId])
REFERENCES [dbo].[SubCategory] ([SubcategoryId]),

CONSTRAINT FK_Client_ContractDetail
FOREIGN KEY ([ClientId])
REFERENCES [dbo].[Client] ([ClientId]),

CONSTRAINT FK_Contractor_ContractDetail
FOREIGN KEY ([ContractorId])
REFERENCES [dbo].[Contractor] ([ContractorId])
ON DELETE CASCADE
ON UPDATE CASCADE,

CONSTRAINT FK_Buyer_ContractDetail
FOREIGN KEY ([BuyerId])
REFERENCES [dbo].[Buyer] ([BuyerId]))
GO

CREATE TABLE [dbo].[ProjectData](
[ContractId] VARCHAR(50) CONSTRAINT PK_ProjectData PRIMARY KEY,
[TenderTitle] VARCHAR(50),
[Description] VARCHAR(200),
[PromisedDays] VARCHAR(50),
[ActualDays] VARCHAR(50),
[ActualStartDate] DATE,
[ActualCompeletionDate] DATE,
[ContractAwardAmount] MONEY,
[ContractCompletionAmount] MONEY,

CONSTRAINT FK_Contract_ProjectData
FOREIGN KEY ([ContractId])
REFERENCES [dbo].[Contract] ([ContractId])
ON DELETE CASCADE
ON UPDATE CASCADE)
GO

CREATE TRIGGER trContractAftInsert ON [dbo].[Contract]
AFTER INSERT
AS
		DECLARE @Id VARCHAR(50);
		DECLARE @Date DATE;

		SELECT @Id = i.ContractId FROM INSERTED i;
		SET @Date = GETDATE();

		UPDATE [dbo].[Contract]
		SET [AddDate] = @Date
		WHERE [ContractId] = @Id;
GO


USE [ContractMaster]
GO

insert into Company values
('P001','BlackBerry','A','299_King','Waterloo','ON','N1I2I3','Mechinal'),
('P002','SubWay','A','239_Queen','Waterloo','ON','N1I1I1','Cici'),
('P003','Remax','A','119_Weber','Waterloo','ON','N1I2W3','Godiva'),
('P004','Dell','A','560_Stone','Waterloo','ON','N1F2I3','Moster'),
('P005','Nikon','A','296_Gin','Waterloo','ON','A1I2I3','Yoby')
GO

insert into Contract values('Con001','2016-05-12','2018-12-31','')
insert into Contract values('Con002','2016-05-12','2018-10-30','')
insert into Contract values('Con003','2016-05-12','2018-12-31','')
insert into Contract values('Con004','2016-05-12','2018-12-31','')
insert into Contract values('Con005','2016-05-12','2018-12-31','')
GO

insert into Client values
('P001'),
('P002'),
('P003'),
('P004'),
('P005')
GO
insert into Buyer values
('P001','Tom','P005','Tom0084@gmail.com'),
('P002','Jerry','P003','Jerry0284@gmail.com'),
('P003','Godge','P002','Godge0484@gmail.com'),
('P004','Yoko','P001','Yoko0081@gmail.com'),
('P005','Mick','P004','Mick0083@gmail.com')
GO


insert into Contractor values
('P001','',''),
('P002','',''),
('P003','',''),
('P004','',''),
('P005','','')
GO


insert into MainCategory values
('Main_1','Accounting'),
('Main_2','Air Balancing'),
('Main_3','Appliances & Furnishings'),
('Main_4','Audio Visual Equipment')

GO


insert into Subcategory values
('Sub_1','Main_4','Audio Visual Equipment'),
('Sub_2','Main_4','Audio Visual Equipment: Media Supplies'),
('Sub_3','Main_3','Appliances'),
('Sub_4','Main_3','Appliances & Furnishings')
GO

insert into ProjectData values
('Con001','AB_Company','','30','','','','',''),
('Con002','Double_Company','','20','','','','',''),
('Con003','Glue_Company','','15','','','','',''),
('Con004','Intel_Company','','20','','','','',''),
('Con005','AMD','','20','','','','','')
GO

insert into ContractDetail values
('Con001','Ray','Main_2','Sub_1','P001','P001','P001','','',''),
('Con002','Lyn','Main_1','Sub_2','P002','P002','P002','','',''),
('Con003','Will','Main_3','Sub_3','P003','P003','P003','','',''),
('Con004','Gostop','Main_3','Sub_4','P004','P004','P004','','',''),
('Con005','Sky','Main_4','Sub_2','P005','P005','P005','','','')

GO

insert into ContactPerson values
('1','Will','P001','Will@conestogac.on.ca','519-111-1111'),
('2','Well','P002','Well@conestogac.on.ca','519-222-3333'),
('3','Rich','P003','Rich@conestoga.on.ca','519-333-4444'),
('4','Dav','P004','Dav@conestogac.on.ca','519-444-5556'),
('5','Rick','P005','Rick@conestogac.on.ca','519-566-5666')


select *
from Buyer

select *
from Client

select *
from Company

select *
from Contractor

select *
from ContactPerson

select *
from ContractDetail

select * from contract

select *
from MainCategory

select *
from SubCategory

select *
from Contract

select *
from ProjectData

