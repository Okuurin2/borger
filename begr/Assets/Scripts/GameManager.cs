using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class GameManager : MonoBehaviour
{
    private Dictionary<string, string[]> burgers;
    private OrderGenerator orderGenerator;
    private List<string[]> ordersList;

    public int meals;
    public int score;
    public float rating;
    public int trash;
    public GameObject trayPrefab;
    public Transform[] traySpawns;
    public ObjectSpawner[] objectSpawners;
    public int timer = 300;

    public float minRespawnTime = 5f;   // Minimum time to respawn a tray
    public float maxRespawnTime = 20f;   // Maximum time to respawn a tray

    public GameObject helpThing;
    public Canvas GameUI;
    public Canvas[] tutorialPages;
    private int currentPage;

    private bool help;

    public TextMeshPro timerText; // Assign the TextMeshPro component in the Inspector
    private bool isTimerRunning = false;
    private float timeRemaining = 300f; // 5 minutes in seconds

    private Dictionary<Transform, GameObject> activeTrays = new Dictionary<Transform, GameObject>();

    private void Start()
    {
        burgers = new Dictionary<string, string[]>
        {
            {"Jalapeno Burger" , new string[]
            {
                "BottomBun", "Beef","Cheese","Tomato","Lettuce","Jalapeno","TopBun"
            }},
            {"Cheeseburger" , new string[]
            {
                "BottomBun","Beef","Cheese","Lettuce","Pickles","TopBun"
            }}

        };
        orderGenerator = FindFirstObjectByType<OrderGenerator>();
        foreach (ObjectSpawner objectSpawner in objectSpawners)
        {
            objectSpawner.enabled = false;
        }
        helpThing.SetActive(false);
        //StartGame();
    }

    public void UpdateRating(float newRating)
    {
        rating = (rating * (meals - 1) + newRating) / meals;
    }

    public void StartGame()
    {
        foreach (ObjectSpawner objectSpawner in objectSpawners)
        {
            objectSpawner.enabled = true;
        }
        foreach (Transform spawn in traySpawns)
        {
            RespawnTrayAfterDelay(spawn);
        }
        StartTimer();
        help = false;
        helpThing.SetActive(false);
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
        Debug.Log("Timer Complete!");
    }

    public void ResetTimer()
    {
        StopAllCoroutines();
        isTimerRunning = false;
        timeRemaining = 300f;
        UpdateTimerDisplay();
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

    void UpdatePageVisibility()
    {
        // Activate the current page and deactivate others
        for (int i = 0; i < tutorialPages.Length; i++)
        {
            tutorialPages[i].gameObject.SetActive(i == currentPage);
        }
    }
}
