var facultyId = 0;
var year = 0;
var degree = 0;
var courseId = 0;
var subjectId = 0;

function getFacultyCourses() {

    facultyId = $('#Faculty').val();

    $('#facultyCourses').load('/StudentsOffice/GetFacultyCoursesCopy?facultyId=' + facultyId);

}

function showSubjects() {

    courseId = $('#Courses').val();
    year = $('#yearTb').val();
    degree = $('#degreeTb').val();

    if (!courseId == "") {
        $.ajax({
            url: '/StudentsOffice/GetCourseSubjects',
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
                alert('An error has occurred retrieving the data.');
            }
        });
    }

}

function setSelectedSubject(radioBtn) {

    subjectId = $(radioBtn).val();

    $.ajax({
        url: '/StudentsOffice/GetExamsForSubject',
        method: 'get',
        data: {
            subjectId: subjectId
        },
        success: function (e) {
            $('#examsForSubject').html(e);
        },
        error: function (e) {
            alert('An error has occurred retrieving the data.');
        }
    });
    

}