SELECT 
    TransactionId,
    AccountId,
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
FROM transactions
WHERE TransactionID = @ID;