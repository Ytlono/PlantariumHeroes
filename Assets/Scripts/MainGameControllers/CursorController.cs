using UnityEngine;

namespace MyGameProject
{
    public class CursorController : MonoBehaviour
    {
        private bool isMouseVisible;
        private bool isGamePaused;

        public bool IsMouseVisible
        {
            get { return isMouseVisible; }
        }

        public bool IsGamePaused
        {
            get { return isGamePaused; }
            set { isGamePaused = value; }
        }

        public void Start()
        {
            LockCursor();
        }

        public void Update()
        {
            if (!IsGamePaused)
            {
                HandleCursorToggle();
            }
        }

        private void HandleCursorToggle()
        {
            if (Input.GetKeyDown(KeyCode.LeftAlt) || Input.GetKeyDown(KeyCode.RightAlt))
            {
                UnlockCursor();
            }

            if (Input.GetKeyUp(KeyCode.LeftAlt) || Input.GetKeyUp(KeyCode.RightAlt))
            {
                LockCursor();
            }
        }

        public void LockCursor()
        {
            if (!IsGamePaused)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                isMouseVisible = false;
            }
        }

        public void UnlockCursor()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            isMouseVisible = true;
        }

        public void PauseGame(bool isPaused)
        {
            IsGamePaused = !isPaused;

            if (!isPaused)
            {
                UnlockCursor();
            }
            else
            {
                LockCursor();
            }
        }
    }
}
