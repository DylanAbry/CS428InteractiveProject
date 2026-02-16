using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ObstacleManager : MonoBehaviour
{
    public Animator[] pushers;
    public List<Animator> selectedPushers = new List<Animator>();
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
}
