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

export const getToken = () => localStorage.getItem("token");

export const parseError = (err) => {
    try {
        const obj = JSON.parse(err.message);
        const message = obj.detail ? obj.detail : obj.error ? obj.error : JSON.stringify(obj);

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

export const updateTaskAnswers = (questions, response) => {
    for (const question of questions) {
        const questionResponse = response["question_responses"].find(
            (questionResponse) => questionResponse.question === question.id
        );

        if (questionResponse) {
            if (question.type === "SINGLE" || question.type === "MULTI") {
                for (const answer of question.answers) {
                    if(answer.id === questionResponse.answer){
                        answer.selected = answer.id;
                    }
                }
            } else if (question.type === "TEXT") {
                question.selected = questionResponse["text_answer"];
            }
        }
    }
};