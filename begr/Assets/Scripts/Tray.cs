using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class Tray: MonoBehaviour
{
    public BurgerBuilder plate;
    public GameObject drink;
    public GameObject fries;
    public TextMeshProUGUI text;
    public TextMeshProUGUI orderNum;

    private GameObject tray;
    private GameManager gameManager;
    private OrderGenerator orderGenerator;
    private float rating = 5;
    private string[] order;
    private Transform spawnPoint;

    private XRSocketInteractor[] sockets;

    private Dictionary<string, List<string>> burgers = new Dictionary<string, List<string>>
        {
            { "Jalapeno Burjer" , new List<string>
            {
                "BotBun", "Beef","Cheese","Lettuce","Tomato","Onion","Jalapenos","TopBun"
            }},
            { "Cheeseburger" , new List<string>
            {
                "BotBun","Beef","Cheese","Lettuce","Onion","Pickles","TopBun"
            }},
            { "Flamin' Stack" , new List<string>
            {
                "BotBun","Beef","Cheese","Beef","Cheese","Jalapenos","Onion","TopBun"
            }}
         };
    

    private void Start()
    {
        tray = gameObject;
        gameManager = FindAnyObjectByType<GameManager>();
        orderGenerator = FindAnyObjectByType<OrderGenerator>();

        order = orderGenerator.GenerateOrder();
        orderNum.text = "Order Num: "+order[0];
        text.text = order[1] + ", " + order[2] + ", " + order[3];
    }

    private void Awake()
    {
        sockets = GetComponentsInChildren<XRSocketInteractor>();
    }

    public void AssignDrink(SelectEnterEventArgs args)
    {
        drink = args.interactableObject.transform.gameObject;
        drink.transform.SetParent(tray.transform, true);
    }

    public void RemoveDrink(SelectExitEventArgs args)
    {
        drink.transform.parent = null;
        drink = null;
    }

    public void AssignFries(SelectEnterEventArgs args)
    {
        fries = args.interactableObject.transform.gameObject;
        fries.transform.SetParent(tray.transform, true);
    }

    public void RemoveFries(SelectExitEventArgs args)
    {
        fries.transform.parent = null;
        fries = null;
    }

    public void Submit()
    {
        gameManager.meals += 1;
        gameManager.OnTraySubmitted(spawnPoint);
        if (drink != null)
        {
            Drink drinkScript = drink.GetComponent<Drink>();
            if (drinkScript.fillLevel < 1)
            {
                rating -= 0.35f;
            }
            if (drinkScript.fillLevel < 0.5f)
            {
                rating -= 1f;
            }
            else
            {
                if (drinkScript.Ice == false)
                {
                    rating -= 0.25f;
                }
                if (drinkScript.typeOfDrink != order[2])
                {
                    rating -= 0.4f;
                }
            }
        }
        else
        {
            rating -= 1;
        }
        List<string> playerBurger = plate.GetAssembledIngredients();
        var matchingIngredients = burgers[order[1]].Intersect(playerBurger).ToList();
        int totalIngredients = playerBurger.Count;
        int totalMatches = matchingIngredients.Count;
        int requiredIngredients = burgers[order[1]].Count;
        float brating = 0f;

        brating += (requiredIngredients-totalMatches) * (2.5f / requiredIngredients);
        if (totalIngredients > requiredIngredients)
        {
            brating += (totalIngredients - requiredIngredients) * (2.5f / requiredIngredients);
        }

        brating = Mathf.Clamp(brating, 0f, 2.5f);
        rating -= brating;

        if (fries != null)
        {

        }
        else
        {
            rating -= 1.5f;
        }

        gameManager.UpdateRating(rating);
        DestroySelf();
    }

    public void DestroySelf()
    {
        if (drink != null)
        {
            Destroy(drink);
        }
        Destroy(gameObject);
    }

    public void SetManager(GameManager manager, Transform spawn)
    {
        spawnPoint = spawn;

    }

}
