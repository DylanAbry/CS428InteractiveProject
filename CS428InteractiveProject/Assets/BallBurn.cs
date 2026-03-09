using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallBurn : MonoBehaviour
{
    public ParticleSystem fire;

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Lava")
        {
            fire.Play();
        }
    }
}
