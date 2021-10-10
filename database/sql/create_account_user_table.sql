CREATE TABLE account_user (
    AccountID int unsigned NOT NULL,
    UserID int unsigned NOT NULL,
    PRIMARY KEY (AccountID, UserID),
    CONSTRAINT FK_User_AccountID FOREIGN KEY (AccountID) REFERENCES account(AccountID),
    CONSTRAINT FK_Account_UserID FOREIGN KEY (UserID) REFERENCES user(UserID)
);