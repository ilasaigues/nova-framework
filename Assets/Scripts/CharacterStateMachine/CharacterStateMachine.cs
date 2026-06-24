using System.Collections.Generic;
using UnityEngine;

namespace HSM
{
    public class CharacterStateMachine : Initializable
    {
        [Inject]
        private CharacterContext _characterContext;

        public readonly State Root;
        public readonly TransitionSequencer Sequencer;

        private bool started;

        public CharacterStateMachine(State root)
        {
            Root = root;
            Sequencer = new TransitionSequencer(this);
        }
        public void Start()
        {
            if (started) return;
            started = true;
            Root.Enter();
        }

        public void ChangeState(State from, State to)
        {
            if (from == to || from == null || to == null) return;

            State lca = TransitionSequencer.Lca(from, to);

            // Exit current branch up to but not including lowest common ancestor
            // Exit from deepest node to shallowest
            for (State s = from; s != lca; s = s.ParentState) s.Exit();

            // enter fro LCA down to target, from shallowest to leaf
            var stateStack = new Stack<State>();
            for (State s = to; s != lca; s = s.ParentState) stateStack.Push(s);
            while (stateStack.Count > 0) stateStack.Pop().Enter();
        }


        public void Update(float deltaTime)
        {
            if (!started) Start();
            //InternalUpdate(deltaTime);
            Sequencer.Update(deltaTime);
        }

        internal void InternalUpdate(float deltaTime) => Root.Update(deltaTime);

    }
}