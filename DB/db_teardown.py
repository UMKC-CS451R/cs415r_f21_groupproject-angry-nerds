import os
# mysql connector library
import mysql
# sqlalchemy is used to connect to db
import sqlalchemy as sql
from sqlalchemy.sql import text
from sqlalchemy.exc import IntegrityError
import json

def main():
    # SET ENVIRONMENT VARIABLES FOR DB
    with open("appsettings.Development.json", "r") as f:
        config = json.load(f)
    conn_str = config["ConnectionStrings"]["localDB"]
    conn_elements = {item[0]: item[1] for item in [line.split("=") for line in conn_str.split(";")] if item != ['']}

    host = conn_elements["server"]
    port = 3306         # standard port
    dbuser = conn_elements["uid"]
    pwd = conn_elements["pwd"]
    dbname = "banking"

    # create SQL engine for DB
    engine = sql.create_engine(f'mysql+mysqlconnector://{dbuser}:{pwd}@{host}:{port}')

    with open('database/sql/delete_db.sql', mode='r') as f:
        f_text = f.read()
    query = text(f_text)
    try:
        with engine.connect() as connection:
            connection.execute(query)
    except IntegrityError:
        print("Failed to delette database")

if __name__ == '__main__':
    main()