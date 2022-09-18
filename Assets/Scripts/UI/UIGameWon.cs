using UnityEngine;

namespace UI
{
    public class UIGameWon : MonoBehaviour
    {
        public void ExitToMenuClicked()
        {
            ServiceLocator.Instance.Get<ApplicationStateManager>().NavigateToState(null,true);
        }
    }
}