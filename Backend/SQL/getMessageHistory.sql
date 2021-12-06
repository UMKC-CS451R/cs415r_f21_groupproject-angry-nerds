SELECT MessageContents, TimeMonth, TimeDay, TimeYear
FROM messages
WHERE UserID = @ID
ORDER BY
    TimeYear DESC, 
    TimeMonth DESC,
    TimeDay DESC,
    MessageID DESC
LIMIT @START_ROW,@NUM_ROWS;