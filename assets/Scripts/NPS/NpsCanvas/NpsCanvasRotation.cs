using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGameProject
{
    public class NpsCanvasRotation : MonoBehaviour
    {
        private Transform cam;

        void Start()
        {
            if (Camera.main != null)
            {
                cam = Camera.main.transform;
            }
            else
            {
                Debug.LogError("������� ������ (Camera.main) �� �������!");
            }
        }

        void LateUpdate()
        {
            if (cam != null)
            {
                transform.LookAt(transform.position + cam.forward);
            }
        }
    }
}