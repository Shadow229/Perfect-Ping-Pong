using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockBounce : MonoBehaviour
{
    public float speed = 5f;
    public float height = 0.5f;
 //   public bool Show = true;


    void Update()
    {
        Vector3 pos = transform.position;
        //calculate the new Y position 
        float y = Mathf.Sin(Time.time * speed) * height + pos.y; ;
        //set y
        transform.position = new Vector3(pos.x, y, pos.z);
    }
}
