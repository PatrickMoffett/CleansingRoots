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
        ServiceLocator.Instance.Get<ApplicationStateManager>().NavigateToState(null,true);
    }

    public void SettingsClicked()
    {
        ServiceLocator.Instance.Get<ApplicationStateManager>().NavigateToState(typeof(SettingsState));
    }
    public void ExitToMenuClicked()
    {
        ServiceLocator.Instance.Get<ApplicationStateManager>().NavigateToState(null,true);
        ServiceLocator.Instance.Get<ApplicationStateManager>().NavigateToState(null,true);
    }

    public void ExitGameClicked()
    {
        Application.Quit();
    }
}
