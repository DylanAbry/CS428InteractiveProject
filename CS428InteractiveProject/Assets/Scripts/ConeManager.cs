using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConeManager : MonoBehaviour
{
    public GameObject[] blueCones;
    public GameObject[] yellowCones;

    public enum ConeColors
    {
        Blue,
        Yellow
    }

    public ConeColors colorState;

    void Awake()
    {
        InvokeRepeating("ChangeColors", 0f, 3.5f);
    }

    void Start()
    {
        colorState = ConeColors.Blue;
        foreach (GameObject cone in blueCones)
        {
            cone.GetComponent<MeshRenderer>().enabled = true;
            cone.GetComponent<MeshCollider>().enabled = true;
        }
        foreach (GameObject cone in yellowCones)
        {
            cone.GetComponent<MeshRenderer>().enabled = false;
            cone.GetComponent<MeshCollider>().enabled = false;
        }
        
    }

    public void ChangeColors()
    {
        if (colorState == ConeColors.Blue)
        {
            foreach (GameObject cone in blueCones)
            {
                cone.GetComponent<MeshRenderer>().enabled = false;
                cone.GetComponent<MeshCollider>().enabled = false;
                
            }
            foreach (GameObject cone in yellowCones)
            {
                cone.GetComponent<MeshRenderer>().enabled = true;
                cone.GetComponent<MeshCollider>().enabled = true;
                cone.GetComponent<AudioSource>().Play();
            }
            colorState = ConeColors.Yellow;        
        }
        else if (colorState == ConeColors.Yellow)
        {
            foreach (GameObject cone in blueCones)
            {
                cone.GetComponent<MeshRenderer>().enabled = true;
                cone.GetComponent<MeshCollider>().enabled = true;
                cone.GetComponent<AudioSource>().Play();
            }
            foreach (GameObject cone in yellowCones)
            {
                cone.GetComponent<MeshRenderer>().enabled = false;
                cone.GetComponent<MeshCollider>().enabled = false;
            }
            colorState = ConeColors.Blue;
        }
        else
        {

        }

        Debug.Log(colorState);
    }
}
