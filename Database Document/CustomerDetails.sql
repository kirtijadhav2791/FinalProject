create table CustomerDetails(
ID int identity(1,1) primary key,
UserID int not null,
UserName varchar(255) not null,
InsertionDate datetime default current_timestamp,
UpdationDate datetime ,
FullName varchar(255) not null,
EmailID varchar(255) not null,
MobileNumber varchar(10),
IsActive bit default 1
)