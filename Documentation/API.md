# API Documentation
Most of the AI endpoints use HTTP POST method as to prevent inclusion of sensitive information in the URL. Authentication and usage is explained in their sections.

All Inputs are expected to be JSON and all outputs are JSON objects.

## Endpoints
- `addAccount` **[POST]**
    - NOTE: **Requires Admin Access**
    - Parameters
        - int `accountType`: The numerical Account Type to add
        - list(int) `users`: List of UserIDs to add to account
    - Returns: Account Information
        ```
        {
            "accountID": int,
            "accountType": string,
            "currentBalanceDollars": int,
            "currentBalanceCents": int,
            "users": [
                {"userID": int,
                "email": string,
                "firstName": string,
                "lastName": string}, ...
            ]
        }
        ```
- `addAccountUser` **[POST]**
    - NOTE: **Requires Admin Access**
    - Parameters
        - int `accountID`: The Account ID to modify
        - list(int) `users`: List of UserIDs to add to account
    - Returns: Updated Account Information
        ```
        {
            "accountID": int,
            "accountType": string,
            "currentBalanceDollars": int,
            "currentBalanceCents": int,
            "users": [
                {"userID": int,
                "email": string,
                "firstName": string,
                "lastName": string}, ...
            ]
        }
        ```
- `addTransaction` **[POST]**
    - NOTE: **Requires Admin Access**
    - Parameters
        - int `accountID`
        - int `amountDollars`
        - int `amountCents`
        - int `timeMonth` (optional - default: today)
        - int `timeDay` (optional - default: today)
        - int `timeYear` (optional - default: today)
        - string `locationStCd`
        - string `countryCd`
        - string `vendor`
    - Returns: A transaction
        ```
        {
            "transactionID": int, 
            "timeMonth": int, 
            "timeDay": int, 
            "timeYear": int, 
            "amountDollars": int, 
            "amountCents": int, 
            "endBalanceDollars": int, 
            "endBalanceCents": int, 
            "locationStCd": string,
            "countryCd": string,
            "vendor": string,
            "vendorDescription": string,
            "vendorCategory": string
        }
        ```
- `addTransfer` **[POST]**
    - Parameters
        - int `fromAccountID`
        - int `toAccountID`
        - int `amountDollars`
        - int `amountCents`
    - Returns: A transaction
        ```
        {
            "transactionID": int, 
            "timeMonth": int, 
            "timeDay": int, 
            "timeYear": int, 
            "amountDollars": int, 
            "amountCents": int, 
            "endBalanceDollars": int, 
            "endBalanceCents": int, 
            "locationStCd": string,
            "countryCd": string,
            "vendor": string,
            "vendorDescription": string,
            "vendorCategory": string
        }
        ```
- `addUser` **[POST]**
    - NOTE: **Requires Admin Access**
    - Parameters
        - string `email`
        - string `firstName`
        - string `lastName`
        - string `addressLine1`: address line 1 of user
        - string `addressLine2`: address line 2 of user
        - string `city`: city of residence        
        - string `postalState`: two letter postal code of state
    - Returns: User Information
        ```
        {
            "userID": int,
            "email": string,
            "firstName": string,
            "lastName": string,
            "SSN": int,
            "addressLine1" string,
            "addressLine2" string,
            "city" string,
            "postalState" string
        }
        ```
- `deleteAccount` **[POST]**
    - NOTE: **Requires Admin Access**
    - Parameters
        - int `accountID`: AccountID to remove
    - Returns: Confirmation
- `deleteAccountUser` **[POST]**
    - NOTEs: 
        - **Requires Admin Access**
        - CANNOT remove all users
    - Parameters
        - int `accountID`: The Account ID to modify
        - list(int) `users`: List of UserIDs to remove from the account
    - Returns: Updated Account Information
        ```
        {
            "accountID": int,
            "accountType": string,
            "currentBalanceDollars": int,
            "currentBalanceCents": int,
            "users": [
                {"userID": int,
                "email": string,
                "firstName": string,
                "lastName": string}, ...
            ]
        }
        ```
- `deleteUser` **[POST]**
    - NOTEs: 
        - **Requires Admin Access**
        - User CANNOT have an open account
    - Parameters
        - int `ID`: UserID to remove
    - Returns: Confirmation
- `getAccount` **[POST]**
    - Parameters
        - int `ID`: The Account ID to get more information for
    - Returns: Information about a account (email is masked)
        ```
        {
            "accountID": int,
            "accountType": string,
            "currentBalanceDollars": int,
            "currentBalanceCents": int,
            "users": [
                {"userID": int,
                "email": string,
                "firstName": string,
                "lastName": string}, ...
            ]
        }
        ```
- `getAccount` **[POST]**
    - Parameters
        - int `Token`: The Account Token (generated from a user lookup) to get more information for
    - Returns: Information about a account (email and account number is masked)
        ```
        {
            "accountID": string,
            "accountType": string,
            "currentBalanceDollars": int,
            "currentBalanceCents": int,
            "users": [
                {"userID": int,
                "email": string,
                "firstName": string,
                "lastName": string}, ...
            ]
        }
        ```
- `getAccountSummary` **[POST]**
    - Parameters
        - int `token`: The Account Token (generated from a user lookup) to get more information for
        - string `timePeriodType`: Type of time the period is in (`D`: Days, `M`: Months, `Y`: Years)
        - int `timePeriod`: Length of time to generate report for
    - Returns: Summary information about a account transactions over past time period
        ```
        {
            "accountID": int,
            "accountType": string,
            "currentBalanceDollars": int,
            "currentBalanceCents": int,
            "previousBalanceDollars": int,
            "previousBalanceCents": int,
            "categories": [
                {"category": string,
                "amountDollars": int,
                "amountCents": int,
                "vendors": [
                    {"vendor": string,
                    "amountDollars": int,
                    "amountCents": int}, ...
                ]}, ...
            ]
        }
        ```
- `getTransaction` **[POST]**
    - Parameters
        - int `ID`: The Transaction ID to get more information for
    - Returns: A transaction
        ```
        {
            "transactionID": int, 
            "timeMonth": int, 
            "timeDay": int, 
            "timeYear": int, 
            "amountDollars": int, 
            "amountCents": int, 
            "endBalanceDollars": int, 
            "endBalanceCents": int,
            "locationStCd": string,
            "countryCd": string,
            "vendor": string,
            "vendorDescription": string,
            "vendorCategory": string
        }
        ```
- `getTransactionHistory` **[POST]**
    - Parameters
        - int `ID`: The Account ID to get transaction history for
        - int `pageSize`: The number of results to return per page
        - int `pageNumber` (default = 0): The page of results you want to view
    - Returns: List of transactions from newest to oldest
        ```
        {
            "Transactions": [
                {"transactionID": int, 
                "timeMonth": int, 
                "timeDay": int, 
                "timeYear": int, 
                "amountDollars": int, 
                "amountCents": int, 
                "endBalanceDollars": int, 
                "endBalanceCents": int, 
                "vendor": string}, ...
            ]
        } 
        ```
- `getTransactionHistory` **[POST]**
    - Parameters
        - int `token`: The Account token to get transaction history for
        - int `pageSize`: The number of results to return per page
        - int `pageNumber` (default = 0): The page of results you want to view
    - Returns: List of transactions from newest to oldest
        ```
        {
            "Transactions": [
                {"transactionID": int, 
                "timeMonth": int, 
                "timeDay": int, 
                "timeYear": int, 
                "amountDollars": int, 
                "amountCents": int, 
                "endBalanceDollars": int, 
                "endBalanceCents": int, 
                "vendor": string}, ...
            ]
        } 
        ```
- `getUser` **[POST]**
    - Parameters
        - int `ID`: The User ID to get more information for
    - Returns: Information about a user (with masked email and account numbers), and the account token can be used to get account information
        ```
        {
            "userID": int,
            "email": string,
            "firstName": string,
            "lastName": string,
            "accounts": [
                {"accountID": string,
                "accountType": string,
                "accountToken": int,
                "currentBalanceDollars": int,
                "currentBalanceCents": int}, ...
            ]
        }
        ```
- `getUserPrivileged` **[POST]**
    - Parameters
        - int `ID`: The User ID to get more information for
    - Returns: Information about a user (with sensitive information and no masking)
        ```
        {
            "userID": int,
            "email": string,
            "firstName": string,
            "lastName": string,
            "SSN": int,
            "addressLine1" string,
            "addressLine2" string,
            "city" string,
            "postalState" string,
            "accounts": [
                {"accountID": string,
                "accountType": string,
                "currentBalanceDollars": int,
                "currentBalanceCents": int,
                "token": int}, ...
            ]
        }
        ```
- `updateUser` **[POST]**
    - NOTE: **Requires Admin Access**
    - Parameters (all optional except UserID)
        - int `ID`: UserID of user account you wish to update
        - string `email`
        - string `firstName`
        - string `lastName`
        - int `SSN`: user social security number
        - string `addressLine1`: address line 1 of user
        - string `addressLine2`: address line 2 of user
        - string `city`: city of residence        
        - string `postalState`: two letter postal code of state
    - Returns: Updated User Information (unmasked)
        ```
        {
            "userID": int,
            "email": string,
            "firstName": string,
            "lastName": string,
            "SSN": int,
            "addressLine1" string,
            "addressLine2" string,
            "city" string,
            "postalState" string
        }
        ```