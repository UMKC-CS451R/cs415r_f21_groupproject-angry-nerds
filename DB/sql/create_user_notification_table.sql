CREATE TABLE user_notifications (
	NoteID int unsigned NOT NULL AUTO_INCREMENT,
    UserID int unsigned NOT NULL,
	CONSTRAINT FK_UN_NoteID FOREIGN KEY (NoteID) REFERENCES notifications(NoteID),
    CONSTRAINT FK_UN_UserID FOREIGN KEY (UserID) REFERENCES user(UserID),
    PRIMARY KEY (UserID, NoteID)
);