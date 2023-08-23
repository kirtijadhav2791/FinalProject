create table ProductDetails(
ProductID int identity(1,1) primary key,
InsertionDate datetime default current_timestamp,
UpdateDate datetime ,
ProductName varchar(255) not null,
ProductType varchar(100) not null,
ProductPrice varchar(10) not null,
ProductDetails varchar(2055),
ProductCompany varchar(255),
ProductImageUrl varchar(512),
PublicId varchar(255),
Quantity int,
IsArchive bit default 0,
IsActive bit default 1
)