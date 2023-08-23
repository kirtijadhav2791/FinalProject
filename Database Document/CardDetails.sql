create table CardDetails(
CardID int identity(1,1) primary key,
UserId int foreign key references [EShoppingApplication].[dbo].[UserDetail](UserId) not null,
InsertionDate datetime default current_timestamp,
ProductID int foreign key references ProductDetails(ProductID) not null,
IsOrder bit default 0,
IsActive bit default 1
)