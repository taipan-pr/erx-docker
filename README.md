# erx-docker
Project setup for testing CI/CD pipeline with containers

## What it does
There are 5 C# projects in the solution
- Erx.Api
- Erx.Database
- Erx.Consumer
- Erx.Queue
- Erx.Seeder

## Steps
1. Clone project
2. Run `docker-compose up --build -d`

## How to use
1. Make `GET` request to `http://localhost:9080/Token` to get a key
2. Make `POST` request to `http://localhost:9080/Queue` to save message to database
    - Use the key from step 1 in `Authorization` header for example: `Authorization: dbbacd6c-4724-4b20-890e-2797b5bc4e93`
    - Use the following JSON as request body
    ```json
    {
      "message": "Bangkok"
    }
    ```
    - If message is `Bangkok` or `Phuket` column `MappingId` in `dbo.Messages` will get an ID from `dbo.Cities`
3. Use [SSMS](https://docs.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms?view=sql-server-ver15) to connect to MSSQL instance
    - Server name: `localhost,4444`
    - Login: `sa`
    - Password: `Passw@rd`
4. Inspect `dbo.Messages` table to see if the meesage is being saved

Postman collection is also available [here](https://www.getpostman.com/collections/47ea0f1e6699785524ac)
