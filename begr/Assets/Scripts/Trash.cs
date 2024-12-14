using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Trash : MonoBehaviour
{
    GameManager gameManager;

    private void Start()
    {
        gameManager = FindAnyObjectByType<GameManager>();
    }
    public void Delete(SelectEnterEventArgs args)
    {
        Destroy(args.interactableObject.transform.gameObject);
        gameManager.trash += 1;
    }
}
