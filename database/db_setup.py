import os
# mysql connector library
import mysql
# sqlalchemy is used to connect to db
import sqlalchemy as sql
from sqlalchemy.sql import text
from sqlalchemy.exc import IntegrityError
# cryptographic hash for user verification
import hashlib
# used for mapping two lists into dict
import functools
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

    with open('database/sql/init.sql', mode='r') as f:
        f_text = f.read()
    query = text(f_text)
    try:
        with engine.connect() as connection:
            connection.execute(query)
    except IntegrityError as e:
        print("Failed to create database")
        print(e)

    db_engine = sql.create_engine(f'mysql+mysqlconnector://{dbuser}:{pwd}@{host}:{port}/{dbname}')

    # CREATE USER TABLE
    with open("database/sql/create_user_table.sql", "r") as f:
        f_text = f.read()
    query = text(f_text)
    try:
        with db_engine.connect() as connection:
            connection.execute(query)
    except IntegrityError as e:
        print("Error creating table user")
        print(e)

    user_fields = ["Email", "UserID", "FirstName", "LastName", "Salt", "Pwd"]
    with open("database/data/USERS.csv", "r", encoding='utf-8-sig') as in_file:
        lines = in_file.readlines()[1:]
        for line in lines:
            elements = [x.strip() for x in line.strip().split(",")]
            elements[1] = int(elements[1])
            elements[5] = str(hashlib.sha3_256(bytes(''.join([elements[4], elements[5]]), 'utf=8')).digest())
            elements[4] = elements[4].encode('utf-8')
            kwargs = {k:v for k,v in zip(user_fields, elements)}
            with open("database/sql/add_user.sql", "r") as f:
                f_text = f.read()
            query = text(f_text)
            try:
                with db_engine.connect() as connection:
                    connection.execute(query, **kwargs)
            except IntegrityError:
                print("Error writing to table user")

    # CREATE SPI TABLE
    with open("database/sql/create_spi_table.sql", "r") as f:
        f_text = f.read()
    query = text(f_text)
    try:
        with db_engine.connect() as connection:
            connection.execute(query)
    except IntegrityError as e:
        print("Error creating table for SPI")
        print(e)

    spi_fields = ["UserID", "SSN", "AddressLine1", "AddressLine2", "City", "PostalState"]
    with open("database/data/SPI.csv", "r", encoding='utf-8-sig') as in_file:
        lines = in_file.readlines()[1:]
        for line in lines:
            elements = [x.strip() for x in line.strip().split(",")]
            elements[0] = int(elements[0])
            elements[1] = int(elements[1])
            elements[3] = elements[3] if elements[3] != "" else None
            kwargs = {k:v for k,v in zip(spi_fields, elements)}
            with open("database/sql/add_spi.sql", "r") as f:
                f_text = f.read()
            query = text(f_text)
            try:
                with db_engine.connect() as connection:
                    connection.execute(query, **kwargs)
            except IntegrityError as e:
                print(f"Error writing table for SPI")
                print(e)

    # CREATE ACCOUNT TYPES TABLE
    with open("database/sql/create_account_types_table.sql", "r") as f:
        f_text = f.read()
    query = text(f_text)
    try:
        with db_engine.connect() as connection:
            connection.execute(query)
    except IntegrityError as e:
        print("Error creating table for Account Types")
        print(e)

    account_types_fields = ["AccountType", "TypeDescription"]
    with open("database/data/ACCOUNT_TYPES.csv", "r", encoding='utf-8-sig') as in_file:
        lines = in_file.readlines()[1:]
        for line in lines:
            elements = [x.strip() for x in line.strip().split(",")]
            elements[0] = int(elements[0])
            kwargs = {k:v for k,v in zip(account_types_fields, elements)}
            with open("database/sql/add_account_type.sql", "r") as f:
                f_text = f.read()
            query = text(f_text)
            try:
                with db_engine.connect() as connection:
                    connection.execute(query, **kwargs)
            except IntegrityError as e:
                print(f"Error writing table for Account Types")
                print(e)

    # CREATE ACCOUNT TABLE
    with open("database/sql/create_account_table.sql", "r") as f:
        f_text = f.read()
    query = text(f_text)
    try:
        with db_engine.connect() as connection:
            connection.execute(query)
    except IntegrityError as e:
        print("Error creating table for Accounts")
        print(e)

    account_fields = ["AccountID", "AccountType"]
    with open("database/data/ACCOUNTS.csv", "r", encoding='utf-8-sig') as in_file:
        lines = in_file.readlines()[1:]
        for line in lines:
            elements = [x.strip() for x in line.strip().split(",")]
            elements[0] = int(elements[0])
            elements[1] = int(elements[1])
            kwargs = {k:v for k,v in zip(account_fields, elements)}
            with open("database/sql/add_account.sql", "r") as f:
                f_text = f.read()
            query = text(f_text)
            try:
                with db_engine.connect() as connection:
                    connection.execute(query, **kwargs)
            except IntegrityError as e:
                print(f"Error writing table for Accounts")
                print(e)

    # CREATE ACCOUNT_USER TABLE
    with open("database/sql/create_account_user_table.sql", "r") as f:
        f_text = f.read()
    query = text(f_text)
    try:
        with db_engine.connect() as connection:
            connection.execute(query)
    except IntegrityError as e:
        print("Error creating table for Accounts")
        print(e)

    account_fields = ["AccountID", "UserID"]
    with open("database/data/ACCOUNT_USERS.csv", "r", encoding='utf-8-sig') as in_file:
        lines = in_file.readlines()[1:]
        for line in lines:
            elements = [x.strip() for x in line.strip().split(",")]
            elements[0] = int(elements[0])
            elements[1] = int(elements[1])
            kwargs = {k:v for k,v in zip(account_fields, elements)}
            with open("database/sql/add_account_user.sql", "r") as f:
                f_text = f.read()
            query = text(f_text)
            try:
                with db_engine.connect() as connection:
                    connection.execute(query, **kwargs)
            except IntegrityError as e:
                print(f"Error writing table for Account Users")
                print(e)

    # CREATE VENDOR CATEGORY TABLE
    with open("database/sql/create_category_table.sql", "r") as f:
        f_text = f.read()
    query = text(f_text)
    try:
        with db_engine.connect() as connection:
            connection.execute(query)
    except IntegrityError as e:
        print("Error creating table for Categories")
        print(e)

    category_fields = ["Vendor", "Category"]
    with open("database/data/VENDOR_CAT.csv", "r", encoding='utf-8-sig') as in_file:
        lines = in_file.readlines()[1:]
        for line in lines:
            elements = [x.strip() for x in line.strip().split(",")]
            kwargs = {k:v for k,v in zip(category_fields, elements)}
            with open("database/sql/add_category.sql", "r") as f:
                f_text = f.read()
            query = text(f_text)
            try:
                with db_engine.connect() as connection:
                    connection.execute(query, **kwargs)
            except IntegrityError as e:
                print("Error writing table for Categories")
                print(e)

    # CREATE TRANSACTION TABLE
    with open("database/sql/create_transaction_table.sql", "r") as f:
        f_text = f.read()
    query = text(f_text)
    try:
        with db_engine.connect() as connection:
            connection.execute(query)
    except IntegrityError as e:
        print("Error creating table for Transactions")
        print(e)

    transaction_fields = ["TransactionID", "AccountID", "TimeMonth", "TimeDay", "TimeYear", 
                        "AmountDollars", "AmountCents", "EndBalanceDollars", "EndBalanceCents", 
                        "LocationStCd", "CountryCd", "Vendor"]
    with open("database/data/TRANSACTIONS.csv", "r", encoding='utf-8-sig') as in_file:
        lines = in_file.readlines()[1:]
        for line in lines:
            elements = [x.strip() for x in line.strip().split(",")]
            elements[0] = int(elements[0])
            elements[1] = int(elements[1])
            elements[2] = int(elements[2])
            elements[3] = int(elements[3])
            elements[4] = int(elements[4])
            elements[5] = int(elements[5])
            elements[6] = int(elements[6])
            elements[7] = int(elements[7])
            elements[8] = int(elements[8])
            kwargs = {k:v for k,v in zip(transaction_fields, elements)}
            with open("database/sql/add_transaction.sql", "r") as f:
                f_text = f.read()
            query = text(f_text)
            try:
                with db_engine.connect() as connection:
                    connection.execute(query, **kwargs)
            except IntegrityError as e:
                print(f"Error writing table for Transactions")
                print(e)

if __name__ == '__main__':
    main()