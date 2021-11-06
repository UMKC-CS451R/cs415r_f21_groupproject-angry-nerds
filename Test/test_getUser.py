import json
import pytest
import requests

@pytest.fixture()
def endpoint_url(base_url):
    return f"{base_url}/api/getUser"

@pytest.fixture()
def validation1():
    return {
        'userId': 1, 
        'firstName': 'John', 
        'lastName': 'Doe', 
        'email': 'johndoe@example.com', 
        'accounts': [
            {
                'accountId': 211111110,
                'typeDescription': 'Savings',
                'endBalanceDollars': 4571,
                'endBalanceCents': 8
            }
        ]
    }

def test_valid(endpoint_url, user1, auth_token1, validation1):
    response = requests.post(
        endpoint_url, 
        verify=False, 
        headers={"Authorization": f"Bearer {auth_token1}"},
        json={
            "UserId":user1["userId"]
        }
    )
    assert response.status_code == 200
    assert response.json() == validation1 

def test_invalid_unauthorized(endpoint_url, user2, auth_token1):
    response = requests.post(
        endpoint_url, 
        verify=False, 
        headers={"Authorization": f"Bearer {auth_token1}"},
        json={
            "userId": user2["userId"]
        }
    )
    assert response.status_code == 401