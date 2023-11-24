import { SERVER_URL } from "./data";
import { getToken, sendAsync } from "./utilities";

export const sendResponse = (response)=>{
    const request = {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
            Authorization: `Token ${getToken()}`
        },
        body: JSON.stringify(response)
    };
    return sendAsync(`${SERVER_URL}api/task-responses/create/`, request);
};

export const getResponse = (uid)=>{
    const request = {
        method: "GET",
        headers: {
            "Content-Type": "application/json",
            Authorization: `Token ${getToken()}`
        }
    };
    return sendAsync(`${SERVER_URL}api/task-responses/${uid}/`, request);
};