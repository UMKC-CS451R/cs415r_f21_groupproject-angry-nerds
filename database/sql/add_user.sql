INSERT INTO user (
    Email,
    UserID,
    FirstName,
    LastName,
    Salt,
    Pwd
)
VALUES (
    :Email,
    :UserID,
    :FirstName,
    :LastName,
    :Salt,
    :Pwd   
);