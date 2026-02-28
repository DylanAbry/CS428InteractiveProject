using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ObstacleManager : MonoBehaviour
{
    public Transform[] pushers;
    public float pushDistance = 2f;
    public float pushSpeed = 6f;
    public float returnSpeed = 4f;

    private Vector3[] startLocalPos;

    void Start()
    {
        startLocalPos = new Vector3[pushers.Length];

        for (int i = 0; i < pushers.Length; i++)
            startLocalPos[i] = pushers[i].localPosition;

        InvokeRepeating(nameof(ActivatePushers), 2f, 2f);
    }

    void ActivatePushers()
    {
        // pick random number to activate
        int numToActivate = Random.Range(1, 4);

        List<int> chosen = new List<int>();

        while (chosen.Count < numToActivate)
        {
            int rand = Random.Range(0, pushers.Length);
            if (!chosen.Contains(rand))
                chosen.Add(rand);
        }

        StartCoroutine(PushRoutine(chosen));
    }

    IEnumerator PushRoutine(List<int> activePushers)
    {
        float t = 0;

        // PUSH FORWARD
        while (t < 1)
        {
            t += Time.deltaTime * pushSpeed;

            foreach (int i in activePushers)
            {
                Vector3 target = startLocalPos[i] + Vector3.forward * pushDistance;
                pushers[i].localPosition =
                    Vector3.Lerp(startLocalPos[i], target, t);
            }

            yield return null;
        }

        yield return new WaitForSeconds(0.5f);

        t = 0;

        // RETURN
        while (t < 1)
        {
            t += Time.deltaTime * returnSpeed;

            foreach (int i in activePushers)
            {
                Vector3 target = startLocalPos[i];
                pushers[i].localPosition =
                    Vector3.Lerp(startLocalPos[i] + Vector3.forward * pushDistance, target, t);
            }

            yield return null;
        }
    }

}
