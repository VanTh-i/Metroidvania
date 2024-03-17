using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private static InputManager instance;
    public static InputManager Instance { get => instance; private set => instance = value; }

    private float moveInput;
    public float MoveInput { get => moveInput; private set => moveInput = value; }

    private bool jumpInputDown;
    public bool JumpInputDown { get => jumpInputDown; private set => jumpInputDown = value; }

    private bool jumpInputUp;
    public bool JumpInputUp { get => jumpInputUp; private set => jumpInputUp = value; }

    private bool dashInput;
    public bool DashInput { get => dashInput; private set => dashInput = value; }


    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("has more 2 input manager");
        }
        instance = this;
    }

    private void Update()
    {
        GetKeyInput();
        GetSpaceInputDown();
        GetSpaceInputUp();
        GetDashInput();
    }

    public float GetKeyInput()
    {
        return moveInput = Input.GetAxisRaw("Horizontal");
    }

    public bool GetSpaceInputDown()
    {
        return jumpInputDown = Input.GetButtonDown("Jump");
    }

    public bool GetSpaceInputUp()
    {
        return jumpInputUp = Input.GetButtonUp("Jump");
    }

    public bool GetDashInput()
    {
        return dashInput = Input.GetKeyDown(KeyCode.LeftControl);
    }
}
