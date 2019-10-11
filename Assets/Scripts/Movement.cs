using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    //Private Variables
    private float fDefaultBouceMultiplier; //, touchTimeStart, touchTimeEnd;
    private Vector2 startPos, endPos, dir;

    [SerializeField]
    private Vector2 LaunchForce = new Vector2(1f, 1f);

    [Range(0,100)]
    public int PowerBar = 40;

    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }


    
    private void Move()
    {
        if (Input.touchCount > 0) //otherwise we access an array before its populated
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                {
                //    touchTimeStart = Time.time;
                    startPos = Input.GetTouch(0).position;
                }
            }
            if (Input.GetTouch(0).phase == TouchPhase.Ended) //when we've stopped touching the screen, apply force to the ball
            {
               // touchTimeEnd = Time.time;
                endPos = Input.GetTouch(0).position;

              //  float timeInverval = touchTimeStart - touchTimeEnd;
                dir = startPos - endPos;

                rb.isKinematic = false;
                rb.AddForce(-dir.x * LaunchForce.x, -dir.y * LaunchForce.y, PowerBar * 4);
            }

        }
    }
}
