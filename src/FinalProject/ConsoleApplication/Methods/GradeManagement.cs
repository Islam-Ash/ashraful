using ConsoleApplication.EntitiyModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication.Methods
{
    public class GradeManagement
    {

        public static void AssignGrade(int teacherId, int studentId, int subjectId, int termId, double grade)
        {
            using (var db = new AppDbContext())
            {
               
                var teacherClassAssignment = db.TeacherClassAssignments.FirstOrDefault(tca => tca.TeacherId == teacherId && tca.Class.Students.Any(s => s.StudentId == studentId));
                if (teacherClassAssignment == null)
                {
                    Console.WriteLine("You are not assigned to the class of this student.\n");
                    return;
                }

               
                var student = db.Students.FirstOrDefault(s => s.StudentId == studentId);
                if (student == null)
                {
                    Console.WriteLine("|--------------------------|");
                    Console.WriteLine("|     Student not found.   |");
                    Console.WriteLine("|--------------------------|");
                    return;
                }

               
                var teacherSubjectAssignment = db.TeacherSubjectAssignments
                    .Include(tsa => tsa.Subject)
                    .FirstOrDefault(tsa => tsa.TeacherId == teacherId && tsa.SubjectId == subjectId);

                if (teacherSubjectAssignment == null)
                {
                    Console.WriteLine("You are not assigned to this subject.\n");
                    return;
                }

               
                var term = db.Terms.FirstOrDefault(t => t.TermId == termId);
                if (term == null)
                {
                    Console.WriteLine("Term not found.\n");
                    return;
                }

               
                var existingGrade = db.ExamGrades.FirstOrDefault(g => g.StudentId == studentId && g.SubjectId == subjectId && g.TermId == termId);

                if (existingGrade != null)
                {
                    existingGrade.ExamGradeValue = grade;
                }
                else
                {
                    var newGrade = new ExamGrade
                    {
                        StudentId = studentId,
                        SubjectId = subjectId,
                        TermId = termId,
                        ExamGradeValue = grade
                    };
                    db.ExamGrades.Add(newGrade);
                }

                db.SaveChanges();

                Console.WriteLine($"Grade {grade} assigned to {student.StudentName}" +
                    $" for subject {teacherSubjectAssignment.Subject.SubjectName} in term {term.TermName}.\n");
            }
        }


        public static void ViewStudentGrades(int teacherId, int studentId)
        {
            using (var db = new AppDbContext())
            {
               
                var student = db.Students.FirstOrDefault(s => s.StudentId == studentId);
                if (student == null)
                {
                    Console.WriteLine("Student not found.");
                    return;
                }

                var classId = student.ClassId;
                var teacherClassAssignment = db.TeacherClassAssignments
                                               .Include(tca => tca.Class)
                                               .FirstOrDefault(tca => tca.TeacherId == teacherId && tca.ClassId == classId);

                if (teacherClassAssignment == null)
                {
                    Console.WriteLine("You are not assigned to the class of this student.");
                    return;
                }

                
                var examGrades = db.ExamGrades
                                  .Include(g => g.Subject) 
                                  .Include(g => g.Term)    
                                  .Where(g => g.StudentId == studentId)
                                  .ToList();

                if (examGrades.Count == 0)
                {
                    Console.WriteLine("No grades found for this student.");
                    return;
                }

                Console.WriteLine($"Grades for student {student.StudentName} in class {teacherClassAssignment.Class.ClassName}:");
                foreach (var grade in examGrades)
                {
                    string subjectName = grade.Subject != null ? grade.Subject.SubjectName : "N/A";
                    string termName = grade.Term != null ? grade.Term.TermName : "N/A";
                   
                    Console.WriteLine($"Subject: {subjectName}, Term: {termName}, Grade: {grade.ExamGradeValue}");
                }
            }
        }

        public static void ViewClassGrades(int classId)
        {
            using (var db = new AppDbContext())
            {
                
                var className = db.Classes.Where(c => c.ClassId == classId).Select(c => c.ClassName).FirstOrDefault();
                if (className == null)
                {
                    Console.WriteLine("Class not found.");
                    return;
                }

                
                var students = db.Students.Where(s => s.ClassId == classId).ToList();
                if (students.Count == 0)
                {
                    Console.WriteLine("No students found in this class.");
                    return;
                }

                Console.WriteLine("|------------------------------------|");
                Console.WriteLine($"|  Grades For Class {className}:    |");
                Console.WriteLine("|------------------------------------|");
           


                Console.WriteLine($"{"Name",-20}{"First",-10}{"Mid",-10}{"Final",-10}");

                
                foreach (var student in students)
                {
                    
                    var termGrades = db.ExamGrades
                                       .Include(g => g.Subject)
                                       .Include(g => g.Term)
                                       .Where(g => g.StudentId == student.StudentId)
                                       .ToList();

                    var firstTermGrade = termGrades.FirstOrDefault(g => g.Term.TermName == "First")?.ExamGradeValue;
                    var midTermGrade = termGrades.FirstOrDefault(g => g.Term.TermName == "Mid")?.ExamGradeValue;
                    var finalTermGrade = termGrades.FirstOrDefault(g => g.Term.TermName == "Final")?.ExamGradeValue;

                    
                    Console.WriteLine($"{student.StudentName,-20}{firstTermGrade,-10:F1}{midTermGrade,-10:F1}{finalTermGrade,-10:F1}");
                }
            }
        }


    }
}
