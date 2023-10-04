# Editing Records with Audit Fields and Optimistic Concurrency in ASP.NET Core MVC

In this lab, you will learn how to implement audit fields and optimistic concurrency control using Rowversion/Timestamp in an ASP.NET Core MVC application.

Due 10th October 2023. Please email me a copy of the completed assignment.

## Lab Tasks

### Task 1: Create a New ASP.NET Core MVC Project

1. Open Visual Studio.

2. Click on "Create a new project."

3. Search for "ASP.NET Core Web Application" in the project templates.

4. Choose the "ASP.NET Core Web Application" template and click "Next."

5. Configure your project as follows:
   - Project name: `NotebookApp`
   - Location: Choose a directory for your project.
   - Authentication: Individual User Accounts.

6. Click "Create" to create the project.

### Task 2: Create a Database Context

1. In the Solution Explorer, right-click on the "NotebookApp" project and select "Add" > "Class."

2. Name the class `ApplicationDbContext.cs` and click "Add."

3. Define the database context class as follows:

```csharp
using Microsoft.EntityFrameworkCore;

namespace NotebookApp.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Define your entity DbSet properties here
    }
}
```

### Task 3: Configure Connection String

1. Open the `appsettings.json` file and configure the connection string as follows:

```json
"ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=NotebookAppDb;Trusted_Connection=True;MultipleActiveResultSets=true"
}
```

### Task 4: Create a Notebook Entity

1. In the Solution Explorer, right-click on the "Models" folder and select "Add" > "Class."

2. Name the class `Notebook.cs` and click "Add."

3. Define the `Notebook` entity class with audit fields and Rowversion/Timestamp as follows:

```csharp
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NotebookApp.Models
{
    public class Notebook
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public string CreatedBy { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        public string LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}
```

### Task 5: Create a Notebooks Controller

1. In the Solution Explorer, right-click on the "Controllers" folder and select "Add" > "Controller."

2. Choose "MVC Controller with views, using Entity Framework" and click "Add."

3. Configure the controller as follows:
   - Model class: `Notebook (NotebookApp.Models)`
   - Data context class: `ApplicationDbContext (NotebookApp.Data)`
   - Controller name: `NotebooksController`

4. Click "Add" to create the controller.

### Task 6:  Implement Edit Action with Optimistic Concurrency

1. In the `Edit` action of the `NotebooksController`, implement optimistic concurrency handling by checking the `RowVersion` property.

```csharp
[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Content,RowVersion")] Notebook notebook)
{
    if (id != notebook.Id)
    {
        return NotFound();
    }

    var existingNotebook = await _context.Notebooks.FindAsync(id);

    if (existingNotebook == null)
    {
        return NotFound();
    }

    if (!ModelState.IsValid)
    {
        return View(notebook);
    }

    // Update audit fields
    existingNotebook.LastModifiedBy = "CurrentUserName"; // Replace with actual user information
    existingNotebook.LastModifiedDate = DateTime.UtcNow;

    // Implement optimistic concurrency check
    if (!notebook.RowVersion.SequenceEqual(existingNotebook.RowVersion))
    {
        ModelState.AddModelError(string.Empty, "Concurrency conflict: The record has been modified by another user.");
        return View(notebook);
    }

    try
    {
        _context.Update(existingNotebook);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
    catch (DbUpdateConcurrencyException)
    {
        ModelState.AddModelError(string.Empty, "Concurrency conflict: The record has been modified by another user.");
        return View(notebook);
    }
}
```

### Task 7: Test the Application

1. Build and run your ASP.NET Core MVC application.

2. Test the application by creating, editing, and deleting notebooks. Pay attention to the audit fields and optimistic concurrency handling.

3. Verify that the audit fields (CreatedBy, CreatedDate, LastModifiedBy, LastModifiedDate) are correctly populated when creating and editing notebooks.

4. Test the optimistic concurrency handling by opening two browser tabs and trying to edit the same notebook concurrently.

## Conclusion

In this lab, you learned how to implement audit fields and optimistic concurrency control using Rowversion/Timestamp in an ASP.NET Core MVC application. These techniques help ensure data integrity and security while allowing multiple users to edit data concurrently.