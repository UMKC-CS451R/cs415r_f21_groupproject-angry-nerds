SELECT Email, user.UserId, FirstName, LastName, RoleDesc, Salt, Pwd
FROM user
LEFT JOIN user_role 
ON user.UserID = user_role.UserID
LEFT JOIN roles 
ON user_role.RoleID = roles.RoleID
WHERE Email = @EMAIL;