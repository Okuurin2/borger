// Import Firebase modules
import { initializeApp } from "https://www.gstatic.com/firebasejs/11.0.2/firebase-app.js";
import { getDatabase, ref, onValue } from "https://www.gstatic.com/firebasejs/11.0.2/firebase-database.js";

// Firebase configuration
import { firebaseConfig } from "./config.js";

// Initialize Firebase
const app = initializeApp(firebaseConfig);
const db = getDatabase(app);

// Reference to the data location in the database
const dataRef = ref(db, "path/to/your/data");

// HTML table element
const table = document.getElementById("data-table");

// Fetch and display data
onValue(dataRef, (snapshot) => {
    // Clear table content except for the header row
    table.innerHTML = "<tr><th>Name</th><th>Score</th><th>Quality</th></tr>";

    // Iterate through the data snapshot
    snapshot.forEach((childSnapshot) => {
        const data = childSnapshot.val();

        // Create a new row for the table
        const row = table.insertRow();

        // Insert cells for Name, Score, and Quality
        const nameCell = row.insertCell(0);
        const scoreCell = row.insertCell(1);
        const qualityCell = row.insertCell(2);

        // Set cell values
        nameCell.textContent = data.Name;
        scoreCell.textContent = data.Score;
        qualityCell.textContent = data.Quality;
    });
});