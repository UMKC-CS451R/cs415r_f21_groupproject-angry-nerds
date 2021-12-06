# Manual Tests

## Backend
Done at the moment in interest of development time

# Add Account
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

