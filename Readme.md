# Payment Gateway API

## A RESTful API to manage payments with various banks.

### Getting Started

  You can either run the application via command prompt or open the solution in visual studio and run the application.

  Open up the command prompt and navigate to the PaymentGateway folder and run the command dotnet run. 

#### Swagger: http://localhost:62279/index.html

#### There are 2 main end points related to payments.

**1.** api/v1/Payments: This will allow you to submit new payment through payment gateway. The status code 201 will return if the payment transaction was processed by the bank either successfully or with failure. You can navigate to the newly created transaction with the url in response header called "location". In case of any validation errors, response will be 400 with the error details.

**2.** api/v1/Payments/{paymentId} : This will allow you to view the existing payment transaction record with status details. The card number is masked due to security reasons.

#### Notes:
  * BankSimulator/api/v1/Charge**: This endpoint is used to simulate the bank response.
  * Currently application is storing the transaction details in local folder called Data with TransactionId as the name of the file.
