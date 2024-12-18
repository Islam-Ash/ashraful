using ConsoleApplication.EntitiyModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication.Methods
{
    public class SubjectManagement
    {
        public static void CreateSubject()
        {
            using (var db = new AppDbContext())
            {
                Console.Write("\nEnter new subject name: ");
                string subjectName = Console.ReadLine();

                var newSubject = new Subject { SubjectName = subjectName };
                db.Subjects.Add(newSubject);
                db.SaveChanges();

                Console.WriteLine("|--------------------------------------|");
                Console.WriteLine("|   Subject Created Successfully!      |");
                Console.WriteLine("|--------------------------------------|\n");
            }
        }
        public static void EditSubject()
        {
            using (var db = new AppDbContext())
            {
                Console.Write("\nEnter Subject Name To Edit: ");
                string subjectName = Console.ReadLine();

                var subjectToEdit = db.Subjects.FirstOrDefault(s => s.SubjectName == subjectName);
                if (subjectToEdit != null)
                {
                    Console.Write("Enter New Subject Name: ");
                    string newSubjectName = Console.ReadLine();

                    subjectToEdit.SubjectName = newSubjectName;
                    db.SaveChanges();

                    Console.WriteLine("|--------------------------------------|");
                    Console.WriteLine("|    Subject Edited Successfully!      |");
                    Console.WriteLine("|--------------------------------------|\n");
                }
                else
                {
                    Console.WriteLine("|--------------------------------------|");
                    Console.WriteLine("|        Subject Is Not Found!         |");
                    Console.WriteLine("|--------------------------------------|\n");
                }
            }
        }

        public static void DeleteSubject()
        {
            using (var db = new AppDbContext())
            {
                Console.Write("\nEnter Subject Name To Delete: ");
                string subjectName = Console.ReadLine();

                var subjectToDelete = db.Subjects.FirstOrDefault(s => s.SubjectName == subjectName);
                if (subjectToDelete != null)
                {
                    db.Subjects.Remove(subjectToDelete);
                    db.SaveChanges();

                    Console.WriteLine("|--------------------------------------|");
                    Console.WriteLine("|    Subject Deleted Successfully!     |");
                    Console.WriteLine("|--------------------------------------|\n");
                }
                else
                {
                    Console.WriteLine("|--------------------------------------|");
                    Console.WriteLine("|        Subject Is Not Found!         |");
                    Console.WriteLine("|--------------------------------------|\n");
                }

            }
        }

        public static void AssignTeacherToSubject()
        {
            using (var db = new AppDbContext())
            {
                Console.WriteLine("|--------------------------------------|");
                Console.WriteLine("|          Available Teachers          |");
                Console.WriteLine("|--------------------------------------|\n");
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
                Console.WriteLine("|          Assigned Subjects           |");
                Console.WriteLine("|--------------------------------------|");

                var teacherSubjectAssignments = db.TeacherSubjectAssignments
                                            .Where(ta => ta.TeacherId == teacher.UserId)
                                            .Select(ta => ta.Subject.SubjectName)
                                            .ToList();

                if (teacherSubjectAssignments.Any())
                {
                    foreach (var subjectName in teacherSubjectAssignments)
                    {
                        Console.WriteLine(subjectName);
                    }
                }
                else
                {
                   
                    Console.WriteLine("|----------------------------------------------|");
                    Console.WriteLine("|     No Subjects Assigned To This Teacher.    |");
                    Console.WriteLine("|----------------------------------------------|\n");

                }

               
                Console.WriteLine("|--------------------------------------|");
                Console.WriteLine("|         Available Subjects           |");
                Console.WriteLine("|--------------------------------------|");
                ViewSubjectsOnly();
                Console.WriteLine();

                while (true)
                {
                    Console.Write("\nEnter subject name to assign the teacher (or type 'done' to finish): ");
                    string subjectName = Console.ReadLine();

                    if (subjectName.ToLower() == "done")
                        break;

                    var selectedSubject = db.Subjects.FirstOrDefault(s => s.SubjectName == subjectName);

                    if (selectedSubject == null)
                    {
                        Console.WriteLine("|--------------------------------------|");
                        Console.WriteLine("|        Subject Is Not Found!         |");
                        Console.WriteLine("|--------------------------------------|\n");
                        continue;
                    }


                    var existingSubjectAssignment = db.TeacherSubjectAssignments.FirstOrDefault(ta => ta.TeacherId == teacher.UserId && ta.SubjectId == selectedSubject.SubjectId);

                    if (existingSubjectAssignment != null)
                    {
                        Console.WriteLine($"The Teacher Is Already Assigned To {subjectName} Subject.\n");
                        continue;
                    }


                    var teacherSubjectAssignment = new TeacherSubjectAssignment
                    {
                        Teacher = teacher,
                        Subject = selectedSubject
                    };

                    db.TeacherSubjectAssignments.Add(teacherSubjectAssignment);
                    db.SaveChanges();
                    Console.WriteLine( "|---------------------------------------------------------------|");
                    Console.WriteLine($"|  Teacher {teacherUsername} Assigned To {subjectName} Subject. |");
                    Console.WriteLine( "|---------------------------------------------------------------|");
                }
            }
        }

        

        public static void ViewSubjectsOnly()
        {
            using (var db = new AppDbContext())
            {
                var subjects = db.Subjects.ToList();
                Console.WriteLine("List Of Subjects");

                foreach (var s in subjects)
                {
                    Console.WriteLine(s.SubjectName);
                }

            }
        }

       

    }
}
