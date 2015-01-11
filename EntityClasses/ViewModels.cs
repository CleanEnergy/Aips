using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityClasses
{
    public class SignOnInformationViewModel
    {
        public SchoolYear SchoolYear { get; set; }

        public int Year { get; set; }

        public int Degree { get; set; }

        public Course Course { get; set; }
    }

    public class EnterExamGradeViewModel
    {
        [Required]
        public int ExamId { get; set; }

        [Required]
        public string StudentId { get; set; }

        [Required]
        public int Grade { get; set; }

        [Required]
        public bool InformStudent { get; set; }

        public User Student { get; set; }
        public Exam Exam { get; set; }
    }

    public class GradeExamViewModel
    {
        public int ExamId { get; set; }

        public List<SignOnExam> SignedOnStudents { get; set; }

        public Exam Exam { get; set; }
    }

    public class AssignedSubjectViewModel
    {
        public string ProfessorId { get; set; }

        public int SubjectId { get; set; }

        public string ProfessorFirstName { get; set; }

        public string ProfessorLastName { get; set; }

        public string SubjectTitle { get; set; }
    }

    public class ScheduleViewModel
    {
        [Required]
        public int SubjectId { get; set; }

        public List<ScheduleIntervalViewModel> Intervals { get; set; }

        [Required]
        public string SelectedIntervalId { get; set; }

        [Required]
        public int SelectedClassroomId { get; set; }

        public ScheduleViewModel()
        {
            Intervals = new List<ScheduleIntervalViewModel>();
        }
    }

    public class LectureScheduleViewModel
    {
        public int LectureId { get; set; }
    }

    public class LabScheduleViewModel
    {
        public int LabId { get; set; }        
    }

    public class ScheduleIntervalViewModel
    {
        public string Id { get; set; }

        public DateTime IntervalStart { get; set; }

        public DateTime IntervalEnd { get; set; }

        public TimeSpan Duration { get; set; }

        public SchoolDay SchoolDay { get; set; }

        public List<Classroom> Classrooms { get; set; }
    }

    public class AssignSubjectToProfessorViewModel
    {
        [Required]
        public int SubjectId { get; set; }

        [Required]
        public string ProfessorId { get; set; }
    }

    public class AssignSubjectToAssistantViewModel
    {
        [Required]
        public int SubjectId { get; set; }

        [Required]
        public string AssistantId { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }
    }

    public class CreateUserViewModel
    {
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        [Required]
        [StringLength(256)]
        public string EMail { get; set; }

        public string PhoneNumber { get; set; }

        [Required]
        [StringLength(30)]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

        [Compare("Password")]
        public string RepeatPassword { get; set; }

        [Required]
        public string RoleId { get; set; }
    }

    public class LockAccountViewModel
    {
        public List<User> UserCollection { get; set; }
    }

    public class MessageViewModel
    {
        [Required]
        [StringLength(50)]
        public string Subject { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public string RecepientUsernames { get; set; }

        public DateTime SentOn { get; set; }

        public string RecepientUsername { get; set; }

        public string SenderUsername { get; set; }
    }

    public class AdminInformationViewModel
    {
        [MaxLength(50)]
        [Required]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }
    }

    public class UniversityEventViewModel
    {
        [Required(AllowEmptyStrings = false)]
        [StringLength(50)]
        public string Title { get; set; }

        [Required]
        public int Day { get; set; }

        [Required]
        public int Month { get; set; }

        [Required]
        public int Year { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Description { get; set; }

        public bool InformAllStudents { get; set; }
    }

    public class ExamViewModel
    {
        [Required]
        public int SubjectId { get; set; }

        [Required]
        public int Day { get; set; }

        [Required]
        public int Month { get; set; }

        [Required]
        public int Year { get; set; }

        [Required]
        public int Hour { get; set; }

        [Required]
        public int Minute { get; set; }

        [Required]
        public int ClassroomId { get; set; }

        public string StudentId { get; set; }
        public int Grade { get; set; }
        public string ClassroomName { get; set; }
        public string SubjectName { get; set; }
        public bool InformStudents { get; set; }
        public DateTime ExamDateAndTime { get; set; }
        public int ExamId { get; set; }
    }

    public class SubjectViewModel
    {
        [Required(AllowEmptyStrings = false)]
        [StringLength(50)]
        public string Title { get; set; }

        public bool IsActive { get; set; }

        [Required]
        public int Year { get; set; }

        [Required]
        public int Degree { get; set; }

        [Required]
        public int CourseId { get; set; }

        [Required]
        public string ProfessorId { get; set; }

        public string Grade { get; set; }
    }

    public class ExamPDFUploadViewModel
    {
        public string AuthorId { get; set; }

        public int ExamId { get; set; }

        public string UploadedFileName { get; set; }
    }

    public class CreateStudentViewModel
    {
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        [Required]
        [StringLength(256)]
        public string EMail { get; set; }

        public string PhoneNumber { get; set; }

        [Required]
        [StringLength(30)]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

        [Compare("Password")]
        public string RepeatPassword { get; set; }

        [Required]
        public int CourseId { get; set; }

        [Required]
        public int SchoolYearId { get; set; }
    }

    public class FacultyViewModel
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
    }

    public class StudentAddedViewModel
    {
        public User Student { get; set; }

        public SchoolYear SchoolYear { get; set; }

        public List<Subject> Subjects { get; set; }
    }

    public class SchoolYearViewModel
    {
        [Required]
        public int YearStartDay { get; set; }

        [Required]
        public int YearStartMonth { get; set; }

        [Required]
        public int YearStartYear { get; set; }

        [Required]
        public int YearEndDay { get; set; }

        [Required]
        public int YearEndMonth { get; set; }

        [Required]
        public int YearEndYear { get; set; }
    }

    public class CourseViewModel
    {
        public int CourseId { get; set; }

        [Required]
        public int FacultyId { get; set; }

        [Required]
        public string Name { get; set; }

        public Faculty Faculty { get; set; }

    }

    public class SurveyViewModel
    {
        public int SurveyId { get; set; }

        [Required]
        [StringLength(150)]
        public string Title { get; set; }

        [Required]
        public bool Visible { get; set; }

        public string SurveyData { get; set; }
    }

    public class TestSurveyViewModel
    {
        public Survey Survey { get; set; }

        public List<SurveyQuestion> SurveyQuestions { get; set; }

        public List<SurveyAnswer> SurveyAnswers { get; set; }
    }

    public class LabViewModel
    {
        public int LabId { get; set; }

        [Required]
        public int SubjectId { get; set; }

        [Required]
        public int StartTimeHour { get; set; }

        [Required]
        public int StartTimeMinute { get; set; }

        [Required]
        public int EndTimeHour { get; set; }

        [Required]
        public int EndTimeMinute { get; set; }

        [Required]
        public int MinimalPresencePercent { get; set; }
    }
}
