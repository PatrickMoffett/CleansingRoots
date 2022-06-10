using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;

public static class Bootstrapper
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void Initialize()
    {
        //Setup Services
        ServiceLocator.Initialize();
        ServiceLocator.Instance.Register(new ApplicationStateManager());
        ServiceLocator.Instance.Register(new AudioManager());
        ServiceLocator.Instance.Register(new LevelSceneManager());
        ServiceLocator.Instance.Register(new UIManager());
        
        //Setup services that must be attached to a GameObject
        GameObject singletonObject = new GameObject("singletonObject",
            typeof(EventSystem),
            typeof(InputSystemUIInputModule),
            typeof(MonoBehaviorService));
        ServiceLocator.Instance.Register(singletonObject.GetComponent<MonoBehaviorService>());
        Object.DontDestroyOnLoad(singletonObject);

        //Start MainMenuState
        ServiceLocator.Instance.Get<ApplicationStateManager>().NavigateToState(typeof(MainMenuState));
    }
}


