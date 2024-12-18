using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication.EntitiyModels
{
    public class Student
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public string RollNumber { get; set; }
        public int ClassId { get; set; }

        public Class Class { get; set; }
        
        public List<ExamGrade> ExamGrades { get; set; }

    }
}
