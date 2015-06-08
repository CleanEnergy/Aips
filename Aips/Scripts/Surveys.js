$(function () {
    addQuestion();
});

var questions = [];

var elementIdCounter = 0;
var questionIdCounter = 0;

function addQuestion() {

    questionIdCounter++;

    var newQuestion = {
        id: questionIdCounter,
        answersIdCounter: 0,
        answers: [],
        answersHolderWrapper: null,
        answersHolderElement: null,
        questionInputElement: null,
        questionHolderElement: null,
        questionTypeSelectElement: null,
        questionType: ''
    };

    var html = '';
    var questionContainerId = ++elementIdCounter;
    var questionTextBoxId = ++elementIdCounter;
    var questionTypeSelectId = ++elementIdCounter;
    var answersListId = ++elementIdCounter;
    var answersHolderWrapperId = ++elementIdCounter;

    html += '<div class="questionHolder form-group" id="' + questionContainerId + '"></div>';
    $('#questionsArea').append(html);

    newQuestion.questionHolderElement = $('#' + questionContainerId);

    html = '<label class="control-label col-md-2">Question</label>';
    html += '<div class="col-md-10">';
    html += '<input class="form-control" type="text" id="' + questionTextBoxId + '" />';
    html += '</div>';
    $('#' + questionContainerId).append(html);

    newQuestion.questionInputElement = $('#' + questionTextBoxId);

    html = '<div class="form-group">';
    html += '<label class="control-label col-md-2">Type</label>';
    html += '<div class="col-md-10">';
    html += '<select id="' + questionTypeSelectId + '" onchange="changeQuestionTemplate(' + newQuestion.id + ')"></select>';
    html += '</div>';
    html += '</div>';
    $('#' + questionContainerId).append(html);

    newQuestion.questionTypeSelectElement = $('#' + questionTypeSelectId);

    html = '<option value="multiple">Multiple</option>';
    html += '<option value="single">Single</option>';
    html += '<option value="text">Text</option>';

    $(newQuestion.questionTypeSelectElement).append(html);

    html = '<div class="form-group" id="' + answersHolderWrapperId + '"></div>';
    $('#' + questionContainerId).append(html);

    newQuestion.answersHolderWrapper = $('#' + answersHolderWrapperId);

    html = '<label class="control-label col-md-2">Answers</label>';
    html += '<div class="col-md-10">';
    html += '<ul class="list-group" id="' + answersListId + '"></ul>';
    html += '</div>';
    $(newQuestion.answersHolderWrapper).append(html);

    newQuestion.answersHolderElement = $('#' + answersListId);
    newQuestion.questionType = 'multiple';

    html = '<label class="col-md-2"></label>';
    html += '<input class="btn btn-danger pull-right" type="button" value="Remove question" onclick="removeQuestion(' + newQuestion.id + ')" />';
    $('#' + questionContainerId).append(html);

    questions.push(newQuestion);

    addAnswer(newQuestion.id);

    html = '<input class="btn btn-default pull" type="button" class="addAnswerBtn" value="Add answer" onclick="addAnswer(' + newQuestion.id + ')" />';
    html += '<hr/>';

    $('#' + questionContainerId).append(html);
}

function changeQuestionTemplate(questionId) {
    var question = GetQuestionObject(questionId);


    var answersListId = ++elementIdCounter;
    var selectedType = $(question.questionTypeSelectElement).val();

    if (selectedType == 'multiple' || selectedType == 'single') {
        if (question.questionType == 'text') {
            html = '<label class="control-label col-md-2">Answers</label>';
            html += '<div class="col-md-10">';
            html += '<ul class="list-group" id="' + answersListId + '"></ul>';
            html += '</div>';
            $(question.answersHolderWrapper).html(html);

            $('.addAnswerBtn').css('visibility', 'visible');

            question.answersHolderElement = $('#' + answersListId);
            question.answers = [];
            addAnswer(questionId);
        }

    } else {
        html = '<label class="control-label col-md-2">Answers</label>';
        html += '<div class="col-md-10">';
        html += '<textarea id="' + answersListId + '"></textarea>';
        html += '</div>';
        $(question.answersHolderWrapper).html(html);

        $('.addAnswerBtn').css('visibility', 'hidden');

        question.answersHolderElement = $('#' + answersListId);

        var question = GetQuestionObject(questionId);
        question.answers = [];
        question.answersIdCounter++
        var answerId = question.answersIdCounter;

        var newAnswer = {
            id: answerId,
            answerInputElement: null,
            answerHolderElement: null
        };

        newAnswer.answerInputElement = $('#' + answersListId);
        question.answers.push(newAnswer);
    }
    question.questionType = selectedType;
}

function removeQuestion(questionId) {

    var indexToRemove = -1;
    for (var i = 0; i < questions.length; i++) {
        if (questions[i].id == questionId) {
            indexToRemove = i;
        }
    }

    var question = GetQuestionObject(questionId);
    $(question.questionHolderElement).remove();

    questions.splice(indexToRemove, 1);


}

function addAnswer(questionId) {

    var question = GetQuestionObject(questionId);
    question.answersIdCounter++
    var answerId = question.answersIdCounter;

    var newAnswer = {
        id: answerId,
        answerInputElement: null,
        answerHolderElement: null
    };

    var answerHolderElementId = ++elementIdCounter;
    var answerInputElementId = ++elementIdCounter;
    
    var html = '<li class="list-group-item" id="' + answerHolderElementId + '"></li>';
    $(question.answersHolderElement).append(html);
    
    newAnswer.answerHolderElement = $('#' + answerHolderElementId);

    html = '<input class="form-control" type="text" id="' + answerInputElementId + '" />';
    $(newAnswer.answerHolderElement).append(html);

    newAnswer.answerInputElement = $('#' + answerInputElementId);

    if (question.answers.length != 0) {
        html = '<div class="text-right" style="margin-top:5px;">';
        html += '<input type="button" value="Remove" class="btn btn-info btn-sm" onclick="removeAnswer(' + questionId + ',' + newAnswer.id + ')" />';
        html += '</div>';
        $(newAnswer.answerHolderElement).append(html);
    }

    question.answers.push(newAnswer);
}

function removeAnswer(questionId, answerId) {

    var questionObject = GetQuestionObject(questionId);
    var answerObject = GetAnswerObject(questionId, answerId);

    $(answerObject.answerHolderElement).remove();

    var indexToRemove = -1;
    for (var i = 0; i < questionObject.answers.length; i++) {
        if (questionObject.answers[i].id == answerId) {
            indexToRemove = i;
        }
    }

    questionObject.answers.splice(indexToRemove, 1);
}

function GetQuestionObject(questionId) {
    for (var i = 0; i < questions.length; i++) {
        if (questions[i].id == questionId) {
            return questions[i];
        }
    }
}
function GetAnswerObject(questionId, answerId) {

    var question = GetQuestionObject(questionId);
    
    for (var i = 0; i < question.answers.length; i++) {
        if (question.answers[i].id == answerId) {
            return question.answers[i];
        }
    }

}

var parsedQuestions = [];

function parseQuestions() {

    parsedQuestions = [];

    for (var i = 0; i < questions.length; i++) {

        var arrayQuestion = questions[i];

        var question = {
            id: arrayQuestion.id,
            type: arrayQuestion.questionType,
            questionText: $(arrayQuestion.questionInputElement).val(),
            answers: []
        };

        if (question.type != 'text') {
            for (var j = 0; j < questions[i].answers.length; j++) {
                var answerText = $(questions[i].answers[j].answerInputElement).val();
                question.answers.push(answerText);

            }
        } else {
            question.answers = [];
            question.answers.push($(questions[i].answers[0].answerInputElement).val());
        }

        parsedQuestions.push(question);
    }

}

function submitSurvey() {

    parseQuestions();

    var form = $('#surveyForm');
    var token = $('input[name="__RequestVerificationToken"]', form).val();

    $.ajax({
        url: '/StudentsOffice/CreateSurvey',
        method: 'post',
        data: {
            Title: $('#surveyTitle').val(),
            Visible: $('#Visible').is(':checked'),
            SurveyData: JSON.stringify(parsedQuestions),
            __RequestVerificationToken: token
        },
        success: function (result) {

            window.location.href = '/StudentsOffice/TestSurvey?id=' + result.id;

        },
        error: function (e) {
            alert('An error has occurred.');
        }
    });

}