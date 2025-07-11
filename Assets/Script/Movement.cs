using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float speed = 5f;
    public float runMultiplier = 1.5f;
    public float jumpForce = 5f;
    public float gravity = -9.81f;
    public float mouseSensitivity = 100f;
    public static bool inputBloccato = false;

    private float xRotation = 0f;
    private float yVelocity = 0f;

    private Camera cameraChild;
    private CharacterController controller;

    public TrashItem oggettoInMano; // ← IMPORTANTE: oggetto che tiene in mano

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        cameraChild = GetComponentInChildren<Camera>();
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (inputBloccato)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            return;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? speed * runMultiplier : speed;
        Vector3 move = transform.right * x + transform.forward * z;
        move *= currentSpeed;

        if (controller.isGrounded)
        {
            yVelocity = -2f;

            if (Input.GetButtonDown("Jump"))
            {
                // 💣 Se tiene in mano rifiuto esplosivo → esplodi
                if (oggettoInMano != null && oggettoInMano.èEsplosivo)
                {
                    GameManager.Instance.Esplodi();
                    return;
                }

                yVelocity = Mathf.Sqrt(jumpForce * -2f * gravity);
            }
        }
        else
        {
            yVelocity += gravity * Time.deltaTime;
        }

        move.y = yVelocity;
        controller.Move(move * Time.deltaTime);

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        cameraChild.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }
}
