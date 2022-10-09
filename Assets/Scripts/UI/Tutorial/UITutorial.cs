using System.Collections.Generic;
using Globals;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Tutorial
{
    public class UITutorial : MonoBehaviour
    {
        
        [SerializeField] private TMP_Text tipTitleText;
        [SerializeField] private TMP_Text tipCount;
        [SerializeField] private TMP_Text tipText;
        [SerializeField] private Image tipImage;
        [SerializeField] private Toggle toggle;
        
        public List<TutorialPageSO> tutorialPages = new List<TutorialPageSO>();
        
        private CanvasGroup _canvasGroup;
        private int currentTipIndex = 0;

        private void Start()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            if (tutorialPages.Count > 0)
            {
                ShowTip(currentTipIndex);
            }
            Show();
            toggle.isOn = GlobalVariables.hideTutorialPopups;
        }

        private void ShowTip(int index)
        {
            tipTitleText.text = tutorialPages[index].tipTitle;
            tipText.text = tutorialPages[index].tipText;
            tipImage.sprite = tutorialPages[index].tipImage;
            UpdateTipCount();
        }

        public void NextTip()
        {
            currentTipIndex++;
            if (currentTipIndex >= tutorialPages.Count)
            {
                currentTipIndex = 0;
            }
            ShowTip(currentTipIndex);
        }

        public void SetTip(int tipToSet)
        {
            if (tipToSet >= 0 && tipToSet < tutorialPages.Count)
            {
                currentTipIndex = tipToSet;
                ShowTip(tipToSet);
            }
        }
        public void PrevTip()
        {
            currentTipIndex--;
            if (currentTipIndex < 0)
            {
                currentTipIndex = tutorialPages.Count - 1;
            }
            ShowTip(currentTipIndex);
        }

        private void UpdateTipCount()
        {
            tipCount.text = "("+(currentTipIndex+1)+"/"+tutorialPages.Count+")";
        }

        public void Hide()
        {
            _canvasGroup.alpha = 0f;
        }

        public void Show()
        {
            _canvasGroup.alpha = 1f;
        }

        public void Close()
        {
            ServiceLocator.Instance.Get<ApplicationStateManager>().NavigateToState(null,true);
        }

        public void ShowTutorialPopups()
        {
            GlobalVariables.hideTutorialPopups = toggle.isOn;
            Debug.Log(GlobalVariables.hideTutorialPopups);
        }
    }
}