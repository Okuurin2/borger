using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private Dictionary<string, string[]> burgers;
    private OrderGenerator orderGenerator;
    private List<string[]> ordersList;

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
    }

    private void Order()
    {
        string[] order = orderGenerator.GenerateOrder();
        ordersList.Add(order);
        
    }
}
