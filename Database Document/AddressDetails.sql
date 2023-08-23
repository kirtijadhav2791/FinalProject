create table AddressDetails(
ID int identity(1,1) primary key,
InsertionDate datetime default current_timestamp,
UpdationDate datetime,
UserID int not null,
Address1 varchar(1024),
Address2 varchar(1024),
City varchar(100),
Distict varchar(100),
State varchar(100),
Country varchar(100),
pincode varchar(6),
IsActive bit default 1
)