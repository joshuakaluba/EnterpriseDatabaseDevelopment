**Lab Title: Many-to-Many Relationships in MVC with Entity Framework**

Due 12th October 2023. Please email me a copy of the completed assignment.

**Objective:**
To understand and implement many-to-many relationships using MVC and Entity Framework in Visual Studio.

**Lab Steps:**

**Step 1: Create a new MVC Project**

1. Open Visual Studio.
2. Go to File -> New -> Project.
3. Select ASP.NET Core Web Application template.
4. Name your project "ManyToManyMVC", Authentication: Individual User Accounts, and click "Create."
5. Select the "Web Application (Model-View-Controller)" template and click "Create."

**Step 2: Create Models**

1. Right-click on the "Models" folder and add two new classes: `Student.cs` and `Course.cs`.

```csharp
// Student.cs
using System.Collections.Generic;

public class Student
{
    public int StudentId { get; set; }
    public string Name { get; set; }
    public List<StudentCourse> StudentCourses { get; set; }
}

// Course.cs
using System.Collections.Generic;

public class Course
{
    public int CourseId { get; set; }
    public string Title { get; set; }
    public List<StudentCourse> StudentCourses { get; set; }
}

// StudentCourse.cs (Join Entity)
public class StudentCourse
{
    public int StudentId { get; set; }
    public Student Student { get; set; }

    public int CourseId { get; set; }
    public Course Course { get; set; }
}
```

**Step 3: Create Controllers and Views**

1. Right-click on the "Controllers" folder and add two new controllers: `StudentsController` and `CoursesController` with CRUD actions.

2. Scaffold the views for both controllers using Entity Framework.

**Step 5: Update Controllers**
1. In `StudentsController`, add actions to handle enrollment and dis-enrollment of students in courses.

```csharp
// Enroll student in a course
public IActionResult Enroll(int studentId, int courseId)
{
    var student = _context.Students.Find(studentId);
    var course = _context.Courses.Find(courseId);
    if (student != null && course != null)
    {
        var studentCourse = new StudentCourse
        {
            Student = student,
            Course = course
        };
        _context.StudentCourses.Add(studentCourse);
        _context.SaveChanges();
    }
    return RedirectToAction("Index");
}

// Dis-enroll student from a course
public IActionResult Disenroll(int studentId, int courseId)
{
    var studentCourse = _context.StudentCourses
        .SingleOrDefault(sc => sc.StudentId == studentId && sc.CourseId == courseId);
    if (studentCourse != null)
    {
        _context.StudentCourses.Remove(studentCourse);
        _context.SaveChanges();
    }
    return RedirectToAction("Index");
}
```

2. Update `CoursesController` similarly.

**Step 6: Create Views for Enrollment**

1. In the "Views/Students" folder, create a new view called `Enroll`.

2. In the `Enroll.cshtml` view, display a list of courses and allow the user to enroll or disenroll students in courses.

**Step 7: Test the Application**

1. Build and run your application.

2. Navigate to the Students and Courses pages, enroll students in courses, and test the many-to-many relationship.

**Conclusion:**
In this lab, you've learned how to create a many-to-many relationship between Students and Courses using the MVC framework with Entity Framework in Visual Studio.