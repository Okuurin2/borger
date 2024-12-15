using UnityEngine;
using System.Collections;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class FriesInteraction : MonoBehaviour
{
    public GameObject friesPrefab;  // Reference to the fries GameObject prefab
    public Transform fryerTransform;  // Reference to the fryer (assigned in the inspector)
    public Transform newFryerPosition;  // Reference to a new position for fries after fryer is destroyed
    public Transform newFryerPosition1;  // Another reference to a new position for fries after fryer is destroyed

    private GameObject friesInstance;  // Store fries instance globally
    private XRGrabInteractable grabInteractable;  // Reference to the XR Grab Interactable component
    private Collider fryerCollider;    // The fryer's main collider for interaction
    private Collider triggerCollider;  // The trigger collider to detect fries

    private Coroutine fryCoroutine;  // Variable to hold the reference to the frying coroutine

    private void Start()
    {
        // Ensure the fryer has the XRGrabInteractable component to be interactable in VR
        grabInteractable = fryerTransform.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
        if (grabInteractable == null)
        {
            Debug.LogError("Fryer does not have an XRGrabInteractable component attached.");
            grabInteractable = fryerTransform.gameObject.AddComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
        }

        // Register the event to detect when the fryer is grabbed
        grabInteractable.selectEntered.AddListener(OnFryerGrabbed);

        // Grab the colliders for fryer's interaction and trigger zones
        fryerCollider = fryerTransform.GetComponent<Collider>();
        triggerCollider = fryerTransform.Find("TriggerCollider")?.GetComponent<Collider>();

        // Set trigger collider to detect fries entering
        if (triggerCollider != null)
        {
            triggerCollider.isTrigger = true;  // This will be used to detect fries
            Debug.Log("TriggerFryerCollider found: " + triggerCollider.gameObject.name);
        }
    }

    private void OnFryerGrabbed(SelectEnterEventArgs args)
    {
        // When the fryer is grabbed, stop the frying process
        if (fryCoroutine != null)
        {
            StopCoroutine(fryCoroutine);
            Debug.Log("Frying process stopped because the fryer was grabbed.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Debug the collider that triggers the event
        Debug.Log("Trigger detected: " + other.name);

        // Check if the fries are colliding with the trigger collider
        if (other.CompareTag("Fries"))
        {
            Debug.Log("Fries entered the fryer");

            // Destroy the fries GameObject when they enter the fryer
            Destroy(other.gameObject);

            // Spawn the fries at the specified position
            SpawnFriesAtPosition1();

            // Optionally, place the fries inside the fryer
            PlaceFriesInFryer();

            // Set the fryer's collider to be solid after fries enter
            OnFryer();
        }

        // New functionality: Check if the fryer collides with a specific object (e.g., tagged "DestructionZone")
        if (other.CompareTag("DestructionZone"))
        {
            Debug.Log("Fryer entered the destruction zone");

            // Destroy the fryer GameObject
            Destroy(gameObject);

            // Spawn fries at a new position after the fryer is destroyed
            SpawnFriesAtPosition();
        }
    }

    private void SpawnFriesAtPosition()
    {
        if (friesPrefab == null)
        {
            Debug.LogError("Fries prefab is not assigned.");
            return;
        }

        // Use a custom spawn position (if assigned), otherwise use the fryer's current position
        Vector3 spawnPosition = newFryerPosition != null ? newFryerPosition.position : fryerTransform.position;

        // Instantiate the fries at the new position
        friesInstance = Instantiate(friesPrefab, spawnPosition, Quaternion.identity);
        Debug.Log("Fries spawned at: " + spawnPosition);

        // Apply Rigidbody to fries (if not already applied)
        Rigidbody friesRigidbody = friesInstance.GetComponent<Rigidbody>();
        if (friesRigidbody == null)
        {
            friesRigidbody = friesInstance.AddComponent<Rigidbody>();
        }

        // Set the Rigidbody to be kinematic and disable gravity to prevent falling out of fryer
        friesRigidbody.isKinematic = true;
        friesRigidbody.useGravity = false;

        // Apply a physics material to prevent bouncing
        Collider friesCollider = friesInstance.GetComponent<Collider>();
        if (friesCollider != null)
        {
            PhysicMaterial noBounceMaterial = new PhysicMaterial
            {
                bounciness = 0f,
                bounceCombine = PhysicMaterialCombine.Minimum,
                frictionCombine = PhysicMaterialCombine.Average
            };

            friesCollider.material = noBounceMaterial;
        }

        // Set fries color to the latest frying color when spawned in the destruction zone
        SetFriesColor(Color.white);

        Debug.Log("Fries position after instantiation: " + friesInstance.transform.position);

        // Start the frying coroutine to change fries color over time
        if (fryCoroutine == null)  // Check if the coroutine is already running
        {
            fryCoroutine = StartCoroutine(FryFries());
            Debug.Log("Fries are starting to fry");
        }
        else
        {
            Debug.Log("Frying coroutine already running");
        }
    }

    private void SpawnFriesAtPosition1()
    {
        if (friesPrefab == null)
        {
            Debug.LogError("Fries prefab is not assigned.");
            return;
        }

        // Use a custom spawn position (if assigned), otherwise use the fryer's current position
        Vector3 spawnPosition = newFryerPosition1 != null ? newFryerPosition1.position : fryerTransform.position;

        // Instantiate the fries at the new position
        friesInstance = Instantiate(friesPrefab, spawnPosition, Quaternion.identity);
        Debug.Log("Fries spawned at: " + spawnPosition);

        // Apply Rigidbody to fries (if not already applied)
        Rigidbody friesRigidbody = friesInstance.GetComponent<Rigidbody>();
        if (friesRigidbody == null)
        {
            friesRigidbody = friesInstance.AddComponent<Rigidbody>();
        }

        // Set the Rigidbody to be kinematic and disable gravity to prevent falling out of fryer
        friesRigidbody.isKinematic = true;
        friesRigidbody.useGravity = false;

        // Apply a physics material to prevent bouncing
        Collider friesCollider = friesInstance.GetComponent<Collider>();
        if (friesCollider != null)
        {
            PhysicMaterial noBounceMaterial = new PhysicMaterial
            {
                bounciness = 0f,
                bounceCombine = PhysicMaterialCombine.Minimum,
                frictionCombine = PhysicMaterialCombine.Average
            };

            friesCollider.material = noBounceMaterial;
        }

        Debug.Log("Fries position after instantiation: " + friesInstance.transform.position);

        // Start the frying coroutine to change fries color over time
        if (fryCoroutine == null)  // Check if the coroutine is already running
        {
            fryCoroutine = StartCoroutine(FryFries());
            Debug.Log("Fries are starting to fry");
        }
        else
        {
            Debug.Log("Frying coroutine already running");
        }
    }

    private void PlaceFriesInFryer()
    {
        if (friesInstance != null && fryerTransform != null)
        {
            // Position fries inside the fryer
            friesInstance.transform.position = fryerTransform.position;

            // Set fries as a child of the fryer
            friesInstance.transform.SetParent(fryerTransform);

            // Disable the fries' Rigidbody physics
            Rigidbody friesRigidbody = friesInstance.GetComponent<Rigidbody>();
            if (friesRigidbody != null)
            {
                friesRigidbody.useGravity = false;  // Prevent fries from falling
                friesRigidbody.isKinematic = true;  // Stop physics interference while fries are inside the fryer
            }

            // Disable fries' collider to prevent interaction with fryer walls
            Collider friesCollider = friesInstance.GetComponent<Collider>();
            if (friesCollider != null)
            {
                friesCollider.enabled = false;  // Disable collider to prevent bouncing and interaction
            }

            Debug.Log("Fries placed inside the fryer");
        }
        else
        {
            Debug.LogError("Fries instance or fryerTransform is not set. Make sure fries are spawned before placing in fryer.");
        }
    }

    private void OnFryer()
    {
        // Get the collider of the fryer object to disable trigger after fries enter
        if (fryerCollider != null)
        {
            fryerCollider.isTrigger = false;  // Make sure it's not a trigger after fries enter
            Debug.Log("Fryer collider set to solid for interaction.");
        }
    }

    private IEnumerator FryFries()
    {
        float fryTime = 0f;

        // Loop through the frying process
        while (fryTime < 10f)
        {
            Debug.Log("Frying fries: " + fryTime + " seconds");

            if (fryTime < 5f)
            {
                // Set fries color to white in the first 5 seconds
                SetFriesColor(Color.white);
                Debug.Log("Fries are raw.");
            }
            else if (fryTime < 9f)
            {
                // Set fries color to yellow in the next 4 seconds
                SetFriesColor(Color.yellow);  // Change to your original color
                Debug.Log("Fries are done.");
            }

            // Debugging the frying time and current color after each update
            Debug.Log("Frying time: " + fryTime + " seconds. Fries color: " + friesInstance.GetComponent<Renderer>().material.color);

            // Wait for 1 second before updating the color again
            fryTime += 1f;
            yield return new WaitForSeconds(1f);
        }

        // After 10 seconds, set fries color to black
        SetFriesColor(Color.black);
        Debug.Log("Fries are burnt.");

        // Final debug message showing frying time and fries color
        Debug.Log("Frying complete: " + fryTime + " seconds. Final fries color: " + friesInstance.GetComponent<Renderer>().material.color);
    }

    private void SetFriesColor(Color color)
    {
        if (friesInstance != null)
        {
            Renderer friesRenderer = friesInstance.GetComponent<Renderer>();
            if (friesRenderer != null)
            {
                friesRenderer.material.color = color;
                Debug.Log("Fries color set to: " + color);
            }
        }
    }
}


































