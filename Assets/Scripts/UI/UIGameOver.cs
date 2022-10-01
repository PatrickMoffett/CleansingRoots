using Globals;
using UnityEngine;

namespace UI
{
    public class UIGameOver : MonoBehaviour
    {
        public void ExitToMenuClicked()
        {
            GlobalVariables.checkpointName = "";
            ServiceLocator.Instance.Get<ApplicationStateManager>().NavigateToState(null,true);
        }

        public void RestartClicked()
        {
            GlobalVariables.checkpointName = "";
            ServiceLocator.Instance.Get<ApplicationStateManager>().NavigateToState(typeof(GameState),true);
        }
        public void RetryClicked()
        {
            ServiceLocator.Instance.Get<ApplicationStateManager>().NavigateToState(typeof(GameState),true);
        }
    }
}
