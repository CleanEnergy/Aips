using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityClasses
{
    public class LabPresenceForSubjectViewModel
    {
        [Display(Name = "Subject")]
        public string SubjectName { get; set; }

        [Display(Name = "Presence percent")]
        public float PresencePercent { get; set; }

        [Display(Name = "Required presence")]
        public float RequiredPresencePercent { get; set; }
    }

    public class MarkLabPresenceViewModel
    {
        [Display(Name = "Student")]
        public string StudentId { get; set; }

        [Display(Name = "First name")]
        public string StudentFirstName { get; set; }

        [Display(Name = "Last name")]
        public string StudentLastName { get; set; }

        [Display(Name = "Was present")]
        public bool WasPresent { get; set; }
    }

    public class SignOnInformationViewModel
    {
        public SchoolYear SchoolYear { get; set; }

        public int Year { get; set; }

        public int Degree { get; set; }

        public Course Course { get; set; }
    }

    public class EnterExamGradeViewModel
    {
        [Display(Name = "Exam")]
        [Required]
        public int ExamId { get; set; }

        [Display(Name = "Student")]
        [Required]
        public string StudentId { get; set; }

        [Display(Name = "Grade")]
        [Required]
        public int Grade { get; set; }

        [Display(Name = "Inform student")]
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
        [Display(Name = "Professor")]
        public string ProfessorId { get; set; }

        [Display(Name = "Subject")]
        public int SubjectId { get; set; }

        [Display(Name = "First name")]
        public string ProfessorFirstName { get; set; }

        [Display(Name = "Last name")]
        public string ProfessorLastName { get; set; }

        [Display(Name = "Subject")]
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
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
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

        public bool Seen { get; set; }

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

        [Display(Name = "Inform students")]
        public bool InformAllStudents { get; set; }
    }

    public class ExamViewModel
    {
        [Display(Name = "Subject")]
        [Required]
        public int SubjectId { get; set; }

        [Display(Name = "Day")]
        [Required]
        public int Day { get; set; }

        [Display(Name = "Month")]
        [Required]
        public int Month { get; set; }

        [Display(Name = "Year")]
        [Required]
        public int Year { get; set; }

        [Display(Name = "Hour")]
        [Required]
        public int Hour { get; set; }

        [Display(Name = "Minute")]
        [Required]
        public int Minute { get; set; }

        [Display(Name = "Classroom")]
        [Required]
        public int ClassroomId { get; set; }

        [Display(Name = "Student")]
        public string StudentId { get; set; }
        [Display(Name = "Grade")]
        public int Grade { get; set; }
        [Display(Name = "Classroom")]
        public string ClassroomName { get; set; }
        [Display(Name = "Subject")]
        public string SubjectName { get; set; }
        [Display(Name = "Inform students")]
        public bool InformStudents { get; set; }
        [Display(Name = "Date and time")]
        public DateTime ExamDateAndTime { get; set; }
        [Display(Name = "Exam")]
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
        [Display(Name = "First name")]
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Display(Name = "Last name")]
        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        [Display(Name = "Email address")]
        [Required]
        [StringLength(256)]
        public string EMail { get; set; }

        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Username")]
        [Required]
        [StringLength(30)]
        public string UserName { get; set; }

        [Display(Name = "Password")]
        [Required]
        public string Password { get; set; }

        [Display(Name = "Repeat password")]
        [Compare("Password")]
        public string RepeatPassword { get; set; }

        [Display(Name = "Course")]
        [Required]
        public int CourseId { get; set; }

        [Display(Name = "School year")]
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
        [Display(Name = "Lab")]
        public int LabId { get; set; }

        [Display(Name = "Subject")]
        [Required]
        public int SubjectId { get; set; }

        [Display(Name = "Start hour")]
        [Required]
        public int StartTimeHour { get; set; }

        [Display(Name = "Start minute")]
        [Required]
        public int StartTimeMinute { get; set; }

        [Display(Name = "End hour")]
        [Required]
        public int EndTimeHour { get; set; }

        [Display(Name = "End minute")]
        [Required]
        public int EndTimeMinute { get; set; }

        [Display(Name = "Minimal presence percent")]
        [Required]
        public int MinimalPresencePercent { get; set; }

        [Display(Name = "Assistant")]
        [Required]
        public string AssistantId  { get; set; }

        [Display(Name = "School day")]
        public SchoolDay SchoolDay { get; set; }
        [Display(Name = "Start time")]
        public DateTime StartTime { get; set; }
        [Display(Name = "End time")]
        public DateTime EndTime { get; set; }
        [Display(Name = "Subject")]
        public Subject Subject { get; set; }
        [Display(Name = "Assistant")]
        public User Assistant { get; set; }
    }
}
