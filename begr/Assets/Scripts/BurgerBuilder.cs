using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class BurgerBuilder : MonoBehaviour
{
    public Transform plate; // Reference to the plate where ingredients are stacked
    private float currentStackHeight = 0.0001f; // Starting height above the plate
    private List<string> assembledIngredients = new List<string>(); // Track placed ingredients
    public XRSocketInteractor socket; // The main socket for stacking

    // Ingredient height data
    private Dictionary<string, float> ingredientHeights = new Dictionary<string, float>()
    {
        {"Beef", 0.0076f },
        {"Cheese", 0.0024f },
        {"Lettuce", 0.0061f },
        {"Tomato", 0.0032f },
        {"Onion", 0.0035f },
        {"Jalapenos", 0.0029f },
        {"Pickles", 0.0024f },
        {"TopBun", -0.0163f},
        {"BotBun",0.03f }
    };

    private void Start()
    {
        socket.selectEntered.AddListener(OnIngredientPlaced);
    }

    private void OnIngredientPlaced(SelectEnterEventArgs args)
    {
        GameObject ingredient = args.interactableObject.transform.gameObject;
        string ingredientName = ingredient.name;

        // Get the height of the ingredient
        if (ingredientHeights.TryGetValue(ingredientName, out float height))
        {
            // Position the ingredient at the top of the stack
            currentStackHeight += height;
            Vector3 stackPosition = plate.position + new Vector3(0, currentStackHeight, 0);
            ingredient.transform.position = stackPosition;

            // Parent the ingredient to the plate
            ingredient.transform.SetParent(plate, true);
            ingredient.transform.rotation = Quaternion.Euler(0,0,0);
            if (ingredientName == "BotBun")
            {
                ingredient.transform.Rotate(180,0,0);
                currentStackHeight -= 0.0134f;
            }
            if (ingredientName == "TopBun")
            {
                FinishBurger();
            }
            ingredient.transform.Rotate(0, Random.Range(0,360), 0);

            // Disable interaction/grabbing after placement
            XRGrabInteractable grabInteractable = ingredient.GetComponent<XRGrabInteractable>();
            if (grabInteractable != null)
            {
                grabInteractable.enabled = false; // Disable grabbing
            }
            Rigidbody rigidbody = ingredient.GetComponent<Rigidbody>();
            if (rigidbody != null)
            {
                rigidbody.isKinematic = true;
            }
            ingredient.transform.parent = plate;
            Debug.Log($"Placed {ingredientName} at height {currentStackHeight}");
        }
        else
        {
            Debug.LogWarning($"Unknown ingredient: {ingredientName}");
        }
    }
    public void FinishBurger()
    {
        socket.GetComponent<XRSocketInteractor>().enabled = false;
    }
    public void ResetStack()
    {
        // Reset the stack for a new burger
        foreach (Transform child in plate)
        {
            Destroy(child.gameObject);
        }

        assembledIngredients.Clear();
        currentStackHeight = 0.01f; // Reset height above the plate
    }

    public List<string> GetAssembledIngredients()
    {
        return new List<string>(assembledIngredients);
    }
}