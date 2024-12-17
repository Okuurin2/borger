// Import Firebase modules
import { initializeApp } from "https://www.gstatic.com/firebasejs/11.0.2/firebase-app.js";
import { getDatabase, ref, get } from "https://www.gstatic.com/firebasejs/11.0.2/firebase-database.js";

// Firebase configuration
import { firebaseConfig } from "./config.js"

// Initialize Firebase
const app = initializeApp(firebaseConfig);
const db = getDatabase(app);

const usersPerPage = 10;
let users = [];
let currentPage = 1;

// Fetch users from Firebase
async function fetchUsers() {
  try {
    const usersRef = ref(db, "Users");
    const snapshot = await get(usersRef);

    if (snapshot.exists()) {
      const data = snapshot.val();
      users = Object.values(data);
      renderUserList();
    } else {
      console.error("No data found");
    }
  } catch (error) {
    console.error("Error fetching data:", error);
  }
}

// Render users with pagination
function renderUserList() {
  const startIndex = (currentPage - 1) * usersPerPage;
  const endIndex = startIndex + usersPerPage;
  const usersToDisplay = users.slice(startIndex, endIndex);

  const userListContainer = document.getElementById("userList");
  userListContainer.innerHTML = "";

  usersToDisplay.forEach((user) => {
    if (user.Type =="Admin") return;
    const userDiv = document.createElement("div");
    userDiv.className = "user";

    // User Name
    const userName = document.createElement("div");
    userName.textContent = user.Name;
    userDiv.appendChild(userName);

    // Expandable Button for Best Rounds
    const roundsButton = document.createElement("button");
    roundsButton.textContent = "Show Top 3 Rounds";
    roundsButton.className = "button";
    roundsButton.onclick = () => toggleRounds(userDiv, user);
    userDiv.appendChild(roundsButton);

    // Append to the list
    userListContainer.appendChild(userDiv);
  });

  // Show/Hide Pagination Buttons
  document.getElementById("prevPageBtn").style.display = currentPage > 1 ? "inline" : "none";
  document.getElementById("nextPageBtn").style.display =
    currentPage * usersPerPage < users.length ? "inline" : "none";
}

// Toggle visibility of the top 3 rounds
function toggleRounds(userDiv, user) {
  let roundsDiv = userDiv.querySelector(".rounds");
  if (!roundsDiv) {
    // Create rounds display if not already created
    roundsDiv = document.createElement("div");
    roundsDiv.className = "rounds";

    // Best Rounds
    for (let i = 1; i <= 3; i++) {
      const roundData = user[`BestRound${i}`];
      const roundDiv = document.createElement("div");
      roundDiv.innerHTML = `
        <strong>Round ${i}</strong><br>
        Meals: ${roundData.meals}, Rating: ${roundData.rating}, Score: ${roundData.score}, Trash: ${roundData.trash}
      `;
      roundsDiv.appendChild(roundDiv);
    }

    userDiv.appendChild(roundsDiv);
  } else {
    // Toggle visibility
    roundsDiv.style.display = roundsDiv.style.display === "none" ? "block" : "none";
  }
}

async function getUser(userId) {
  try {
    const usersRef = ref(db, "Users/"+userId);
    const snapshot = await get(usersRef);

    if (snapshot.exists()) {
      const data = snapshot.val();
      return data.Type
    } else {
      console.error("No data found");
    }
  } catch (error) {
    console.error("Error fetching data:", error);
  }
}
// Change page
function changePage(direction) {
  currentPage += direction;
  renderUserList();
}

// Fetch and render users on page load
//const userId = localStorage.getItem("userID");
const userId = "TestUUID2"
if (  (userId).Type == "Admin")
{
  fetchUsers();
}
