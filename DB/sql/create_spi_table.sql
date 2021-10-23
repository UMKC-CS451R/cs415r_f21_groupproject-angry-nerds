CREATE TABLE spi (
    UserID int unsigned NOT NULL PRIMARY KEY,
    SSN int unsigned NOT NULL UNIQUE,
    AddressLine1 varchar(100) NOT NULL,
    AddressLine2 varchar(100) NULL,
    City varchar(100) NOT NULL,
    PostalState varchar(2) NOT NULL
);