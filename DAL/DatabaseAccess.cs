using EntityClasses;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace DAL
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext()
            : base("AipsConnection",
            throwIfV1Schema: false) 
        { 
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<ScheduledClassroom>().HasRequired(x => x.Classroom).WithRequiredDependent().WillCascadeOnDelete(false); 

            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<AssistantsSubjects> AssistantsSubjects { get; set; }

        public DbSet<ScheduledClassroom> ScheduledClassrooms { get; set; }

        public DbSet<ClassroomType> ClassroomTypes { get; set; }

        public DbSet<Classroom> Classrooms { get; set; }

        public DbSet<SchoolDay> SchoolDays { get; set; }

        public DbSet<Lecture> Lectures { get; set; }

        public DbSet<ProfessorsSubjects> ProfessorsSubjects { get; set; }

        public DbSet<UserMessage> UserMessages { get; set; }

        public DbSet<AdminMessage> AdminMessages { get; set; }

        public DbSet<ContactHours> ContactHours { get; set; }

        public DbSet<Subject> Subjects { get; set; }

        public DbSet<Exam> Exams { get; set; }

        public DbSet<Thesis> Theses { get; set; }

        public DbSet<UniversityEvent> UniversityEvents { get; set; }

        public DbSet<SignOnExam> ExamSignOns { get; set; }

        public DbSet<SignOnInformation> SignOnInformation { get; set; }

        public DbSet<ExamStudent> ExamStudent { get; set; }

        public DbSet<Survey> Surveys { get; set; }

        public DbSet<SurveyQuestion> SurveyQuestions { get; set; }

        public DbSet<SurveyAnswer> SurveyAnswers { get; set; }

        public DbSet<FinishedSurvey> FinishedSurveys { get; set; }

        public DbSet<Lab> Labs { get; set; }

        public DbSet<ExamCopy> ExamCopies { get; set; }

        public DbSet<Faculty> Faculties { get; set; }

        public DbSet<Course> Courses { get; set; }

        public DbSet<SchoolYear> SchoolYears { get; set; }

        public DbSet<SurveyQuestionType> SurveyQuestionTypes { get; set; }
    }
}
