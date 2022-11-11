using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttackController : AttackController
{
    private PlayerInputs _playerInputs;
    private InputAction _fireInput;

    protected override bool Fire()
    {
        return _fireInput.IsPressed();
    }

    protected override void OnAwake()
    {
        _playerInputs = new PlayerInputs();
        _fireInput = _playerInputs.Player.Fire;
    }

    private void OnEnable()
    {
        _fireInput.Enable();
    }

    private void OnDisable()
    {
        _fireInput.Disable();
    }
}
