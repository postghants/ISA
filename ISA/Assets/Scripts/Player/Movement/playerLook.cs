using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class playerLook : MonoBehaviour
{
    [SerializeField, Range(0.01f, 2f)]public float mouseSensitivity;
    public Transform playerCamera;
    public movementLimiter moveLimit;
    private float xRotation;
    public InputMap inputMap;
    public InputAction Look;
    void Start()
    {
        inputMap = new InputMap();
        Look = inputMap.Map.Look;
        inputMap.Enable();
        Look.Enable();
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Look.ReadValue<Vector2>().x * mouseSensitivity;
        float mouseY = Look.ReadValue<Vector2>().y * mouseSensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);

    }
}
