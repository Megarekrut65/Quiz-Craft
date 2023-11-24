import { SERVER_URL } from "./data";
import { sendAsync } from "./utilities";
import {getToken} from "./user-api";

export const saveTask = async(task)=>{
    const request = {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
            Authorization: `Token ${getToken()}`
        },
        body: JSON.stringify(task)
    };
    return sendAsync(`${SERVER_URL}api/tasks/create/`, request);
};

export const getTaskById = async(id)=>{
    const request = {
        method: "GET",
        headers: {
            "Content-Type": "application/json",
            Authorization: `Token ${getToken()}`
        }
    };
    return sendAsync(`${SERVER_URL}api/tasks/${id}/`, request);
};

export const getTaskByUid = async(uid)=>{
    const request = {
        method: "GET",
        headers: {
            "Content-Type": "application/json",
            Authorization: `Token ${getToken()}`
        }
    };
    return sendAsync(`${SERVER_URL}api/tasks/uid/get-task/${uid}/`, request);
};

export const shareTask = (id)=>{
    const data = {
        "task_id":id
    };

    const request = {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
            Authorization: `Token ${getToken()}`
        },
        body: JSON.stringify(data)
    };

    return sendAsync(`${SERVER_URL}api/tasks/generate-uid/`, request);
};

export const getTaskUid = (id)=>{
    const request = {
        method: "GET",
        headers: {
            "Content-Type": "application/json",
            Authorization: `Token ${getToken()}`
        }
    };

    return sendAsync(`${SERVER_URL}api/tasks/uid/${id}`, request);
};

export const closeTask = (id)=>{
    const request = {
        method: "DELETE",
        headers: {
            "Content-Type": "application/json",
            Authorization: `Token ${getToken()}`
        }
    };
    return sendAsync(`${SERVER_URL}api/tasks/uid/${id}`, request);
};

export const getTasks = ()=>{
    const request = {
        method: "GET",
        headers: {
            "Content-Type": "application/json",
            Authorization: `Token ${getToken()}`
        }
    };

    return sendAsync(`${SERVER_URL}api/tasks/`, request);
};