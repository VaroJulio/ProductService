# ProductService

Service to manage a master of products for testing purposes.


## How to run the project
Browse to the PeoductService.Api folder. Ensure you are in the same level of the file ProductService.Api.csproj. If you verifid the previous steps, now continue opening a system console in the mentioned folder and execute the following commands:

```
dotnet build
 ```
 ```
dotnet run
```

Then, go to the [link](http://localhost:5217/swagger/index.html) 


## Architecture Used

### Clean Architecture
This Architecture helps us to create software that is simple, understandable, flexible, testable, and maintainable. Visit [here](https://blog.cleancoder.com/uncle-bob/2011/11/22/Clean-Architecture.html).

## Patterns used
[Here](https://deviq.com/) can find a good collection of resources describing various design pattern and elements of DDD.

### Aggregate Pattern
An aggregate is a collection of one or more related entities (and possibly value objects). Each aggregate has a single root entity, referred to as the aggregate root. The aggregate root is responsible for controlling access to all of the members of its aggregate. It's perfectly acceptable to single-entity aggregates, in which case that entity is itself the root of its aggregate. In addition to controlling access, the aggregate root is also responsible for ensuring the consistency of the aggregate. This is why it is important to ensure that the aggregate root does not directly expose its children, but rather controls access itself.

See [here](https://deviq.com/domain-driven-design/aggregate-pattern) for a more in depth discussion.

### Repository Pattern
It is a pattern to keep the data persistency out of the domain. The repository encapsulate the operations that we can perform on the database. This way, we achieve a separation of concerns. Visit [here](https://martinfowler.com/eaaCatalog/repository.html).

### Specification Pattern
Used to extend the capabilities of a Repository. It´s all about encapsulate query expressions into the specification object. This way we avoid to add a lot of customized queries to a repository pattern.

### REPR (Request End Point Response]) Pattern
The REPR pattern is a pattern for structuring the code for a request end point. It is a pattern that is used in the API Project. The pattern is described [here](https://deviq.com/design-patterns/repr-design-pattern).

## Logging and APM
For structured logging we use Serilog. We send this to two different sinks (console and file). In the case of the file logging, we only register the entries for the enpoints calling duration. The path of the file is the following: .logs/logRequestDuration.txt.