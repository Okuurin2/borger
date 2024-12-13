using System.Collections.Generic;
using UnityEngine;

public class BurgerOrderGenerator : MonoBehaviour
{
    public List<string> GenerateRandomOrder()
    {
        // Fixed parts of the order
        List<string> order = new List<string> { "Bottom Bun", "Meat" };

        // Randomized optional ingredients
        List<string> optionalIngredients = new List<string> { "Cheese", "Onion", "Tomato" };

        // Shuffle the optional ingredients
        optionalIngredients = ShuffleList(optionalIngredients);

        // Randomly decide which ingredients to include
        List<string> includedIngredients = new List<string>();
        foreach (string ingredient in optionalIngredients)
        {
            if (Random.value > 0.5f) // 50% chance to include each ingredient
            {
                includedIngredients.Add(ingredient);
            }
        }

        // Add the randomized ingredients to the order
        order.AddRange(includedIngredients);

        // Add the top bun at the end
        order.Add("Top Bun");

        return order;
    }

    private List<string> ShuffleList(List<string> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            string temp = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
        return list;
    }

    // Example usage
    private void Start()
    {
        List<string> randomOrder = GenerateRandomOrder();
        Debug.Log("Generated Order: " + string.Join(", ", randomOrder));
    }
}
