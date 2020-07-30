= Payment Gateway API

== A RESTful API to manage payments with various banks.

== Getting Started

* You can run the application via command prompt. Go to the PaymentGateway folder and run the command dotnet run. You should see the message on command prompt with listening on http://localhost:62279.
* Navigate to http://localhost:62279 to see the swagger page.

* There are 2 main end points related to payments.
1. /api/v1/Payments : This will allow you to post a new payment through payment gateway. It will return a payment transaction record with success or failure status. Response header should also give you location of the newly created transaction record. In case of failure, it will give you the information in the response with error code and error.
2. /api/v1/Payments/{paymentId} : This will allow you to view the existing payment transaction record with status.

* You can also simulate the bank processing through /BankSimulator/api/v1/Charge endpoint. This will allow you to mock the response from bank.
