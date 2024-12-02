using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class ResetTimer : MonoBehaviour
{
    // Time to restart the scene
    private TimeSpan restartTime = new TimeSpan(8, 0, 0); // 8:00 AM
    private bool hasRestartedToday = false;

    void Start()
    {
        CheckRestart();
        InvokeRepeating(nameof(CheckRestart), 1f, 60f); // Check every minute
    }

    void CheckRestart()
    {
        DateTime now = DateTime.Now;

        // Check if it's the restart time and hasn't restarted yet today
        if (now.TimeOfDay >= restartTime && !hasRestartedToday)
        {
            hasRestartedToday = true;
            RestartScene();
        }
        else if (now.TimeOfDay < restartTime)
        {
            hasRestartedToday = false; // Reset for the next day
        }
    }

    void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
