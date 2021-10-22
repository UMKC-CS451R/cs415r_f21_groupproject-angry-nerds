import requests
from time import sleep

def test_authentication(base_url):
    response = requests.post(
        f"{base_url}/api/getToken",
        verify=False,
        json={
            "username": "bogus",
            "password": "dumbPWD"
        }
    )
    auth_token = response.cookies.get_dict().get("AuthToken")
    assert auth_token == None
    assert response.status_code == 400
    assert response.json() == {"message": "Username or password is incorrect"}


def test_validation(base_url, example_user):
    response = requests.post(
        f"{base_url}/api/getToken",
        verify=False,
        json=example_user)
    auth_token = response.cookies.get_dict()["AuthToken"]

    response = requests.get(
        f"{base_url}/api/refreshToken",
        verify=False
    )
    assert response.status_code == 401
    assert response.json() == {"message": "Unauthorized"}


def test_refresh_token(base_url, example_user):
    response = requests.post(
        f"{base_url}/api/getToken",
        verify=False,
        json=example_user)
    auth_token = response.cookies.get_dict()["AuthToken"]

    sleep(2) # prevent the tokens from being signed within the same second

    response = requests.get(
        f"{base_url}/api/refreshToken",
        verify=False,
        cookies={"AuthToken": auth_token}
    )
    refreshed_token = response.cookies.get_dict()["AuthToken"]
    assert refreshed_token != auth_token
