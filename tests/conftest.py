import pytest
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