import { SERVER_URL } from "./data";
import { sendAsync } from "./utilities";
import {getToken} from "./user-api";

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

export const getResponseByUid = (uid)=>{
    const request = {
        method: "GET",
        headers: {
            "Content-Type": "application/json",
            Authorization: `Token ${getToken()}`
        }
    };
    return sendAsync(`${SERVER_URL}api/task-responses/${uid}/`, request);
};
export const getResponseById = (id)=>{
    const request = {
        method: "GET",
        headers: {
            "Content-Type": "application/json",
            Authorization: `Token ${getToken()}`
        }
    };
    return sendAsync(`${SERVER_URL}api/tasks/response/${id}/`, request);
};

export const getResponses = (taskId)=>{
    const request = {
        method: "GET",
        headers: {
            "Content-Type": "application/json",
            Authorization: `Token ${getToken()}`
        }
    };
    return sendAsync(`${SERVER_URL}api/tasks/responses/${taskId}/`, request);
};