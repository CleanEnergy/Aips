var hours = 0;
var selectedCourseId = 0;
var selectedYear = 0;
var selectedDegree = 0;
var selectedSubjectId = 0;
var isLab = false;

$(function () {
    $('#isLab').val(false);
});

function getCoursesForFaculty() {
    var selectedFacultyId = $('#FacultyId').val();
    $('#coursesDiv').load('GetCoursesForFaculty?facultyId=' + selectedFacultyId);
}

function showSubjects() {
    selectedCourseId = $('#Courses').val();
    selectedYear = $('#yearTb').val();
    selectedDegree = $('#degreeTb').val();
    if (!selectedCourseId == "") {
        $('#subjectsWrapper').load('/StudentsOffice/GetCourseSubjects?courseId=' + selectedCourseId + '&year=' + selectedYear + '&degree=' + selectedDegree);
    }
}

function setSelectedSubject(radioBtn) {
    selectedSubjectId = $(radioBtn).val();
    $('#SubjectId').val(selectedSubjectId);
}

function showTimetable() {
    hours = $('#requiredHours').val();

    $.ajax({
        url: '/StudentsOffice/GetTimetable',
        method: 'get',
        data: {
            hours: hours,
            courseId: selectedCourseId,
            year: selectedYear,
            degree: selectedDegree,
            subjectId: selectedSubjectId,
            isLab: isLab
        },
        success: function (e) {
            $('#timetable').html(e);
            if (e.indexOf('Error') == -1) {
                $('#createScheduleBtn').removeClass('hidden');
            }
        },
        error: function (e) {
            alert('An error occurred retrieving the data.');
        }
    });
}

function setSelectedClassroom(classroomId, intervalId) {

    $('#SelectedClassroomId').val(classroomId);
    $('#SelectedIntervalId').val(intervalId);
}

function setLabOrLecture() {
    isLab = $('#labOrLectureSelect').val() == 'lab';
    $('#isLab').val(isLab);
}