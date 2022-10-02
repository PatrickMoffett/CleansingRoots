using System.Collections.Generic;
using Constants;
using Globals;
using Systems.AudioManager;
using UnityEngine;

public class MainMenuState : BaseApplicationState
{
    public readonly string UI_PREFAB = UIPrefabs.MainMenuUI;
    private AudioClip _mainMenuMusic = Resources.Load<AudioClip>("MainMenuMusicReducedVolume");
    public readonly int SCENE_NAME = (int)SceneIndexes.INITIAL_SCENE;
    private UIWidget _uiWidget;

    public MainMenuState()
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
            SetupState();
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
        
        ServiceLocator.Instance.Get<LevelSceneManager>().LoadLevel(SCENE_NAME);
    }

    public void SetupState()
    {
        ServiceLocator.Instance.Get<LevelSceneManager>().LoadLevel(SCENE_NAME);
        _uiWidget = ServiceLocator.Instance.Get<UIManager>().LoadUI(UI_PREFAB);
        _uiWidget.UIObject.GetComponent<UIMainMenu>()?.Setup();
        ServiceLocator.Instance.Get<MusicManager>().StartSong(_mainMenuMusic);
        
    }

    public void TeardownState()
    {
        if(_uiWidget != null)
        {
            ServiceLocator.Instance.Get<UIManager>().RemoveUIByGuid(_uiWidget.GUID);
        }
    }
}