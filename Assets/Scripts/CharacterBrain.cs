using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class CharacterBrain : MonoBehaviour
{
    // Scriptable Objects
    public TimeContext TimeContext;
    public CharacterConfig Config;

    // Components
    public Animator Animator { get; private set; }
    public Rigidbody Rigidbody { get; private set; }
    public CapsuleCollider CapsuleCollider { get; private set; }

    // Modules
    public CharacterStateMachine StateMachine { get; private set; }
    public ICharacterInput Input { get; private set; }
    public CharacterMotor Motor { get; private set; }


    void Awake()
    {
        // Gather components
        Animator = GetComponent<Animator>();
        Rigidbody = GetComponent<Rigidbody>();
        CapsuleCollider = GetComponent<CapsuleCollider>();
        Input = new ConcreteCharacterInput();
        // Instantiate POCOs
        StateMachine = new();
        Motor = new();

        // Initialize all modules and components AFTER getting and creating all variables
        InitializeModules();
    }

    void InitializeModules()
    {
        // Inject all dependencies. This should error out if any dependencies are missing
        StateMachine.Initialize(TimeContext, Config, Motor);
        Debug.Log(StateMachine.Initialized);
        Motor.Initialize(TimeContext, Rigidbody, CapsuleCollider);
        Debug.Log(Motor.Initialized);
    }

}
