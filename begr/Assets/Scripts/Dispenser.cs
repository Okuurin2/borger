using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class Dispenser : MonoBehaviour
{
    //public XRSimpleInteractable[] buttons;

    private Dictionary<string, Color> colours;
    public GameObject cup;
    private float fillRate = 0.3333f;
    private Coroutine fillCoroutine;
    void Start()
    {
        colours = new Dictionary<string, Color>()
        {
            { "Coke", new Color(1,0,0)},
            { "Sprite", new Color(0,1,0)},
            { "Milo", new Color(0,0,0)},
            { "Lemon Tea", new Color(1,1,0)}
        };
    }

    public void CupEntered(SelectEnterEventArgs args)
    {
        cup = args.interactableObject.transform.gameObject;
        Debug.Log($"Selected Object: {cup.name}");
    }

    public void CupExited(SelectExitEventArgs args)
    {
        cup = null;
    }
    public void Pressed(SelectEnterEventArgs args)
    {
        if (cup != null)
        {
            Drink drink = cup.GetComponent<Drink>();
            string type = args.interactableObject.transform.gameObject.name;
            if (colours.TryGetValue(type, out Color color) && drink.typeOfDrink == "" && fillCoroutine == null)
            {
                drink.typeOfDrink = type;
                cup.GetComponent<Renderer>().material.color = color;
                fillCoroutine = StartCoroutine(FillCup(args.interactableObject.transform.gameObject.name));
            }
            else if (type == "Ice")
            {
                drink.Ice = true;
            }
            Debug.Log("Pressed");
        }
    }

    public void Release(SelectExitEventArgs args)
    {

        if (fillCoroutine != null)
        {
            StopCoroutine(fillCoroutine);
            fillCoroutine = null;
        }
    }

    private IEnumerator FillCup(string drinkType)
    {
        Drink drink = cup.GetComponent<Drink>();

        float fillLevel = drink.fillLevel;


        while (fillLevel < 1.0f) 
        {
            fillLevel += fillRate * Time.deltaTime; 
            drink.fillLevel = Mathf.Clamp01(fillLevel);
            yield return null;
        }
    }
}
