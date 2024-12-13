using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class OrderGenerator : MonoBehaviour
{
    private Dictionary<string,string[]> burgers;
    private string[] drinks;
    private string[] fries;
    public int orderNum = 0;

    public void GenerateOrder()
    {
        orderNum++;
    }
    private void Start()
    {
        burgers = new Dictionary<string,string[]>
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
