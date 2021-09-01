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


## Highlights
### Autofac

This is an Inversion of Control (IOC) container that allows us to be lazy and more flexible on how we construct our applications. 

Check out the `Startup.cs` to see where this begins, and follow it's trail to the `AutofacStart.cs`

Also, DotNet 3+ has it's own IOC container built in. It may not be necessary to use Autofac. 

### MediatR
"MediatR Pattern/Library is used to reduce dependencies between objects. 

It allows in-process messaging,but it will not allow direct communication between objects. Instead of this it forces to communicate via MediatR only, such as classes that don't have dependencies on each other, that's why they are less coupled." (See quote url in Appendix)

#### Pipelines (Logging, Validation, Transactions)
In addition to the benefits of Inversion of control, you can also use Mediatr's Pipelines to address things like logging, validation, and even authentication if you wanted to. 

The pipeline sets up a series of handlers that will take action on each `Request` depending on how you configure them. 

Check out the `AutofacStart.cs` module for where configuration is happening. 

##### Logger
This Project has `LoggingBehavior.cs` configured to happen first thing, so any `Request` that comes in will be logged. 

##### Validation pre-Domain Layer.
Check out `CreatePetValidator.cs`

##### Cons
- Mediatr sadly only uses one thread. Perhaps this will change for even more speed. 
- For `Mediatr` notifications, sadly you can't throw execptions and catch them. If you use a `notification` be sure there won't be any errors. 

### "Fluent Validation"
This is an elegant library to ensure your business rules are being followed. Check out `CreatePetValidator.cs` to see the basic usage. 

### CQRS
Check out the Microsoft Articles on CQRS, and especially look at the eShops reference project they've posted (see appendix).

### OpenAPI
I've scaffolded the API & it's serializable objects via NSwag Studio using an OpenAPI document. Check out this document in `PetStore.OpenAPI`

### Presentation
Check out the Presentation called `README.presentation.pdf` in the project root. It shows highlights of this app.

#### APPENDIX:

- Author's Github: https://github.com/paulito-bandito
- ASCI Font URL: https://patorjk.com/software/taag/#p=display&f=Doom&t=Paul%20likes%20to%20move%20it%20move%20it
- Autofac:https://autofac.org/
- Fluent Validation: https://fluentvalidation.net/
- Mediator: https://github.com/jbogard/MediatR
- MediatR Quote: https://www.c-sharpcorner.com/article/introduction-to-mediatr-pattern/
- Microsoft CQRS & DDD Howto: https://docs.microsoft.com/en-us/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/apply-simplified-microservice-cqrs-ddd-patterns
- Microsoft eShops: https://docs.microsoft.com/en-us/dotnet/architecture/cloud-native/introduce-eshoponcontainers-reference-app