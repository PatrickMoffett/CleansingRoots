using System.Collections.Generic;

    /// <summary>
    /// State Representing the current state of an Application State
    /// @Inactive means the state is no longer in Application Manager State Stack and should teardown
    /// @Background means the state is not the Active/Top State of the ApplicationStateManager
    /// @Active means the state is the currently Active/Top State of the ApplicationStateManager
    /// </summary>
    public enum State
    {
        INACTIVE,
        ACTIVE,
        BACKGROUND,
    }
    public abstract class BaseApplicationState
    {
        
        public State CurrentState { get; protected set; }
        public abstract void Transition(State toState, BaseApplicationState prevStateClass = null, Dictionary<string, object> options = null);
    }
