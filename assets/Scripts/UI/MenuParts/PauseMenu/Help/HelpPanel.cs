using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGameProject
{
    public class HelpPanel : MonoBehaviour
    {
        [SerializeField] private GameObject helpPanel;
        private ActiveState helpState;
        
        public ActiveState HelpState
        {
            get { return helpState; }
            set { helpState = value; }
        }

        public GameObject HelpPanelUI
        {
            get { return helpPanel; }
            set { helpPanel = value; }
        }

        public void ToggleShopPanel()
        {
            if (!IsHelpActive())
            {
                SetHelpActive(true);
                HelpState = ActiveState.ON;
            }
            else
            {
                SetHelpActive(false);
                HelpState = ActiveState.OFF;
            }
        }

        public bool IsHelpActive()
        {
            return HelpState == ActiveState.ON;
        }

        public void SetHelpActive(bool isActive)
        {
            HelpPanelUI.SetActive(isActive);
        }
    }
}