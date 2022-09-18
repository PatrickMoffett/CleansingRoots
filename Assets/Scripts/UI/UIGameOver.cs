using UnityEngine;

namespace UI
{
    public class UIGameOver : MonoBehaviour
    {
        public void ExitToMenuClicked()
        {
            ServiceLocator.Instance.Get<ApplicationStateManager>().NavigateToState(null,true);
        }

        public void RestartClicked()
        {
            ServiceLocator.Instance.Get<ApplicationStateManager>().NavigateToState(typeof(GameState),true);
        }
    }
}
