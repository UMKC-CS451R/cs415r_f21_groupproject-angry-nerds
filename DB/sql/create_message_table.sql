CREATE TABLE messages (
	MessageID int unsigned AUTO_INCREMENT PRIMARY KEY,
	UserID int unsigned NOT NULL,
	MessageContents varchar(300) NOT NULL, 
	TimeMonth int unsigned NOT NULL,
	TimeDay int unsigned NOT NULL,
	TimeYear int unsigned NOT NULL,
    CONSTRAINT FK_M_UserID FOREIGN KEY (UserID) REFERENCES user(UserID)
);