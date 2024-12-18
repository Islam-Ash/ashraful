using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication.EntitiyModels
{
    public class User
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public UserRole Role { get; set; }


        public List<TeacherSubjectAssignment> TeacherSubjectAssignments { get; set; }
        public List<TeacherClassAssignment> TeacherClassAssignments { get; set; }


    }
}


