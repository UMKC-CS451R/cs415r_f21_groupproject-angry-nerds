CREATE TABLE account (
    AccountID int unsigned NOT NULL AUTO_INCREMENT PRIMARY KEY,
    AccountType int unsigned NOT NULL,
    CONSTRAINT FK_AccountType FOREIGN KEY (AccountType) REFERENCES account_type(AccountType)
);