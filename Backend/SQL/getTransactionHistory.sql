SELECT TransactionID, TimeMonth, TimeDay, TimeYear, AmountDollars, AmountCents, EndBalanceDollars, EndBalanceCents, Vendor
FROM transactions
WHERE AccountID = @ID
ORDER BY
    TimeYear DESC, 
    TimeMonth DESC,
    TimeDay DESC,
    TransactionID DESC
LIMIT @START_ROW,@NUM_ROWS;