# Auditing Fields and Optimistic Concurrency

## Introduction

In modern web applications, it's essential to implement auditing and optimistic concurrency control to ensure data integrity and security. In this lesson, we will explore how to implement audit fields and optimistic concurrency in an ASP.NET Core application.

## Step 1: Setting up the ASP.NET Core Application

1. Create a new ASP.NET Core Web Application using Visual Studio or the .NET CLI:

2. Apply migrations to create the database:

## Step 2: Implementing Audit Fields

To implement audit fields, such as `CreatedBy`, `CreatedDate`, `LastModifiedBy`, and `LastModifiedDate`, follow these steps:

1. Create a base class for entities that include audit fields:

```csharp
public abstract class AuditableEntity
{
    [ScaffoldColumn(false)]
    public string CreatedBy { get; set; }

    [ScaffoldColumn(false)]
    public DateTime CreatedDate { get; set; }

    [ScaffoldColumn(false)]
    public string LastModifiedBy { get; set; }

    [ScaffoldColumn(false)]
    public DateTime? LastModifiedDate { get; set; }
}
```

2. Inherit from the `AuditableEntity` class in your domain models:

```csharp
public class YourEntity : AuditableEntity
{
    // Add your entity properties here
}
```

3. Override `SaveChanges` method in your `DbContext` to automatically populate audit fields:

```csharp
public override int SaveChanges()
{
    var entries = ChangeTracker.Entries()
        .Where(e => e.Entity is AuditableEntity && (e.State == EntityState.Added || e.State == EntityState.Modified));

    foreach (var entry in entries)
    {
        var entity = (AuditableEntity)entry.Entity;

        if (entry.State == EntityState.Added)
        {
            entity.CreatedDate = DateTime.UtcNow;
            entity.CreatedBy = "Username"; // Replace with actual user information
        }
        else
        {
            entity.LastModifiedDate = DateTime.UtcNow;
            entity.LastModifiedBy = "Username"; // Replace with actual user information
        }
    }

    return base.SaveChanges();
}
```

## Step 3: Implementing Optimistic Concurrency with Rowversion/Timestamp

1. Add a `RowVersion` property to your entity class:

```csharp
public class YourEntity : AuditableEntity
{
    // Other properties

    [Timestamp]
    [ScaffoldColumn(false)]
    public byte[] RowVersion { get; set; }
}
```

2. When updating an entity, make sure to include the `RowVersion` in the form or request payload.

3. When processing the update request, check the `RowVersion` in the database against the one provided in the request. If they don't match, it means another user has modified the entity concurrently, and you should handle the conflict accordingly (e.g., by informing the user or merging changes).