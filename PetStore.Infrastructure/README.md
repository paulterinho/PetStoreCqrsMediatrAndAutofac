# Infrastructure

## Setup

### Step 1) Run DB Scripts
Figure out which DB you want to add the two tables, `petCommand.Pet` & `petQuery.Pet` to be located in
and run the `PetStore.Infrastructure/Scripts/Migrations/PetStoreMigrations_8-30-2021.sql` migration script.

### Step 2) Update the connection string.
`appsettings.json` is the config file this application looks at when it starts the API project. 

Make sure you alter that `PetStore.API/appsettings.json`'s `ConnectionStrings > DefaultConnection` value
so that the db name matches the one you've placed your tables in.

The connection string looks like this:  `data source=localhost; initial catalog=cxdw; integrated security=True;` 
make sure you replace the `cxdw` part of that string with the name of your database.
