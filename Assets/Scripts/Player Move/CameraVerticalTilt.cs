using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraVerticalTilt : MonoBehaviour
{
    PlayerInput playerInput;
    private float cameraXRotation;

    [Header("Player Turn")]
    [SerializeField] private float turnSpeed;
    [SerializeField] private bool invertMouse;

    private void Start()
    {
        playerInput = PlayerInput.GetInstance();
    }

    void Update()
    {
        CameraRotation();
    }

    void CameraRotation()
    {
        cameraXRotation += playerInput.mouseY * Time.deltaTime * turnSpeed * (invertMouse ? 1 : -1);
        cameraXRotation = Mathf.Clamp(cameraXRotation, -40, 40);

        transform.localRotation = Quaternion.Euler(cameraXRotation, 0, 0);
    }
}
