using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    public TextMeshProUGUI timerDisplay;
    public TextMeshProUGUI recordTimeDisplay;
    public float timer;
    public float bestTime;
    public playerMovement playerScript;


    void Start()
    {
        timer = 0f;

        bestTime = PlayerPrefs.GetFloat("BestTime", 0f);

        if (bestTime > 0f)
        {
            int minutes = Mathf.FloorToInt(bestTime / 60f);
            int seconds = Mathf.FloorToInt(bestTime % 60f);

            recordTimeDisplay.text = "RECORD: " + string.Format("{0:00}:{1:00}", minutes, seconds);
        }
        else
        {
            recordTimeDisplay.text = "RECORD: --:--";
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (playerScript.gameActive)
        {
            timer += Time.deltaTime;
        }

        DisplayTime(timer);
    }

    void DisplayTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);

        timerDisplay.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void SaveBestTime()
    {
        bestTime = PlayerPrefs.GetFloat("BestTime", float.MaxValue);

        if (timer < bestTime)
        {
            PlayerPrefs.SetFloat("BestTime", timer);
            PlayerPrefs.Save();
            Debug.Log("New Best Time!");
        }
    }
}
