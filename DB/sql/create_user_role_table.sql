CREATE TABLE user_role(
	RoleID int unsigned NOT NULL,
    UserID int unsigned NOT NULL,
	CONSTRAINT FK_UR_RoleID FOREIGN KEY (RoleID) REFERENCES roles(RoleID),
    CONSTRAINT FK_UR_UserID FOREIGN KEY (UserID) REFERENCES user(UserID)
);