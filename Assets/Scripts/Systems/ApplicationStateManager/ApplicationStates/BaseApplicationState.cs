using System.Collections.Generic;

    public enum State
    {
        Inactive,
        Active,
        Background,
    }
    public abstract class BaseApplicationState
    {
        
        public State CurrentState { get; protected set; }
        public abstract void Transition(State toState, BaseApplicationState prevStateClass = null, Dictionary<string, object> options = null);
    }
