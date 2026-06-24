using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class ConcreteCharacterInput : MonoBehaviour, ICharacterInput
{
    public ICharacterInput.InputWrapper<bool> JumpWrapper { get; private set; } = new();

    public ICharacterInput.InputWrapper<bool> DashWrapper { get; private set; } = new();

    public ICharacterInput.InputWrapper<Vector2> MovementWrapper { get; private set; } = new();

    public InputActionReference MoveInputAction;
    public InputActionReference DashInputAction;
    public InputActionReference JumpInputAction;

    public void Update()
    {
        JumpWrapper.Value = JumpInputAction.action.ReadValue<float>() > 0;
        DashWrapper.Value = DashInputAction.action.ReadValue<bool>();
        MovementWrapper.Value = MoveInputAction.action.ReadValue<Vector2>();
    }

}
