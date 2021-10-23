INSERT INTO transactions (
    TransactionID,
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
    :TransactionID,
    :AccountID,
    :TimeMonth,
    :TimeDay,
    :TimeYear,
    :AmountDollars,
    :AmountCents,
    :EndBalanceDollars,
    :EndBalanceCents,
    :LocationStCd,
    :CountryCd,
    :Vendor
);