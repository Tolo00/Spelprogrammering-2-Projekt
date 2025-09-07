using UnityEngine;

namespace Arosoul.Essentials.StateMachine {

    public abstract class State {
        protected Object Handler { get; private set; } // The Object (script) that initialized the state machine.
        public void SetHandler(Object handler) {
            Handler = handler;
        }

        private float enterTime = 0;
        protected float TimeInState => Time.time - enterTime;
        
        public virtual void Enter() {
            enterTime = Time.time;
        }

        public virtual void Exit() { }
        public virtual void UpdateState() { }
    }

}