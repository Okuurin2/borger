using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public GameObject objPrefab; // Prefab of the grabable cup
    private Queue<GameObject> objPool = new Queue<GameObject>();
    //tbh this is a useless script

    // Get a cup from the pool
    public GameObject GetObj()
    {
        GameObject obj = Instantiate(objPrefab);
        obj.name = objPrefab.name;
        return obj;
    }

    // Return a cup to the pool
    public void ReturnObj(GameObject cup)
    {
        cup.SetActive(false);
        objPool.Enqueue(cup);
    }
}
