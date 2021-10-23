import json
import pytest
import requests

@pytest.fixture()
def endpoint_url(base_url):
    return f"{base_url}/api/getTransaction"

@pytest.fixture()
def validation1():
    with open("tests/resources/getTransaction1.json", "r") as f:
        return json.load(f)

@pytest.fixture()
def validation2():
    with open("tests/resources/getTransaction2.json", "r") as f:
        return json.load(f)

def test_valid(endpoint_url, auth_token1, validation1):
    response = requests.post(
        endpoint_url, 
        verify=False, 
        cookies={"AuthToken":auth_token1},
        json={
            "TransactionId":1
        }
    )
    assert response.status_code == 200
    assert response.json() == validation1 

def test_valid(endpoint_url, auth_token2, validation2):
    response = requests.post(
        endpoint_url, 
        verify=False, 
        cookies={"AuthToken":auth_token2},
        json={
            "TransactionId":117
        }
    )
    assert response.status_code == 200
    assert response.json() == validation2

@pytest.mark.parametrize(
    "id", 
    [(0)])
def test_request_error_no_content(id, auth_token1, endpoint_url):
    response = requests.post(
        endpoint_url, 
        verify=False, 
        cookies={"AuthToken":auth_token1},
        json={
            "TransactionId":id
        }
    )
    assert response.status_code == 204

@pytest.mark.parametrize(
    "id", 
    [(98)])
def test_request_error_unauthorized(id, auth_token1, endpoint_url):
    response = requests.post(
        endpoint_url, 
        verify=False, 
        cookies={"AuthToken":auth_token1},
        json={
            "TransactionId":id
        }
    )
    assert response.status_code == 401