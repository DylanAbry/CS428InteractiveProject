using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConeManager : MonoBehaviour
{
    public GameObject blueCones;
    public GameObject yellowCones;

    public enum ConeColors
    {
        Blue,
        Yellow
    }

    public ConeColors colorState;

    void Start()
    {
        colorState = ConeColors.Blue;
        blueCones.SetActive(true);
        yellowCones.SetActive(false);
        InvokeRepeating("ChangeColors", 1f, 3f);
    }

    public void ChangeColors()
    {
        if (colorState == ConeColors.Blue)
        {
            blueCones.SetActive(false);
            yellowCones.SetActive(true);
            colorState = ConeColors.Yellow;        
        }
        else if (colorState == ConeColors.Yellow)
        {
            blueCones.SetActive(true);
            yellowCones.SetActive(false);
            colorState = ConeColors.Blue;
        }
        else
        {

        }

        Debug.Log(colorState);
    }
}
