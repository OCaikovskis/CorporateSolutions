Framework: ASP.NET Core Web API (.NET 9)

Testing Tool: Postman (for manual testing)

Authentication: JWT (Token-based)

Database: EF Core with migrations + seeding

User Authentication
POST /api/auth/login
Accepts Username, Password

Returns JWT token if valid
Error if invalid

Roles:
Admin: Full access
User: Read-only access

Product Management
Database table: product
Fields: title, quantity, price

Endpoints:
GET api/products – returns list
GET api/products/{id} – returns product
POST api/products – create (admin only)
PUT api/products/{id} – update (admin only)
DELETE api/products/{id} – delete (admin only)

Audit Logging
Table: audit
Fields: product ID, changed fields, old/new values, user, timestamp

GET api/audit?from=yyyy-mm-dd&to=yyyy-mm-dd
Filter by modification date

Unit Testing
Target: VAT calculation logic

Framework: xUnit