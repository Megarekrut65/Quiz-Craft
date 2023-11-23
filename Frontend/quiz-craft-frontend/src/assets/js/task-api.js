import { SERVER_URL } from "./data";
import { sendAsync } from "./utilities";

export const saveTask = async(task)=>{
    console.log(task);

    const request = {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify(task)
    };
    return sendAsync(`${SERVER_URL}api/tasks/create/`, request);
};

export const getTaskById = async(id)=>{
    const request = {
        method: "GET",
        headers: {
            "Content-Type": "application/json"
        }
    };
    return sendAsync(`${SERVER_URL}tasks/${id}/`, request);
};

export const getTaskByUid = async(uid)=>{
    const request = {
        method: "GET",
        headers: {
            "Content-Type": "application/json"
        }
    };
    return sendAsync(`${SERVER_URL}tasks/uid/get-task/${uid}/`, request);
};