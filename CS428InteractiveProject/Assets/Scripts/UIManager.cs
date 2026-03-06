using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject pauseScreen;
    // Start is called before the first frame update
    void Start()
    {
        pauseScreen.SetActive(false);

    }

    void Update()
    {
        if (!pauseScreen.activeInHierarchy)
        {
            if (Input.GetKeyDown(KeyCode.Escape)) 
            {
                pauseScreen.SetActive(true);
                Time.timeScale = 0f;
            }
        }
        
    }

    public void ContinuePlaying ()
    {
        pauseScreen.SetActive(false);
        Time.timeScale = 1f;
    }
}
