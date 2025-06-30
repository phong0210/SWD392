Implement the core data access patterns for a .NET application using Entity Framework Core with PostgreSQL.

- **Generic IRepository<T> Interface:** Define standard async CRUD operations: GetByIdAsync, ListAllAsync, AddAsync, UpdateAsync, DeleteAsync, ListAsync(ISpecification<T>).
- **Generic Repository<T> Base Class:** Implement the IRepository<T> interface using an EF Core DbContext.
- **IUnitOfWork Interface:** Expose IRepository<T> instances for each aggregate root (Product, Order, User). Include a `SaveChangesAsync()` method to handle transactions.
- **Specification Pattern:** Create an `ISpecification<T>` interface and a base `Specification<T>` class to define query criteria, includes, and ordering. This will be used to build complex, reusable queries without exposing IQueryable in the application layer.
- **EF Core Integration:** The implementation should be designed to work with a PostgreSQL database via the Npgsql EF Core provider.
