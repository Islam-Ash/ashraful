using ConsoleApplication.Methods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication
{
    public class Admin
    {
        public static bool AdminLogin(string username, string password)
        {
            using (var db = new AppDbContext())
            {
                var admin = db.Users.FirstOrDefault(u => u.UserName == username && u.Password == password && u.Role == UserRole.Admin);
                if (admin != null)
                {
                    Console.WriteLine("\nAdmin login successful!");
                    while (true)
                    {
                        Console.WriteLine("\nAdmin Menu:");
                        Console.WriteLine("1. Create Class");
                        Console.WriteLine("2. Create Subject");
                        Console.WriteLine("3. Create Teachers");
                        Console.WriteLine("4. View Classes");
                        Console.WriteLine("5. View Subjects");
                        Console.WriteLine("6. View Teachers");
                        Console.WriteLine("7. Logout\n");
                        Console.Write("Enter your choice: ");
                        string adminChoice = Console.ReadLine();

                        switch (adminChoice)
                        {
                            case "1":
                                ClassManagement.CreateClass();
                                break;
                            case "2":
                                SubjectManagement.CreateSubject();
                                break;
                            case "3":
                                TeacherManagement.CreateTeacher();
                                break;
                            case "4":
                                ViewClasses();
                                break;
                            case "5":
                                ViewSubjects();
                                break;
                            case "6":
                                ViewTeachers();
                                break;
                            case "7":
                                Console.WriteLine("Logging out...\n");
                                return true; 
                            default:
                                Console.WriteLine("Invalid choice. Please try again.\n");
                                break;
                        }
                    }

                }
                else
                {
                    Console.WriteLine("Admin login failed. Please try again.\n");
                }
            }

            return false; 
        }


        public static void ViewClasses()
        {
            ClassManagement.ViewClassesOnly();
           
            while (true)
            {
                Console.WriteLine("\nWhat you want to do?");
                Console.WriteLine("1. Edit Class");
                Console.WriteLine("2. Delete Class");
                Console.WriteLine("3. Assign Teacher to Class");
                Console.WriteLine("4. Go Back\n");
                Console.Write("Enter your choice: ");
                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        ClassManagement.EditClass();
                        break;
                    case "2":
                        ClassManagement.DeleteClass();
                        break;
                    case "3":
                        ClassManagement.AssignTeacherToClass();
                        break;
                    case "4":
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.\n");
                        break;
                }
            }

        }

        public static void ViewSubjects()
        {
            SubjectManagement.ViewSubjectsOnly();
            
            while (true)
            {
                Console.WriteLine("\nWhat you want to do?");
                Console.WriteLine("1. Edit Subject");
                Console.WriteLine("2. Delete Subject");
                Console.WriteLine("3. Assign Teacher to Subject");
                Console.WriteLine("4. Go Back\n");
                Console.Write("Enter your choice: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        SubjectManagement.EditSubject();
                        break;
                    case "2":
                        SubjectManagement.DeleteSubject();
                        break;
                    case "3":
                        SubjectManagement.AssignTeacherToSubject();
                        break;
                    case "4":
                        return;

                    default:
                        Console.WriteLine("Invalid choice. Please try again.\n");
                        break;
                }
            }
        }

        
    
        public static void ViewTeachers()
        {
            TeacherManagement.ViewTeachersOnly();
           
            while (true)
            {
                Console.WriteLine("\nWhat you want to do?");
                Console.WriteLine("1. Edit Teacher");
                Console.WriteLine("2. Delete Teacher");
                Console.WriteLine("3. Go Back\n");
                Console.Write("Enter your choice: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        TeacherManagement.EditTeacher();
                        break;
                    case "2":
                        TeacherManagement.DeleteTeacher();
                        break;

                    case "3":
                        return;

                    default:
                        Console.WriteLine("Invalid choice. Please try again.\n");
                        break;
                }
            }
        }

    }
}



