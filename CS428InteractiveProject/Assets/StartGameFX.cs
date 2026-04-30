using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class StartGameFX : MonoBehaviour
{
    public TextMeshProUGUI tmp;
    public float floatAmplitude = 10f;
    public float floatSpeed = 1.5f;
    public float glowSpeed = 2f;
    public float maxGlow = 0.7f;

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.localPosition;
    }

    void Update()
    {
        // Floating motion
        float y = Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;
        transform.localPosition = startPos + new Vector3(0, y, 0);

        // Glow pulse
        float t = (Mathf.Sin(Time.time * glowSpeed) + 1f) / 2f;
        tmp.outlineWidth = Mathf.Lerp(0f, maxGlow, t);
    }
}
