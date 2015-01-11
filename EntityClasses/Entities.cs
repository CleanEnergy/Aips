using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityClasses
{
    public class User : IdentityUser
    {
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        [Required]
        [StringLength(30)]
        public override string UserName
        {
            get
            {
                return base.UserName;
            }
            set
            {
                base.UserName = value;
            }
        }

        [Required]
        [StringLength(256)]
        public override string Email
        {
            get
            {
                return base.Email;
            }
            set
            {
                base.Email = value;
            }
        }

    }

    public class UserMessage
    {
        [Column]
        [Key]
        public int UserMessageId { get; set; }

        [Column]
        [Required]
        [StringLength(50)]
        public string Subject { get; set; }

        [Column]
        [Required]
        public string Content { get; set; }

        [Column]
        [Required]
        public DateTime SentOn { get; set; }

        [Column]
        [Required]
        [StringLength(30)]
        public string SenderUsername { get; set; }

        [Column]
        [Required]
        [StringLength(30)]
        public string RecepientUsername { get; set; }

        [Column]
        public bool IsTrash { get; set; }
    }

    public class AdminMessage
    {
        public int AdminMessageId { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public DateTime PostedOn { get; set; }

        public virtual User Admin { get; set; }
    }

    public class Professor : User
    {
        
    }

    public class ContactHours
    {
        [Key]
        [Column]
        public int ContactHoursId { get; set; }

        [Column]
        [Required]
        [MaxLength(10)]
        public string Cabinet { get; set; }

        [Column]
        [Required(AllowEmptyStrings = false)]
        [MaxLength(20)]
        public string Day { get; set; }

        [Column]
        [Required(AllowEmptyStrings = false)]
        [MaxLength(4)]
        public string Time { get; set; }

        [Column]
        [Required]
        public virtual Professor Professor { get; set; }
    }

    public class Student : User
    {
    }

    public class Subject
    {
        [Key]
        [Column]
        public int SubjectId { get; set; }

        [Column]
        [Required(AllowEmptyStrings = false)]
        [MaxLength(50)]
        public string Title { get; set; }

        [Column]
        public bool IsActive { get; set; }

        [Column]
        public int Year { get; set; }

        [Column]
        public int Degree { get; set; }

        [Column]
        public virtual Course Course { get; set; }
    }

    public class ProfessorsSubjects
    {
        [Key]
        [ForeignKey("Subject")]
        [Required]
        [Column(Order = 0)]
        public int SubjectId { get; set; }

        [Key]
        [ForeignKey("Professor")]
        [Required]
        [Column(Order = 1)]
        public string ProfessorId { get; set; }

        public virtual Subject Subject { get; set; }
        public virtual User Professor { get; set; }
    }

    public class AssistantsSubjects
    {
        [Key]
        [ForeignKey("Subject")]
        [Required]
        [Column(Order = 0)]
        public int SubjectId { get; set; }

        [Key]
        [ForeignKey("Assistant")]
        [Required]
        [Column(Order = 1)]
        public string AssistantId { get; set; }

        public virtual Subject Subject { get; set; }
        public virtual User Assistant { get; set; }
    }

    public class Exam
    {
        [Key]
        [Column]
        public int ExamId { get; set; }

        [Column]
        [Required]
        public DateTime DateAndTime { get; set; }

        [Column]
        [Required]
        [ForeignKey("Subject")]
        public int SubjectId { get; set; }

        [Column]
        [Required]
        [ForeignKey("Classroom")]
        public int ClassroomId { get; set; }

        public virtual Subject Subject { get; set; }
        public virtual Classroom Classroom { get; set; }
    }

    [Table("ExamStudent")]
    public class ExamStudent
    {
        [Key]
        [Column]
        public int ExamStudentId { get; set; }

        [Column]
        [Required]
        public int Grade { get; set; }

        [Column]
        [ForeignKey("Exam")]
        [Required]
        public int ExamId { get; set; }

        [Column]
        [ForeignKey("Student")]
        [Required]
        public string StudentId { get; set; }

        public virtual Exam Exam { get; set; }

        public virtual Student Student { get; set; }

    }

    public class Thesis
    {
        [Key]
        [Column]
        public int ThesisId { get; set; }

        [Column]
        [Required]
        public byte[] FileBytes { get; set; }

        [Column]
        [Required]
        public virtual Student Author { get; set; }
    }

    public class UniversityEvent
    {
        [Key]
        [Column]
        public int UniversityEventId { get; set; }

        [Column]
        [Required(AllowEmptyStrings = false)]
        [MaxLength(50)]
        public string Title { get; set; }

        [Column]
        [Required]
        public DateTime DateAndTime { get; set; }

        [Column]
        [Required]
        public string Description { get; set; }

        public override string ToString()
        {
            return "Title: " + Title + " Date: " + DateAndTime.ToShortDateString() + " at " + DateAndTime.ToShortTimeString() + " Description: " + Description;
        }
    }

    public class SignOnExam
    {
        [Key]
        [Column]
        public int SignOnExamId { get; set; }

        [Column]
        [Required]
        [ForeignKey("Student")]
        public string StudentId { get; set; }

        [Column]
        [Required]
        [ForeignKey("Exam")]
        public int ExamId { get; set; }

        [Column]
        [Required]
        public bool IsSignedOff { get; set; }

        public virtual Exam Exam { get; set; }
        public virtual User Student { get; set; }
    }

    public class SignOnInformation
    {
        [Key]
        [Column]
        public int SignOnInformationId { get; set; }

        [Column]
        [Required]
        [ForeignKey("SchoolYear")]
        public int SchoolYearId { get; set; }

        [Column]
        [Required]
        [ForeignKey("Student")]
        public string StudentId { get; set; }

        [Column]
        [Required]
        public int Year { get; set; }

        [Column]
        [Required]
        public int Degree { get; set; }

        [Column]
        [Required]
        [ForeignKey("Course")]
        public int CourseId { get; set; }

        public virtual SchoolYear SchoolYear { get; set; }
        public virtual User Student { get; set; }
        public Course Course { get; set; }
    }

    public class Survey
    {
        [Key]
        [Column]
        public int SurveyId { get; set; }

        [Column]
        [Required]
        [MaxLength(150)]
        public string Title { get; set; }

        [Column]
        [Required]
        public bool Visible { get; set; }
    }

    public class SurveyQuestion
    {
        [Key]
        [Column]
        public int SurveyQuestionId { get; set; }

        [Column]
        [Required]
        public string Question { get; set; }

        [ForeignKey("Survey")]
        public int SurveyId { get; set; }

        public virtual Survey Survey { get; set; }

        [Column]
        [ForeignKey("SurveyQuestionType")]
        public int SurveyQuestionTypeId { get; set; }

        public virtual SurveyQuestionType SurveyQuestionType { get; set; }

    }

    public class SurveyQuestionType
    {
        [Column]
        [Key]
        public int TypeId { get; set; }

        [Column]
        [Required]
        public string Name { get; set; }
    }

    public class SurveyAnswer
    {
        [Key]
        [Column]
        public int SurveyAnswerId { get; set; }

        [Column]
        [Required]
        [MaxLength(50)]
        public string Answer { get; set; }

        [Column]
        [ForeignKey("SurveyQuestion")]
        [Required]
        public int SurveyQuestionId { get; set; }

        public virtual SurveyQuestion SurveyQuestion { get; set; }
    }

    public class FinishedSurvey
    {
        [Key]
        [Column]
        public int FinishedSurveyId { get; set; }

        [Column]
        [Required]
        [ForeignKey("User")]
        public string UserId { get; set; }

        [Column]
        [Required]
        [ForeignKey("Survey")]
        public int SurveyId { get; set; }

        [Column]
        [Required]
        public string AnswerIds { get; set; }

        public virtual User User { get; set; }
        public virtual Survey Survey { get; set; }
    }

    

    public class ExamCopy
    {
        [Key]
        [Column]
        public int ExamCopyId { get; set; }

        [ForeignKey("Exam")]
        [Required]
        [Column]
        public int ExamId { get; set; }

        [ForeignKey("Student")]
        [Required]
        [Column]
        public string StudentId { get; set; }

        [Column]
        public byte[] ExamImage { get; set; }

        [Column]
        [Required]
        public string MimeType { get; set; }

        public virtual Exam Exam { get; set; }
        public virtual User Student { get; set; }
    }

    public class SchoolYear
    {
        [Key]
        public int SchoolYearId { get; set; }

        [Column]
        public DateTime YearStart { get; set; }

        [Column]
        public DateTime YearEnd { get; set; }

        public virtual ICollection<User> EnrolledStudents { get; set; }
    }

    public class Faculty
    {
        [Key]
        public int FacultyId { get; set; }

        [Column]
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
    }

    public class Course
    {
        [Key]
        public int CourseId { get; set; }

        [Column]
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Column]
        [Required]
        public virtual Faculty Faculty { get; set; }

    }

    public class Lecture
    {
        [Key]
        [Column]
        public int LectureId { get; set; }

        [Column]
        [Required]
        [ForeignKey("Professor")]
        public string ProfessorId { get; set; }

        [Column]
        [Required]
        [ForeignKey("ScheduledClassroom")]
        public int ScheduledClassroomId { get; set; }

        [Column]
        [Required]
        [ForeignKey("Subject")]
        public int SubjectId { get; set; }

        public virtual User Professor { get; set; }
        public virtual ScheduledClassroom ScheduledClassroom { get; set; }
        public virtual Subject Subject { get; set; }
    }

    public class Lab
    {
        [Key]
        [Column]
        public int LabId { get; set; }

        [Column]
        [Required]
        [ForeignKey("Assistant")]
        public string AssistantId { get; set; }

        [Column]
        [Required]
        [ForeignKey("ScheduledClassroom")]
        public int ScheduledClassroomId { get; set; }

        [Column]
        [Required]
        [ForeignKey("Subject")]
        public int SubjectId { get; set; }

        public virtual User Assistant { get; set; }
        public virtual ScheduledClassroom ScheduledClassroom { get; set; }
        public virtual Subject Subject { get; set; }
    }

    public class SchoolDay
    {
        [Key]
        [Column]
        public int SchoolDayId { get; set; }

        [Column]
        [Required]
        [StringLength(15)]
        public string Name { get; set; }
    }

    public class ClassroomType
    {
        [Key]
        [Column]
        public int ClassroomTypeId { get; set; }

        [Column]
        [Required]
        [StringLength(30)]
        public string Name { get; set; }
    }

    public class Classroom
    {
        [Key]
        [Column]
        public int ClassroomId { get; set; }

        [Column]
        [Required]
        [StringLength(10)]
        public string Name { get; set; }

        [ForeignKey("ClassroomType")]
        public int ClassroomTypeId { get; set; }

        public virtual ClassroomType ClassroomType { get; set; }
    }

    public class ScheduledClassroom
    {
        [Key]
        public int ScheduledClassroomId { get; set; }

        [ForeignKey("Classroom")]
        public int ClassroomId { get; set; }

        [Column]
        [Required]
        public DateTime ScheduleStart { get; set; }

        [Column]
        [Required]
        public DateTime ScheduleEnd { get; set; }

        [Column]
        [Required]
        [ForeignKey("SchoolDay")]
        public int SchoolDayId { get; set; }

        public virtual SchoolDay SchoolDay { get; set; }
        public virtual Classroom Classroom { get; set; }
    }
}
