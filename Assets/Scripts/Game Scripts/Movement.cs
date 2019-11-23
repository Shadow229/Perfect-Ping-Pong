using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;
//using UnityEngine.EventSystems;
//using System;
using Random = UnityEngine.Random;

public class Movement : MonoBehaviour
{
    [Header("Required Game Objects")]
    public GameObject trajectoryLine;
    public GameObject ghostTrajectory;
    public GameObject dust;
    public GameObject canvas;   

    [Space]
    [Header("Audio")]
    public AudioClip[] bounceSound;
    public AudioClip[] HitBallSound;

    [Space]
    [Header("Dev Variables")]
    //visable private variables
    [SerializeField]
    [Range(0f,1f)]
    private float AutoAimAmt = 1f;
    //public float velocityLimit = 100f;
    [SerializeField]
    private float angleLimit = 10f;
    [SerializeField]
    private float rotationLimit = 10f;
    [SerializeField]
    private readonly bool GhostLine = false; //Dev only
    [SerializeField]
    private Vector2 Sensitivity = new Vector2(1f, 2f);    


    //getters and setters
    public int ReboundCount { get; set; } = 0;
    public int BounceCount { get; set; } = 0;
    public bool SetReady { set { _Ready = value; } }
    public bool IsLiveShot { get { return LiveShot; } set { LiveShot = value; } }
    public bool HasBegan { get; private set; }


    //Private Variables
    private Rigidbody rb;
    private Vector2[] SolutionAngle;
    private Vector2 startPos = Vector2.zero;
    private Vector2 releaseAngle = Vector2.zero;
    private Vector2 MaxAngle = new Vector2(70,89);
    private Vector2 Pixel2Angle;
    private float[] SolutionPower;
    private float Power;
    private float OrigRot;
    private bool _Ready;    
    private bool LiveShot = false;
    private AudioSource audioSource;


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
        GetComponent<AudioSource>().volume = GameManager.Instance.SFXVol;
        Sensitivity.x *= GameManager.Instance.SensitivityMultiplier;
        Sensitivity.y *= GameManager.Instance.SensitivityMultiplier;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
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

    public void SetSolutionValues(Vector2[] Angle, float[] Power)
    {
        SolutionAngle = Angle;
        SolutionPower = Power;
    }


    private void Move()
    {
        //Check we're touching the screen otherwise we access an array before its populated
        //if we're over the power bar or touching any of the UI: stop the throwing
        //check the ball is reset and ready to be thrown
        //check the challenge is unlocked
        if (Input.touchCount > 0 && !canvas.GetComponent<UIDetection>().CheckUIpressed() && _Ready && GameManager.Instance.IsCurrentChallengeUnlocked()) 
        {
            

            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                {
                    //log began throw
                    HasBegan = true;
                    //    touchTimeStart = Time.time;
                    startPos = Input.GetTouch(0).position;

                    //get original rotation
                    OrigRot = transform.eulerAngles.y;

                    //show our trajectory line
                    trajectoryLine.GetComponent<MeshRenderer>().enabled = true;
                }
            }
            //we're moving around on the screen - update the trajectory line and store angles
            if(Input.GetTouch(0).phase == TouchPhase.Moved && HasBegan)
            {
                Vector2 mvPos = Input.GetTouch(0).position;

                //release angle
                releaseAngle.y = Mathf.Clamp(Vector2.Distance(startPos, mvPos) * Sensitivity.y * Pixel2Angle.y, 0f, MaxAngle.y);

                //rotation
                Vector3 wb = Camera.main.WorldToScreenPoint(transform.position);

                //difference between ball and touched input in pixels provides 2 sides of a right angle tri
                Vector2 dif = mvPos - new Vector2(wb.x, wb.y);
                //SohCahToa
                float tan = dif.x / dif.y;
                //inverse tan for angle theta
                float theta = Mathf.Rad2Deg * Mathf.Atan(tan);

                //Debug.Log(theta);

                //flip it so the direction is always away from our touch input
                releaseAngle.x = dif.y > 0 ? theta + OrigRot - 180 : theta + OrigRot;

                //rotate the trajectory line
                trajectoryLine.transform.eulerAngles = new Vector3(0f, releaseAngle.x, 0f);
                //rotate the ball (for the direction arrow)
                transform.eulerAngles = new Vector3(0f, releaseAngle.x, 0f);


                // PowerBar = SolutionPower[GameManager.Instance.CurrentChallenge - 1];
                float yDiff = releaseAngle.y - SolutionAngle[GameManager.Instance.CurrentChallenge - 1].y;
                Power = SolutionPower[GameManager.Instance.CurrentChallenge - 1];

                //if the player is under the y angle limit adjust the power too
                if (Mathf.Abs(yDiff) > angleLimit)
                {
                    Power += yDiff - angleLimit;
                }

                //calculate velocity:
                /// F / m = accel -then- accel * t = v
                float v = (Power / rb.mass) * Time.fixedDeltaTime;

                //draw the trajectory line
                trajectoryLine.GetComponent<Trajectory>().DrawArc(v, releaseAngle.y);


            }
            //when we've stopped touching the screen, apply force to the ball
            if (Input.GetTouch(0).phase == TouchPhase.Ended && HasBegan) 
            {
                //clear began log for next throw
                HasBegan = false;
                
                //hide the direction
                transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;

                //hide our trajectory line
                trajectoryLine.GetComponent<MeshRenderer>().enabled = false;

                //add trajectory angle values to the ball
                transform.eulerAngles = new Vector3(-releaseAngle.y, releaseAngle.x, 0f);

                //run the auto aim to assist the players release
                AutoAim(releaseAngle);

                //play the sound
                GetComponent<AudioSource>().PlayOneShot(HitBallSound[Random.Range(0, 5)]);

                //update final release angle and velocity
                transform.eulerAngles = new Vector3(-releaseAngle.y, releaseAngle.x, 0f);
                //set the release velocity back to the forward transform and add force
                rb.AddForce(transform.forward * Power);


                //update our ghost line --Dev purposes ONLY - to help get test shots and set auto aim values
                if (GhostLine)
                {
                    ghostTrajectory.GetComponent<MeshRenderer>().enabled = true;
                    ghostTrajectory.GetComponent<GhostTrajectory>().CopyTrajectory();
                }
                else
                {
                    ghostTrajectory.GetComponent<MeshRenderer>().enabled = false;
                }

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

    private void AutoAim(Vector2 angle)
    {
        int i = GameManager.Instance.CurrentChallenge -1;

        //sense check for stored solution
        if (SolutionAngle.Length <= i || SolutionPower.Length <= i)
        {
            Debug.Log("No Solution Stored for Auto-Aim!");
            return;
        }

        //if the x is within a range - push towards target x rotation
        float xDiff = angle.x - SolutionAngle[i].x;

        if (Mathf.Abs(xDiff) <= rotationLimit)
        {
            releaseAngle.x -= xDiff * AutoAimAmt;
        }

        //if angle is within a range - push toward target angle
        float yDiff = angle.y - SolutionAngle[i].y;

        if (Mathf.Abs(yDiff) <= angleLimit)
        {
            releaseAngle.y -= yDiff * AutoAimAmt;
        }
    }
}
