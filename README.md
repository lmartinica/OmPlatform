# OmPlatform

Online Management platform service for a basic online store. 
It provides RESTful APIs for browsing products and placing orders, built with OOP principles and SQL Server for data persistence. 
The platform is designed for scalability and maintainability using modern design patterns and Entity Framework (Code First).

## Architecture

| Folder                        | Description|
|-------------------------------|--------------------------------------------------------------------------------|
| Controllers                   | APIs for users, products and order management                                  |
| Core                          | Various classes for error and responses handling and some general classes      |
| DTOs                          | Each API will use a Create, Update and Get Dto model                           |
| Interfaces                    | Interfaces for service and repository classes                                  |
| Migrations                    | Entity framework database changes                                              |
| Models                        | Database models for each entity: User, Product, Order and OrderItem            |
| Queries                       | URL filters and parameters for entities (Only ListProducts API)                |
| Repositories                  | Database operations for each entity type                                       |
| Services                      | Business logic for each entity type and authentication                         |
| Snapshots                     | Images for database schema, Bruno collection (API testing), unit tests results |
| UnitTestProject               | Integration tests for authentication and authorization for all APIs            |
|                               | Unit tests for all service classes that are exposed in APIs                    |								 

## General Flow
- First the controller is called
- The controller passes information to service layer
- If data is needed from database, the service layer will call the repository layer
- Data will be mapped from database layer to service layer using DTOs
- Results are returned based on service layer logic handling
- The Result.cs class will handle potential errors that are returned and Dto objects for successful API calls
- All entities are having IDs of Guid type for better API security

### Authentication and Authorization
- In Controllers folder, AuthController.cs exposed 2 APIs: Register and Login
- The Register API will create a new user if the email address was not used before
- Login API will return a JWT token based on provided credentials
- The Auth service exposes method for generating JWT tokens
- The Current User service manages data access levels based on user role (admin or user)
- In UserService, createUser method passwords are hashed and stored securely in database
- All APIs have access Data annotations tags [Authorize] and [Authorize(Roles = Constants.Admin)]
- Only Auth APIs are accessible without JWT token

### User Management
- Admins can read, update or delete any user.
- Users can read and update only their own details
- Admins cannot be created from APIs, they can only be created directly in the database

### Product Management
- In Products controller only Admin users can add, update, and delete products
- Users can list and read product details
- The GetList API, supports various filters and binary search algorithm for efficient querying

### Order Placement
- Admins can read all orders that exists in the database
- Users can only have access to their own orders.
- Users can place orders using product IDs and quantity.
- Order controller and order service classes expose methods that show proper validation of orders:
	- Order is not allowed without at least one Product
	- For each product provided all must have proper requested stock availability in database
	- If any product in the order is not found, the entire order is rejected.
	- For proper saving in database, unit of work pattern is used to have a single transaction for:
		- updating products
		- order creation operation

### Database Implementation
- Tables were created with Entity Framework migrations based on the models defined
- Foreign keys and constraints were defined to maintain data integrity

### Design Patterns
- Repository Pattern: separation of database operations from bussiness logic
- UnitOfWork Pattern: creating orders and updating products in a single database transaction

### Reporting & Analytics
- Admins can generate various reports: sales, topProducts, topCustomers
- Reports are created with LINQ in Entity Framework in reports repository

### Performance Optimization & Unit Testing
- IMemoryCache is used to cache products, orders and users
- If specific database table is changed, cache is deleted and database read transaction will take place
- Unit tests were created using xUnit to verify each service functionality

## Setup

To run this application locally, follow these steps to get everything up and running
- Install packages:

| Main repository                                      | Test repository                                         |
|------------------------------------------------------|---------------------------------------------------------|
| Microsoft.AspNetCore.Authentication.JwtBearer        | Microsoft.AspNetCore.Mvc.Testing                        |
| Microsoft.EntityFrameworkCore.SqlServer              | Microsoft.Data.Sqlite.Core                              |
| Microsoft.EntityFrameworkCore.Tools                  | Microsoft.EntityFrameworkCore.Sqlite                    |
|                                                      | xUnit                                                   |

- Configure in appsettings.json default database connection string and JWT Key.
- For Testing, create a new test project and move the files from UnitTestProject folder inside the Test project.
- Also add reference in test project to main project

