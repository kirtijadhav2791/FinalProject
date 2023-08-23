create table WishListDetails(
WishListID int identity(1,1) primary key,
UserId int foreign key references [EShoppingApplication].[dbo].[UserDetail](UserId) not null,
InsertionDate datetime default current_timestamp,
ProductID int,
IsActive bit default 1
)