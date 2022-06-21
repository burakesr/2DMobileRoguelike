using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
[DisallowMultipleComponent]
public class PlayerInputController : MonoBehaviour
{
    [Header("PLAYER INPUTS")]
    public Vector2 move;
    public Vector2 look;
    public float mouseScrollY;
    public bool isShooting;
    public bool isRolling;
    public bool isReloading;

    private Controls controls;

    private void OnEnable()
    {
        controls = new Controls();
        controls.Enable();

        controls.PlayerInput.Move.performed += ctx => OnMove(ctx);
        controls.PlayerInput.Move.canceled += ctx => OnMove(ctx);

        controls.PlayerInput.Look.performed += ctx => OnLook(ctx);
        controls.PlayerInput.Look.canceled += ctx => OnLook(ctx);

        controls.PlayerInput.Shoot.performed += ctx => OnShoot(ctx);
        controls.PlayerInput.Shoot.canceled += ctx => OnShoot(ctx);

        controls.PlayerInput.Reload.started += ctx => OnReload(ctx);
        controls.PlayerInput.Reload.canceled += ctx => OnReload(ctx);

        controls.PlayerInput.MouseScroll.performed += ctx => OnMouseScroll(ctx);
        controls.PlayerInput.MouseScroll.canceled+= ctx => OnMouseScroll(ctx);

        controls.PlayerInput.Roll.started += ctx => OnRoll(ctx);
        controls.PlayerInput.Roll.canceled += ctx => OnRoll(ctx);
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void OnShoot(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            isShooting = true;
        }
        else if (ctx.canceled)
        {
            isShooting = false;
        }
    }

    private void OnReload(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            isReloading = true;
        }
        else if (ctx.canceled)
        {
            isReloading = false;
        }
    }

    private void OnMove(InputAction.CallbackContext ctx)
    {
        move = ctx.ReadValue<Vector2>();
    }

    private void OnLook(InputAction.CallbackContext ctx)
    {
        look = ctx.ReadValue<Vector2>();
    }

    private void OnRoll(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            isRolling = true;
        }
        else if (ctx.canceled)
        {
            isRolling = false;
        }
    }

    private void OnMouseScroll(InputAction.CallbackContext ctx)
    {
        mouseScrollY = ctx.ReadValue<float>();
    }

    private void Update()
    {
        isRolling = controls.PlayerInput.Roll.WasPerformedThisFrame();
    }
}