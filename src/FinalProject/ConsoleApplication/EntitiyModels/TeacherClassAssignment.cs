using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication.EntitiyModels
{
    public class TeacherClassAssignment
    {

        public int TeacherId { get; set; }
        public User Teacher { get; set; }

        public int ClassId { get; set; }
        public Class Class { get; set; }
    }
}
