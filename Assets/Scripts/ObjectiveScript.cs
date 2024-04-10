using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectiveScript : MonoBehaviour
{
    private GameObject winScreen;
    private StageManager stageManS;
    private bool isCollected = false;

    private void Start()
    {
        stageManS = FindObjectOfType<StageManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && gameObject.tag != "Objective")
        {
            FinishStage();
        }

        if (collision.gameObject.tag == "Player" && gameObject.tag == "Objective" && !isCollected)
        {
            CollectObjective();
        }
    }

    private void CollectObjective()
    {
        stageManS.ObjectiveCollected();
        isCollected = true;
        GetComponent<SpriteRenderer>().enabled = false; // make animation for this instead!
    }

    private void FinishStage()
    {
        stageManS.HandleLevelWin();
    }
}
