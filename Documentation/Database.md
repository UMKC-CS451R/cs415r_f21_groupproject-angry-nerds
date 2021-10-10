# Schmea Definition for Relational Database

## USER
Identify users of our services.

| Field | Type | Other |
| -- | -- | -- |
| Email | string(320) | Primary Key |
| UserID | unsigned int | Indexed |
| FirstName | string(50) | NA |
| LastName | string(50) | NA |
| Salt | BINARY(20) | NA |
| Password | BLOB(512) | NA |

## SPI
Tie our user information to protected information. This is a separate table to restrict unnecessary access.

| Field | Type | Other |
| -- | -- | -- |
| UserID | unsigned int | Primary Key and Foreign Key |
| AddressLine1 | varchar(100) | NA |
| AddressLine2 | varchar(100) | NULLABLE |
| City | varchar(100) | NA |
| PostalState | varchar(2) | NA |
| SSN | unsigned int | NA |

## NOTIFICATION
This would define the types of notification rules users can select from, describe the rule for the user, and provide a function name that actually implements the queries.

| Field | Type | Other |
| -- | -- | -- |
| FnName | string(64) | Primary Key |
| Name | string(64) | NA |
| Description | string(256) | NA |

## USER_NOTIFICATION
This should include entries to enable notification functions for users.

| Field | Type | Other |
| -- | -- | -- |
| UserID | unsigned int | Primary Key |
| FnName | string(64) | NA |

## ACCOUNT
This would tie our users to the accounts that they have ownership of.

| Field | Type | Other |
| -- | -- | -- |
| AccountID | unsigned int | Primary Key |
| AccountType | int | Enumerated Account Types - Foreign Key |

## ACCOUNT_TYPE
This would tie our users to the accounts that they have ownership of.

| Field | Type | Other |
| -- | -- | -- |
| AccountType | unsigned int | Primary Key |
| TypeDescription | string(100) | NA |

Possible account types:
1. Checking
2. Savings

## ACCOUNT_USER
Relation Table for Accounts to Users
Assumption: all users have same account access

| Field | Type | Other |
| -- | -- | -- |
| AccountID | unsigned int | Primary Key |
| UserID | unsigned int | Foreign Key |

## TRANSACTIONS
This would tie accounts to individual transactions

| Field | Type | Other |
| -- | -- | -- |
| TransactionID | unsigned int | Primary Key |
| AccountID | unsigned int | Foreign Key |
| TimeYear | unsigned smallint | NA |
| TimeMonth | unsigned smallint | NA |
| TimeDay | unsigned smallint | NA |
| AmountDollars | int | NA |
| AmountCents | int | NA |
| EndBalanceDollars | int | NA |
| EndBalanceCents | int | NA |
| LocationStCd | varchar(2) | NA |
| CountryCd | varchar(2) | NA |
| Vendor | VARCHAR(100) | Foreign Key |

## CATEGORY
List and description of transaction categories

| Field | Type | Other |
| -- | -- | -- |
| Category | unsigned int | Primary Key |
| Description | VARCHAR | NA |

## VENDOR_CAT
Tie vendors to transaction categories

| Field | Type | Other |
| -- | -- | -- |
| Vendor | VARCHAR(100) | Primary Key |
| Category | unsigned int | NA |

## ADMIN
Identify accounts with write access for accounts and transactions.

| Field | Type | Other |
| -- | -- | -- |
| AdminID | unsigned int | Primary Key |
| Salt | BINARY(20) | NA |
| Password | BLOB(512) | NA |

## Account_Tokens
Identify token aliases given to client to reference an account.

| Field | Type | Other |
| -- | -- | -- |
| UserID | unsigned int | Composite Primary Key |
| Token | unsigned int | Composite Primary Key AUTO-INCREMENT |
| AccountID | unsigned int | Foreign Key |
| TimeStamp | ts | DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP |