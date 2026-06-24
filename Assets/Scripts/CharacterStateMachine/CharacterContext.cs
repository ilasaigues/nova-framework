using UnityEngine;

namespace HSM
{
    public class CharacterContext : Initializable
    {
        [Inject] public TimeContext TimeContext { get; set; }
        [Inject] public CharacterConfig CharacterConfig { get; set; }

        [Inject] public Animator Animator { get; set; }
        [Inject] public Rigidbody Rigidbody { get; set; }

        [Inject] public CharacterStateMachine StateMachine { get; set; }
        [Inject] public ICharacterInput Input { get; set; }

        [Inject] public CharacterConfig Config { get; set; }

        public bool Grounded;
        public Vector3 Velocity;
    }
}