SELECT Email, user.UserId, FirstName, LastName, RoleDesc, SSN, AddressLine1, IFNULL(AddressLine2, "") AS AddressLine2, City, PostalState
FROM user
LEFT JOIN user_role 
ON user.UserID = user_role.UserID
LEFT JOIN roles 
ON user_role.RoleID = roles.RoleID
LEFT JOIN spi
ON user.UserID = spi.UserID
WHERE user.UserID = @ID;