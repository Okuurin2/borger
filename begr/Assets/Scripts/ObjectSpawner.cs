using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine;
using System.Collections;

public class ObjectSpawner : MonoBehaviour
{
    public ObjectPool objectPool;               // Reference to the cup pool
    public Transform spawnPoint;         // Where the grabable cup will appear

    private bool cooldown;

    private void OnTriggerEnter(Collider other)
    {
        // Detect player hand or controller interaction
        if (other.CompareTag("GameController"))
        {
            DispenseObj();
            cooldown = true;
            StartCoroutine(Timer());
        }
    }

    public void Spawn()
    {
        if (!cooldown)
        {
            cooldown = true;
            DispenseObj();
            StartCoroutine(Timer());
        }
    }

    private void DispenseObj()
    {
        // Get a cup from the pool
        GameObject obj = objectPool.GetObj();

        // Position it at the top of the stack
        obj.transform.SetPositionAndRotation(spawnPoint.position, spawnPoint.rotation);

        // Enable interaction for the spawned cup
        XRGrabInteractable grabInteractable = obj.GetComponent<XRGrabInteractable>();
        if (grabInteractable != null)
        {
            grabInteractable.enabled = true;
        }
    }

    private IEnumerator Timer()
    {
        yield return new WaitForSeconds(1f);
        cooldown = false;
    }
}