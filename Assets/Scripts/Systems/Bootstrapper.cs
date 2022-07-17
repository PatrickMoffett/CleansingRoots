using AI.WaypointNavigation;
using Constants;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;

public static class Bootstrapper
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void Initialize()
    {
        ServiceLocator.Initialize();
        
        //Setup services that must be attached to a GameObject
        GameObject singletonObject = new GameObject("singletonObject",
            typeof(EventSystem),
            typeof(InputSystemUIInputModule),
            typeof(MonoBehaviorService));
        ServiceLocator.Instance.Register(singletonObject.GetComponent<MonoBehaviorService>());
        Object.DontDestroyOnLoad(singletonObject);
        
        //Setup Services
        ServiceLocator.Instance.Register(new ApplicationStateManager());
        ServiceLocator.Instance.Register(new AudioManager());
        ServiceLocator.Instance.Register(new LevelSceneManager());
        ServiceLocator.Instance.Register(new UIManager());
        ServiceLocator.Instance.Register(new WaypointNavigationSystem());
        
        //Start MainMenuState
        ServiceLocator.Instance.Get<ApplicationStateManager>().NavigateToState(typeof(MainMenuState));
#if UNITY_EDITOR
        int currentLevelIndex = ServiceLocator.Instance.Get<LevelSceneManager>().GetLevelIndex();
        if (currentLevelIndex != (int)SceneIndexes.INITIAL_SCENE)
        {
            ServiceLocator.Instance.Get<ApplicationStateManager>().NavigateToState(typeof(GameState));
            ServiceLocator.Instance.Get<LevelSceneManager>().LoadLevel(currentLevelIndex);
        }
#endif
    }
}


