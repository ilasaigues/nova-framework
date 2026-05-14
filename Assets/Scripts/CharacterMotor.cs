using UnityEngine;

public class CharacterMotor : Initializable
{
    private bool _initialized;

    [Inject]
    private TimeContext TimeContext;
    [Inject]
    private Rigidbody _rigidbody;
    [Inject]
    private CapsuleCollider _capsuleCollider;

    public void DoFixedUpdate()
    {
        var fixedDeltaTime = TimeContext.FixedDeltaTime;
    }
}