using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [Tooltip("Adjust player movement speed")]
    public float movementSpeed = 5f;
    [Tooltip("Adjust mouse sensitivity")]
    public float mouseSensitivity = 100f;

    [Header("Camera Reference")]
    [SerializeField] private Transform playerCamera;

    private CharacterController controller;
    private float xRotation = 0f;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        
        // Lock and hide cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Auto-find camera if not assigned
        if (playerCamera == null && GetComponentInChildren<Camera>() != null)
        {
            playerCamera = GetComponentInChildren<Camera>().transform;
        }
    }

    void Update()
    {
        HandleMouseLook();
        HandleMovement();
    }

    void HandleMouseLook()
    {
        // Get mouse input
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Vertical camera rotation
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        
        // Apply rotations
        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    void HandleMovement()
    {
        // Get keyboard input
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // Create movement vector relative to player's orientation
        Vector3 move = transform.right * x + transform.forward * z;
        move = Vector3.ClampMagnitude(move, 1f) * movementSpeed;

        // Apply movement
        controller.SimpleMove(move);
    }
}