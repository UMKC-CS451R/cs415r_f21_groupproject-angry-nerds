# Manual Tests

## Backend
Done at the moment in interest of development time

### Add Account
Sign in as admin, add account, and verify it exists in DB
```
import requests
token = requests.post("https://localhost:44347/api/getToken", verify=False, json={"email": "admin", "password": "admin"}).json()["token"]
response = requests.post("https://localhost:44347/api/addAccount", verify=False, headers={"Authorization": f"Bearer {token}"}, json={"AccountType": "Checking", "InitBalanceDollars": 1, "InitBalanceCents": 0, "Users": [1]})
```

Sign in as user, attempt to add account, and verify error and no addition to DB
```
import requests
token = requests.post("https://localhost:44347/api/getToken", verify=False, json={"email": "johndoe@example.com", "password": "hunter2"}).json()["token"]
response = requests.post("https://localhost:44347/api/addAccount", verify=False, headers={"Authorization": f"Bearer {token}"}, json={"AccountType": "Checking", "InitBalanceDollars": 1, "InitBalanceCents": 0, "Users": [1]})
assert response.status_code == 401
```

### Add Account User
Verify user is added to account with admin account
Verify 401 with non-admin account
```
response = requests.post("https://localhost:44347/api/addAccountUser", verify=False, headers={"Authorization": f"Bearer {token}"}, json={"AccountId": 822222228, "Users": [1]})
```

### Add Transaction
Verify new transaction is added to database & the math on the money is correct
Verify a new message is created for out of state transaction
```
 response = requests.post("https://localhost:44347/api/addTransaction", verify=False, headers={"Authorization": f"Bearer {token}"}, json={"AccountId": 822222228, "TimeMonth": 11, "TimeDay":
 3, "TimeYear": 2023, "AmountDollars": -3, "AmountCents": 87, "LocationStCd": "WA", "CountryCd":"US", "Vendor": "McDonalds"})
```