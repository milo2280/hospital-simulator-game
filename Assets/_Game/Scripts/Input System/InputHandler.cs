using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

public class InputHandler : MonoBehaviour
{
    public Vector2 move;
    public Vector2 look;
    public bool isJump;
    public bool isLook = true;

    private void Start()
    {
        SetActiveLook(isLook);
    }

    #region Input System
#if ENABLE_INPUT_SYSTEM
    public void OnMove(InputValue value)
    {
        MoveInput(value.Get<Vector2>());
    }

    public void OnLook(InputValue value)
    {
        LookInput(isLook ? value.Get<Vector2>() : Vector2.zero);
    }
#endif
    #endregion

    #region Outputs
    public void MoveInput(Vector2 newMoveDirection)
    {
        move = newMoveDirection;
    }

    public void LookInput(Vector2 newLookDirection)
    {
        look = newLookDirection;
    }
    #endregion

    public void SetActiveLook(bool isLook)
    {
        this.isLook = isLook;
        Cursor.lockState = isLook ? CursorLockMode.Locked : CursorLockMode.None;
    }
}
