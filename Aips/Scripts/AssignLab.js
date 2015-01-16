var facultyId = 0;

var courseId = 0;
var year = 0;
var degree = 0;

var subjectId = 0;

function getFacultyCourses() {
    facultyId = $('#Faculties').val();

    if (facultyId != "") {
        $.ajax({
            url: '/StudentsOffice/GetFacultyCoursesCopy',
            method: 'get',
            data: {
                facultyId: facultyId,
            },
            success: function (e) {
                $('#facultyCourses').html(e);
            },
            error: function (e) {
                alert('An error has ocured retrieving the data.');
            }
        });
    }
}
function showSubjects() {

    courseId = $('#Courses').val();
    year = $('#yearTb').val();
    degree = $('#degreeTb').val();

    if (!courseId == "") {
        $.ajax({
            url: '/StudentsOffice/GetUnassignedSubjectsForAssistants',
            method: 'get',
            data: {
                courseId: courseId,
                year: year,
                degree: degree
            },
            success: function (e) {
                $('#courseSubjects').html(e);
                $('#timetable-section').remove();
            },
            error: function (e) {
                alert('An error has ocured retrieving the data.');
            }
        });
    }

}
function setSelectedSubject(radioBtn) {

    subjectId = $(radioBtn).val();
    $('#SubjectId').val(subjectId);

    $.ajax({
        url: '/StudentsOffice/GetAssistants',
        method: 'get',
        success: function (e) {
            $('#assistants').html(e);
        },
        error: function (e) {
            alert('An error has ocured retrieving the data.');
        }
    });

}