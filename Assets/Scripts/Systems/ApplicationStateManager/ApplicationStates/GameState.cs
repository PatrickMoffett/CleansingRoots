using System.Collections.Generic;
using Constants;
using Systems.AudioManager;
using UnityEngine;

public class GameState : BaseApplicationState
{
    public readonly string UI_PREFAB = UIPrefabs.GameUI;
    public readonly int SCENE_NAME = (int)SceneIndexes.ROBOT_FACTORY_SCENE;
    private AudioClip _mainMenuMusic = Resources.Load<AudioClip>("MainMenuMusicReducedVolume");
    private UIWidget _uiWidget;

    public GameState()
    {

    }

    public override void Transition(State toState, BaseApplicationState prevStateClass = null, Dictionary<string, object> options = null)
    {
        // Guard against same state transition
        if (toState == CurrentState)
        {
            return;
        }

        if (toState == State.ACTIVE && CurrentState == State.INACTIVE)
        {
            SetupState();
        }
        else if (toState == State.INACTIVE && CurrentState == State.ACTIVE)
        {
            TeardownState();
        }
        else if (toState == State.BACKGROUND && CurrentState == State.ACTIVE)
        {
            SetToBackgroundStateFromActive();
        }
        else if (toState == State.ACTIVE && CurrentState == State.BACKGROUND)
        {
            SetToActiveStateFromBackground();
        }

        CurrentState = toState;
    }

    private void SetToActiveStateFromBackground()
    {
        if (_uiWidget != null)
        {
            _uiWidget.UIObject.SetActive(true);
        }
    }

    private void SetToBackgroundStateFromActive()
    {
        if (_uiWidget != null)
        {
            _uiWidget.UIObject.SetActive(false);
        }
    }

    public void SetupState()
    {
        _uiWidget = ServiceLocator.Instance.Get<UIManager>().LoadUI(UI_PREFAB);
        ServiceLocator.Instance.Get<LevelSceneManager>().LoadLevel(SCENE_NAME);
        ServiceLocator.Instance.Get<MusicManager>().StartSong(_mainMenuMusic,1f);
    }

    public void TeardownState()
    {
        if (_uiWidget != null)
        {
            ServiceLocator.Instance.Get<UIManager>().RemoveUIByGuid(_uiWidget.GUID);
        }
    }
}