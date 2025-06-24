# OmPlatform

- Online Management platform service for a basic online store
- This project offers APIs for users to browse products and place orders
- It is designed with OOP principles and SQL Server for data persistence
- Functionalities are exposed via a RESTful API with various design patterns for maintainability and scalability 
- The APIs can be integrated and tested with Postman, Bruno and Fiddler
- Database is designed and mantained using code first approach from Entity Framework

## Arhitecture

| Folder                        | Description                                                                    |
|-------------------------------|--------------------------------------------------------------------------------|
| Controllers                   | HTTP endpoints for all APIs                                                    |
| Core                          | Various classes for error and responses handling and some general classes      |
| DTOs                          | Each endpoint will use a Create, Update and Get Dto model                      |
| Interfaces                    | Interfaces for service and repository classes                                  |

                  
- Migrations:                    Entity framework database changes
- Models:                        Database models for each entity: User, Product, Order and OrderItem
- Queries:                       URL filters and parameters for entities (Only ListProducts API)
- Repositories:                  Database operations for each entity type
- Services:                      Bussiness logic for each entity type and authentication
- Snapshots:                     Images for database schema, Bruno collection (API testing), unit tests results
- UnitTestProject:               Integrations tests for authentication and authorization for all endpoints
								 Unit tests for all service classes that are exposed in APIs

## General Flow
- First the controller is called
- The controller passes information to service layer
- If data is needed from database, the service layer will call the repository layer
- Data will be mapped from database layer to service layer using DTOs
- Results are returned based on service layer logic handling
- The Result.cs class will handle potential errors that are returned and Dto objects for succesful API calls
- All entities are having IDs of Guid type for better API security

### Authentication and Authorization
- In Controllers folder, AuthController.cs exposed 2 endpoints: Register and Login
- The Register endpoint will create a new user if the email adress was not used before
- Login endpoint will return a JWT token based on provided credentials
- The Auth service exposes method for generating Jwt tokens
- The Current User service elevates access if user role is admin
- In UserService, createUser method passwords are hashed and stored securely in database
- All Endpoints have access Data annotations tags [Authorize] and [Authorize(Roles = Constants.Admin)]
- Only Auth endpoints are allowed without Jwt token

### User Management
- Admins can read, update or delete any user.
- Users can read and update only their own details
- Admins cannot be created from APIs, they can only be created directly in the database

### Product Management
- In Products controller only Admin users can add, update, and delete products.
- Users can list and read product details.
- The GetList endpoint, supports various filters and binary search.

### Order Placement
- Admins can read all orders that exists in the database
- Users can only have access to their own orders.
- Users can place orders using product IDs and quantity.
- Order controller and order service classes expose methods that show proper validation of orders:
	- Order is not allowed without at least one Product
	- For each product provided all must have proper requested stock availability in database
	- If one product is not found, order is created
	- For proper saving in database, unit of work pattern is used for updating products and order creation operations

### Database Implementation
- Tables where created with Entity Framework migrations based on the models defined
- Foreign keys and constraints where defined to maintain data integrity.

### Design Patterns
- Repository Pattern: separation of database operations from bussines logic
- UnitOfWork Pattern: create order and update products in a single database transaction

### Reporting & Analytics
- Admins can generate various reports: sales, topProducts, topCustomers.
- Reports are created with LINQ in Entity Framework in reports repository.

### Performance Optimization & Unit Testing
- IMemoryCache is used to cache products, orders and users
- If specific database table is changed, cache is deleted and database read transaction will take place
- Unit tests where created using xUnit to verify each service functionality.

## Setup
- Install packages:
	- Main repository: 
		- Microsoft.AspNetCore.Authentication.JwtBearer
		- Microsoft.EntityFrameworkCore.SqlServer
		- Microsoft.EntityFrameworkCore.Tools
	- Test repository:
		- Microsoft.AspNetCore.Mvc.Testing
		- Microsoft.Data.Sqlite.Core
		- Microsoft.EntityFrameworkCore.Sqlite
		- xUnit
- Configure in "appsettings.json" default database connection string and Jwt Key.
- For Testing, create a new test project and move the files from "UnitTestProject" inside the Test project.
- Also add reference in test project to main project

