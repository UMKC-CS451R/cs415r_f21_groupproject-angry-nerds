START TRANSACTION;
INSERT INTO account_user (
    AccountID,
    UserID
)
VALUES (
    @AccountId,
    @UserId
);
COMMIT;