using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallKick : MonoBehaviour
{
    public float kickForce = 6f;   
    public float liftForce = 1.2f; 

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            Rigidbody rb = GetComponent<Rigidbody>();

            Vector3 dir = (transform.position - col.transform.position).normalized;
            dir.y = 0;

            rb.AddForce(dir * kickForce, ForceMode.Impulse);
            rb.AddForce(Vector3.up * liftForce, ForceMode.Impulse);
        }
    }
}
