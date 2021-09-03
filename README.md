# README.md
 <pre>
 ______    _     _____ _                 
 | ___ \  | |   /  ___| |                
 | |_/ /__| |_  \ `--.| |_ ___  _ __ ___ 
 |  __/ _ \ __|  `--. \ __/ _ \| '__/ _ \
 | | |  __/ |_  /\__/ / || (_) | | |  __/
 \_|  \___|\__| \____/ \__\___/|_|  \___|
</pre>                                         
 
Based on the Microsoft eShops example (see appendix below), this is a simpler example of how to leverage Mediatr and Autofac in a CQRS project.

Check out my presentation called `README.presentation.pdf` in the project root. It shows more highlights of this app.


## Setup

### 1) Run DB Scripts
Figure out which DB you want to add the two tables, `petCommand.Pet` & `petQuery.Pet` to be located in
and run the `PetStore.Infrastructure/Scripts/Migrations/PetStoreMigrations_8-30-2021.sql` migration script so those 
two tables exist.

Syntax assumes SQL Server.

### 2) Update the connection string.
`appsettings.json` is the config file this application looks at when it starts the API project. 

Make sure you alter that `PetStore.API/appsettings.json`'s `ConnectionStrings > DefaultConnection` value
so that the db name matches the one you've placed your tables in.

The connection string looks like this:  `data source=localhost; initial catalog=cxdw; integrated security=True;` 
make sure you replace the `cxdw` part of that string with the name of your database.

### 3) Run the API project.
Run the API project

### 4) Import the Postman file
Import the `PetStore.Postman.json` into postman to interact with the API project. 

#### Order of operations:
1. Create a Pet (do this a few times so you can test out the List Pets functionality)
2. Get a Pet by ResoureID.
3. List all Pets.

# Presentation
Check out the `README.presentation.pdf` for the highlights of this project.