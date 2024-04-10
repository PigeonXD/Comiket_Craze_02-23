using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class ObjectiveMarkerTracking : MonoBehaviour
{

    public Transform target;
    private Vector3 player;
    private Vector3 axis = new Vector3(0, 0, 1);
    
    void Start()
    {
        target = GameObject.Find("Objective").gameObject.transform;
        player = GameObject.Find("Player").transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //transform.RotateAround(player, axis, Time.deltaTime * 50);
        transform.rotation = 
    }
}
