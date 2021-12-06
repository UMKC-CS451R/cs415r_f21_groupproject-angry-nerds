SELECT NoteFnName
FROM user
LEFT JOIN user_notifications
ON user.UserID = user_notifications.UserID
LEFT JOIN notifications
ON user_notifications.NoteID = notifications.NoteID
WHERE user.UserID = @ID;