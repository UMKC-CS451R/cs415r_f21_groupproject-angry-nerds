/* Verify AccountType exists beforehand */
START TRANSACTION;
INSERT INTO account (
    AccountType
)
VALUES (
    (SELECT AccountType
    FROM account_type
    WHERE TypeDescription = @TypeDescription)
);
INSERT INTO account_user (
	AccountID,
    UserID
)
SELECT last_insert_id(), UserID FROM user
WHERE UserID = @PrimaryUser;
SELECT last_insert_id();
INSERT INTO transactions (
    AccountID,
    TimeMonth,
    TimeDay,
    TimeYear,
    AmountDollars,
    AmountCents,
    EndBalanceDollars,
    EndBalanceCents,
    LocationStCd,
    CountryCd,
    Vendor
)
VALUES (
    last_insert_id(),
    MONTH(current_date()),
    DAY(current_date()),
    YEAR(current_date()),
    @AmountDollars,
    @AmountCents,
    @AmountDollars,
    @AmountCents,
    "MO",
    "US",
    "Deposit"
);
COMMIT;