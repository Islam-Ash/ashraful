using ConsoleApplication.EntitiyModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication.Methods
{
    public class TeacherManagement
    {
        public static void CreateTeacher()
        {
            using (var db = new AppDbContext())
            {
                Console.Write("Enter New Teacher Username: ");
                string username = Console.ReadLine();

                Console.Write("Enter New Teacher Password: ");
                string password = Console.ReadLine();

                var newTeacher = new User { UserName = username, Password = password, Role = UserRole.Teacher };
                db.Users.Add(newTeacher);
                db.SaveChanges();

                Console.WriteLine("|-------------------------------------|");
                Console.WriteLine("|    Teacher Created successfully!    |");
                Console.WriteLine("|-------------------------------------|");
               
            }
        }
        public static void ViewTeachersOnly()
        {
            using (var db = new AppDbContext())
            {
                var teachers = db.Users.Where(u => u.Role == UserRole.Teacher).ToList();

                Console.WriteLine("List Of Teachers:");
                foreach (var teacher in teachers)
                {
                    Console.WriteLine($"Username: {teacher.UserName}");
                }
            }
        }

        public static void EditTeacher()
        {
            using (var db = new AppDbContext())
            {
                Console.Write("\nEnter Teacher Name To Edit: ");
                string teacherName = Console.ReadLine();

                var teacherToEdit = db.Users.FirstOrDefault(u => u.UserName == teacherName);
                if (teacherToEdit != null)
                {
                    Console.Write("Enter New Teacher Name: ");
                    string newTeacherName = Console.ReadLine();

                    teacherToEdit.UserName = newTeacherName;
                    db.SaveChanges();

                    Console.WriteLine("|-------------------------------------|");
                    Console.WriteLine("|    Teacher Edited Successfully!     |");
                    Console.WriteLine("|-------------------------------------|");
                    
                }
                else
                {
                    Console.WriteLine("|-------------------------------------|");
                    Console.WriteLine("|        Teacher Not Found.!          |");
                    Console.WriteLine("|-------------------------------------|");
                   
                }
            }
        }
        public static void DeleteTeacher()
        {
            using (var db = new AppDbContext())
            {
                Console.Write("Enter Username Of Teacher To Delete: ");
                string username = Console.ReadLine();

                var teacherToDelete = db.Users.FirstOrDefault(u => u.UserName == username && u.Role == UserRole.Teacher);

                if (teacherToDelete != null)
                {
                    db.Users.Remove(teacherToDelete);
                    db.SaveChanges();

                    Console.WriteLine("|-------------------------------------|");
                    Console.WriteLine("|    Teacher Deleted Successfully!    |");
                    Console.WriteLine("|-------------------------------------|");
                    
                }
                else
                {
                    Console.WriteLine("|-------------------------------------|");
                    Console.WriteLine("|        Teacher Not Found.!          |");
                    Console.WriteLine("|-------------------------------------|");
                    
                }
            }
        }

        public static void PrintTeacherAssignedSubjects(int teacherId)
        {
            using (var db = new AppDbContext())
            {
                var teacherAssignedSubjects = db.TeacherSubjectAssignments
                    .Where(tsa => tsa.TeacherId == teacherId)
                    .Select(tsa => new
                    {
                       
                        SubjectId = tsa.SubjectId,
                        SubjectName = tsa.Subject.SubjectName
                    })
                    .ToList();

                Console.WriteLine("|-----------------------------------------------|");
                Console.WriteLine("|     Teacher Assigned Subjects Information    |");
                Console.WriteLine("|-----------------------------------------------|");

                if (teacherAssignedSubjects.Any())
                {
                    foreach (var assignment in teacherAssignedSubjects)
                    {
                        Console.WriteLine($" Subject ID: {assignment.SubjectId}, Subject Name: {assignment.SubjectName}");
                    }
                }
                else
                {
                    Console.WriteLine("No subjects assigned to this teacher.");
                }
            }
        }
        
        




    }
}
