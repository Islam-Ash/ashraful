using ConsoleApplication.EntitiyModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication
{
    public class AppDbContext : DbContext
    {
        private readonly string _connectionString;

        public AppDbContext()
        {
            _connectionString = "Server=.\\SQLEXPRESS;Database=CSharpB16;User Id=csharpb16;Password=123456;Trust Server Certificate=True";
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
                optionsBuilder.UseSqlServer(_connectionString);

            base.OnConfiguring(optionsBuilder);
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<TeacherSubjectAssignment> TeacherSubjectAssignments { get; set; } 
        public DbSet<TeacherClassAssignment> TeacherClassAssignments { get; set; }
        public DbSet<Term> Terms { get; set; }
        public DbSet<ExamGrade> ExamGrades { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<TeacherSubjectAssignment>()
                .HasKey(ta => new { ta.TeacherId, ta.SubjectId });


            modelBuilder.Entity<TeacherSubjectAssignment>()
                .HasOne(ta => ta.Teacher)
                .WithMany(u => u.TeacherSubjectAssignments)
                .HasForeignKey(ta => ta.TeacherId);

            modelBuilder.Entity<TeacherSubjectAssignment>()
                .HasOne(ta => ta.Subject)
                .WithMany(s => s.TeacherSubjectAssignments)
                .HasForeignKey(ta => ta.SubjectId);

          

            //for Teacher to Class Assignment
            modelBuilder.Entity<TeacherClassAssignment>()
               .HasKey(tca => new { tca.TeacherId, tca.ClassId }); 

            modelBuilder.Entity<TeacherClassAssignment>()
                .HasOne(tca => tca.Teacher)
                .WithMany(u => u.TeacherClassAssignments)
                .HasForeignKey(tca => tca.TeacherId);

            modelBuilder.Entity<TeacherClassAssignment>()
                .HasOne(tca => tca.Class)
                .WithMany(c => c.TeacherClassAssignments)
                .HasForeignKey(tca => tca.ClassId);

            // Class to student relationship
            modelBuilder.Entity<Student>()
                .HasOne(s => s.Class)
                .WithMany(c => c.Students)
                .HasForeignKey(s => s.ClassId);
               


            //seeding
            modelBuilder.Entity<User>().HasData(
               new User { UserId = 10, UserName = "Ashraful Islam", Password = "ashraful", Role = UserRole.Admin }
               );

            modelBuilder.Entity<ExamGrade>()
       .HasOne(g => g.Student)
       .WithMany(s => s.ExamGrades)
       .HasForeignKey(g => g.StudentId);

            modelBuilder.Entity<ExamGrade>()
                .HasOne(g => g.Subject)
                .WithMany() 
                .HasForeignKey(g => g.SubjectId);

            modelBuilder.Entity<ExamGrade>()
                .HasOne(g => g.Term)
                .WithMany() 
                .HasForeignKey(g => g.TermId);

            //seeding for term
            modelBuilder.Entity<Term>().HasData(
               new Term { TermId = 1, TermName = "First" },
               new Term { TermId = 2, TermName = "Mid"},
               new Term { TermId = 3, TermName = "Final"}
               );
        }
    }
}