using UnityEngine;

public class ConcreteCharacterInput : ICharacterInput
{
    public ICharacterInput.InputWrapper<bool> JumpWrapper { get; private set; }

    public ICharacterInput.InputWrapper<bool> DashWrapper { get; private set; }

    public ICharacterInput.InputWrapper<Vector2> MovementWrapper { get; private set; }

    public ConcreteCharacterInput()
    {
        JumpWrapper = new();
        DashWrapper = new();
        MovementWrapper = new();
    }
}
