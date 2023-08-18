# BasicRepos
A project to build .NET repository services more quickly

## Getting Started

To add Basic Repositories to your service collection simply use `AddBasicRepos<TDbContext>()` on the Service Collection. For example:

```csharp
builder.Services.AddBasicRepos<MyDbContext>();
```

This will add a repository registration for every `DbSet<T>` on the target `DbContext`. 

> To add support for `DbSet<T>`s in another `DbContext`, simply call `AddBasicRepos<TDbContext>()` again with the second type.
>
> For example:
>
> ```csharp
> builder.Services.AddBasicRepos<MyDbContext>()
>                 .AddBasicRepos<MyOtherDbContext>();
> ```

Using `AddBasicRepos<MyDbContext>()` will register each of the following repository types for each `DbSet<T>`:

- `IRepository<T>`: repository with read and write abilities on the underlying data store, with no cache
- `IReadOnlyRepository<T>`: repository with only the ability to read from the underlying data store
- `ICachedRepository<T>`: repository which wraps an in-memory cache of the items it manages