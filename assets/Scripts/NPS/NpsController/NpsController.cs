using MyGameProject;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Microsoft
{
    public class NpsController : MonoBehaviour
    {
        [SerializeField]
        private GameObject NPSChallengePanel;

        [SerializeField]
        private Image panelCon;

        private void Start()
        {
            if (NPSChallengePanel != null)
            {
                NPSChallengePanel.SetActive(false);
            }

            if (panelCon != null)
            {
                panelCon.gameObject.SetActive(true);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                if (NPSChallengePanel != null)
                {
                    NPSChallengePanel.SetActive(true);
                }

                if (panelCon != null)
                {
                    panelCon.gameObject.SetActive(false);
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                if (NPSChallengePanel != null)
                {
                    NPSChallengePanel.SetActive(false);
                }

                if (panelCon != null)
                {
                    panelCon.gameObject.SetActive(true);
                }
            }
        }
    }
}
