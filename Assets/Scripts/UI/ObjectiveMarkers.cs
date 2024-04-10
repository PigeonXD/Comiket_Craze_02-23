using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveMarkers : MonoBehaviour
{

    private StageManager stageManS;
    private int markerAmount;

    void Start()
    {
        stageManS = FindObjectOfType<StageManager>();
        markerAmount = stageManS.objectiveCount;

        for(int i = 0; i < markerAmount; i++)
        {
            //Instantiate
        }
    }
}
