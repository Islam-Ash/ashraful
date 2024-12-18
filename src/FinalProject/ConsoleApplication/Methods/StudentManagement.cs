using ConsoleApplication.EntitiyModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication.Methods
{
    public class StudentManagement
    {
        public static void CreateStudent(int teacherId)
        {
            using (var db = new AppDbContext())
            {
                var teacher = db.Users.FirstOrDefault(t => t.UserId == teacherId);
                string teacherName = teacher != null ? teacher.UserName : "Teacher";

                var teacherClasses = db.TeacherClassAssignments
                    .Where(tca => tca.TeacherId == teacherId)
                    .Select(tca => tca.ClassId)
                    .ToList();

                if (teacherClasses.Count == 0)
                {
                    Console.WriteLine( "|-------------------------------------------------------|");
                    Console.WriteLine($"|  {teacher.UserName} Is Not Assigned To Any Classes.   |");
                    Console.WriteLine( "|-------------------------------------------------------|");
                    return;
                }

                Console.WriteLine("|--------------------------------------------|");
                Console.WriteLine($"|   Classes assigned to {teacher.UserName}: |");
                Console.WriteLine("|--------------------------------------------|");

                foreach (var classId in teacherClasses)
                {
                    var className = db.Classes.Find(classId)?.ClassName;
                    if (className != null)
                        Console.WriteLine($"- {className}");
                }

                Console.Write("\nEnter The Class Name To Add The Student To: ");
                var classNameInput = Console.ReadLine();

                
                var selectedClass = db.Classes.FirstOrDefault(c => c.ClassName == classNameInput && teacherClasses.Contains(c.ClassId));
                if (selectedClass == null)
                {
                    Console.WriteLine("|--------------------------------------------------------------|");
                    Console.WriteLine($"|  {teacher.UserName} Is Not Assigned To The Selected Class.  |");
                    Console.WriteLine("|--------------------------------------------------------------|");
                   
                    return;
                }

                Console.Write("\nEnter Student's Name: ");
                var studentName = Console.ReadLine();

                Console.Write("Enter Student's Roll Number: ");
                var rollNumber = Console.ReadLine();

               
                var newStudent = new Student
                {
                    StudentName = studentName,
                    RollNumber = rollNumber,
                    ClassId = selectedClass.ClassId
                };

                db.Students.Add(newStudent);
                db.SaveChanges();

                Console.WriteLine("|-------------------------------------|");
                Console.WriteLine("|    Student Created successfully!    |");
                Console.WriteLine("|-------------------------------------|");
            }
        }


        public static void EditStudent(int teacherId)
        {
            using (var db = new AppDbContext())
            {
                var teacher = db.Users.FirstOrDefault(t => t.UserId == teacherId);
                string teacherName = teacher != null ? teacher.UserName : "Teacher";

                var assignedClasses = db.TeacherClassAssignments
                                        .Where(tca => tca.TeacherId == teacherId)
                                        .Select(tca => tca.Class)
                                        .ToList();

                if (assignedClasses.Count == 0)
                {

                    Console.WriteLine("|-------------------------------------------------------|");
                    Console.WriteLine($"|  {teacher.UserName} Is Not Assigned To Any Classes.  |");
                    Console.WriteLine("|-------------------------------------------------------|");
                   
                    return;
                }

                Console.WriteLine("|--------------------------------------------|");
                Console.WriteLine($"|   Classes assigned to {teacher.UserName}: |");
                Console.WriteLine("|--------------------------------------------|");
               
                foreach (var assignedClass in assignedClasses)
                {
                    Console.WriteLine($"{assignedClass.ClassName}");
                }

                Console.Write("\nEnter The Name Of The Class To Edit The Student In: ");
                string className = Console.ReadLine();

                var selectedClass = assignedClasses.FirstOrDefault(c => c.ClassName == className);
                if (selectedClass == null)
                {
                    Console.WriteLine("|--------------------------------------------------------------|");
                    Console.WriteLine($"|  {teacher.UserName} Is Not Assigned To The Selected Class.  |");
                    Console.WriteLine("|--------------------------------------------------------------|");
                  
                    return;
                }

                Console.Write("\nEnter The Roll Number Of The Student To Edit: ");
                string rollNumber = Console.ReadLine();

                var studentToEdit = db.Students.FirstOrDefault(s => s.RollNumber == rollNumber && s.ClassId == selectedClass.ClassId);
                if (studentToEdit == null)
                {
                    Console.WriteLine("|--------------------------------------------------------------|");
                    Console.WriteLine("|          Student Not Found In The Selected Class..           |");
                    Console.WriteLine("|--------------------------------------------------------------|");
                    
                    return;
                }

                Console.WriteLine("Enter New Student Details:");

                Console.Write($"Student Name ({studentToEdit.StudentName}): ");
                string studentName = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(studentName))
                    studentToEdit.StudentName = studentName;

                db.SaveChanges();

                Console.WriteLine("|-------------------------------------|");
                Console.WriteLine("|     Student Edited successfully!    |");
                Console.WriteLine("|-------------------------------------|");
                
            }
        }

        public static void DeleteStudent(int teacherId)
        {
            using (var db = new AppDbContext())
            {
                var teacher = db.Users.FirstOrDefault(t => t.UserId == teacherId);
                string teacherName = teacher != null ? teacher.UserName : "Teacher";

                var assignedClasses = db.TeacherClassAssignments
                                        .Where(tca => tca.TeacherId == teacherId)
                                        .Select(tca => tca.Class)
                                        .ToList();

                if (assignedClasses.Count == 0)
                {
                    Console.WriteLine("|-------------------------------------------------------|");
                    Console.WriteLine($"|  {teacher.UserName} Is Not Assigned To Any Classes.  |");
                    Console.WriteLine("|-------------------------------------------------------|");
                   
                    return;
                }

                Console.WriteLine("|--------------------------------------------|");
                Console.WriteLine($"|   Classes assigned to {teacher.UserName}: |");
                Console.WriteLine("|--------------------------------------------|");
               
                foreach (var assignedClass in assignedClasses)
                {
                    Console.WriteLine($"{assignedClass.ClassName}");
                }

                Console.Write("\nEnter The Name Of The Class To Delete The Student From: ");
                string className = Console.ReadLine();

                var selectedClass = assignedClasses.FirstOrDefault(c => c.ClassName == className);
                if (selectedClass == null)
                {
                    Console.WriteLine("|--------------------------------------------------------------|");
                    Console.WriteLine($"|  {teacher.UserName} Is Not Assigned To The Selected Class.  |");
                    Console.WriteLine("|--------------------------------------------------------------|");
                    
                    return;
                }

                Console.Write("\nEnter The Roll Number Of The Student To Delete: ");
                string rollNumber = Console.ReadLine();

                var studentToDelete = db.Students.FirstOrDefault(s => s.RollNumber == rollNumber && s.ClassId == selectedClass.ClassId);
                if (studentToDelete == null)
                {
                    Console.WriteLine("|--------------------------------------------------------------|");
                    Console.WriteLine("|          Student Not Found In The Selected Class..           |");
                    Console.WriteLine("|--------------------------------------------------------------|");
                   
                    return;
                }

                db.Students.Remove(studentToDelete);
                db.SaveChanges();

                Console.WriteLine("|-------------------------------------|");
                Console.WriteLine("|    Student Deleted successfully!    |");
                Console.WriteLine("|-------------------------------------|");
               
            }
        }

        public static void ViewStudents(int teacherId)
        {
            using (var db = new AppDbContext())
            {
                var teacher = db.Users.FirstOrDefault(t => t.UserId == teacherId);
                string teacherName = teacher != null ? teacher.UserName : "Teacher";

                var assignedClasses = db.TeacherClassAssignments
                                        .Where(tca => tca.TeacherId == teacherId)
                                        .Select(tca => tca.Class)
                                        .ToList();

                if (assignedClasses.Count == 0)
                {
                    Console.WriteLine("|-------------------------------------------------------|");
                    Console.WriteLine($"|  {teacher.UserName} Is Not Assigned To Any Classes.   |");
                    Console.WriteLine("|-------------------------------------------------------|");
                   
                    return;
                }

                Console.WriteLine("|--------------------------------------------|");
                Console.WriteLine($"|   Classes assigned to {teacher.UserName}: |");
                Console.WriteLine("|--------------------------------------------|");
               
                foreach (var assignedClass in assignedClasses)
                {
                    Console.WriteLine($"{assignedClass.ClassName}");
                }

                Console.Write("\nEnter The Name Of The Class To View Students: ");
                string className = Console.ReadLine();

                var selectedClass = assignedClasses.FirstOrDefault(c => c.ClassName == className);
                if (selectedClass == null)
                {
                    Console.WriteLine("|--------------------------------------------------------------|");
                    Console.WriteLine($"|  {teacher.UserName} Is Not Assigned To The Selected Class.  |");
                    Console.WriteLine("|--------------------------------------------------------------|");
                    
                    return;
                }

                var students = db.Students.Where(s => s.ClassId == selectedClass.ClassId).ToList();
                if (students.Count == 0)
                {
                    Console.WriteLine("|--------------------------------------------------------------|");
                    Console.WriteLine("|           No Student Found In The Selected Class.            |");
                    Console.WriteLine("|--------------------------------------------------------------|");
                   ;
                    return;
                }

                Console.WriteLine("|----------------------------------------------------|");
                Console.WriteLine("|          Student In The Selected Class.            |");
                Console.WriteLine("|----------------------------------------------------|");
               
                foreach (var student in students)
                {
                    Console.WriteLine($"Name: {student.StudentName}, Roll Number: {student.RollNumber}");
                }
            }
        }

    }
}
