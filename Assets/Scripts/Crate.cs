using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crate : MonoBehaviour
{
    public GameObject crateObj;
    private float oldGravity;
    private bool isGrabbed;
   
    // box position - player position when grabbed
    private Vector2 grabLocalPos;
    
    //called when a player grabs this crate
    public void EnterGrabbedState()
    {
        isGrabbed = true;
        grabLocalPos = transform.localPosition;
        oldGravity = GetComponent<Rigidbody2D>().gravityScale;
        //keep block from falling
        GetComponent<Rigidbody2D>().gravityScale = 0;
        
        //keep block from sliding
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        // keep it from hitting things
        GetComponent<Collider2D>().enabled = false;
    }

    //called when a player releases this crate
    public void ExitGrabbedState()
    {
        isGrabbed = false;
        GetComponent<Rigidbody2D>().gravityScale = oldGravity;
        GetComponent<Collider2D>().enabled = true;
    }

    void Update()
    {
        if (isGrabbed)
        {
            transform.position = transform.parent.TransformPoint(grabLocalPos);
        }
    }
}
