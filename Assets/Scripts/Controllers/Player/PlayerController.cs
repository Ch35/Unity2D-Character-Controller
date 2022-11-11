using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : CharacterController
{
    private PlayerInputs _playerInputs;
    private InputAction _moveInput;

    protected override float MoveY()
    {
        return _moveInput.ReadValue<Vector2>().y;
    }

    protected override float MoveX()
    {
        return _moveInput.ReadValue<Vector2>().x;
    }

    protected override void OnAwake()
    {
        _playerInputs = new PlayerInputs();
        _moveInput = _playerInputs.Player.Move;
    }

    private void OnEnable()
    {
        _moveInput.Enable();
    }

    private void OnDisable()
    {
        _moveInput.Disable();
    }
}
