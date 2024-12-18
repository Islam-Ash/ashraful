using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication.EntitiyModels
{
    public class Class
    {
        public int ClassId { get; set; }
        public string ClassName { get; set; }
        public List<Student> Students { get; set; }
        public List<TeacherClassAssignment> TeacherClassAssignments { get; set; }
    }
}

