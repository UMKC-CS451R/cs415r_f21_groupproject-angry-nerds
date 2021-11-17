import json
import pytest
import requests

@pytest.fixture()
def endpoint_url(base_url):
    return f"{base_url}/api/getTransactionHistory"

@pytest.fixture()
def validation():
    with open("Test/resources/getTransactionHistory.json", "r") as f:
        return json.load(f)

@pytest.fixture()
def validation_paged():
    with open("Test/resources/getTransactionHistoryPaged.json", "r") as f:
        return json.load(f)

def test_valid(endpoint_url, auth_token1, validation):
    response = requests.post(
        endpoint_url, 
        verify=False, 
        headers={"Authorization": f"Bearer {auth_token1}"},
        json={
            "AccountId":211111110,
            "pageSize":5, 
            "pageNumber":0
        }
    )
    assert response.status_code == 200
    assert response.json() == validation    

def test_valid_paged(endpoint_url, auth_token1, validation_paged):
    response = requests.post(
        endpoint_url, 
        verify=False, 
        headers={"Authorization": f"Bearer {auth_token1}"},
        json={
            "AccountId":211111110,
            "pageSize":10, 
            "pageNumber":3
        }
    )
    assert response.status_code == 200
    assert response.json() == validation_paged

@pytest.mark.parametrize(
    "id, page_size, page_number", 
    [(211111110, 10, 60)])
def test_request_error_no_content(id, page_size, page_number, auth_token1, endpoint_url):
    response = requests.post(
        endpoint_url, 
        verify=False, 
        headers={"Authorization": f"Bearer {auth_token1}"},
        json={
            "AccountId":id,
            "pageSize":page_size, 
            "pageNumber":page_number
        }
    )
    assert response.status_code == 404

@pytest.mark.parametrize(
    "id, page_size, page_number", 
    [(9, 10, 3)])
def test_request_error_unauthorized(id, page_size, page_number, auth_token1, endpoint_url):
    response = requests.post(
        endpoint_url, 
        verify=False, 
        headers={"Authorization": f"Bearer {auth_token1}"},
        json={
            "AccountId":id,
            "pageSize":page_size, 
            "pageNumber":page_number
        }
    )
    assert response.status_code == 401