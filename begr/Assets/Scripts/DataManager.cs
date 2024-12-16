using Firebase.Database;
using Firebase;
using UnityEngine;
using Firebase.Database;
using Firebase.Extensions;
using System.Net.NetworkInformation;
using Google.MiniJSON;
using System.Collections.Generic;

public class DataManager : MonoBehaviour
{
    private DatabaseReference dbReference;

    private string playerUUID; // Assuming this is from your AuthManager script
    private AuthManager authManager;

    void Start()
    {
        // Initialize Firebase
        dbReference = FirebaseDatabase.DefaultInstance.RootReference;

        authManager = FindAnyObjectByType<AuthManager>();
        playerUUID = "TestUUID";
    }

    private void GetUUID()
    {
        //playerUUID = authManager.userId;
        playerUUID = "TestUUID";
    }

    // Call this method when you want to save the player's stats to Firebase


    public void CreatePlayer(string name)
    {
        GetUUID();
        var round = new Round(0, 0, 0, 0);

        var Player = new Player
        {
            Name = name,
            BestRound1 = round,
            BestRound2 = round,
            BestRound3 = round
        };

        string json = JsonUtility.ToJson(Player);
        dbReference.Child("Users").Child(playerUUID).SetRawJsonValueAsync(json).ContinueWithOnMainThread(task =>
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

    public void PushRoundData(int score, int meals, float rating, int trash)
    {
        GetUUID();
        Debug.Log("Checking Database");
        dbReference.Child("Users").Child(playerUUID).GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                Debug.Log("Getting User Rounds");
                DataSnapshot snapshot = task.Result;
                List<Round> rounds = new List<Round> {};
                Round currentRound = new Round(score, meals, rating, trash);
                rounds.Add(currentRound);
                foreach (var child in snapshot.Children)
                {
                    if (child.Key.StartsWith("BestRound"))
                    {
                        Round roundData = new Round
                        (
                            score = int.Parse(child.Child("score").Value.ToString()),
                            meals = int.Parse(child.Child("meals").Value.ToString()),
                            rating = float.Parse(child.Child("rating").Value.ToString()),
                            trash = int.Parse(child.Child("trash").Value.ToString())
                        );
                        rounds.Add(roundData);
                    }
                }
                rounds.Sort((a, b) => (int)b.score.CompareTo((int)a.score));
                while (rounds.Count > 3)
                {
                    rounds.RemoveAt(rounds.Count - 1);
                }

                Debug.Log("Pushing Data");
                for (int i = 0; i < rounds.Count; i++)
                {
                    string roundKey = $"BestRound{i + 1}";
                    Dictionary<string, object> round = new Dictionary<string, object>()
                    {
                        {"score",rounds[i].score},
                        {"meals",rounds[i].meals},
                        {"rating",rounds[i].rating},
                        {"trash",rounds[i].trash}
                    };
                    dbReference.Child("Users").Child(playerUUID).Child(roundKey).UpdateChildrenAsync(round).ContinueWithOnMainThread(pushTask =>
                    {
                        if (pushTask.IsCompleted)
                        {
                            Debug.Log($"Data updated for {roundKey}.");
                        }
                        else
                        {
                            Debug.LogError($"Failed to update data for {roundKey}: {pushTask.Exception}");
                        }
                    });
                }
            }
            else if (task.IsCanceled ||  task.IsFaulted)
            {
                Debug.LogError($"Failed to retrieve player data: {task.Exception}");
            }
        });
    }
}


[System.Serializable]
public class Round
{
    public int meals;
    public int score;
    public float rating;
    public int trash;
    public Round() { }

    public Round(int meals, int score, float rating, int trash)
    {
        this.meals = meals;
        this.score = score;
        this.rating = rating;
        this.trash = trash;
    }
}
[System.Serializable]
public class Player
{
    public string Name;
    public Round BestRound1;
    public Round BestRound2;
    public Round BestRound3;
}