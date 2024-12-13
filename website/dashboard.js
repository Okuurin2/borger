// Import Firebase modules
import { initializeApp } from "https://www.gstatic.com/firebasejs/11.0.2/firebase-app.js";
import { getDatabase, ref, onValue } from "https://www.gstatic.com/firebasejs/11.0.2/firebase-database.js";

// Firebase configuration
import { firebaseConfig } from "./config.js";

// Initialize Firebase
const app = initializeApp(firebaseConfig);
const db = getDatabase(app);

// Reference to the data location in the database
const dataRef = ref(db, "Users");

// HTML table element
const table = document.getElementById("data-table");

// Fetch and display data
onValue(dataRef, (snapshot) => {
    // Clear table content except for the header row
    table.innerHTML = "<tr><th>Name</th><th>Score</th><th>Rating</th></tr>";

    // Check if the snapshot has any data
    if (snapshot.exists()) {
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
            if (data.Type == "Player")
            {
                nameCell.textContent = data.Name || "N/A";
                scoreCell.textContent = data.Score !== undefined ? data.Score : "N/A";
                qualityCell.textContent = data.Rating !== undefined ? data.Rating : "N/A";
            }
        });
    } else {
        // If no data, display a message
        const row = table.insertRow();
        const cell = row.insertCell(0);
        cell.colSpan = 3;
        cell.textContent = "No data available.";
        cell.style.textAlign = "center";
    }
});