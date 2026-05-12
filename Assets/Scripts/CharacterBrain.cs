using UnityEngine;

[RequireComponent(typeof(Animator))]
public class CharacterBrain : MonoBehaviour
{
    public CharacterConfig Config;

    public CharacterStateMachine StateMachine { get; private set; }
    public CharacterMotor Motor { get; private set; }
    public ICharacterInput Input { get; private set; }
    public Animator Animator { get; private set; }

    void Awake()
    {
        // Gather references and instantiate POCOs
        Animator = GetComponent<Animator>();
        StateMachine = new();
        Motor = new();
        Input = new ConcreteCharacterInput();
    }

    void InitializeModules()
    {
        // Inject all dependencies
    }

}
