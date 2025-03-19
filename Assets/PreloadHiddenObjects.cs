using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// USAGE:
// 1. Add preload script to empty game object in the scene hierarchy.
// 2. Assign loading screen to the field in the component Inspector.

public class PreloadHiddenObjects : MonoBehaviour
{
    [Header("Settings")]
    public float preloadDuration = 0.5f; // Duration to keep hidden objects visible (in seconds)

    [Header("Loading Screen")]
    public GameObject loadingScreen; // Reference to the loading screen UI Panel

    private List<GameObject> hiddenObjects = new List<GameObject>(); // List to store hidden GameObjects
    private PlayerController playerController; // Reference to the PlayerController script
    private Footsteps footsteps; // Reference to the Footsteps script

    private void Start()
    {
        // Find the PlayerController script
        playerController = FindObjectOfType<PlayerController>();
        if (playerController == null)
        {
            Debug.LogError("PlayerController not found in the scene!");
            return;
        }

        // Find the Footsteps script
        footsteps = FindObjectOfType<Footsteps>();
        if (footsteps == null)
        {
            Debug.LogError("Footsteps script not found in the scene!");
            return;
        }

        // Disable player movement and camera rotation
        playerController.isMovementEnabled = false;
        playerController.isCameraRotationEnabled = false;

        // Disable footsteps
        footsteps.enabled = false;

        // Enable the loading screen
        if (loadingScreen != null)
        {
            loadingScreen.SetActive(true);
            Debug.Log("Loading screen enabled.");
        }

        // Find all hidden GameObjects in the scene
        FindHiddenObjects();

        // Unhide all hidden GameObjects
        UnhideObjects();

        // Start coroutine to re-hide the objects and re-enable player movement after a brief moment
        StartCoroutine(RehideObjectsAfterDelay());
    }

    private void FindHiddenObjects()
    {
        // Find all GameObjects in the scene, including inactive ones
        GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();

        // Check each GameObject to see if it is hidden
        foreach (GameObject obj in allObjects)
        {
            // Ensure the GameObject is in the scene (not a prefab) and is initially inactive
            if (!obj.activeInHierarchy && obj.scene.IsValid())
            {
                hiddenObjects.Add(obj); // Add hidden GameObject to the list
                Debug.Log($"Found hidden GameObject: {obj.name}");
            }
        }

        // Debug: Log the number of hidden objects found
        Debug.Log($"Found {hiddenObjects.Count} hidden GameObjects.");
    }

    private void UnhideObjects()
    {
        // Unhide all GameObjects in the hiddenObjects list
        foreach (GameObject obj in hiddenObjects)
        {
            obj.SetActive(true);
            Debug.Log($"Unhidden GameObject: {obj.name}");
        }

        // Debug: Log that objects have been unhidden
        Debug.Log("Unhidden all hidden GameObjects.");
    }

    private IEnumerator RehideObjectsAfterDelay()
    {
        // Wait for the specified duration
        yield return new WaitForSeconds(preloadDuration);

        // Re-hide all GameObjects in the hiddenObjects list
        foreach (GameObject obj in hiddenObjects)
        {
            obj.SetActive(false);
            Debug.Log($"Re-hidden GameObject: {obj.name}");
        }

        // Debug: Log that objects have been re-hidden
        Debug.Log("Re-hidden all hidden GameObjects.");

        // Disable the loading screen
        if (loadingScreen != null)
        {
            loadingScreen.SetActive(false);
            Debug.Log("Loading screen disabled.");
        }

        // Re-enable player movement, camera rotation, and footsteps
        if (playerController != null)
        {
            playerController.isMovementEnabled = true;
            playerController.isCameraRotationEnabled = true;
            Debug.Log("Player movement and camera rotation re-enabled.");
        }

        if (footsteps != null)
        {
            footsteps.enabled = true;
            Debug.Log("Footsteps re-enabled.");
        }
    }
}