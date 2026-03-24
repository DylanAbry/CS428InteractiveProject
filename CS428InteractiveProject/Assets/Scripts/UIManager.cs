using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject pauseScreen;
    public playerMovement playerScript;

    // Start is called before the first frame update
    void Start()
    {
        pauseScreen.SetActive(false);

    }
    
    public void PauseManager()
    {
        if (pauseScreen.activeInHierarchy)
        {
            pauseScreen.SetActive(false);
            Time.timeScale = 1f;
            playerScript.timePanel.SetActive(true);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            pauseScreen.SetActive(true);
            Time.timeScale = 0f;
            playerScript.timePanel.SetActive(false);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
