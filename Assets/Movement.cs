using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float speed;
    public float mouseSensitivity;
    public static bool inputBloccato = false;


    float initY;
    float xRotation = 0f;

    Camera cameraChild;
    CharacterController controller;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        initY = transform.position.y;

        cameraChild = GetComponentInChildren<Camera>();
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (inputBloccato)
        {
            // Blocca il mouse
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            return;
        }
        else
        {
            // Ripristina controllo mouse
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * speed * Time.deltaTime);

        Vector3 currentPosition = transform.position;
        currentPosition.y = initY;
        transform.position = currentPosition;



        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        cameraChild.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }
}
