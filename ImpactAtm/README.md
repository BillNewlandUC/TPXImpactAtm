# TPXImpact ATM

The program is responsible for validating customer account details and performing basic
operations including balance inquiries and cash withdrawals. It is not responsible for 
the user interface.

## Getting Started

Running the program will open a CLI. The user is able to enter commands at the prompt 
which are processed when the user presses enter.

The ATM is started by entering a number which is the total funds available for dispensing
by the ATM, e.g.

    80000

A new user session is initiated by entering the customer's account number followed by the 
expected PIN and the the PIN they actually entered separated by spaces. Each new session
will start with an account balance and overdraft limit of zero, e.g.

    12345678 1234 1234

If the PIN numbers do not match the command will return ACCOUNT_ERR.

    12345678 1234 999
    ACCOUNT_ERR

Once the account has been initialised the customer's current balance and overdraft limit 
can be entered separeted by a space, e.g.

    500 100

To withdraw fund the command W is entered followed by the amount to be withdrawn separated 
by a space. The program will output the rmaining funds in the customer's acount E.g.

    W 100
    400

Withdrawals are only possible if:
1. The customer has enough available funds (balance + overdraft limit). If this condition is not met the command will return FUNDS_ERR.
2. The ATM has enough money to fulfil the transaction. If this condition is not met the command will return ATM_ERR.

Balance enquiries can be made using the command B, the program will then output the customer's current balalnce, e.g.

    B
    400


The user session is terminated by entering a blank line.

## Running the tests

There is a seperate project *ImpactAtmTests* which contains tests for the individual commans.

In the *ComanndHandlerTests* class there is a test called *Example_Success* which will run apply the commands 
in the specification and checks for the expected returns.

There are some further tests that test the following scenarios
- Incorrect PIN supplied
- Insufficient funds in the customer's account
- Insufficient funds in the ATM
- Customer uses overdraft facility resulting in a negative balance

