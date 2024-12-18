using ConsoleApplication.EntitiyModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication.Methods
{
    public class ClassManagement
    {

        public static void CreateClass()
        {
            using (var db = new AppDbContext())
            {
                Console.Write("\nEnter New Class Name: ");
                string className = Console.ReadLine();

                var newClass = new Class { ClassName = className };
                db.Classes.Add(newClass);
                db.SaveChanges();
                Console.WriteLine("|--------------------------------------|");
                Console.WriteLine("|     Class Created Successfully!      |");
                Console.WriteLine("|--------------------------------------|\n");
               
            }
        }
        public static void ViewClassesOnly()
        {
            using (var db = new AppDbContext())
            {
                var classes = db.Classes.ToList();

                Console.WriteLine("List Of Classes:");
                foreach (var c in classes)
                {
                    Console.WriteLine(c.ClassName);
                }

            }
        }

        public static void AssignTeacherToClass()
        {
            using (var db = new AppDbContext())
            {
                Console.WriteLine("|--------------------------------------|");
                Console.WriteLine("|          Available Teachers          |");
                Console.WriteLine("|--------------------------------------|");
                
                TeacherManagement.ViewTeachersOnly();

                Console.Write("\nEnter Teacher's Username To Assign: ");
                string teacherUsername = Console.ReadLine();

                var teacher = db.Users.FirstOrDefault(u => u.UserName == teacherUsername && u.Role == UserRole.Teacher);

                if (teacher == null)
                {
                    Console.WriteLine("|--------------------------------------|");
                    Console.WriteLine("|        Teacher Is Not Found!         |");
                    Console.WriteLine("|--------------------------------------|\n");
                   
                    return;
                }

                Console.WriteLine("|--------------------------------------|");
                Console.WriteLine("|           Assigned Classes           |");
                Console.WriteLine("|--------------------------------------|");
               
                var assignedClasses = db.TeacherClassAssignments
                                        .Where(tca => tca.TeacherId == teacher.UserId)
                                        .Select(tca => tca.Class.ClassName)
                                        .ToList();

                if (assignedClasses.Any())
                {
                    foreach (var className in assignedClasses)
                    {
                        Console.WriteLine(className);
                    }
                }
                else
                {
                    Console.WriteLine("|----------------------------------------------|");
                    Console.WriteLine("|     No Class Assigned To This Teacher.    |");
                    Console.WriteLine("|----------------------------------------------|\n");
                   
                }

                Console.WriteLine("|--------------------------------------|");
                Console.WriteLine("|          Available Classes           |");
                Console.WriteLine("|--------------------------------------|");
               
                ViewClassesOnly();
                Console.WriteLine();

                while (true)
                {
                    Console.Write("\nEnter Class Name To Assign The Teacher (or type 'done' to finish): ");
                    string className = Console.ReadLine();

                    if (className.ToLower() == "done")
                        break;

                    var selectedClass = db.Classes.FirstOrDefault(c => c.ClassName == className);

                    if (selectedClass == null)
                    {
                        Console.WriteLine("|--------------------------------------|");
                        Console.WriteLine("|        Class Is Not Found!         |");
                        Console.WriteLine("|--------------------------------------|\n");
                        
                        continue;
                    }

                    var existingAssignment = db.TeacherClassAssignments
                                                .FirstOrDefault(tca => tca.TeacherId == teacher.UserId && tca.ClassId == selectedClass.ClassId);

                    if (existingAssignment != null)
                    {
                        Console.WriteLine($"The Teacher Is Already Assigned To {className} Class.\n");
                        continue;
                    }

                    var teacherClassAssignment = new TeacherClassAssignment
                    {
                        Teacher = teacher,
                        Class = selectedClass
                    };

                    db.TeacherClassAssignments.Add(teacherClassAssignment);
                    db.SaveChanges();

                    Console.WriteLine( "|---------------------------------------------------------------|");
                    Console.WriteLine($"|    Teacher {teacherUsername} Assigned To {className} Class.   |");
                    Console.WriteLine(" |---------------------------------------------------------------|");
                    
                }
            }
        }

        public static void EditClass()
        {
            using (var db = new AppDbContext())
            {
                Console.Write("\nEnter Class Name To Edit: ");
                string className = Console.ReadLine();

                var classToEdit = db.Classes.FirstOrDefault(c => c.ClassName == className);
                if (classToEdit != null)
                {
                    Console.Write("\nEnter New Class Name: ");
                    string newClassName = Console.ReadLine();

                    classToEdit.ClassName = newClassName;
                    db.SaveChanges();

                    Console.WriteLine("|--------------------------------------|");
                    Console.WriteLine("|      Class Edited Successfully!      |");
                    Console.WriteLine("|--------------------------------------|\n");
                    
                }
                else
                {
                    Console.WriteLine("|--------------------------------------|");
                    Console.WriteLine("|         Class Is Not Found!          |");
                    Console.WriteLine("|--------------------------------------|\n");
                    
                }
            }
        }

        public static void DeleteClass()
        {
            using (var db = new AppDbContext())
            {
                Console.Write("\nEnter Class Name To Delete: ");
                string className = Console.ReadLine();

                var classToDelete = db.Classes.FirstOrDefault(c => c.ClassName == className);
                if (classToDelete != null)
                {
                    db.Classes.Remove(classToDelete);
                    db.SaveChanges();

                    Console.WriteLine("|--------------------------------------|");
                    Console.WriteLine("|      Class Deleted Successfully!     |");
                    Console.WriteLine("|--------------------------------------|\n");
               
                }
                else
                {
                    Console.WriteLine("|--------------------------------------|");
                    Console.WriteLine("|         Class Is Not Found!          |");
                    Console.WriteLine("|--------------------------------------|\n");
                   
                }
            }
        }

    }
}
