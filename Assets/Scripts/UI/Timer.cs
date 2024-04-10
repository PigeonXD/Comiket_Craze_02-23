using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    private Text text;
    private float time;
    private bool active = false;

    void Start()
    {
        text = GetComponent<Text>();
    }

    void Update()
    {
        if (active)
        {
            time += Time.deltaTime;
        }
        TimeSpan timeS = TimeSpan.FromSeconds(time);
        text.text = timeS.Minutes.ToString("00") + ":" + timeS.Seconds.ToString("00"); // + ":" + timeS.Milliseconds.ToString("000");
    }

    public void ActivateStopwatch()
    {
        active = true;
    }
}
