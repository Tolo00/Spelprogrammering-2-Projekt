using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Arosoul.Essentials.StateMachine {

    public class StateMachine<EState> {

        private Stack<State> stateStack = new();
        private Dictionary<EState, State> stateBindings;

        private bool initSuccessful = false;
        private UnityEngine.Object handler; // The object that initialized the object
        
        public EState State => StateToEState(GetCurrentState());
        public State StateRef => GetCurrentState();
        private string HandlerName => handler.name.Split(' ')[0];

        // Events
        public event EventHandler<EState> OnStateChanged;

        public StateMachine(State[] states, UnityEngine.Object handler) {
            // Initialize State Machine
            this.handler = handler;
            
            EState[] enumValues = (EState[])Enum.GetValues(typeof(EState));
            if (states.Length == 0 || enumValues.Length == 0 || states.Length != enumValues.Length) {
                Debug.LogError($"{HandlerName}: StateMachine failed to initialize! (Invalid EState to State Binding)");
                return;
            }

            // Bind EState to State in a dictionary
            stateBindings = new Dictionary<EState, State>();
            for (int i = 0; i < enumValues.Length; i++) {
                states[i].SetHandler(handler);
                stateBindings.Add(enumValues[i], states[i]);
            }

            initSuccessful = true;
            Debug.Log($"{HandlerName}: StateMachine initialized successfully!");

            // Change into the first state
            ChangeState(enumValues[0]);
        }

        /// <summary>
        /// Change into a new state.
        /// </summary>
        /// <param name="clearStack">Remove all underlying states from stack.</param>
        public void ChangeState(EState eState, bool clearStack = true) {
            if (!ValidateState()) {
                return;
            }

            // Exit current state
            if (StateRef != null) {
                StateRef.Exit();
            }

            if (clearStack) stateStack.Clear();
            else stateStack.Pop();

            // Enter new state
            stateStack.Push(EStateToState(eState));
            StateRef.Enter();

            Debug.Log($"{HandlerName}: StateMachine successfully changed into state '{typeof(EState)}.{eState}'.");
            OnStateChanged?.Invoke(this, State);
        }

        /// <summary>
        /// Add state into stack.
        /// Temporarily override state.
        /// </summary>
        public void PushState(EState eState) {
            if (!ValidateState()) {
                return;
            }

            StateRef.Exit();
            stateStack.Push(EStateToState(eState));
            StateRef.Enter();

            Debug.Log($"{HandlerName}: StateMachine successfully pushed '{typeof(EState)}.{eState}' into stack.");
            OnStateChanged?.Invoke(this, State);
        }

        /// <summary>
        /// Remove state from top of stack.
        /// Change into state underneath.
        /// </summary>
        /// <returns>null, if state could not be popped due to no state underneath.</returns>
        public EState PopState() {
            EState poppedEState = default;

            if (!ValidateState()) {
                return poppedEState;
            }

            if (stateStack.Count <= 1) {
                // Last state cannot be popped
                Debug.LogWarning($"{HandlerName}: StateMachine failed to pop '{typeof(EState)}.{State}' from stack! (No state exists to replace it)");
                return poppedEState;
            }
            
            StateRef.Exit();
            poppedEState = StateToEState(stateStack.Pop());
            StateRef.Enter();

            Debug.Log($"{HandlerName}: StateMachine successfully popped '{typeof(EState)}.{poppedEState}' from stack.");
            OnStateChanged?.Invoke(this, State);
            return poppedEState;
        }

        /// <summary>
        /// Update the current state.
        /// Should be called once during MonoBehaviour Update cycle.
        /// </summary>
        public void UpdateState() {
            StateRef.UpdateState();
        }

        private State GetCurrentState() {
            if (stateStack.Count == 0) return null;
            return stateStack.Peek();
        }

        private State EStateToState(EState eState) {
            return stateBindings[eState];
        }

        private EState StateToEState(State state) {
            EState eState = stateBindings.FirstOrDefault(pair => pair.Value == state).Key;
            if (!eState.Equals(default(int))) {
                return eState;
            } else {
                Debug.LogError($"{HandlerName}: EState could not be found! 'Key: {State}'");
                return eState;
            }
        }

        private bool ValidateState() {
            if (initSuccessful) return true;

            Debug.LogError($"{HandlerName}: Method failed to execute! (Validation not successful)");
            return false;
        }
    }

}