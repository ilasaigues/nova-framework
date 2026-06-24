using System;
using System.Collections.Generic;
using System.Threading;
namespace HSM
{
    public class TransitionSequencer
    {
        public readonly CharacterStateMachine Machine;

        private ISequence sequencer; // current transition
        private Action nextPhase;   // switch structure between phases
        (State from, State to)? pending; // store single pending request
        State lastFrom, lastTo; // store last request values, for debugging mostly



        public TransitionSequencer(CharacterStateMachine machine)
        {
            Machine = machine;
        }

        public void RequestTransition(State from, State to)
        {
            if (to == null || from == to) return;
            if (sequencer != null) { pending = (from, to); return; }
            BeginTransition(from, to);
        }

        static List<PhaseStep> GatherPhaseSteps(List<State> chain, bool deactivate)
        {
            var steps = new List<PhaseStep>();
            for (int i = 0; i < chain.Count; i++)
            {
                var activities = chain[i].Activities;

                for (int j = 0; j < activities.Count; j++)
                {
                    var a = activities[j];
                    if (deactivate)
                    {
                        if (a.Status == ActivityStatus.Active)
                            steps.Add(ct => a.DeactivateAsync(ct));
                    }
                    else
                    {
                        if (a.Status == ActivityStatus.Inactive)
                            steps.Add(ct => a.ActivateAsync(ct));
                    }
                }
            }
            return steps;
        }

        // get bottom->up ladder of states to exit from "from" up to but excluding "lca"
        static List<State> StatesToExit(State from, State lca)
        {
            var list = new List<State>();
            for (var s = from; s != null && s != lca; s = s.ParentState) list.Add(s);
            return list;
        }

        // get top->down ladder of states to enter from "to" up to but excluding "lca"
        static List<State> StatesToEnter(State to, State lca)
        {
            var stack = new Stack<State>();
            for (var s = to; s != lca; s = s.ParentState) stack.Push(s);
            return new List<State>(stack);
        }

        CancellationTokenSource cts;
        public readonly bool UseSequential = true; // set false to use parallel

        void BeginTransition(State from, State to)
        {
            var lca = Lca(from, to);
            var exitChain = StatesToExit(from, lca);
            var enterChain = StatesToEnter(from, lca);

            // Deactivate the old transition branch
            var exitSteps = GatherPhaseSteps(exitChain, deactivate: true);
            sequencer = UseSequential
                ? new SequentialPhase(exitSteps, cts.Token)
                : new ParallelPhase(exitSteps, cts.Token);
            sequencer.Start();

            nextPhase = () =>
            {
                // Change State
                Machine.ChangeState(from, to);
                // Activate the new transition branch
                var enterSteps = GatherPhaseSteps(enterChain, deactivate: false);
                sequencer = UseSequential
                    ? new SequentialPhase(enterSteps, cts.Token)
                    : new ParallelPhase(enterSteps, cts.Token);
                sequencer.Start();
            };
        }

        void EndTransition()
        {
            sequencer = null;
            if (pending.HasValue)
            {
                (State from, State to) = pending.Value;
                pending = null;
                BeginTransition(from, to);
            }
        }

        public void Update(float deltaTime)
        {
            if (sequencer != null)
            {
                if (sequencer.Update())
                {
                    if (nextPhase != null)
                    {
                        var n = nextPhase;
                        nextPhase = null;
                        n();
                    }
                    else
                    {
                        EndTransition();
                    }
                }
                return;
            }
            Machine.InternalUpdate(deltaTime);
        }

        // Get Lowest Common Ancestor of two hierarchical states
        public static State Lca(State a, State b)
        {
            // get all parents of A
            var parentsA = new HashSet<State>();
            foreach (State s in a.PathToRoot()) parentsA.Add(s);

            // Find first parent of B that is also an ancestor of A
            foreach (State s in b.PathToRoot())
            {
                if (parentsA.Contains(s)) return s;
            }

            // if no common ancestor, return null
            return null;
        }
    }
}