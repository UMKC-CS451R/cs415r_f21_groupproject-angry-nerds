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
def example_user():
    return {"username": "test", "password": "test"}

@pytest.fixture()
def auth_token(base_url, example_user):
    response = requests.post(
        f"{base_url}/api/getToken",
        verify=False,
        json=example_user)
    token = response.cookies.get_dict()["AuthToken"]
    return token