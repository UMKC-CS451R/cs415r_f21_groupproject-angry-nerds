START TRANSACTION;
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
    @AccountId,
    @TimeMonth,
    @TimeDay,
    @TimeYear,
    @AmountDollars,
    @AmountCents,
    (WITH
        data AS (
        SELECT EndBalanceDollars, EndBalanceCents
        FROM transactions 
        WHERE AccountID = @AccountID 
        ORDER BY 
            TimeYear DESC, 
            TimeMonth DESC, 
            TimeDay DESC, 
            TransactionID DESC
        LIMIT 1),
        temp AS (
            SELECT data.EndBalanceCents + @AmountCents * IF(@AmountDollars < 0, -1, 1) as cents
            FROM data
        ),
        dollars AS (
            SELECT 
                data.EndBalanceDollars + @AmountDollars + IF(temp.cents < 0, -1, 0) as EndBalanceDollars,
                temp.cents + IF(temp.cents < 0, 100, 0) as EndBalanceCents
            FROM data
            JOIN temp
        ) 
    SELECT EndBalanceDollars FROM dollars),
	(WITH 
        data AS (
        SELECT EndBalanceDollars, EndBalanceCents
        FROM transactions 
        WHERE AccountID = @AccountID 
        ORDER BY 
            TimeYear DESC, 
            TimeMonth DESC, 
            TimeDay DESC, 
            TransactionID DESC
        LIMIT 1),
        temp AS (
            SELECT data.EndBalanceCents + @AmountCents * IF(@AmountDollars < 0, -1, 1) as cents
            FROM data
        ),
        dollars AS (
            SELECT 
                data.EndBalanceDollars + @AmountDollars + IF(temp.cents < 0, -1, 0) as EndBalanceDollars,
                temp.cents + IF(temp.cents < 0, 100, 0) as EndBalanceCents
            FROM data
            JOIN temp
        ) 
    SELECT EndBalanceCents FROM dollars),
    @LocationStCd,
    @CountryCd,
    @Vendor
);
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
WHERE TransactionID = last_insert_id();
COMMIT;