using UnityEngine;

namespace UI.Tutorial
{
    [CreateAssetMenu(menuName="ScriptableObject/TutorialPage")]
    public class TutorialPageSO : ScriptableObject
    {
        public string tipTitle;
        public string tipText;
        public Sprite tipImage;
    }

}