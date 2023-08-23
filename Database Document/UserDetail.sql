 create table UserDetail
 (
  UserId int identity(1,1),
  UserName varchar(255),
  PassWord varchar(255),
  Role VARCHAR(10) NOT NULL CHECK (Role IN('customer', 'admin', 'master')),
  InsertionDate varchar(255) default current_timestamp,
  IsActive bit default 1
 );