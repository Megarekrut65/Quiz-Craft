/**
 * 
 * @param {string} endpoint - path to server request
 * @param {Map} request - request to send it to server
 * @returns promise to server answer data or error
 */
export const sendAsync = async (endpoint, request) => {
    const response = await fetch(endpoint, request);

    if (!response.ok) {
        const text = await response.text();

        throw new Error(text);
    }
    if (response.statusText === "No Content") return response.text();
    return response.json();
};


export const parseError = (err) => {
    try {
        const obj = JSON.parse(err.message);
        const message = obj.detail ? obj.detail : obj.error ? obj.error : JSON.stringify(obj);
        if (typeof message !== "string"){
            return message.message?message.message:JSON.stringify(message);
        } 
        if (message === "Invalid token."){
            window.location.href = "/login";
        }
        return message;
    } catch (error) {
        return err;
    }
};

export const copyToBuffer = (text) => {
    const item = document.createElement("div");
    item.textContent = text;
    document.body.appendChild(item);

    const range = document.createRange();
    range.selectNodeContents(item);
    const selection = window.getSelection();
    selection.removeAllRanges();
    selection.addRange(range);

    document.execCommand('copy');

    document.body.removeChild(item);
};


const addGrade = (question, answers)=>{
    
    if (question.type === "SINGLE") {
        for (let i in answers) {
            if (answers[i].selected) {
                if(answers[i].correct){
                    question.grade = question.maxGrade;
                }
                break;
            }
        }
        return;
    }
    
    if (question.type === "MULTI") {
        let correct = 0, selectedCorrect = 0, selected=0;
        for (let i in answers) {
            if (answers[i].selected) {
                selected++;

                if(answers[i].correct){
                    selectedCorrect++;
                }
            }
            if(answers[i].correct){
                correct++;
            }
        }
        question.grade = Math.max(parseInt((selectedCorrect/correct  - (selected-selectedCorrect)/correct)* question.maxGrade), 0);
    }
};

export const updateTaskAnswers = (questions, response) => {
    let sumGrade = 0, isGrade = false;

    for (const questionResponse of response["question_responses"]) {
        const question = questions.find((question) => question.id === questionResponse.question);

        if (question) {
            question.responseId = questionResponse.id;
            
            
            if(questionResponse.grade != null && questionResponse.grade != undefined){
                sumGrade += questionResponse.grade;
                question.grade = questionResponse.grade;
                isGrade = true;
            }

            if (question.type === "SINGLE" || question.type === "MULTI") {

                for (const answer of question.answers) {
                    if(answer.id === questionResponse.answer){
                        
                        answer.selected = answer.id;
                    }
                }

                if(questionResponse.grade == null || questionResponse.grade == undefined) addGrade(question, question.answers);
                continue;
            }

            if (question.type === "TEXT") {
                question.selected = questionResponse["text_answer"];
                if (question.maxGrade == 0) question.grade = 0;
                if(questionResponse.grade == null || questionResponse.grade == undefined) question.grade = 0;
            }
        }
    }

    return isGrade?sumGrade:undefined;
};