using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomePrompts : MonoBehaviour
{



    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            GameObject.Find("Map/LevelSelect").gameObject.GetComponent<Animator>().SetTrigger("map");
        }
    }
}
