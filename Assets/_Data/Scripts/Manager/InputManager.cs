using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private static InputManager instance;
    public static InputManager Instance { get => instance; private set => instance = value; }

    private float moveInputHorizontal;
    public float MoveInputHorizontal { get => moveInputHorizontal; private set => moveInputHorizontal = value; }

    private float moveInputVertical;
    public float MoveInputVertical { get => moveInputVertical; private set => moveInputVertical = value; }

    private bool jumpInputDown;
    public bool JumpInputDown { get => jumpInputDown; private set => jumpInputDown = value; }

    private bool jumpInputUp;
    public bool JumpInputUp { get => jumpInputUp; private set => jumpInputUp = value; }

    private bool dashInput;
    public bool DashInput { get => dashInput; private set => dashInput = value; }

    private bool attackInput;
    public bool AttackInput { get => attackInput; private set => attackInput = value; }

    private bool healInput;
    public bool HealInput { get => healInput; set => healInput = value; }


    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        GetKeyInputHorizontal();
        GetKeyInputVertical();
        GetSpaceInputDown();
        GetSpaceInputUp();
        GetDashInput();
        GetAttackInput();
        GetHealingInput();
    }

    public float GetKeyInputHorizontal()
    {
        return moveInputHorizontal = Input.GetAxisRaw("Horizontal");
    }
    public float GetKeyInputVertical()
    {
        return moveInputVertical = Input.GetAxisRaw("Vertical");
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

    public bool GetAttackInput()
    {
        return attackInput = Input.GetMouseButtonDown(0);
    }

    public bool GetHealingInput()
    {
        return healInput = Input.GetMouseButton(1);
    }

}
