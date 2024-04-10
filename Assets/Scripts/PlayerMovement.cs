using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEditor.Experimental.GraphView.GraphView;

public class PlayerMovement : MonoBehaviour
{
    public bool canMove = false;
    private enum playerStance { Crowd, Walk, Run, Boost }
    [SerializeField] playerStance currentStance;
    [Header("Player GameObject Components")] // Player GameObject Components
    private Rigidbody2D rb;
    private BoxCollider2D[] bCol;
    [Space]
    [Header("Velocity Variables")] // Velocity Variables
    private float speedX;
    private float speedY;
    public float speedAbsoluteHighest;
    [Space] // Velocity
    [SerializeField] float maxVelWalk = 1.5f; 
    [SerializeField] float maxVelRun = 7.5f;
    [SerializeField] float maxVelBoost = 15;
    [Space] // Acceleration
    [SerializeField] float accWalk = 50;
    [SerializeField] float accRun = 2.5f;
    [SerializeField] float accBoost = 5;
    [Space] // Deceleration
    [SerializeField] float decelerationFromBoost = 1; // Speed of Boost Decay
    [SerializeField] float decelerationPassive = 2.5f; // Deceleration When No Input
    [SerializeField] float decelerationActive = 10; // Deceleration When Opposite Input
    [Space]
    [SerializeField] float curMaxVelocity;
    [SerializeField] float curAcceleration;
    [Space]
    //[Header("Acceleration Limit")]
    private float curAccDefault;
    private float curAccLimit;
    private float curAccLTH_H;
    private float curAccLTH_L;

    private float bounceThresholdX = 4;
    private float bounceThresholdY = 4;

    [Space]
    [Header("Crowd Deceleration Values")]
    //[SerializeField] float crowdLineDeceleration = 1;
    //[SerializeField] float crowdThinDeceleration = 5;
    [Space]
    [SerializeField] float crowdLineThreshold = 2;
    [SerializeField] float crowdThinThreshold = 3;
    [SerializeField] float crowdNormalThreshold = 4;
    [SerializeField] float crowdThickThreshold = 5;

    void Start()
    {
        ChangeStance("Walk");
        rb = GetComponent<Rigidbody2D>();
        bCol = GetComponents<BoxCollider2D>(); // Main = 0 / Horizontal = 1 / Vertical = 2
        //Debug.Log(bCol[0].size + "=0   " + bCol[1].size + "=1" + "    " + bCol[2].size);
    }

    private void Update()
    {
        if (Mathf.Abs(speedX) >= Mathf.Abs(speedY)) speedAbsoluteHighest = Mathf.Abs(speedX);
        else if (Mathf.Abs(speedY) > Mathf.Abs(speedX)) speedAbsoluteHighest = Mathf.Abs(speedY);

        if (currentStance != playerStance.Crowd)
        {
            StanceControls();
        }

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            ChangeStance("Boost");
        }
    }

    private void FixedUpdate()
    {
        if (speedX > curAccLTH_H || speedX < -curAccLTH_H || speedY > curAccLTH_H || speedY < -curAccLTH_H) { curAcceleration = curAccLimit; }
        else if (speedX < curAccLTH_L && speedX > -curAccLTH_L || speedY < curAccLTH_L && speedY > -curAccLTH_L) { curAcceleration = curAccDefault; }

        if (canMove) 
        {
            HorizontalMovement();
            VerticalMovement();
        }

        transform.position =  new Vector2(transform.position.x + speedX * Time.deltaTime, transform.position.y + speedY * Time.deltaTime);
    }

    private void HorizontalMovement()
    {
        if (Input.GetKey("left") && speedX > -curMaxVelocity)
        {
            if (speedX > 0) speedX = speedX - decelerationActive * Time.deltaTime;
            else speedX = speedX - curAcceleration * Time.deltaTime;
        }
        else if (Input.GetKey("right") && speedX < curMaxVelocity)
        {
            if (speedX < 0) speedX = speedX + decelerationActive * Time.deltaTime;
            else speedX = speedX + curAcceleration * Time.deltaTime;
        }
        else if (speedX > curMaxVelocity)
        {
            if (speedX > decelerationFromBoost * Time.deltaTime) speedX = speedX - decelerationFromBoost * Time.deltaTime;
            else if (speedX < -decelerationFromBoost * Time.deltaTime) speedX = speedX + decelerationFromBoost * Time.deltaTime;
        }
        else
        {
            if (speedX > decelerationPassive * Time.deltaTime) speedX = speedX - decelerationPassive * Time.deltaTime;
            else if (speedX < -decelerationPassive * Time.deltaTime) speedX = speedX + decelerationPassive * Time.deltaTime;
            else speedX = 0;
        }
    }

    private void VerticalMovement()
    {
        if (Input.GetKey("down") && speedY > -curMaxVelocity)
        {
            if (speedY > 0) speedY = speedY - decelerationActive * Time.deltaTime;
            else speedY = speedY - curAcceleration * Time.deltaTime;
        }
        else if (Input.GetKey("up") && speedY < curMaxVelocity)
        {
            if (speedY < 0) speedY = speedY + decelerationActive * Time.deltaTime;
            else speedY = speedY + curAcceleration * Time.deltaTime;
        }
        else if (speedY > curMaxVelocity)
        {
            if (speedY > decelerationFromBoost * Time.deltaTime) speedY = speedY - decelerationFromBoost * Time.deltaTime;
            else if (speedY < -decelerationFromBoost * Time.deltaTime) speedY = speedY + decelerationFromBoost * Time.deltaTime;
        }
        else
        {
            if (speedY > decelerationPassive * Time.deltaTime) speedY = speedY - decelerationPassive * Time.deltaTime;
            else if (speedY < -decelerationPassive * Time.deltaTime) speedY = speedY + decelerationPassive * Time.deltaTime;
            else speedY = 0;
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.otherCollider == bCol[1]) 
        {
            if (speedX > bounceThresholdX || speedX < -bounceThresholdX) speedX *= -0.75f;
        }
        if (collision.otherCollider == bCol[2])
        { 
            if (speedY > bounceThresholdY || speedY < -bounceThresholdY) speedY *= -0.75f; 
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "BoostPad":
                StartCoroutine(BoostTimer());
                break;
            
        }
        if (currentStance != playerStance.Crowd && currentStance != playerStance.Boost)
        {
            switch (collision.gameObject.name)
            {
                case "CrowdLine":
                    if (speedX > crowdLineThreshold || speedY > crowdLineThreshold || speedX < -crowdLineThreshold || speedY < -crowdLineThreshold)
                    {
                        speedX /= 1.2f; speedY /= 1.2f; ChangeStance("Crowd");
                    }
                    else Knockback();
                    break;
                case "CrowdThin":
                    if (speedX > crowdThinThreshold || speedY > crowdThinThreshold || speedX < -crowdThinThreshold || speedY < -crowdLineThreshold)
                    {
                        speedX /= 1.5f; speedY /= 1.5f; ChangeStance("Crowd");
                    }
                    else Knockback();
                    break;
                case "CrowdNormal":
                    if (speedX > crowdNormalThreshold || speedY > crowdNormalThreshold || speedX < -crowdNormalThreshold || speedY < -crowdNormalThreshold)
                    {
                        speedX /= 1.75f; speedY /= 1.75f; ChangeStance("Crowd");
                    }
                    else Knockback();
                    break;
                case "CrowdThick":
                    if (speedX > crowdThickThreshold || speedY > crowdThickThreshold || speedX < -crowdThickThreshold || speedY < -crowdThickThreshold)
                    {
                        speedX /= 2f; speedY /= 2f; ChangeStance("Crowd");
                    }
                    else Knockback();
                    break;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.name == "MapTrigger")
        {
            collision.transform.GetChild(0).gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        switch (collision.gameObject.name)
        {
            case "CrowdLine":
                StanceControls();
                break;
            case "CrowdThin":
                StanceControls();
                break;
            case "CrowdNormal":
                StanceControls();
                break;
            case "CrowdThick":
                StanceControls();
                break;
        }

        if (collision.gameObject.name == "MapTrigger")
        {
            collision.transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    IEnumerator BoostTimer()
    {
        bool cycle = true;
        if (cycle)
        {
            ChangeStance("Boost");
            yield return new WaitForSecondsRealtime(3);
            ChangeStance("Run");
            cycle = false;
        }
       
    }

    private void StanceControls()
    {
        if(curMaxVelocity != maxVelBoost)
        {
            if (Input.GetKey(KeyCode.LeftShift)) ChangeStance("Run");
            else ChangeStance("Walk");
        }
    }

    private void ChangeStance(string stance)
    {
        switch (stance)
        {
            case "Crowd":
                currentStance = playerStance.Crowd;
                curMaxVelocity = 3;
                curAcceleration = 10;
                break;
            case "Walk":
                currentStance = playerStance.Walk;
                curMaxVelocity = maxVelWalk;
                curAcceleration = accWalk;
                break;
            case "Run":
                currentStance = playerStance.Run;
                curMaxVelocity = maxVelRun;
                curAcceleration = accRun;
                break;
            case "Boost":
                currentStance = playerStance.Boost;
                curMaxVelocity = maxVelBoost;
                curAcceleration = accBoost;
                break;
            default:
                Debug.Log("Wrong Stance String!");
                break;
        }
        curAccDefault = curAcceleration;
        curAccLimit = (curAcceleration / 10) * 4; // 40%
        curAccLTH_H = (curMaxVelocity / 10) * 7; // 70%
        curAccLTH_L = (curMaxVelocity / 10) * 3; // 30%
    }

    private void Knockback()
    {
        speedX *= -1; speedY *= -1;
        // add a proper knockback function!
    }
}
