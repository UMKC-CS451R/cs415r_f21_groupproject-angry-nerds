# ---
# jupyter:
#   jupytext:
#     text_representation:
#       extension: .py
#       format_name: light
#       format_version: '1.5'
#       jupytext_version: 1.13.3
#   kernelspec:
#     display_name: Python 3 (ipykernel)
#     language: python
#     name: python3
# ---

# # Reload database
# Initial transaction information, accounts, and user info

from DB import db_teardown, db_setup

db_teardown.main()
db_setup.main()

# # Live Demo - Admin Utilities

import requests

# Get Authorization Token
admin_token = requests.post(
    "https://localhost:44347/api/getToken", 
    verify=False,
    json={"email": "admin", "password": "admin"}
).json()["token"]

response = requests.post(
    "https://localhost:44347/api/getToken", 
    verify=False,
    json={"email": "johndoe@example.com", "password": "hunter2"}
)
user_token1 = response.json()["token"]
print(response.status_code)
print(response.json())

# ## Add Transaction
# This:
# - adds a transaction to an account
# - gets the notifications the user has enabled
# - checks and sends notifications, if applicable
# - returns the full information about the transaction, including ending balance amount and ID

response = requests.post(
    "https://localhost:44347/api/addTransaction", 
    verify=False, 
    headers={"Authorization": f"Bearer {admin_token}"}, 
    json={
        "AccountId": 822222228, 
        "TimeMonth": 11, 
        "TimeDay": 3, 
        "TimeYear": 2021, 
        "AmountDollars": -3, 
        "AmountCents": 87, 
        "LocationStCd": "WA", 
        "CountryCd":"US", 
        "Vendor": "McDonalds"
    }
)
response.json()

response = requests.post(
    "https://localhost:44347/api/addTransaction", 
    verify=False, 
    headers={"Authorization": f"Bearer {admin_token}"}, 
    json={
        "AccountId": 822222228, 
        "TimeMonth": 11, 
        "TimeDay": 30, 
        "TimeYear": 2021, 
        "AmountDollars": -85, 
        "AmountCents": 54, 
        "LocationStCd": "KS", 
        "CountryCd":"US", 
        "Vendor": "IKEA"
    }
)
response.json()

response = requests.post(
    "https://localhost:44347/api/addTransaction", 
    verify=False, 
    headers={"Authorization": f"Bearer {admin_token}"}, 
    json={
        "AccountId": 822222228, 
        "TimeMonth": 12, 
        "TimeDay": 2, 
        "TimeYear": 2021, 
        "AmountDollars": 105, 
        "AmountCents": 2, 
        "LocationStCd": "KS", 
        "CountryCd":"US", 
        "Vendor": "UMKC"
    }
)
response.json()


