using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// DESCRIPTION:
// When placed on gameobject, player can interact with object which is highlighted and push a key ('E') to be teleported to a specified target gameobject.

// USAGE: 
// 1. Attach the Script: Attach this script to the GameObject you want the player to interact with.
// 2. Set the Target Position: Assign an empty GameObject (or any GameObject) to the targetPosition field in the Inspector. This will be the location the player is teleported to.
// 3. Set Up the Collider: Ensure the GameObject has a Collider component (e.g., Box Collider) and that it is set as a trigger.
// 4. Player Tag: Ensure your player GameObject is tagged as "Player".
// 5. Set the Text UI Gameobject: 

public class InteractableTeleporterRaycast : MonoBehaviour
{
    [Header("Teleport Settings")]
    public Transform targetPosition; // The position to teleport the player to
    public float interactionDistance = 5f; // The maximum distance for interaction
    public KeyCode interactKey = KeyCode.E; // The key to press for interaction

    [Header("UI Settings")]
    public string interactMessage = "Press E to Interact"; // The message to display when the player is near the object
    public Color highlightColor = Color.yellow; // The color to highlight the object when interactable
    public GameObject interactionTextObject; // Reference to the TextMeshProUGUI GameObject

    private Renderer objectRenderer; // The renderer of the object
    private Color originalColor; // The original color of the object
    private bool isLookingAtObject = false; // Whether the player is looking at the object

    private Camera playerCamera; // The player's camera for raycasting

    private void Start()
    {
        // Get the renderer component and store the original color
        objectRenderer = GetComponent<Renderer>();
        if (objectRenderer != null)
        {
            originalColor = objectRenderer.material.color;
        }

        // Ensure the target position is set
        if (targetPosition == null)
        {
            Debug.LogError("Target Position is not set for the teleporter!");
        }

        // Find the player's camera
        playerCamera = Camera.main;
        if (playerCamera == null)
        {
            Debug.LogError("Main Camera not found in the scene!");
        }

        // Ensure the interaction text is initially hidden
        if (interactionTextObject != null)
        {
            interactionTextObject.SetActive(false);
        }
    }

    private void Update()
    {
        // Check if the player is looking at the object
        CheckIfLookingAtObject();

        // If the player is looking at the object and presses the interact key, teleport them
        if (isLookingAtObject && Input.GetKeyDown(interactKey))
        {
            TeleportPlayer();
        }
    }

    private void CheckIfLookingAtObject()
    {
        // Create a ray from the camera's position forward
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;

        // Check if the ray hits this object
        if (Physics.Raycast(ray, out hit, interactionDistance))
        {
            if (hit.collider.gameObject == gameObject)
            {
                if (!isLookingAtObject)
                {
                    isLookingAtObject = true;
                    HighlightObject(true);
                    ShowInteractMessage(true);
                }
            }
            else
            {
                if (isLookingAtObject)
                {
                    isLookingAtObject = false;
                    HighlightObject(false);
                    ShowInteractMessage(false);
                }
            }
        }
        else
        {
            if (isLookingAtObject)
            {
                isLookingAtObject = false;
                HighlightObject(false);
                ShowInteractMessage(false);
            }
        }
    }

    private void HighlightObject(bool highlight)
    {
        // Change the object's color to indicate it's interactable
        if (objectRenderer != null)
        {
            objectRenderer.material.color = highlight ? highlightColor : originalColor;
        }
    }

    private void ShowInteractMessage(bool show)
    {
        // Display or hide the interaction message GameObject
        if (interactionTextObject != null)
        {
            interactionTextObject.SetActive(show);

            // Debugging: Log the state of the text object
            // Debug.Log("Interaction Text Active: " + interactionTextObject.activeSelf);
            // Debug.Log("Interaction Text Position: " + interactionTextObject.transform.position);
        }
        else
        {
            Debug.LogError("Interaction Text Object is not assigned!");
        }
    }

    private void TeleportPlayer()
    {
        // Find the player GameObject
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogError("Player not found! Ensure the player GameObject is tagged as 'Player'.");
            return;
        }

        // Ensure the target position is set
        if (targetPosition == null)
        {
            Debug.LogError("Target Position is not set!");
            return;
        }

        // Get the CharacterController component
        CharacterController controller = player.GetComponent<CharacterController>();
        if (controller != null)
        {
            // Disable the CharacterController before updating the position
            controller.enabled = false;
        }

        // Teleport the player to the target position
        player.transform.position = targetPosition.position;
        Debug.Log("Player teleported to " + targetPosition.name);

        // Re-enable the CharacterController after updating the position
        if (controller != null)
        {
            controller.enabled = true;
        }
    }
}
