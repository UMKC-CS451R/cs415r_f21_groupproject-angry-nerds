CREATE TABLE transactions (
    TransactionID int unsigned NOT NULL AUTO_INCREMENT PRIMARY KEY,
    AccountID int unsigned NOT NULL,
    TimeMonth int unsigned NOT NULL,
    TimeDay int unsigned NOT NULL,
    TimeYear int unsigned NOT NULL,
    AmountDollars int NOT NULL,
    AmountCents int NOT NULL,
    EndBalanceDollars int NOT NULL,
    EndBalanceCents int NOT NULL,
    LocationStCd varchar(2) NOT NULL,
    CountryCd varchar(2) NOT NULL,
    Vendor varchar(100) NOT NULL,
    CONSTRAINT FK_AccountID FOREIGN KEY (AccountID) REFERENCES account(AccountID),
    CONSTRAINT FK_Vendor FOREIGN KEY (Vendor) REFERENCES vendor_cat(Vendor)
);