using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeedSlider : MonoBehaviour
{
    private Slider slider;
    private PlayerMovement playerS;
    private float speedToSlider;

    void Start()
    {
        slider = GetComponent<Slider>();
        playerS = transform.parent.parent.gameObject.GetComponent<PlayerMovement>();
    }

    void Update()
    {
        speedToSlider = Mathf.InverseLerp(0, 15, playerS.speedAbsoluteHighest);
        slider.value = speedToSlider;
    }
}
