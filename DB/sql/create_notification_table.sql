CREATE TABLE notifications (
	NoteID int unsigned NOT NULL AUTO_INCREMENT PRIMARY KEY,
    NoteFnName varchar(50) NOT NULL UNIQUE,
    NoteDesc varchar(300) NOT NULL
);