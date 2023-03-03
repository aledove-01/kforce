# HealthEquity + KForce

This challenge is about a basic application using Razor, JavaScript, and .NET. 
The idea of the project is that given a list of vehicles, the user guesses the price. If the guess is incorrect, it displays a red message. If it's correct, within +/- $5000, it shows a green message. 
The project is developed in ASP MVC and Razor Pages. 
It has controller tests, uses the repository design pattern, Entity Framework code-first, and SQL Server. 

The backend includes CRUD operations that can be tested using a tool like Postman (a JSON file with the controller calls is included, named kforce.postman_collection.json). Front-end screens CRUD were not requested. 

The appsettings.json file of the project must contain the connection string to the database.

### Also, the SQL code for the second section is included. 
<pre><code>
SELECT CONCAT(c.FirstName, ' ', c.LastName) AS FullName,
       c.Age,
       o.OrderID,
       o.DateCreated,
       o.MethodofPurchase AS PurchaseMethod,
       od.ProductNumber,
       od.ProductOrigin
FROM Customer c
JOIN Orders o ON c.PersonID = o.PersonID
JOIN OrderDetails od ON o.OrderID = od.OrderID
WHERE od.ProductID = '1112222333';
</code></pre>



