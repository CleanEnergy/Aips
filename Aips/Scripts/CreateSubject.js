var facultyId = 0;

function getFacultyCourses() {

    facultyId = $('#Faculty').val();

    $('#coursesProfessors').load('/StudentsOffice/GetCoursesWithProfessors?facultyId=' + facultyId);

}

function showSubjects() {

    if (!courseId == "") {
        $.ajax({
            url: '/StudentsOffice/GetCoursesWithProfessors',
            method: 'get',
            data: {
                facultyId: facultyId,
            },
            success: function (e) {
                $('#coursesProfessors').html(e);
            },
            error: function (e) {
                alert('An error has ocured retrieving the data.');
            }
        });
    }

}