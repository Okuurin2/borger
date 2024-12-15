using Firebase.Database;
using Firebase;
using UnityEngine;
using Firebase.Extensions;
using System.Net.NetworkInformation;
using Google.MiniJSON;

public class DataManager : MonoBehaviour
{
    private DatabaseReference reference;

    private string playerUUID; // Assuming this is from your AuthManager script
    private AuthManager authManager;

    void Start()
    {
        // Initialize Firebase
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            FirebaseApp app = FirebaseApp.DefaultInstance;
            reference = FirebaseDatabase.DefaultInstance.RootReference;
        });

        authManager = FindAnyObjectByType<AuthManager>();
        // Assuming AuthManager gives us the player's UUID (You may need to fetch this differently depending on your auth setup)
        playerUUID = "TestUUID";
    }

    private void GetUUID()
    {
        playerUUID = authManager.userId;
    }

    // Call this method when you want to save the player's stats to Firebase
    public void SaveStatsToFirebase(int meals, int score, float rating, int trash)
    {
        if (reference != null)
        {
            // Create a dictionary with player's stats
            var playerStats = new PlayerStatsData
            {
                meals = meals,
                score = score,
                rating = rating,
                trash = trash
            };

            // Convert the dictionary to JSON format
            string json = JsonUtility.ToJson(playerStats);

            // Push the data to Firebase under the user's UUID
            reference.Child("Users").Child(playerUUID).SetRawJsonValueAsync(json).ContinueWithOnMainThread(task =>
            {
                if (task.IsCompleted)
                {
                    Debug.Log("Player stats saved to Firebase.");
                }
                else
                {
                    Debug.LogError("Failed to save player stats: " + task.Exception);
                }
            });
        }
        else
        {
            Debug.LogError("Firebase reference is null.");
        }
    }

    public void CreatePlayer(string name)
    {
        var playerStats = new PlayerStatsData
        {
            meals = 0,
            score = 0,
            rating = 0,
            trash = 0
        };

        string json = JsonUtility.ToJson(playerStats);
        reference.Child("Users").Child(playerUUID).SetRawJsonValueAsync(json).ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                Debug.Log("Player stats saved to Firebase.");
            }
            else
            {
                Debug.LogError("Failed to save player stats: " + task.Exception);
            }
        });

        var Player = new Player
        {
            Name = name
        };
        json = JsonUtility.ToJson(Player);
        reference.Child("Users").Child(playerUUID).SetRawJsonValueAsync(json).ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                Debug.Log("Player created");
            }
            else
            {
                Debug.LogError("Failed to create player: " + task.Exception);
            }
        });
    }
}

// Class to represent the player's stats data
[System.Serializable]
public class PlayerStatsData
{
    public int meals;
    public int score;
    public float rating;
    public int trash;
}
[System.Serializable]
public class Player
{
    public string Name;
}