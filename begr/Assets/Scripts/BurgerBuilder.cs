using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BurgerBuilder : MonoBehaviour
{
    public Transform baseSocket; // Starting point for the burger stack
    private int currentLayer = 0; // Current layer of the stack
    public float layerHeight = 0.02f; // Height of each ingredient layer
    private List<string> assembledIngredients = new List<string>(); // Track placed ingredients

    public void PlaceIngredient(GameObject ingredient)
    {
        // Snap ingredient to the next layer position
        Vector3 newPosition = baseSocket.position + new Vector3(0, layerHeight * currentLayer, 0);
        ingredient.transform.position = newPosition;
        ingredient.transform.rotation = baseSocket.rotation;

        // Disable physics to lock the ingredient in place
        Rigidbody rb = ingredient.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
        }

        // Parent the ingredient to the burger base
        ingredient.transform.SetParent(baseSocket);

        // Track the ingredient name for order checking
        assembledIngredients.Add(ingredient.name);

        currentLayer++;
    }
}
