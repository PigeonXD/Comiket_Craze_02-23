using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    // Camera Movement
    [SerializeField] float xCamMove;
    [SerializeField] float yCamMove;
    [SerializeField] float camSpeed = 0.1f;

    // Camera Zoom
    private Camera mainCam;
    private PlayerMovement playerMovS;
    private float pSpeed;

    private void Start()
    {
        mainCam = GetComponent<Camera>();
        playerMovS = transform.parent.GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        // Camera Movement
        xCamMove = Input.GetAxis("Horizontal");
        yCamMove = Input.GetAxis("Vertical");

        if(playerMovS.canMove) transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(xCamMove, yCamMove, -10), camSpeed);

        // Camera Zoom
        pSpeed = playerMovS.speedAbsoluteHighest / 5;
        // if (Mathf.Abs(playerMovS.speedX) >= Mathf.Abs(playerMovS.speedY)) pSpeed = Mathf.Abs(playerMovS.speedX) / 5;
        // else if (Mathf.Abs(playerMovS.speedX) < Mathf.Abs(playerMovS.speedY)) pSpeed = Mathf.Abs(playerMovS.speedY) / 5;
        if (pSpeed > 0.5f) mainCam.orthographicSize = Mathf.Lerp(mainCam.orthographicSize, 5 + pSpeed, camSpeed);
        else mainCam.orthographicSize = Mathf.Lerp(mainCam.orthographicSize, 5, 0.01f);
    }
}
