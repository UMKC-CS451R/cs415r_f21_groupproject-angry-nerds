import requests
from time import sleep

def test_authentication(base_url):
    response = requests.post(
        f"{base_url}/api/getToken",
        verify=False,
        json={
            "email": "bogus",
            "password": "dumbPWD"
        }
    )
    auth_token = response.json().get("token")
    assert auth_token == None
    assert response.status_code == 400
    assert response.json() == {"message": "Username or password is incorrect"}


def test_validation(base_url, user1):
    response = requests.post(
        f"{base_url}/api/getToken",
        verify=False,
        json={k:v for k,v in user1.items() if k in ["email", "password"]})
    auth_token = response.json()["token"]

    response = requests.get(
        f"{base_url}/api/refreshToken",
        verify=False
    )
    assert response.status_code == 401
    assert response.json() == {"message": "Unauthorized"}


def test_refresh_token(base_url, user1):
    response = requests.post(
        f"{base_url}/api/getToken",
        verify=False,
        json={k:v for k,v in user1.items() if k in ["email", "password"]})
    auth_token = response.json()["token"]

    sleep(2) # prevent the tokens from being signed within the same second

    response = requests.get(
        f"{base_url}/api/refreshToken",
        verify=False,
        headers={"Authorization": f"Bearer {auth_token}"}
    )
    refreshed_token = response.json()["token"]
    assert refreshed_token != auth_token
