using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class Movement : MonoBehaviour
{
    //Private Variables
    private Vector2 startPos = Vector2.zero;
    private Rigidbody rb;

    public GameObject trajectoryLine;
    public GameObject ghostTrajectory;
    public GameObject dust;
    public GameObject canvas;

    public Vector2 Sensitivity = new Vector2(1f, 2f);

    public Vector2 releaseAngle = Vector2.zero;
    public Vector3 releaseVelocity = Vector3.zero;

    public Vector2[] SolutionAngle;
    public Vector3[] SolutionVelocity;

    public AudioClip[] bounceSound;

    public float velocityLimit = 100f;
    public float angleLimit = 10f;
    public float rotationLimit = 10f;

    [Range(0f,1f)]
    public float AutoAimAmt = 1f;

    //private bool GhostLine; //Dev only

    private Vector2 MaxAngle = new Vector2(70,89);
    private Vector2 Pixel2Angle;
    private AudioSource audioSource;

    public float PowerBar = 100;

    public int BounceCount = 0;
    public int ReboundCount = 0;
    public bool LiveShot = false;
    private bool _Ready;
    public bool _Began;

    public bool SetReady {set{_Ready = value;}}

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Pixel2Angle.y = MaxAngle.y / GameManager.Instance.ScreenHeight;
        Pixel2Angle.x = (MaxAngle.x * 2) / GameManager.Instance.ScreenWidth;
        audioSource = GetComponent<AudioSource>();
        _Ready = true;
    }

    private void Awake()
    {
        Sensitivity.x *= GameManager.Instance.SensitivityMultiplier;
        Sensitivity.y *= GameManager.Instance.SensitivityMultiplier;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
       // CheckDeadBall();
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
            if (!collision.gameObject.CompareTag("Target") && !_Ready)
            {
                BounceCount++;

                
                //if its a dead ball play the runoff audio
                if (rb.velocity.magnitude < 1f)
                {
                    if (!audioSource.isPlaying)
                    {
                        audioSource.clip = bounceSound[bounceSound.Length - 1];
                        audioSource.Play();
                    }

                }
                //otherwise bounce per bouncecount
                else
                {
                    audioSource.clip = bounceSound[BounceCount - 1];
                    audioSource.Play();
                }
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
                    //OriginalYRot = transform.eulerAngles.y;

                    //log began throw
                    _Began = true;
                    //    touchTimeStart = Time.time;
                    startPos = Input.GetTouch(0).position;

                    //show our trajectory line
                    trajectoryLine.GetComponent<MeshRenderer>().enabled = true;
                }
            }
            //we're moving around on the screen - update the trajectory line and store angles
            if(Input.GetTouch(0).phase == TouchPhase.Moved && _Began)
            {
                Vector2 mvPos = Input.GetTouch(0).position;

                //movement amount * sensitivity, converted to angle and clamped between min and max
                releaseAngle.y = Mathf.Clamp((mvPos.y - startPos.y) * Sensitivity.y * Pixel2Angle.y, 0f, MaxAngle.y);

                //rotation
                releaseAngle.x = Mathf.Clamp((((mvPos.x) - (GameManager.Instance.ScreenWidth * 0.5f)) * Sensitivity.x * Pixel2Angle.x), -MaxAngle.x, MaxAngle.x) + transform.eulerAngles.y;

                //rotate the trajectory line
                trajectoryLine.transform.eulerAngles = new Vector3(0f, releaseAngle.x, 0f);

                //calculate velocity:
                /// F / m = accel -then- accel * t = v
                float v = ((PowerBar * 4) / rb.mass) * Time.fixedDeltaTime;

                //draw the trajectory line
                trajectoryLine.GetComponent<Trajectory>().DrawArc(v, releaseAngle.y);


            }
            if (Input.GetTouch(0).phase == TouchPhase.Ended && _Began) //when we've stopped touching the screen, apply force to the ball
            {
                //clear began log for next throw
                _Began = false;

                //hide our trajectory line
                trajectoryLine.GetComponent<MeshRenderer>().enabled = false;

                //add trajectory angle values to the ball
                transform.eulerAngles = new Vector3(-releaseAngle.y, releaseAngle.x, 0f);

                //calc velocity
                releaseVelocity = transform.forward * PowerBar * 4; 

                //run the auto aim to assist the players release
                AutoAim(releaseAngle, releaseVelocity);

                //update final release angle and velocity
                transform.eulerAngles = new Vector3(-releaseAngle.y, releaseAngle.x, 0f);
                //set the release velocity back to the forward transform and add force
                rb.AddForce(releaseVelocity.magnitude * transform.forward); 


                //update our ghost line -- Dev purposes ONLY - remove at production
                //if (GhostLine)
                //{
                //    ghostTrajectory.GetComponent<MeshRenderer>().enabled = true;
                //    ghostTrajectory.GetComponent<GhostTrajectory>().CopyTrajectory();
                //}
                //else
                //{
                //    ghostTrajectory.GetComponent<MeshRenderer>().enabled = false;
                //}

                //mark our shot as live - IEnumerator gives the force time to effect the ball
                StartCoroutine(LiveBall(0.1f));

                //disable more throwing until the ball has reset
                _Ready = false;
            }

        }
        else
        {
            CheckDeadBall();
        }
    }

    private IEnumerator LiveBall(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        LiveShot = true;
    }


    void CheckDeadBall()
    {
        //rbMag = rb.velocity.magnitude;     
                
        //magnitude zero's out at high arc's - added position check to stop a respawn if a high shot is made
        if(LiveShot && rb.velocity.magnitude < 0.5f && transform.position.y < 0.01f)
        {
            //force the respawn if our magnitude drops too low
            GetComponent<Ball>().Respawn(true);
        }
        else
        {
            //allow a regular respawn check from the ball script
            GetComponent<Ball>().Respawn(false);
        }
    }


    private void AutoAim(Vector2 angle, Vector3 velocity)
    {
        int i = GameManager.Instance.CurrentChallenge -1;

        //sense check for stored solution
        if (SolutionAngle.Length <= i || SolutionVelocity.Length <= i)
        {
            //Debug.Log("No Solution Stored for Auto-Aim!");
            return;
        }
        //if the x is within a range - push towards target x rotation
        float xDiff = angle.x - SolutionAngle[i].x;

        if (Mathf.Abs(xDiff) <= rotationLimit)
        {
            releaseAngle.x -= xDiff * AutoAimAmt;
            //Debug.Log("Rotation Difference: " + xDiff + ". Rotation Changed");
        }
       // else
       // {
            //Debug.Log("Rotation Difference: " + xDiff + ". Rotation Not Changed");
       // }

        //if angle is within a range - push toward target angle
        float yDiff = angle.y - SolutionAngle[i].y;
        Vector3 VelocityVar = new Vector3(velocity.x - SolutionVelocity[i].x, velocity.y - SolutionVelocity[i].y, velocity.z - SolutionVelocity[i].z);

        if (Mathf.Abs(yDiff) <= angleLimit)
        {
            releaseAngle.y -= yDiff * AutoAimAmt;
            //Debug.Log("Angle Magnitude: " + yDiff + ". Release Angle Changed");
        }
        //else
        //{
            //Debug.Log("Angle Magnitude: " + yDiff + ". Release Angle Not Changed");
        //}

        //if velocity is within a range - push toward target velocity
        if (Mathf.Abs(VelocityVar.magnitude) <= velocityLimit)
        {
            releaseVelocity -= VelocityVar * AutoAimAmt;
            //Debug.Log("Velocity Magnitude: " + VelocityVar.magnitude + ". Velocity Changed");
        }
       // else
        //{
            //Debug.Log("Velocity Magnitude: " + VelocityVar.magnitude + ". Velocity Not Changed");
        //}


        //just some debugging for the console
        //if (Mathf.Abs(VelocityVar.magnitude) <= velocityLimit && Mathf.Abs(yDiff) <= angleLimit)
        //{
        //    Debug.Log("Its going in");
        //}
        //else
        //{
        //    Debug.Log("Its not going in");
        //}

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
