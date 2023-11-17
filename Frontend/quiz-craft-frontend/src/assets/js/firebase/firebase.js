import { initializeApp } from 'firebase/app';

const firebaseConfig = {
    apiKey: "AIzaSyDApa0Lneblwt9R3_Jw2bCv22Ijx6127-c",
    authDomain: "quiz-craft-app.firebaseapp.com",
    projectId: "quiz-craft-app",
    storageBucket: "quiz-craft-app.appspot.com",
    messagingSenderId: "723472284010",
    appId: "1:723472284010:web:2946793f240836bec3cd13",
    measurementId: "G-J5N80QXHW0"
};

export const init = () => initializeApp(firebaseConfig);