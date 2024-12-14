using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    private Dictionary<string, List<string>> burgers = new Dictionary<string, List<string>>
        {
            { "Jalapeno Burger" , new List<string>
            {
                "BottomBun", "Beef","Cheese","Tomato","Lettuce","Onion","Jalapeno","TopBun"
            }},
            { "Cheeseburger" , new List<string>
            {
                "BottomBun","Beef","Cheese","Lettuce","Onion","Pickles","TopBun"
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

    public void AssignDrink(SelectEnterEventArgs args)
    {
        drink = args.interactableObject.transform.gameObject;
    }

    public void RemoveDrink(SelectExitEventArgs args)
    {
        drink = null;
    }

    public void AssignFries(SelectEnterEventArgs args)
    {
        fries = args.interactableObject.transform.gameObject;
    }

    public void RemoveFries(SelectExitEventArgs args)
    {
        fries = null;
    }

    public void Submit()
    {
        gameManager.meals += 1;
        if (drink != null)
        {
            Drink drinkScript = drink.GetComponent<Drink>();
            if (drinkScript.Ice == false)
            {
                rating -= 0.25f;
            }
            if (drinkScript.typeOfDrink != order[2])
            {
                rating -= 0.75f;
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

        brating += (requiredIngredients-totalMatches) * 0.25f;
        if (totalIngredients > requiredIngredients)
        {
            brating += (totalIngredients - requiredIngredients) * 0.25f;
        }

        brating = Mathf.Clamp(brating, 0f, 2.5f);
        rating -= brating;

        gameManager.UpdateRating(rating);
        Destroy(tray);
    }
}
