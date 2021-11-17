SELECT user.UserID, user.Email, user.FirstName, user.LastName
FROM user
INNER JOIN account_user
ON user.UserID = account_user.UserID
WHERE account_user.AccountID = @ID;