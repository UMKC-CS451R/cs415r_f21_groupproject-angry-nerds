import React from 'react';

export const RefreshUser = (user) => {
    let API = "https://localhost:44347/api/";
    let query = "refreshToken";
    return fetch(API + query, {
        method: 'GET',
        mode: 'cors',
        headers: {
            "Content-Type": "application/json",
            "Authorization": "Bearer " + user["token"]
        }
    })
    .then((response) => {
        if (response.ok) {
            return response.json().then(json => {
                let newUser = user;
                newUser["token"] = json["token"];
                newUser["tokenExpires"] = json["tokenExpires"];
                return newUser;
            });
        }
        else {
            return null;
        }
    });
}

export const GetUser = (user) => {
    let API = "https://localhost:44347/api/";
    let query = "getUser";
    return fetch(API + query, {
        method: 'POST',
        mode: 'cors',
        headers: {
            "Content-Type": "application/json",
            "Authorization": "Bearer " + user["token"]
        },
        body: JSON.stringify({"UserId": user["userId"]})
    })
    .then(response => response.json())
    .then(json => {
        // console.log(json);
        const newUser = {...user, "accounts": json["accounts"]};
        console.log(newUser);
        return newUser;
    }); 
};