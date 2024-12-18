﻿// <auto-generated />
using ConsoleApplication;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ConsoleApplication.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20240326132358_CreateExamGradesTable")]
    partial class CreateExamGradesTable
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("ConsoleApplication.EntitiyModels.Class", b =>
                {
                    b.Property<int>("ClassId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ClassId"));

                    b.Property<string>("ClassName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ClassId");

                    b.ToTable("Classes");
                });

            modelBuilder.Entity("ConsoleApplication.EntitiyModels.ExamGrade", b =>
                {
                    b.Property<int>("ExamGradeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ExamGradeId"));

                    b.Property<double>("ExamGradeValue")
                        .HasColumnType("float");

                    b.Property<int>("StudentId")
                        .HasColumnType("int");

                    b.Property<int>("SubjectId")
                        .HasColumnType("int");

                    b.Property<int>("TermId")
                        .HasColumnType("int");

                    b.HasKey("ExamGradeId");

                    b.HasIndex("StudentId");

                    b.HasIndex("SubjectId");

                    b.HasIndex("TermId");

                    b.ToTable("ExamGrades");
                });

            modelBuilder.Entity("ConsoleApplication.EntitiyModels.Student", b =>
                {
                    b.Property<int>("StudentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("StudentId"));

                    b.Property<int>("ClassId")
                        .HasColumnType("int");

                    b.Property<string>("RollNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("StudentName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("StudentId");

                    b.HasIndex("ClassId");

                    b.ToTable("Students");
                });

            modelBuilder.Entity("ConsoleApplication.EntitiyModels.Subject", b =>
                {
                    b.Property<int>("SubjectId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("SubjectId"));

                    b.Property<string>("SubjectName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("SubjectId");

                    b.ToTable("Subjects");
                });

            modelBuilder.Entity("ConsoleApplication.EntitiyModels.TeacherClassAssignment", b =>
                {
                    b.Property<int>("TeacherId")
                        .HasColumnType("int");

                    b.Property<int>("ClassId")
                        .HasColumnType("int");

                    b.HasKey("TeacherId", "ClassId");

                    b.HasIndex("ClassId");

                    b.ToTable("TeacherClassAssignments");
                });

            modelBuilder.Entity("ConsoleApplication.EntitiyModels.TeacherSubjectAssignment", b =>
                {
                    b.Property<int>("TeacherId")
                        .HasColumnType("int");

                    b.Property<int>("SubjectId")
                        .HasColumnType("int");

                    b.HasKey("TeacherId", "SubjectId");

                    b.HasIndex("SubjectId");

                    b.ToTable("TeacherSubjectAssignments");
                });

            modelBuilder.Entity("ConsoleApplication.EntitiyModels.Term", b =>
                {
                    b.Property<int>("TermId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TermId"));

                    b.Property<string>("TermName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("TermId");

                    b.ToTable("Terms");

                    b.HasData(
                        new
                        {
                            TermId = 1,
                            TermName = "First"
                        },
                        new
                        {
                            TermId = 2,
                            TermName = "Mid"
                        },
                        new
                        {
                            TermId = 3,
                            TermName = "Final"
                        });
                });

            modelBuilder.Entity("ConsoleApplication.EntitiyModels.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserId"));

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Role")
                        .HasColumnType("int");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            UserId = 10,
                            Password = "ashraful",
                            Role = 0,
                            UserName = "Ashraful Islam"
                        });
                });

            modelBuilder.Entity("ConsoleApplication.EntitiyModels.ExamGrade", b =>
                {
                    b.HasOne("ConsoleApplication.EntitiyModels.Student", "Student")
                        .WithMany("ExamGrades")
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ConsoleApplication.EntitiyModels.Subject", "Subject")
                        .WithMany()
                        .HasForeignKey("SubjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ConsoleApplication.EntitiyModels.Term", "Term")
                        .WithMany()
                        .HasForeignKey("TermId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Student");

                    b.Navigation("Subject");

                    b.Navigation("Term");
                });

            modelBuilder.Entity("ConsoleApplication.EntitiyModels.Student", b =>
                {
                    b.HasOne("ConsoleApplication.EntitiyModels.Class", "Class")
                        .WithMany("Students")
                        .HasForeignKey("ClassId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Class");
                });

            modelBuilder.Entity("ConsoleApplication.EntitiyModels.TeacherClassAssignment", b =>
                {
                    b.HasOne("ConsoleApplication.EntitiyModels.Class", "Class")
                        .WithMany("TeacherClassAssignments")
                        .HasForeignKey("ClassId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ConsoleApplication.EntitiyModels.User", "Teacher")
                        .WithMany("TeacherClassAssignments")
                        .HasForeignKey("TeacherId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Class");

                    b.Navigation("Teacher");
                });

            modelBuilder.Entity("ConsoleApplication.EntitiyModels.TeacherSubjectAssignment", b =>
                {
                    b.HasOne("ConsoleApplication.EntitiyModels.Subject", "Subject")
                        .WithMany("TeacherSubjectAssignments")
                        .HasForeignKey("SubjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ConsoleApplication.EntitiyModels.User", "Teacher")
                        .WithMany("TeacherSubjectAssignments")
                        .HasForeignKey("TeacherId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Subject");

                    b.Navigation("Teacher");
                });

            modelBuilder.Entity("ConsoleApplication.EntitiyModels.Class", b =>
                {
                    b.Navigation("Students");

                    b.Navigation("TeacherClassAssignments");
                });

            modelBuilder.Entity("ConsoleApplication.EntitiyModels.Student", b =>
                {
                    b.Navigation("ExamGrades");
                });

            modelBuilder.Entity("ConsoleApplication.EntitiyModels.Subject", b =>
                {
                    b.Navigation("TeacherSubjectAssignments");
                });

            modelBuilder.Entity("ConsoleApplication.EntitiyModels.User", b =>
                {
                    b.Navigation("TeacherClassAssignments");

                    b.Navigation("TeacherSubjectAssignments");
                });
#pragma warning restore 612, 618
        }
    }
}
