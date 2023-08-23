create table FeedbackDetail(
FeedbackID int identity(1,1) primary key,
InsertionDate datetime default current_timestamp
UserID int foreign key references UserDetail(UserID),
Feedback text
);