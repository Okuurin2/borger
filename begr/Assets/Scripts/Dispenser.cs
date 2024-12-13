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
<<<<<<< Updated upstream
            if (colours.TryGetValue(type, out Color color) && (drink.typeOfDrink == "" || drink.typeOfDrink == type) && fillCoroutine == null)
            {
                drink.typeOfDrink = type;
                Transform circle = cup.transform.Find("Circle");
                circle.GetComponent<Renderer>().material.color = color;
                fillCoroutine = StartCoroutine(FillCup());
=======
            if (colours.TryGetValue(type, out Color color) && drink.typeOfDrink == "" && fillCoroutine == null)
            {
                drink.typeOfDrink = type;
                cup.GetComponent<Renderer>().material.color = color;
                fillCoroutine = StartCoroutine(FillCup(args.interactableObject.transform.gameObject.name));
>>>>>>> Stashed changes
            }
            else if (type == "Ice")
            {
                drink.Ice = true;
<<<<<<< Updated upstream
                Transform ice = drink.transform.Find("Ice");
                ice.gameObject.SetActive(true);
=======
>>>>>>> Stashed changes
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

<<<<<<< Updated upstream
    private IEnumerator FillCup()
=======
    private IEnumerator FillCup(string drinkType)
>>>>>>> Stashed changes
    {
        Drink drink = cup.GetComponent<Drink>();

        float fillLevel = drink.fillLevel;
<<<<<<< Updated upstream
        Transform circle = cup.transform.Find("Circle");

        while (fillLevel < 1.0f) 
        {
            fillLevel += fillRate * Time.deltaTime;     
            drink.fillLevel = Mathf.Clamp01(fillLevel);
            float size = 0.02215714f - 0.01863194f;
            size= size* fillLevel + 0.01863194f;
            float height = 0.00827f + 0.00973f;
            height = height * fillLevel - 0.00973f;
            circle.localScale = new Vector3(size, 0.00001f, size);
            circle.localPosition = new Vector3(0, height, 0);
=======


        while (fillLevel < 1.0f) 
        {
            fillLevel += fillRate * Time.deltaTime; 
            drink.fillLevel = Mathf.Clamp01(fillLevel);
>>>>>>> Stashed changes
            yield return null;
        }
    }
}
