using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication.EntitiyModels
{
    public class Grade
    {
        public int GradeId { get; set; }
        public int StudentId { get; set; }
        public int SubjectId { get; set; }
        public int TermId { get; set; }
        public double GradeValue { get; set; }

        public Student Student { get; set; }
        public Subject Subject { get; set; }
        public Term Term { get; set; }
    }
}
