SELECT account.AccountID, account_type.TypeDescription, t.EndBalanceDollars, t.EndBalanceCents
FROM account
INNER JOIN account_type
ON account.AccountType = account_type.AccountType
INNER JOIN (
    SELECT t1.AccountID, t1.EndBalanceDollars, t1.EndBalanceCents
    FROM transactions t1
    WHERE t1.TransactionID = (
        SELECT MAX(t2.TransactionID)
        FROM transactions t2
        WHERE t2.AccountID = t1.AccountID)
    ) as t
ON t.AccountID = account.AccountID
WHERE account.AccountID = @ID;