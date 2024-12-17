import { firebaseConfig } from './config.js';
import { initializeApp } from "https://www.gstatic.com/firebasejs/11.0.2/firebase-app.js";

import{
    getDatabase,
    ref,
    push
} from "https://www.gstatic.com/firebasejs/11.0.2/firebase-database.js";

import { 
    getAuth,
    createUserWithEmailAndPassword,
    signInWithEmailAndPassword
} from "https://www.gstatic.com/firebasejs/11.0.2/firebase-auth.js";

const app = initializeApp(firebaseConfig);
const db = getDatabase(app);


// Handle Firebase Login (basic example for `login.html`)
document.getElementById("loginForm")?.addEventListener("submit", async (e) => {
    e.preventDefault();

    const email = document.getElementById("email").value;
    const password = document.getElementById("password").value;
    const auth = getAuth();
    signInWithEmailAndPassword(auth, email, password)
      .then((userCredential) => {
        const user = userCredential.user;
        localStorage.setItem('userID', user.uid); 
        window.location.href = "dashboard.html";
      })
      .catch((error) => {
        const errorCode = error.code;
        const errorMessage = error.message;
        document.getElementById("errorMessage").innerText = errorCode + ": " + errorMessage;
      });
});