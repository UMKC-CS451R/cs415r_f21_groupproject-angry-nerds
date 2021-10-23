import json
import pytest
import requests
import subprocess

@pytest.fixture(scope="session", autouse=True)
def recreate_db():
    try:
        subprocess.call("python database/db_teardown.py", shell=True)
    except:
        pass
    finally:
        subprocess.call("python database/db_setup.py", shell=True)

@pytest.fixture(scope="session")
def base_url():
    return "https://localhost:44347"

@pytest.fixture()
def user1():
    with open("tests/resources/testUser1.json", "r") as f:
        return json.load(f)

@pytest.fixture()
def user2():
    with open("tests/resources/testUser2.json", "r") as f:
        return json.load(f)

@pytest.fixture()
def auth_token1(base_url, user1):
    response = requests.post(
        f"{base_url}/api/getToken",
        verify=False,
        json={k:v for k,v in user1.items() if k in ["email", "password"]})
    token = response.cookies.get_dict()["AuthToken"]
    return token

@pytest.fixture()
def auth_token2(base_url, user2):
    response = requests.post(
        f"{base_url}/api/getToken",
        verify=False,
        json={k:v for k,v in user2.items() if k in ["email", "password"]})
    token = response.cookies.get_dict()["AuthToken"]
    return token