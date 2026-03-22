using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeRotation : MonoBehaviour
{
    public Vector3 rotationSpeed = new Vector3(0, 0, 100);
    

    void Update()
    {
        this.gameObject.transform.Rotate(rotationSpeed * Time.deltaTime);
    }
}
