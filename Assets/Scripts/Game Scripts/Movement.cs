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
   // public GameObject ChallengeManager;

    public float SensitivityX = 1.5f;
    public float SensitivityY = 2f;

    public Vector2 releaseAngle = Vector2.zero;
    public Vector3 releaseVelocity = Vector3.zero;

    public Vector2[] SolutionAngle;
    public Vector3[] SolutionVelocity;

    //public Vector2 releaseAngleIN = new Vector2(-14.9f, 53.5f);
    //public Vector3 releaseVelocityIN = new Vector3(-61.3f,321.6f,229.8f);

    public float velocityLimit = 100f;
    public float angleLimit = 10f;
    public float rotationLimit = 10f;

    [Range(0f,1f)]
    public float AutoAimAmt = 1f;

    public bool GhostLine;

    public float sw = Screen.width;
    public float sh = Screen.height;

    public float maxYAngle = 89;
    public float Pixel2AngleY;
    public Vector2 HandPos;


    public float maxXAngle = 70;

    public float Pixel2AngleX;

    public float PowerBar = 100;

    public int BounceCount = 0;
    public int ReboundCount = 0;

    Rigidbody rb;

    public float angleY;
    public float angleX;

    public bool LiveShot = false;

    public float rbMag;

    private bool _Ready;
    public bool _Began;

    private float OriginalYRot;

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

            //add a bounce count after its been thrown, except for when we hit our target 
            if (collision.gameObject.tag != "Target" && !_Ready)
            {
                BounceCount++;
            }
 
        }
    }


    private void Move()
    {
        //if we're over the power bar or touching any of the UI: stop the throwing
        if (Input.touchCount > 0 && !canvas.GetComponent<UIDetection>().CheckUIpressed() && _Ready) //otherwise we access an array before its populated
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                {
                    //set the balls original rotation
                    OriginalYRot = transform.eulerAngles.y;

                    //log began throw
                    _Began = true;
                    //    touchTimeStart = Time.time;
                    startPos = Input.GetTouch(0).position;

                    //show our trajectory line
                    trajectoryLine.GetComponent<MeshRenderer>().enabled = true;
                }
            }
            if(Input.GetTouch(0).phase == TouchPhase.Moved && _Began)
            {
                mvPos = Input.GetTouch(0).position;


                HandPos = mvPos;

                dir = mvPos - startPos;

                angleY =  Mathf.Clamp(dir.y * SensitivityY * Pixel2AngleY, 0f, maxYAngle);

                float F = (PowerBar * 4) ;
                float v = (F / rb.mass) * Time.fixedDeltaTime;

                //rotation
                angleX = Mathf.Clamp((mvPos.x * SensitivityX * Pixel2AngleX) - maxXAngle, -maxXAngle, maxXAngle) + OriginalYRot;

               // rotate the whole ball
                   transform.eulerAngles = new Vector3(0, angleX, 0f);
               // and the line
                   trajectoryLine.transform.eulerAngles = new Vector3(0f, angleX, 0f);


                //draw my trajectory line
                trajectoryLine.GetComponent<Trajectory>().DrawArc(v, angleY);


            }
            if (Input.GetTouch(0).phase == TouchPhase.Ended && _Began) //when we've stopped touching the screen, apply force to the ball
            {
                //clear began log for next throw
                _Began = false;
                // touchTimeEnd = Time.time;
                endPos = Input.GetTouch(0).position;

              //  float timeInverval = touchTimeStart - touchTimeEnd;
                dir = startPos - endPos;


                //calc auto aim
                releaseAngle = new Vector2(angleX, angleY);
                //add trajectory angle values to the ball
                transform.eulerAngles = new Vector3(-releaseAngle.y, releaseAngle.x, 0f);

                //calc velocity
                releaseVelocity = transform.forward * PowerBar * 4; 


                AutoAim(releaseAngle, releaseVelocity);


                rb.isKinematic = false;

                //update final release angle and velocity
                transform.eulerAngles = new Vector3(-releaseAngle.y, releaseAngle.x, 0f);
                rb.AddForce(releaseVelocity.magnitude * transform.forward); //set the release velocity back to the forward transform


                //transform.eulerAngles = new Vector3(-angleY, angleX, 0f);
                //rb.AddForce(transform.forward * PowerBar * 4);

                //update our ghost line
                if (GhostLine)
                {
                    ghostTrajectory.GetComponent<MeshRenderer>().enabled = true;
                    ghostTrajectory.GetComponent<GhostTrajectory>().CopyTrajectory();
                }
                else
                {
                    ghostTrajectory.GetComponent<MeshRenderer>().enabled = false;
                }

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


    private void AutoAim(Vector2 angle, Vector3 velocity)
    {
        int i = GameManager.Instance.CurrentChallenge -1;
        //if the x is within a range - push towards target x rotation
        float xDiff = angle.x - SolutionAngle[i].x;

        if (Mathf.Abs(xDiff) <= rotationLimit)
        {
            releaseAngle.x -= xDiff * AutoAimAmt;
            Debug.Log("Rotation Difference: " + xDiff + ". Rotation Changed");
        }
        else
        {
            Debug.Log("Rotation Difference: " + xDiff + ". Rotation Not Changed");
        }

        //if angle is within a range - push toward target angle
        float yDiff = angle.y - SolutionAngle[i].y;
        Vector3 VelocityVar = new Vector3(velocity.x - SolutionVelocity[i].x, velocity.y - SolutionVelocity[i].y, velocity.z - SolutionVelocity[i].z);

        if (Mathf.Abs(yDiff) <= angleLimit)
        {
            releaseAngle.y -= yDiff * AutoAimAmt;
            Debug.Log("Angle Magnitude: " + yDiff + ". Release Angle Changed");
        }
        else
        {
            Debug.Log("Angle Magnitude: " + yDiff + ". Release Angle Not Changed");
        }

        //if velocity is within a range - push toward target velocity
        if (Mathf.Abs(VelocityVar.magnitude) <= velocityLimit)
        {
            releaseVelocity -= VelocityVar * AutoAimAmt;
            Debug.Log("Velocity Magnitude: " + VelocityVar.magnitude + ". Velocity Changed");
        }
        else
        {
            Debug.Log("Velocity Magnitude: " + VelocityVar.magnitude + ". Velocity Not Changed");
        }


        //just some debugging for the console
        if (Mathf.Abs(VelocityVar.magnitude) <= velocityLimit && Mathf.Abs(yDiff) <= angleLimit)
        {
            Debug.Log("Its going in");
        }
        else
        {
            Debug.Log("Its not going in");
        }

    }


    /*


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

    */
}
