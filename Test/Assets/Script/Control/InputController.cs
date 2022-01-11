using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    public float vertical;
    public float horizontal;

    private void Awake()
    {
        //設定游標狀態
        Cursor.lockState = CursorLockMode.Locked;
        //游標顯示狀態
        Cursor.visible = false;
    }

    private void Update()
    {
        CheckCursorState();
        vertical = Input.GetAxis("Vertical");
        horizontal = Input.GetAxis("Horizontal");
    }

    void CheckCursorState()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Cursor.lockState == CursorLockMode.None)
                Cursor.lockState = CursorLockMode.Locked;
            else
                Cursor.lockState = CursorLockMode.None;
        }
    }

    //取得Mouse X的Axis
    public float GetMouseXAxis()
    {
        if (CanProcessInput())
        {
            return Input.GetAxis("Mouse X");
        }
        return 0;
    }

    //取得Mouse Y的Axis
    public float GetMouseYAxis()
    {
        if (CanProcessInput())
        {
            return Input.GetAxis("Mouse Y");
        }
        return 0;
    }

    //取得Mouse Z的Axis
    public float GetMouseScrollWheelAxis()
    {
        if (CanProcessInput())
        {
            return Input.GetAxis("Mouse ScrollWheel");
        }
        return 0;
    }

    //是否按下滑鼠左鍵
    public bool GetFireInputDown()
    {
        if (CanProcessInput())
        {
            return Input.GetMouseButtonDown(0);
        }

        return false;
    }

    //是否持續按住滑鼠左鍵
    public bool GetFireInputHeld()
    {
        if (CanProcessInput())
        {
            return Input.GetMouseButton(0);
        }

        return false;
    }

    //是否放開滑鼠左鍵
    public bool GetFireInputUp()
    {
        if (CanProcessInput())
        {
            return Input.GetMouseButtonUp(0);
        }

        return false;
    }

    //是否按下滑鼠右鍵
    public bool GetAimInputDown()
    {
        if (CanProcessInput())
        {
            return Input.GetMouseButtonDown(1);
        }

        return false;
    }

    //是否按下Reload
    public bool GetReloadInputDown()
    {
        if (CanProcessInput())
        {
            return Input.GetKeyDown(KeyCode.R);
        }

        return false;
    }

    //是否切換武器
    public int GetSwitchWeaponInput(){
        if(CanProcessInput())
        {
            if(Input.GetKeyDown(KeyCode.Q))
            {
                return -1;
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                return 1;
            }
        }

        return 0;
    }

    public bool CanProcessInput()
    {
        // 如果Cursor狀態不在鎖定中就不能處理Input
        return Cursor.lockState == CursorLockMode.Locked;
    }

    /// 取得Input移動的值(Vector3)
    public Vector3 GetMoveInput()
    {
        if (CanProcessInput())
        {
            Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));

            // 將移動輸入限制最大幅度1，否則對角線移動可能會超過定義的最大移動速度
            move = Vector3.ClampMagnitude(move, 1);
            Vector3.Magnitude(move);

            return move;
        }
        return Vector3.zero;
    }

    //是否按下Sprint
    public bool GetSprintInput()
    {
        if (CanProcessInput())
        {
            return Input.GetKey(KeyCode.LeftShift);
        }

        return false;
    }

    public bool GetJumpInputDown()
    {
        if (CanProcessInput())
        {
            return Input.GetKeyDown(KeyCode.Space);
        }

        return false;
    }
}


