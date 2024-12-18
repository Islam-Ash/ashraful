using ConsoleApplication.EntitiyModels;
using ConsoleApplication.Methods;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication
{
    public class Teacher
    {
        public static bool TeacherLogin(string username, string password)
        {
            using (var db = new AppDbContext())
            {
                var teacher = db.Users.FirstOrDefault(u => u.UserName == username && u.Password == password && u.Role == UserRole.Teacher);

                if (teacher != null)
                {
                    Console.WriteLine("|-------------------------------|");
                    Console.WriteLine("|   Teacher login successful!   |");
                    Console.WriteLine("|-------------------------------|\n");
                    Console.WriteLine($"Logged in as teacher: {teacher.UserName}\n");

                    while (true)
                    {
                        Console.WriteLine("\nPlease Select an Option:");
                        Console.WriteLine("1. Create Student");
                        Console.WriteLine("2. Edit Student");
                        Console.WriteLine("3. Delete Student");
                        Console.WriteLine("4. View Students");
                        Console.WriteLine("5. Assign Grades");
                        Console.WriteLine("6. View student Grades");
                        Console.WriteLine("7. View Class Grades");
                        Console.WriteLine("8. Logout\n");
                        Console.Write("Enter your choice: ");
                        string TeacherChoice = Console.ReadLine();

                        switch (TeacherChoice)
                        {
                            case "1":
                                StudentManagement.CreateStudent(teacher.UserId);
                                break;
                            case "2":
                                StudentManagement.EditStudent(teacher.UserId);
                                break;
                            case "3":
                                StudentManagement.DeleteStudent(teacher.UserId);
                                break;
                            case "4":
                                StudentManagement.ViewStudents(teacher.UserId);
                                break;
                            case "5":
                                TeacherManagement.PrintTeacherAssignedSubjects(teacher.UserId);

                                Console.Write("Enter Student ID: ");
                                int studentId = int.Parse(Console.ReadLine());
                               
                                Console.Write("Enter Subject ID: ");
                                int subjectId = int.Parse(Console.ReadLine());

                                Console.Write("Enter Term ID(Type 1,2,0r 3): ");
                                int termId = int.Parse(Console.ReadLine());

                                Console.Write("Enter Grade: ");
                                double grade = double.Parse(Console.ReadLine());

                                GradeManagement.AssignGrade(teacher.UserId, studentId, subjectId, termId, grade);
                                break;

                            case "6":
                                Console.WriteLine("Enter student ID: ");
                                int student_Id = int.Parse(Console.ReadLine());

                                GradeManagement.ViewStudentGrades(teacher.UserId, student_Id);
                                break;

                            case "7":
                                Console.WriteLine("Enter Class ID:");
                                int classId = int.Parse(Console.ReadLine());

                                GradeManagement.ViewClassGrades(classId);
                                break;

                            case "8":
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
                    Console.WriteLine("|--------------------------------------------------|");
                    Console.WriteLine("|     Teacher login failed. Please try again.      |");
                    Console.WriteLine("|--------------------------------------------------|");
                }
            }

            return false; 
        }

     

    }
}

