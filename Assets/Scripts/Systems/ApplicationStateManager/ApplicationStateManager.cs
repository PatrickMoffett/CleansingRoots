using System;
using System.Collections;
using System.Collections.Generic;
using Systems.ApplicationStateManager.ApplicationStates;
using UnityEngine;

public class ApplicationStateManager : IService
{
    private Stack<BaseApplicationState> _states = new Stack<BaseApplicationState>();

    public ApplicationStateManager()
    {
        
    }

    /// <summary>
    /// Navigates to a given state
    /// </summary>
    /// <param name="stateType">State to push</param>
    /// <param name="popPreviousState">Option to pop current state off the stack</param>
    /// <param name="options">Optional data to be sent to the state</param>
    public void NavigateToState(Type stateType, bool popCurrentState = false, Dictionary<string, object> options = null)
    {
        // If true, pop the current state
        if (popCurrentState)
        {
            PopState();
        }
        // Setup and push new state based on type
        if(stateType == null)
        {
            // No-OP, popCurrentState should be true in this case
        }
        else if(stateType.Equals(typeof(MainMenuState)))
        {
            PushState(new MainMenuState(), options);
        }
        else if(stateType.Equals(typeof(GameState)))
        {
            PushState(new GameState(),options);
        }
        else if (stateType.Equals(typeof(SettingsState)))
        {
            PushState(new SettingsState(),options);
        }
        else if (stateType.Equals(typeof(PauseMenuState)))
        {
            PushState(new PauseMenuState(),options);
        }
        else if (stateType.Equals(typeof(CreditsState)))
        {
            PushState(new CreditsState(),options);
        }
        else if (stateType.Equals(typeof(GameOverState)))
        {
            PushState(new GameOverState(),options);
        }else if (stateType.Equals(typeof(TutorialState)))
        {
            PushState(new TutorialState(),options);
        }else if (stateType.Equals(typeof(GameWonState)))
        {
            PushState(new GameWonState(),options);
        }
        else
        {
            Debug.LogWarningFormat("Unsupported State Type: {0}", stateType.ToString());
        }
    }

    /// <summary>
    /// Pushes a new state onto the stack
    /// </summary>
    /// <param name="state">State to push</param>
    /// <param name="options">Optional data for the state</param>
    private void PushState(BaseApplicationState state, Dictionary<string, object> options)
    {
        // Grab previous state if one exists
        BaseApplicationState prevState = _states.Count == 0 ? null : _states.Peek();

        // Add new state to the stack
        _states.Push(state);

        // Transition previous state to background if one exists
        if(prevState != null)
        {
            prevState.Transition(State.BACKGROUND);
        }

        // Transition new state to active
        state.Transition(State.ACTIVE, prevState, options);
    }

    /// <summary>
    /// Pops the latest state off of the stack
    /// </summary>
    private void PopState()
    {
        BaseApplicationState popState = _states.Count == 0 ? null : _states.Pop();

        if (popState != null)
        {
            // Transition state to inactive for cleanup
            popState.Transition(State.INACTIVE);

            // Grab next state on the stack if one exists
            BaseApplicationState topState = _states.Count == 0 ? null : _states.Peek();

            if (topState != null)
            {
                topState.Transition(State.ACTIVE, popState);
            }
        }
    }

    public Type GetCurrentState()
    {
        return _states.Peek().GetType();
    }
}
