using System.Collections.Generic;

public class GameOverState : BaseApplicationState
{
    public readonly string UI_PREFAB = "UIGameOver";
    public readonly string SCENE_NAME = "Main";
    private UIWidget _uiWidget;

    public GameOverState()
    {
        
    }

    public override void Transition(State toState, BaseApplicationState prevStateClass = null, Dictionary<string, object> options = null)
    {
        // Guard against same state transition
        if (toState == CurrentState)
        {
            return;
        }

        if(toState == State.Active && CurrentState == State.Inactive)
        {
            SetupState();
        }
        else if(toState == State.Inactive && CurrentState == State.Active)
        {
            TeardownState();
        }
        else if(toState== State.Background && CurrentState == State.Active)
        {
            SetToBackgroundStateFromActive();
        }
        else if(toState == State.Active && CurrentState == State.Background)
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

    public void SetupState()
    {
        _uiWidget = ServiceLocator.Instance.Get<UIManager>().LoadUI(UI_PREFAB);
        ServiceLocator.Instance.Get<LevelSceneManager>().LoadLevel(SCENE_NAME);
    }

    public void TeardownState()
    {
        if(_uiWidget != null)
        {
            ServiceLocator.Instance.Get<UIManager>().RemoveUIByGuid(_uiWidget.GUID);
        }
    }
}