# Payment Gateway API

## A RESTful API to manage payments with various banks.

### Getting Started

  You can either run the application via command prompt or open the solution in visual studio and run the application.

  Open up the command prompt and navigate to the PaymentGateway folder and run the command dotnet run. 

##### Swagger: http://localhost:62279/index.html

#### There are 2 main end points related to payments.

**1. api/v1/Payments:** This will allow you to submit new payment through payment gateway. API also supports basic input validations for card number, expiry year, month etc.
#### Response codes:
          201: New transaction record with success/failure payment status code. Location header with newly created resource url.
          400: Any validation error on payment request.
          500: Unknown error.

**2. api/v1/Payments/{paymentId}** : This will allow you to view the existing payment transaction record with status details. The card number is masked due to security reasons.
#### Response codes:
          200: Transaction record.
          404: Record not found.
          500: Unknown error.

#### Process diagram: [a relative link](PaymentGatewayAPI.png)

#### Notes:
  * BankSimulator/api/v1/Charge: This endpoint is used to simulate the bank response.
  * Currently application is storing the transaction details in local folder called Data with TransactionId as the name of the file.

#### Future enhancements:
1. Adding authentication token for post and get requests.
2. Containerisation.
