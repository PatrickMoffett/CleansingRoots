using UnityEngine;

public class UICredits : MonoBehaviour
{
    public void ExitCredits()
    {
        ServiceLocator.Instance.Get<ApplicationStateManager>().NavigateToState(null,true);
    }
}