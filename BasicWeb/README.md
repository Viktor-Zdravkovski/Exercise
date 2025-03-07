# Basic Web App

This is a basic web application that allows users to manage **Companies**, **Countries**, and **Contacts**. The app provides **CRUD (Create, Read, Update, Delete)** operations for these entities and includes additional functionalities like filtering contacts based on **Country** and **Company**.

### Features:
- **CRUD Operations** for **Company**, **Country**, and **Contact**
- **GetContactsWithCompanyAndCountry**: Get a list of contacts, including information about the associated company and country.
- **FilterContacts**: Filter contacts by **Country** and **Company**.
- **User Registration and Login**: Users can register and log in to the application.
- **Authorization**: Deleting a country requires the user to be logged in.

---

## API Endpoints

### 1. **User Registration & Login**

- **POST /api/auth/register**
    - Registers a new user.
    - **Request body**: 
      ```json
      {
        "email": "user@example.com",
        "password": "password123",
        "name": "John Doe"
      }
      ```

- **POST /api/auth/login**
    - Logs in an existing user and returns a JWT token.
    - Already existing in DB
    - **Request body**: 
      ```json
      {
        "email": "TestUser@gmail.com",
        "password": "TestUser"
      }
      ```

    - **Response**: 
      ```json
      {
        "token": "JWT_TOKEN_HERE"
      }
      ```

---

### 2. **Company CRUD Operations**

- **GET /api/companies**
    - Returns a list of all companies.

- **POST /api/companies**
    - Creates a new company.
    - **Request body**:
      ```json
      {
        "name": "Company Name"
      }
      ```

- **PUT /api/companies/{id}**
    - Updates an existing company by ID.
    - **Request body**:
      ```json
      {
        "name": "Updated Company Name"
      }
      ```

- **DELETE /api/companies/{id}**
    - Deletes a company by ID.

---

### 3. **Country CRUD Operations**

- **GET /api/countries**
    - Returns a list of all countries.

- **POST /api/countries**
    - Creates a new country.
    - **Request body**:
      ```json
      {
        "name": "Country Name"
      }
      ```

- **PUT /api/countries/{id}**
    - Updates an existing country by ID.
    - **Request body**:
      ```json
      {
        "name": "Updated Country Name"
      }
      ```

- **DELETE /api/countries/{id}**
    - Deletes a country by ID. This endpoint **requires the user to be logged in**.
    - **Authorization**: This action requires an authenticated user (JWT token in the Authorization header).

---

### 4. **Contact CRUD Operations**

- **GET /api/contacts**
    - Returns a list of all contacts.

- **POST /api/contacts**
    - Creates a new contact.
    - **Request body**:
      ```json
      {
        "companyId": 1,
        "countryId": 1,
        "name" : "Created Name"
      }
      ```

- **PUT /api/contacts/{id}**
    - Updates an existing contact by ID.
    - **Request body**:
      ```json
      {
        "id": 1,
        "Name": "Updated Name"
      }
      ```

- **DELETE /api/contacts/{id}**
    - Deletes a contact by ID.

---

### 5. **Advanced Features**

- **GET /api/contacts/company-country**
    - Fetches contacts with their associated **Company** and **Country**.
  
- **GET /api/contacts/filter**
    - Filters contacts by **CountryId** and **CompanyId**.
    - **Query parameters**:
      - `countryId`: The ID of the country to filter by.
      - `companyId`: The ID of the company to filter by.

    Example: `GET /api/contacts/filter?countryId=1&companyId=2`

---

## Authentication & Authorization

- The **Login** and **Register** endpoints are used for user authentication. After login, a **JWT token** is returned and should be included in the Authorization header of subsequent requests to endpoints that require authentication.

- **Authorization**: 
  - The **DELETE /api/countries/{id}** endpoint requires the user to be authenticated (logged in).
  - Pass the JWT token in the request header as follows:
    ```http
    Authorization: Bearer JWT_TOKEN_HERE
    ```

---

## Technologies Used

- **ASP.NET Core**: Web framework for the API.
- **Entity Framework Core**: ORM for database interactions.
- **SQL Server**: Database server.
- **JWT**: For user authentication.
- **Swagger**: For API documentation and testing.