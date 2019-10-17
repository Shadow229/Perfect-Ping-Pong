using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Movement : MonoBehaviour
{
    //Private Variables
    private float fDefaultBouceMultiplier; //, touchTimeStart, touchTimeEnd;
    private Vector2 startPos, mvPos, endPos, dir;

    public GameObject trajectoryLine;
    public GameObject ghostTrajectory;
    public GameObject dust;
    public GameObject canvas;

    public float SensitivityX = 1.5f;
    public float SensitivityY = 2f;


    public float sw = Screen.width;
    public float sh = Screen.height;

    public float maxYAngle = 89;
    public float Pixel2AngleY;
    public Vector2 HandPos;

    public float maxXAngle = 70;

    public float Pixel2AngleX;

    public float PowerBar = 40;

    public int BounceCount = 0;

    Rigidbody rb;
    const float mVconst = 50f;

    public float angleY;
    public float angleX;

    public bool LiveShot = false;

    public float rbMag;

    private bool _Ready;
    public bool SetReady {set{_Ready = value;}}

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Pixel2AngleY = maxYAngle / sh;
        Pixel2AngleX = (maxXAngle * 2) / sw;
        _Ready = true;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        CheckDeadBall();
    }

    //public function for the UI slider to adjust the power
    public void SetPower(Slider slider)
    {
        PowerBar = slider.value;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (LiveShot)
        {
            Instantiate(dust, transform.position, Quaternion.identity);
            dust.GetComponent<ParticleSystem>().Play();

            //add a bounce, except for when we hit our target
            if (collision.gameObject.tag != "Target")
            {
                BounceCount++;
            }
 
        }
    }


    private void Move()
    {
        //if we're over the power bar : stop the throwing
        if (Input.touchCount > 0 && !canvas.GetComponent<UIDetection>().CheckPowerBar() && _Ready) //otherwise we access an array before its populated
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                {
                    //    touchTimeStart = Time.time;
                    startPos = Input.GetTouch(0).position;

                    //show our trajectory line
                    trajectoryLine.GetComponent<MeshRenderer>().enabled = true;
                }
            }
            if(Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                mvPos = Input.GetTouch(0).position;


                HandPos = mvPos;

                dir = mvPos - startPos;

                angleY =  Mathf.Clamp(dir.y * SensitivityY * Pixel2AngleY, 0f, maxYAngle);

                float F = (PowerBar * 4) ;
                float v = (F / rb.mass) * Time.fixedDeltaTime;

                //rotation
                angleX = Mathf.Clamp((mvPos.x * SensitivityX * Pixel2AngleX) - maxXAngle, -maxXAngle, maxXAngle);

               // rotate the whole ball
                   transform.eulerAngles = new Vector3(0f, angleX, 0f);
               // and the line
                   trajectoryLine.transform.eulerAngles = new Vector3(0f, angleX, 0f);


                //draw my trajectory line
                trajectoryLine.GetComponent<Trajectory>().DrawArc(v, angleY);


            }
            if (Input.GetTouch(0).phase == TouchPhase.Ended) //when we've stopped touching the screen, apply force to the ball
            {
                // touchTimeEnd = Time.time;
                endPos = Input.GetTouch(0).position;

              //  float timeInverval = touchTimeStart - touchTimeEnd;
                dir = startPos - endPos;

                rb.isKinematic = false;
   
                transform.eulerAngles = new Vector3(-angleY, angleX, 0f);
                rb.AddForce(transform.forward * PowerBar * 4);

                //update our ghost line
                ghostTrajectory.GetComponent<GhostTrajectory>().CopyTrajectory();

                //hide our trajectory line
                trajectoryLine.GetComponent<MeshRenderer>().enabled = false;

                //mark our shot as live - IEnumerator gives the force time to effect the ball
                StartCoroutine(LiveBall(0.5f));

                //disable more throwing until the ball has reset
                _Ready = false;

            }

        }
    }

    private IEnumerator LiveBall(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        LiveShot = true;
    }

        void CheckDeadBall()
    {
        rbMag = rb.velocity.magnitude;     
                
        //magnitude zero's out at high arc's - added position check to stop a respawn if a high shot is made
        if(LiveShot && rb.velocity.magnitude < 0.5f && transform.position.y < 0.01f)
        {
            GetComponent<Ball>().Respawn(true);
        }
    }


    private void AutoAim()
    {
        GameObject Target = GameObject.FindGameObjectWithTag("Target");

        Vector3 Targetpos = Target.transform.position;


    }



    Vector3 calcBallisticVelocityVector(Vector3 source, Vector3 target, float angle)
    {
        Vector3 direction = target - source;
        float h = direction.y;
        direction.y = 0;
        float distance = direction.magnitude;
        float a = angle * Mathf.Deg2Rad;
        direction.y = distance * Mathf.Tan(a);
        distance += h / Mathf.Tan(a);

        // calculate velocity
        float velocity = Mathf.Sqrt(distance * Physics.gravity.magnitude / Mathf.Sin(2 * a));
        return velocity * direction.normalized;
    }
}
