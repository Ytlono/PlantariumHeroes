using MyGameProject;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopGame : MonoBehaviour
{
    private PlayerController playerController;
    private CameraController cameraController;
    private CursorController cursorController;
    void Awake()
    {
        playerController = FindAnyObjectByType<PlayerController>();
        cameraController = FindAnyObjectByType<CameraController>();
        cursorController = FindAnyObjectByType<CursorController>();
    }

    public void StopGameProcesses(bool isStop)
    {
        switch (isStop)
        {
            case true:
                Time.timeScale = 0f;
                EnablingControllers(false);
                break;
            case false:
                Time.timeScale = 1f;
                EnablingControllers(true);
                break;
        }
    }

    private void EnablingControllers(bool isEnable)
    {
        cursorController.PauseGame(isEnable);
        playerController.enabled = isEnable;
        cameraController.enabled = isEnable;
    }
}
