import { SERVER_URL } from "./data";
import { sendAsync } from "./utilities";



export const register = (username, email, fullname, password, role)=>{
    const request = {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify({"username":username, "password":password,"email":email, "role":role, "fullname":fullname})
    };
    return sendAsync(`${SERVER_URL}api/register/`, request);
};

export const login = (username, password)=>{
    const request = {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify({"username":username, "password":password})
    };
    return sendAsync(`${SERVER_URL}api/login/`, request)
    .then((res)=>{
        localStorage.setItem("token", res.token);
        localStorage.setItem("username", username);

        changeState(true); 
        return res;
    });
};

export const logout = ()=>{
    const request = {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
            Authorization: `Token ${getToken()}`
        },
        body:"{}"
    };
    return sendAsync(`${SERVER_URL}api/logout/`, request).then(
        ()=>{
            localStorage.clear();
            changeState(false);
        });
};

const events = [];

export const subscribeUserChangeState = (event)=>{
    events.push(event);
};

const changeState = (logged)=>{
    events.forEach(event=>event(logged));
}

export const userIsLogged = ()=>{
    if(localStorage.getItem("token")) return true;
    
    return false;
}

export const getUsername = ()=>localStorage.getItem("username");
export const getToken = () => localStorage.getItem("token");
export const getRole = () => localStorage.getItem("role");

export const isLoggedAs = (to, from, next, role) =>{
    if(userIsLogged() && (role === getRole()||true)){
        next();
        return;
    }
    next({ name: "unauthorized" });
};