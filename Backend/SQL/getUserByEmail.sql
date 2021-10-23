SELECT Email, UserId, FirstName, LastName, Salt, Pwd
FROM user
WHERE Email = @EMAIL;