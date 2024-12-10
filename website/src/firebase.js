function redirectToLogin() {
    window.location.href = "login.html";
}

import { firebaseConfig } from './config.js';

import{
    getDatabase,
    ref,
    push
} from "https://www.gstatic.com/firebasejs/11.0.2/firebase-app.js";

const app = initializeApp(firebaseConfig);
const db = getDatabase(app);

// Handle Firebase Login (basic example for `login.html`)
document.getElementById("loginForm")?.addEventListener("submit", async (e) => {
    e.preventDefault();

    const email = document.getElementById("email").value;
    const password = document.getElementById("password").value;

    try {
        await app.auth().signInWithEmailAndPassword(email, password);
        window.location.href = "dashboard.html"; // Redirect to dashboard
    } catch (error) {
        document.getElementById("errorMessage").innerText = error.message;
    }
});