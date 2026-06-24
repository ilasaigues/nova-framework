using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace HSM
{
    public abstract class State
    {

        public CharacterStateMachine Machine
        {
            get
            {
                if (_machine == null)
                {
                    if (ParentState != null) _machine = ParentState.Machine; // cache if parent has machine
                    else
                    {
                        throw new NullReferenceException($"State of type {GetType()} has no state machine assigned, or has no ancestor with a state machine assigned.");
                    }
                }
                return _machine;
            }
            set
            {
                _machine = value;
            }
        }
        private CharacterStateMachine _machine;
        public readonly State ParentState;
        public State ActiveChild;

        readonly List<IActivity> activities = new();
        public IReadOnlyList<IActivity> Activities => activities;

        public State(CharacterStateMachine machine, State parentState)
        {
            _machine = machine;
            ParentState = parentState;
        }

        public void AddActivity(IActivity a)
        {
            if (a != null)
            {
                activities.Add(a);
            }
        }

        // Initial child to enter state. If null,this is is a leaf state
        protected virtual State GetInitialState() => null;

        // What state should I switch to this frame. If null, stay
        protected virtual State GetTransition() => null;


        protected virtual void OnEnter() { }
        protected virtual void OnExit() { }
        protected virtual void OnUpdate(float deltaTime) { }

        // Ensure parent -> child entering, for appropriate initialization
        internal void Enter()
        {
            if (ParentState != null) ParentState.ActiveChild = this;
            OnEnter();
            State init = GetInitialState();
            init?.Enter();
        }

        // Ensure child -> parent exiting, for appropriate state collapse
        internal void Exit()
        {
            ActiveChild?.Exit();
            ActiveChild = null;
            OnExit();
        }

        internal void Update(float deltaTime)
        {
            State t = GetTransition();
            if (t != null)
            {
                Machine.Sequencer.RequestTransition(this, t);
                return;
            }

            ActiveChild?.Update(deltaTime);
            OnUpdate(deltaTime);
        }

        // Get deepest currently-active descendant state
        public State Leaf()
        {
            State s = this;
            while (s.ActiveChild != null) s = s.ActiveChild;
            return s;
        }

        // Yields this state and then each ancestor up to the root
        public IEnumerable<State> PathToRoot()
        {
            for (State s = this; s != null; s = s.ParentState) yield return s;
        }
    }
}