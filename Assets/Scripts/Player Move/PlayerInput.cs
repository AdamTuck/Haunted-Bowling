using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-100)]
public class PlayerInput : MonoBehaviour
{
    public float horizontal { get; private set; }
    public float vertical { get; private set; }
    public float mouseX { get; private set; }
    public float mouseY { get; private set; }
    public bool interact { get; private set; }
    public bool throwBall { get; private set; }
    public bool switchMovement { get; private set; }
    public bool leftBtn { get; private set; }
    public bool rightBtn { get; private set; }
    public bool escape { get; private set; }

    private bool clear;

    // Singleton
    private static PlayerInput instance;

    private void Awake()
    {
        if (instance && instance != this)
        {
            Destroy(instance);
            return;
        }

        instance = this;
    }

    public static PlayerInput GetInstance()
    {
        return instance;
    }

    // Update is called once per frame
    void Update()
    {
        ClearInputs();
        SetInputs();
    }

    // FixedUpdate runs independant of framerate
    private void FixedUpdate()
    {
        clear = true;
    }

    void SetInputs()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        mouseX = Input.GetAxis("Mouse X");
        mouseY = Input.GetAxis("Mouse Y");

        interact = interact || Input.GetKeyDown(KeyCode.E);
        throwBall = throwBall || Input.GetKeyDown(KeyCode.W);

        switchMovement = switchMovement || Input.GetKeyDown(KeyCode.Space);

        leftBtn = leftBtn || Input.GetButtonDown("Fire1");
        rightBtn = rightBtn || Input.GetButtonDown("Fire2");

        escape = escape || Input.GetKeyDown(KeyCode.Escape);
    }

    void ClearInputs()
    {
        if (!clear)
            return;

        horizontal = 0;
        vertical = 0;

        mouseX = 0;
        mouseY = 0;

        interact = false;
        throwBall = false;

        switchMovement = false;

        leftBtn = false;
        rightBtn = false;

        escape = false;
    }
}