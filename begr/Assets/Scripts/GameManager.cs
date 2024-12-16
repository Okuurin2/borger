using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class GameManager : MonoBehaviour
{
    public int meals;
    public int score;
    public float rating;
    public int trash;
    public GameObject trayPrefab;
    public Transform[] traySpawns;
    public ObjectSpawner[] objectSpawners;
    public int timer = 300;
    public bool gameStarted;

    public float minRespawnTime = 15f;   // Minimum time to respawn a tray
    public float maxRespawnTime = 30f;   // Maximum time to respawn a tray

    public GameObject helpThing;
    public Canvas GameUI;
    public Canvas[] tutorialPages;
    public Canvas EndUI;
    private int currentPage;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI mealsText;
    public TextMeshProUGUI ratingText;
    public TextMeshProUGUI trashText;

    private bool help;

    private DataManager dataManager;

    public TextMeshPro timerText; // Assign the TextMeshPro component in the Inspector
    private bool isTimerRunning = false;
    private float timeRemaining = 300f; // 5 minutes in seconds

    private Dictionary<Transform, GameObject> activeTrays = new Dictionary<Transform, GameObject>();

    public bool skipLogin;

    private void Start()
    {
        foreach (ObjectSpawner objectSpawner in objectSpawners)
        {
            objectSpawner.enabled = false;
        }
        helpThing.SetActive(false);
        EndUI.enabled = false;
        GameUI.enabled = false;
        dataManager = FindAnyObjectByType<DataManager>();
        if (skipLogin)
        {
            StartGame();
        }
    }

    public void UpdateRating(float newRating)
    {
        rating = (rating * (meals - 1) + newRating) / meals;
    }

    public void StartGame()
    {
        timeRemaining = timer;
        foreach (ObjectSpawner objectSpawner in objectSpawners)
        {
            objectSpawner.enabled = true;
        }
        foreach (Transform spawn in traySpawns)
        {
            SpawnTray(spawn);
        }
        GameUI.enabled = false;
        StartTimer();
        help = false;
        helpThing.SetActive(false);
        gameStarted = true;
    }

    public void HelpToggle()
    {
        if (help)
        {
            help = false;
            helpThing.SetActive(false);
            GameUI.gameObject.SetActive(true);
        }
        else
        {
            help = true;
            helpThing.SetActive(true);
            GameUI.gameObject.SetActive(false);
        }
        
    }


    private void SpawnTray(Transform spawnPoint)
    {
        // Instantiate a tray at the given spawn point
        GameObject tray = Instantiate(trayPrefab, spawnPoint.position, spawnPoint.rotation);
        tray.GetComponent<Tray>().SetManager(this, spawnPoint); // Link tray to GameManager
        activeTrays[spawnPoint] = tray; // Track the spawned tray
        //tray.transform.Rotate(0,90,0);
    }

    public void OnTraySubmitted(Transform spawnPoint)
    {
        // Remove the tray from the tracking dictionary
        activeTrays.Remove(spawnPoint);

        // Start a coroutine to respawn a tray after a random interval
        StartCoroutine(RespawnTrayAfterDelay(spawnPoint));
    }

    private IEnumerator RespawnTrayAfterDelay(Transform spawnPoint)
    {
        float delay = Random.Range(minRespawnTime, maxRespawnTime);
        yield return new WaitForSeconds(delay);
        SpawnTray(spawnPoint);
    }

    public void StartTimer()
    {
        if (!isTimerRunning)
        {
            isTimerRunning = true;
            StartCoroutine(UpdateTimer());
        }
    }

    // Coroutine to handle the countdown
    private IEnumerator UpdateTimer()
    {
        while (timeRemaining > 0)
        {
            timeRemaining -= 1;
            UpdateTimerDisplay();
            yield return new WaitForSeconds(1);
        }

        timeRemaining = 0;
        UpdateTimerDisplay();

        // Timer complete logic
        TimerComplete();
    }

    // Update the TextMeshPro display
    private void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(timeRemaining / 60);
        int seconds = Mathf.FloorToInt(timeRemaining % 60);
        timerText.text = $"{minutes:00}:{seconds:00}"; // Format as MM:SS
    }

    // Function called when the timer ends
    private void TimerComplete()
    {
        isTimerRunning = false;
        gameStarted = false;
        StopAllCoroutines();
        EndGame();
    }

    public void ResetGame()
    {
        StopAllCoroutines();
        isTimerRunning = false;
        timeRemaining = timer;
        UpdateTimerDisplay();
        score = 0;
        rating = 0;
        meals = 0;
        trash = 0;
        EndUI.enabled = false;
        StartGame();
    }

    public void NextPage()
    {
        if (currentPage < tutorialPages.Length - 1)
        {
            currentPage++;
            UpdatePageVisibility();
        }
    }

    public void PreviousPage()
    {
        if (currentPage > 0)
        {
            currentPage--;
            UpdatePageVisibility();
        }
    }

    private void UpdatePageVisibility()
    {
        // Activate the current page and deactivate others
        for (int i = 0; i < tutorialPages.Length; i++)
        {
            tutorialPages[i].gameObject.SetActive(i == currentPage);
        }
    }

    private void EndGame()
    {
        EndUI.enabled = true;
        score = Mathf.RoundToInt(meals * rating);
        rating = Mathf.Round(rating*10)/10;
        scoreText.text = "Final Score: " + score.ToString();
        mealsText.text = "Total Meals Served: " + meals.ToString();
        ratingText.text = "Average Rating: " + rating.ToString();
        trashText.text = "Items Trashed: " + trash.ToString();
        foreach (GameObject tray in activeTrays.Values)
        {
            tray.GetComponent<Tray>().DestroySelf();
        }
        foreach (ObjectSpawner objectSpawner in objectSpawners)
        {
            objectSpawner.enabled = false;
        }
        activeTrays = new Dictionary<Transform, GameObject>();
        dataManager.PushRoundData(score,meals,rating,trash);
    }
}
