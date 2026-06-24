using System.Linq;
using System.Threading.Tasks;
using AstralCore;
using UnityEngine;

namespace HSM
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Animator))]
    public class PlayerStateDriver : MonoBehaviour
    {

        public CharacterContext ctx = new();
        public Transform groundCheck;
        public float groundRadius;
        public LayerMask groundMask;

        public bool DrawGizmos;


        public TimeContext timeContext;
        public CharacterConfig config;
        public ICharacterInput characterInput;


        Rigidbody rb;
        CharacterStateMachine machine;
        State root;

        // debugging
        string lastPath;

        void Awake()
        {
            rb = gameObject.GetOrAddComponent<Rigidbody>();
            rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
            var animator = GetComponent<Animator>();

            characterInput = GetComponent<ConcreteCharacterInput>();

            root = new PlayerRoot(null, ctx);
            machine = new CharacterStateMachine(root);
            root.Machine = machine;
            ctx.Rigidbody = rb;
            ctx.Initialize(timeContext, config, animator, rb, machine, characterInput);


        }


        void Update()
        {
            ctx.Grounded = Physics.CheckSphere(groundCheck.position, groundRadius, groundMask);
            machine.Update(ctx.TimeContext.DeltaTime);

            // debugging
            var path = StatePath(machine.Root.Leaf());
            if (path != lastPath)
            {
                AstralCore.Logger.Log(LogCategory.StateMachine, path);
                lastPath = path;
            }
        }

        void FixedUpdate()
        {
            var v = rb.linearVelocity;
            v.x = ctx.Velocity.x;
            rb.linearVelocity = v;
        }

        void OnDrawGizmosSelected()
        {
            if (!DrawGizmos) return;
            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(groundCheck.position, groundRadius);
        }

        static string StatePath(State s)
        {
            return string.Join(" > ", s.PathToRoot().Reverse().Select(state => state.GetType().Name));
        }

    }
}