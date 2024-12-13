using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class OrderGenerator : MonoBehaviour
{
    private Dictionary<string,string[]> burges;
    private string[] burgers;
    private string[] drinks;
    private string[] fries;
    private int orderNum = 0;

    public string[] GenerateOrder()
    {
        orderNum++;
        string chosenBurger = burgers[Random.Range(0, burgers.Length)];
        string chosenDrink = drinks[Random.Range(0, drinks.Length)];
        string chosenFries = fries[Random.Range(0, fries.Length)];
        return new string[] {orderNum.ToString(), chosenBurger, chosenDrink, chosenFries};
    }
    private void Start()
    {
        burges = new Dictionary<string,string[]>
        {
            {"Jalapeno Burger" , new string[]
            {
                "BottomBun", "Beef","Cheese","Tomato","Lettuce","Onion","Jalapeno","TopBun"
            }},
            {"Cheeseburger" , new string[]
            {
                "BottomBun","Beef","Cheese","Lettuce","Onion","Pickles","TopBun"
            }}

        };

        burgers = new string[]
        {
            "Jalapeno Burjer","Cheeseburger"
        };

        drinks = new string[]
        {
            "Coke","Sprite","Milo","Lemon Tea"
        };

        fries = new string[]
        {
            "Regular","Truffle"
        };
    }
}
