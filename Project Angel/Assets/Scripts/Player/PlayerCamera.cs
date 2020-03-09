using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public float xRotMin, xRotMax;
    public Vector2 mouseSensitivity;
    private float verticalLookRotation;
    public Transform playerTransform;
    public Transform cameraPivot;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void FixedUpdate()
    {

        playerTransform.Rotate(Vector3.up * Input.GetAxis("Mouse X") * mouseSensitivity.x);
        verticalLookRotation += Input.GetAxis("Mouse Y") * mouseSensitivity.y;
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, xRotMin, xRotMax);
        cameraPivot.localEulerAngles = Vector3.left * verticalLookRotation;
    }
}
