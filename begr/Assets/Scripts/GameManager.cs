using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        
    }

    public void UpdateRating(float newRating)
    {
        rating = (rating * (meals - 1) + newRating) / meals;
    }
}
