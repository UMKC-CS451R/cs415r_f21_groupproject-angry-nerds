# Developers' Guide

## Workstation Setup
Helpful guide for getting started with development.

### Tools
- Git Bash (Windows) 
- Visual Studio 2019
- MySQL Database
- Python 3.8 or 3.9

Git Bash and Visual Studio are simple download and installs. Installation of C# project dependencies will be asked by Visual Studio upon opening of the solution file (*.sln). 

Python 3.7 has a security issue that was fixed with 3.8, so 3.8 or 3.9 is recommended if you are installing a new python version, but any version of python 3 will work. Python is used for testing and creating the development database. It is recommended that you create a virtual environment for this project (using whatever method you are comfortable, i.e. venv, poetry, or conda).

#### Python Virtual Environment
This guide uses venv, which is now a standard package of python. Any method works.

1. Install python version from list of [releases](https://www.python.org/downloads/)
    - You can either add the new installation of python to your PATH variable (an option of the installer)
    - OR not add it to PATH but provide an alias instead (useful for having multiple python versions). Using git bash, you can create an alias (for git bash and python 3.9) by adding the following line to `~/.bash_profile`: `alias python39="winpty ~/AppData/Local/Programs/Python39/python.exe"`.
2. Verify installation: `python --version`
3. Create virtual environment in the folder `.venv` by running this command in the root of this project directory: `python -m venv .venv`
4. Activate virtual environment
    - In Git Bash: `source .venv/Scripts/activate`
    - In Windows command prompt: `".venv/Scripts/activate.bat"`
5. Verify you are in the virtual environment. There should be a `(.venv)` printed before each command line. (Optional) Run `which python` or the windows equivalent to see if the python executable points to your virtual environment.
6. In your virtual environment, install your packages: `python -m pip install -r requirements.txt`
7. Verify the installed packages: `python -m pip freeze`

#### App Settings
The file `appsettings.Development.json` should be added to the project root. It should look like this:
```
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning",
      "Microsoft.Hosting.Lifetime": "Information"
    }
  },
  "ConnectionStrings": {
    "localDB": "server=localhost;database=banking;uid=USERNAME;pwd=PASSWORD;"
  }
}
```

#### Development Database
For MySQL this guide assumes you have installed it and set up a root account or user with necessary access (since this is a local development environment, the security of an account is not super significant). 

You will need to update the connection string in the appsettings.Development.json file. Mostly likily, the connection string will require renaming the server to `localhost` and filling in the username and password. Since you do not want your database password to be public, the Developing appsettings is excluded for git using the .gitignore file. If the appsettings format needs to be updated, do so in the [App Settings section](#App-Settings)

Once you have MySQL and Python installed and your connection string updated, you can run the python script  to create the development database: `python database/db_setup.py`.

## Running Tests
To run the tests, have your workstation environment configured, then run `pytest`.