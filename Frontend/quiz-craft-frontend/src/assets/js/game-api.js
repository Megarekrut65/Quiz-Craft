import { SERVER_URL } from "./data";
import { sendAsync } from "./utilities";
import {getToken} from "./user-api";

export const saveGame = async(game)=>{
    const request = {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
            Authorization: `Token ${getToken()}`
        },
        body: JSON.stringify(game)
    };
    return sendAsync(`${SERVER_URL}api/games/`, request);
};

export const getGameById = async(id)=>{
    const request = {
        method: "GET",
        headers: {
            "Content-Type": "application/json",
            Authorization: `Token ${getToken()}`
        }
    };
    return sendAsync(`${SERVER_URL}api/games/${id}/`, request);
};

export const getGameByUid = async(uid)=>{
    const request = {
        method: "GET",
        headers: {
            "Content-Type": "application/json",
            Authorization: `Token ${getToken()}`
        }
    };
    return sendAsync(`${SERVER_URL}api/games/uid/get-game/${uid}/`, request);
};

export const shareGame = (id)=>{
    const data = {
        "game_id":id
    };

    const request = {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
            Authorization: `Token ${getToken()}`
        },
        body: JSON.stringify(data)
    };

    return sendAsync(`${SERVER_URL}api/games/generate-uid/`, request);
};

export const getGameUid = (id)=>{
    const request = {
        method: "GET",
        headers: {
            "Content-Type": "application/json",
            Authorization: `Token ${getToken()}`
        }
    };

    return sendAsync(`${SERVER_URL}api/games/uid/${id}/`, request);
};

export const closeGame = (id)=>{
    const request = {
        method: "DELETE",
        headers: {
            "Content-Type": "application/json",
            Authorization: `Token ${getToken()}`
        }
    };
    return sendAsync(`${SERVER_URL}api/games/uid/${id}`, request);
};

export const getGames = ()=>{
    const request = {
        method: "GET",
        headers: {
            "Content-Type": "application/json",
            Authorization: `Token ${getToken()}`
        }
    };

    return sendAsync(`${SERVER_URL}api/games/`, request);
};