using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ObstacleManager : MonoBehaviour
{
    /**public float moveDistance = 2.52f;
    public float moveTime = 0.3f;
    
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("MovePushers", 2f, 2f);
    }

    public void MovePushers()
    {
        selectedPushers.Clear();
        int numPushers = Random.Range(1, 4);
        int selectedNum;
        do
        {
            selectedNum = Random.Range(0, pushers.Length);
            if (!selectedPushers.Contains(pushers[selectedNum]))
            {
                selectedPushers.Add(pushers[selectedNum]);
            }
            else
            {

            }

        } while (selectedPushers.Count < numPushers);

        for (int i = 0; i < selectedPushers.Count; i++)
        {
            selectedPushers[i].Play("Knockem");
        }
    }

    IEnumerator MoveAlongZ(float distance, float duration)
    {
        Vector3 startPos = transform.localPosition;
        Vector3 targetPos = startPos + new Vector3(0, 0, distance);

        float elapsed = 0f;

        while (elapsed < duration)
        {
            transform.localPosition = Vector3.Lerp(startPos, targetPos, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = targetPos;
        yield return new WaitForSeconds(0.25f);

        while (elapsed < duration)
        {
            transform.localPosition = Vector3.Lerp(startPos, targetPos, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

    }**/
}
