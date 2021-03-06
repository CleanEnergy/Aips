namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ScheduledClassroomKeyFix : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Lectures", "ScheduledClassroom_ClassroomId", "dbo.ScheduledClassrooms");
            DropForeignKey("dbo.Lectures", "SchoolDayId", "dbo.SchoolDays");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Classrooms", "ClassroomTypeId", "dbo.ClassroomTypes");
            DropForeignKey("dbo.ContactHours", "Professor_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Courses", "Faculty_FacultyId", "dbo.Faculties");
            DropForeignKey("dbo.ExamCopies", "ExamId", "dbo.Exams");
            DropForeignKey("dbo.ExamCopies", "StudentId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Exams", "Subject_SubjectId", "dbo.Subjects");
            DropForeignKey("dbo.SignOnExams", "Exam_ExamId", "dbo.Exams");
            DropForeignKey("dbo.SignOnExams", "Student_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.ExamStudent", "ExamId", "dbo.Exams");
            DropForeignKey("dbo.ExamStudent", "StudentId", "dbo.AspNetUsers");
            DropForeignKey("dbo.FinishedSurveys", "SurveyId", "dbo.Surveys");
            DropForeignKey("dbo.FinishedSurveys", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Labs", "ClassroomId", "dbo.Classrooms");
            DropForeignKey("dbo.Labs", "SchoolDayId", "dbo.SchoolDays");
            DropForeignKey("dbo.Labs", "SubjectId", "dbo.Subjects");
            DropForeignKey("dbo.Lectures", "SubjectId", "dbo.Subjects");
            DropForeignKey("dbo.ScheduledClassrooms", "SchoolDayId", "dbo.SchoolDays");
            DropForeignKey("dbo.ProfessorsSubjects", "ProfessorId", "dbo.AspNetUsers");
            DropForeignKey("dbo.ProfessorsSubjects", "SubjectId", "dbo.Subjects");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.SignOnInformations", "Course_CourseId", "dbo.Courses");
            DropForeignKey("dbo.SignOnInformations", "SchoolYear_SchoolYearId", "dbo.SchoolYears");
            DropForeignKey("dbo.SignOnInformations", "Student_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.SurveyAnswers", "SurveyQuestionId", "dbo.SurveyQuestions");
            DropForeignKey("dbo.SurveyQuestions", "SurveyId", "dbo.Surveys");
            DropForeignKey("dbo.SurveyQuestions", "SurveyQuestionTypeId", "dbo.SurveyQuestionTypes");
            DropForeignKey("dbo.Theses", "Author_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Lectures", new[] { "SchoolDayId" });
            DropIndex("dbo.Lectures", new[] { "ScheduledClassroom_ClassroomId" });
            DropPrimaryKey("dbo.ScheduledClassrooms");
            AddColumn("dbo.ScheduledClassrooms", "ScheduledClassroomId", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.ScheduledClassrooms", "ScheduledClassroomId");
            AddForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Classrooms", "ClassroomTypeId", "dbo.ClassroomTypes", "ClassroomTypeId");
            AddForeignKey("dbo.ContactHours", "Professor_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Courses", "Faculty_FacultyId", "dbo.Faculties", "FacultyId");
            AddForeignKey("dbo.ExamCopies", "ExamId", "dbo.Exams", "ExamId");
            AddForeignKey("dbo.ExamCopies", "StudentId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Exams", "Subject_SubjectId", "dbo.Subjects", "SubjectId");
            AddForeignKey("dbo.SignOnExams", "Exam_ExamId", "dbo.Exams", "ExamId");
            AddForeignKey("dbo.SignOnExams", "Student_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.ExamStudent", "ExamId", "dbo.Exams", "ExamId");
            AddForeignKey("dbo.ExamStudent", "StudentId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.FinishedSurveys", "SurveyId", "dbo.Surveys", "SurveyId");
            AddForeignKey("dbo.FinishedSurveys", "UserId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Labs", "ClassroomId", "dbo.Classrooms", "ClassroomId");
            AddForeignKey("dbo.Labs", "SchoolDayId", "dbo.SchoolDays", "SchoolDayId");
            AddForeignKey("dbo.Labs", "SubjectId", "dbo.Subjects", "SubjectId");
            AddForeignKey("dbo.Lectures", "SubjectId", "dbo.Subjects", "SubjectId");
            AddForeignKey("dbo.ScheduledClassrooms", "SchoolDayId", "dbo.SchoolDays", "SchoolDayId");
            AddForeignKey("dbo.ProfessorsSubjects", "ProfessorId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.ProfessorsSubjects", "SubjectId", "dbo.Subjects", "SubjectId");
            AddForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles", "Id");
            AddForeignKey("dbo.SignOnInformations", "Course_CourseId", "dbo.Courses", "CourseId");
            AddForeignKey("dbo.SignOnInformations", "SchoolYear_SchoolYearId", "dbo.SchoolYears", "SchoolYearId");
            AddForeignKey("dbo.SignOnInformations", "Student_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.SurveyAnswers", "SurveyQuestionId", "dbo.SurveyQuestions", "SurveyQuestionId");
            AddForeignKey("dbo.SurveyQuestions", "SurveyId", "dbo.Surveys", "SurveyId");
            AddForeignKey("dbo.SurveyQuestions", "SurveyQuestionTypeId", "dbo.SurveyQuestionTypes", "TypeId");
            AddForeignKey("dbo.Theses", "Author_Id", "dbo.AspNetUsers", "Id");
            DropColumn("dbo.Lectures", "ScheduledClassroomId");
            DropColumn("dbo.Lectures", "SchoolDayId");
            DropColumn("dbo.Lectures", "ScheduledClassroom_ClassroomId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Lectures", "ScheduledClassroom_ClassroomId", c => c.Int());
            AddColumn("dbo.Lectures", "SchoolDayId", c => c.Int(nullable: false));
            AddColumn("dbo.Lectures", "ScheduledClassroomId", c => c.Int(nullable: false));
            DropForeignKey("dbo.Theses", "Author_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.SurveyQuestions", "SurveyQuestionTypeId", "dbo.SurveyQuestionTypes");
            DropForeignKey("dbo.SurveyQuestions", "SurveyId", "dbo.Surveys");
            DropForeignKey("dbo.SurveyAnswers", "SurveyQuestionId", "dbo.SurveyQuestions");
            DropForeignKey("dbo.SignOnInformations", "Student_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.SignOnInformations", "SchoolYear_SchoolYearId", "dbo.SchoolYears");
            DropForeignKey("dbo.SignOnInformations", "Course_CourseId", "dbo.Courses");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.ProfessorsSubjects", "SubjectId", "dbo.Subjects");
            DropForeignKey("dbo.ProfessorsSubjects", "ProfessorId", "dbo.AspNetUsers");
            DropForeignKey("dbo.ScheduledClassrooms", "SchoolDayId", "dbo.SchoolDays");
            DropForeignKey("dbo.Lectures", "SubjectId", "dbo.Subjects");
            DropForeignKey("dbo.Labs", "SubjectId", "dbo.Subjects");
            DropForeignKey("dbo.Labs", "SchoolDayId", "dbo.SchoolDays");
            DropForeignKey("dbo.Labs", "ClassroomId", "dbo.Classrooms");
            DropForeignKey("dbo.FinishedSurveys", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.FinishedSurveys", "SurveyId", "dbo.Surveys");
            DropForeignKey("dbo.ExamStudent", "StudentId", "dbo.AspNetUsers");
            DropForeignKey("dbo.ExamStudent", "ExamId", "dbo.Exams");
            DropForeignKey("dbo.SignOnExams", "Student_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.SignOnExams", "Exam_ExamId", "dbo.Exams");
            DropForeignKey("dbo.Exams", "Subject_SubjectId", "dbo.Subjects");
            DropForeignKey("dbo.ExamCopies", "StudentId", "dbo.AspNetUsers");
            DropForeignKey("dbo.ExamCopies", "ExamId", "dbo.Exams");
            DropForeignKey("dbo.Courses", "Faculty_FacultyId", "dbo.Faculties");
            DropForeignKey("dbo.ContactHours", "Professor_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Classrooms", "ClassroomTypeId", "dbo.ClassroomTypes");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropPrimaryKey("dbo.ScheduledClassrooms");
            DropColumn("dbo.ScheduledClassrooms", "ScheduledClassroomId");
            AddPrimaryKey("dbo.ScheduledClassrooms", "ClassroomId");
            CreateIndex("dbo.Lectures", "ScheduledClassroom_ClassroomId");
            CreateIndex("dbo.Lectures", "SchoolDayId");
            AddForeignKey("dbo.Theses", "Author_Id", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.SurveyQuestions", "SurveyQuestionTypeId", "dbo.SurveyQuestionTypes", "TypeId", cascadeDelete: true);
            AddForeignKey("dbo.SurveyQuestions", "SurveyId", "dbo.Surveys", "SurveyId", cascadeDelete: true);
            AddForeignKey("dbo.SurveyAnswers", "SurveyQuestionId", "dbo.SurveyQuestions", "SurveyQuestionId", cascadeDelete: true);
            AddForeignKey("dbo.SignOnInformations", "Student_Id", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.SignOnInformations", "SchoolYear_SchoolYearId", "dbo.SchoolYears", "SchoolYearId", cascadeDelete: true);
            AddForeignKey("dbo.SignOnInformations", "Course_CourseId", "dbo.Courses", "CourseId", cascadeDelete: true);
            AddForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles", "Id", cascadeDelete: true);
            AddForeignKey("dbo.ProfessorsSubjects", "SubjectId", "dbo.Subjects", "SubjectId", cascadeDelete: true);
            AddForeignKey("dbo.ProfessorsSubjects", "ProfessorId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.ScheduledClassrooms", "SchoolDayId", "dbo.SchoolDays", "SchoolDayId", cascadeDelete: true);
            AddForeignKey("dbo.Lectures", "SubjectId", "dbo.Subjects", "SubjectId", cascadeDelete: true);
            AddForeignKey("dbo.Labs", "SubjectId", "dbo.Subjects", "SubjectId", cascadeDelete: true);
            AddForeignKey("dbo.Labs", "SchoolDayId", "dbo.SchoolDays", "SchoolDayId", cascadeDelete: true);
            AddForeignKey("dbo.Labs", "ClassroomId", "dbo.Classrooms", "ClassroomId", cascadeDelete: true);
            AddForeignKey("dbo.FinishedSurveys", "UserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.FinishedSurveys", "SurveyId", "dbo.Surveys", "SurveyId", cascadeDelete: true);
            AddForeignKey("dbo.ExamStudent", "StudentId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.ExamStudent", "ExamId", "dbo.Exams", "ExamId", cascadeDelete: true);
            AddForeignKey("dbo.SignOnExams", "Student_Id", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.SignOnExams", "Exam_ExamId", "dbo.Exams", "ExamId", cascadeDelete: true);
            AddForeignKey("dbo.Exams", "Subject_SubjectId", "dbo.Subjects", "SubjectId", cascadeDelete: true);
            AddForeignKey("dbo.ExamCopies", "StudentId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.ExamCopies", "ExamId", "dbo.Exams", "ExamId", cascadeDelete: true);
            AddForeignKey("dbo.Courses", "Faculty_FacultyId", "dbo.Faculties", "FacultyId", cascadeDelete: true);
            AddForeignKey("dbo.ContactHours", "Professor_Id", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Classrooms", "ClassroomTypeId", "dbo.ClassroomTypes", "ClassroomTypeId", cascadeDelete: true);
            AddForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Lectures", "SchoolDayId", "dbo.SchoolDays", "SchoolDayId", cascadeDelete: true);
            AddForeignKey("dbo.Lectures", "ScheduledClassroom_ClassroomId", "dbo.ScheduledClassrooms", "ClassroomId");
        }
    }
}
