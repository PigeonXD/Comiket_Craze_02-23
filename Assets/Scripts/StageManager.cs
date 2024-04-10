using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    private GameObject goalCollection;
    private GameObject goalObj;

    public int objectiveCount;
    public int objectiveCollected = 0;

    private void Awake()
    {
        goalCollection = GameObject.Find("GoalCollection").gameObject;
        objectiveCount = goalCollection.transform.childCount - 1;
    }

    void Start()
    {
        goalObj = goalCollection.transform.Find("Goal").gameObject;
        //Debug.Log(objectiveCount + " - obj count, " + goalObj + " - goal obj");
    }

    public void ObjectiveCollected()
    {
        objectiveCollected++;
        CheckObjectiveCount();
    }

    private void CheckObjectiveCount()
    {
        if(objectiveCollected >= objectiveCount)
        {
            goalObj.SetActive(true);
        }
    }

    public void HandleLevelStart()
    {
        FindObjectOfType<Timer>().ActivateStopwatch();
        FindObjectOfType<PlayerMovement>().canMove = true;
    }

    public void HandleLevelWin()
    {
        GameObject.Find("WinScreen").gameObject.GetComponent<Animator>().SetBool("win", true);
    }
}
