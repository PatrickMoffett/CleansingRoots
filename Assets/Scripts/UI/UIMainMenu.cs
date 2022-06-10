using UnityEngine;

public class UIMainMenu : MonoBehaviour
{
    [SerializeField] private GameObject _quitButton;
    public void Setup()
    {
        // Disable quit button on WebGL builds
#if UNITY_WEBGL
        _quitButton?.SetActive(false);
#endif
    }
    
    public void OnStartClicked()
    {
        ServiceLocator.Instance.Get<ApplicationStateManager>().NavigateToState(typeof(GameState));
    }

    public void OnSettingsClicked()
    {
        ServiceLocator.Instance.Get<ApplicationStateManager>().NavigateToState(typeof(SettingsState));
    }

    public void OnCreditsClicked()
    {
        ServiceLocator.Instance.Get<ApplicationStateManager>().NavigateToState(typeof(CreditsState));
    }

    public void OnQuitClicked()
    {
        Application.Quit();
    }
}
