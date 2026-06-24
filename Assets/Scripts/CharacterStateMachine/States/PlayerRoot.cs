using UnityEngine;

namespace HSM
{

    public class Move : State
    {
        readonly CharacterContext ctx;

        public Move(CharacterStateMachine m, State parent, CharacterContext ctx) : base(m, parent)
        {
            this.ctx = ctx;
        }

        protected override State GetTransition()
        {
            if (!ctx.Grounded) return ((PlayerRoot)ParentState).Airborne;

            return Mathf.Abs(ctx.Input.MovementWrapper.Value.x) <= 0.01f ? ((Grounded)ParentState).Idle : null;
        }

        protected override void OnUpdate(float deltaTime)
        {
            var target = ctx.Input.MovementWrapper.Value.x * ctx.Config.horizontalSpeed;
            ctx.Velocity.x = Mathf.MoveTowards(ctx.Velocity.x, target, ctx.Config.horizontalAcceleration * deltaTime);
        }
    }

    public class Idle : State
    {
        readonly CharacterContext ctx;
        public Idle(CharacterStateMachine m, State parent, CharacterContext ctx) : base(m, parent)
        {
            this.ctx = ctx;
        }

        protected override State GetTransition()
        {
            return Mathf.Abs(ctx.Input.MovementWrapper.Value.x) > 0.01f ? ((Grounded)ParentState).Move : null;
        }

        protected override void OnEnter()
        {
            ctx.Velocity.x = 0;
        }
    }


    public class Grounded : State
    {
        readonly CharacterContext ctx;

        public readonly Idle Idle;
        public readonly Move Move;

        public Grounded(CharacterStateMachine m, State parent, CharacterContext ctx) : base(m, parent)
        {
            this.ctx = ctx;
            Idle = new Idle(m, this, ctx);
            Move = new Move(m, this, ctx);
            //Add(new DelayActivationActivity { seconds = 0.5f });
        }

        protected override State GetInitialState() => Idle;

        protected override State GetTransition()
        {
            if (ctx.Input.JumpWrapper.Value == true)
            {
                var rb = ctx.Rigidbody;
                if (rb != null)
                {
                    var config = ctx.Config;

                    var v = rb.linearVelocity;
                    v.y = config.jumpSpeed;
                    rb.linearVelocity = v;
                }
                return ((PlayerRoot)ParentState).Airborne;
            }
            return ctx.Grounded ? null : ((PlayerRoot)ParentState).Airborne;
        }

    }


    public class Airborne : State
    {
        readonly CharacterContext ctx;

        public Airborne(CharacterStateMachine m, State parent, CharacterContext ctx) : base(m, parent)
        {
            this.ctx = ctx;
        }

        protected override State GetTransition() =>
            ctx.Grounded ? ((PlayerRoot)ParentState).Grounded : null;

    }

    public class PlayerRoot : State
    {

        public readonly Grounded Grounded;
        public readonly Airborne Airborne;
        readonly CharacterContext ctx;

        public PlayerRoot(CharacterStateMachine m, CharacterContext ctx) : base(m, null)
        {
            this.ctx = ctx;
            Grounded = new Grounded(m, this, ctx);
            Airborne = new Airborne(m, this, ctx);
        }

        protected override State GetInitialState() => Grounded;
        protected override State GetTransition() => ctx.Grounded ? null : Airborne;
    }
}