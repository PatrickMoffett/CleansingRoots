using System.Collections.Generic;
using Constants;
using UI.Tutorial;
using UnityEngine;

namespace Systems.ApplicationStateManager.ApplicationStates
{
    public class TutorialState : BaseApplicationState
    {
    public readonly string UI_PREFAB = UIPrefabs.TutorialUI;
    private UIWidget _uiWidget;
    private UITutorial _tutorial;

    public TutorialState()
    {

    }

    public override void Transition(State toState, BaseApplicationState prevStateClass = null, Dictionary<string, object> options = null)
    {
        // Guard against same state transition
        if (toState == CurrentState)
        {
            return;
        }

        if(toState == State.ACTIVE && CurrentState == State.INACTIVE)
        {
            if (options != null)
            {
                if (options.ContainsKey("Page"))
                {
                    int page = (int) options["Page"];
                    SetupState(page);
                }
                else
                {
                    SetupState();
                }
            }
            else
            {
                SetupState();
            }
        }
        else if(toState == State.INACTIVE && CurrentState == State.ACTIVE)
        {
            TeardownState();
        }
        else if(toState== State.BACKGROUND && CurrentState == State.ACTIVE)
        {
            SetToBackgroundStateFromActive();
        }
        else if(toState == State.ACTIVE && CurrentState == State.BACKGROUND)
        {
            SetToActiveStateFromBackground();
        }

        CurrentState = toState;
    }

    private void SetToBackgroundStateFromActive()
    {
        if (_uiWidget != null)
        {
            _uiWidget.UIObject.SetActive(false);
        }
    }

    private void SetToActiveStateFromBackground()
    {
        if (_uiWidget != null)
        {
            _uiWidget.UIObject.SetActive(true);
        }
    }

    private void SetupState(int page = 0)
    {
        _uiWidget = ServiceLocator.Instance.Get<UIManager>().LoadUI(UI_PREFAB);
        _tutorial = _uiWidget.UIObject.GetComponent<UITutorial>();
        _tutorial.SetTip(page);
        Time.timeScale = 0f;
    }

    private void TeardownState()
    {
        if(_uiWidget != null)
        {
            ServiceLocator.Instance.Get<UIManager>().RemoveUIByGuid(_uiWidget.GUID);
        }
    }
    }
}