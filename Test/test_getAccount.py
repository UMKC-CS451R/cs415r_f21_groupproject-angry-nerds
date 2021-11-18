import json
import pytest
import requests

@pytest.fixture()
def endpoint_url(base_url):
    return f"{base_url}/api/getAccount"

@pytest.fixture()
def validation():
    with open("Test/resources/getAccount.json", "r") as f:
        return json.load(f)


def test_valid(endpoint_url, auth_token2, validation):
    response = requests.post(
        endpoint_url, 
        verify=False, 
        headers={"Authorization": f"Bearer {auth_token2}"},
        json={
            "AccountId":822222228
        }
    )
    assert response.status_code == 200
    assert response.json() == validation 

@pytest.mark.parametrize(
    "id", 
    [(213)])
def test_request_error_no_content(id, auth_token1, endpoint_url):
    response = requests.post(
        endpoint_url, 
        verify=False, 
        headers={"Authorization": f"Bearer {auth_token1}"},
        json={
            "AccountId":id
        }
    )
    assert response.status_code == 404

@pytest.mark.parametrize(
    "id", 
    [(411111111)])
def test_request_error_unauthorized(id, auth_token1, endpoint_url):
    response = requests.post(
        endpoint_url, 
        verify=False, 
        headers={"Authorization": f"Bearer {auth_token1}"},
        json={
            "AccountId":id
        }
    )
    assert response.status_code == 401