CREATE TABLE user (
    Email varchar(320) NOT NULL UNIQUE,
    UserID int unsigned NOT NULL AUTO_INCREMENT PRIMARY KEY,
    FirstName varchar(50) NOT NULL,
    LastName varchar(50) NOT NULL,
    Salt BLOB(192) NOT NULL,
    Pwd BLOB(256) NOT NULL
);