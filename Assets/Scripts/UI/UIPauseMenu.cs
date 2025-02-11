﻿using Systems.ApplicationStateManager.ApplicationStates;
using UnityEngine;

public class UIPauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject _quitButton;
    public void Setup()
    {
#if UNITY_WEBGL
        _quitButton?.SetActive(false);
#endif
    }
    
    public void ResumeClicked()
    {
        //remove pause menu state
        ServiceLocator.Instance.Get<ApplicationStateManager>().NavigateToState(null,true);
    }

    public void TutorialClicked()
    {
        ServiceLocator.Instance.Get<ApplicationStateManager>().NavigateToState(typeof(TutorialState));
    }
    public void SettingsClicked()
    {
        ServiceLocator.Instance.Get<ApplicationStateManager>().NavigateToState(typeof(SettingsState));
    }
    public void ExitToMenuClicked()
    {
        //remove pause menu state
        ServiceLocator.Instance.Get<ApplicationStateManager>().NavigateToState(null,true);
        //remove game state
        ServiceLocator.Instance.Get<ApplicationStateManager>().NavigateToState(null,true);
    }

    public void ExitGameClicked()
    {
        Application.Quit();
    }
}
