INSERT INTO messages (
    UserID,
	MessageContents, 
	TimeMonth,
	TimeDay,
	TimeYear
)
VALUES (
    @ID,
	@Message, 
	@TimeMonth,
	@TimeDay,
	@TimeYear
);