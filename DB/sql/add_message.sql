INSERT INTO messages (
    UserID,
	MessageContents, 
	TimeMonth,
	TimeDay,
	TimeYear
)
VALUES (
    :UserID,
	:MessageContents, 
	:TimeMonth,
	:TimeDay,
	:TimeYear
);