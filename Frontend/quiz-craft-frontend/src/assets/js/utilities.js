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
    question.grade = 0;
    
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
        let correct = 0, selected = 0;
        for (let i in answers) {
            if (answers[i].selected) {
                
                if(answers[i].correct){
                    selected++;
                }
                correct++;
            }
        }
        if(correct == selected){
            question.grade = question.maxGrade;
        }
    }
};

export const updateTaskAnswers = (questions, response) => {
    for (const question of questions) {
        const questionResponse = response["question_responses"].find(
            (questionResponse) => questionResponse.question === question.id
        );

        if (questionResponse) {
            question.responseId = questionResponse.id;

            if (question.type === "SINGLE" || question.type === "MULTI") {
                for (const answer of question.answers) {
                    if(answer.id === questionResponse.answer){
                        
                        answer.selected = answer.id;
                    }
                }

                addGrade(question, question.answers);
                continue;
            }

            if (question.type === "TEXT") {
                question.selected = questionResponse["text_answer"];
            }
        }
    }
};