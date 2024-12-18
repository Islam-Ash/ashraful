using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication.EntitiyModels
{
    public class TeacherSubjectAssignment
    {
        public int TeacherId { get; set; }
        public int SubjectId { get; set; }
       
        public User Teacher { get; set; }
        public Subject Subject { get; set; }



    }
}
