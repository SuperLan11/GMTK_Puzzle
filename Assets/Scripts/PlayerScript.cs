using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [SerializeField] GameObject playerObj;
    [SerializeField] float moveSpeed;
    [SerializeField] float grabRange;
    [SerializeField] GameObject grabTrigger;
    [SerializeField] private GameObject grabbedObject;
    [SerializeField] bool isTriggerVisible;
    private Animator animator;

    private bool facingRight;
    private bool isGrabbing;    

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

        facingRight = true;
        isGrabbing = false;
        
        Renderer grabTriggerRenderer = grabTrigger.GetComponent<Renderer>();
        // make grabTriggerRenderer invisible
        if (!isTriggerVisible)            
            grabTriggerRenderer.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {        
        float xDirection = 0;
        
        // move player right
        if (Input.GetKey(KeyCode.D))
        {
            xDirection = 1f;
            // flip player and grab trigger when turning
            if (!facingRight)            
                playerObj.transform.eulerAngles = new Vector3(0, 0, 0);                     
            facingRight = true;
        }
        // move player left
        else if(Input.GetKey(KeyCode.A))
        {
            xDirection = -1f;
            if (facingRight)            
                playerObj.transform.eulerAngles = new Vector3(0, 180, 0);             
            facingRight = false;
        }

        if (xDirection != 0)
        {
            animator.SetBool("isMoving", true);
        }
        
        GetComponent<Rigidbody2D>().velocity = new Vector2(xDirection * moveSpeed, GetComponent<Rigidbody2D>().velocity.y);
        
        // gets a crate in grab range, if any
        // if multiple crates in range, gets the one that first entered the range
        Transform grabbableCrate = GetGrabbableCrate();

        // flip grab toggle
        if (Input.GetKeyDown(KeyCode.Space) && (isGrabbing || grabbableCrate != null))
        {
            isGrabbing = !isGrabbing;

            if (isGrabbing)
            {
                grabbableCrate.SetParent(playerObj.transform);
                grabbableCrate.gameObject.GetComponent<Crate>().EnterGrabbedState();
                grabbedObject = grabbableCrate.gameObject;
            }
            else
            {
                // unparents crate from player
                grabbedObject.transform.SetParent(null);
                grabbedObject.GetComponent<Crate>().ExitGrabbedState();
                grabbedObject = null;
            }
        }

        // resize player and grabbed crate when shift pressed
        if (Input.GetKeyDown(KeyCode.LeftControl) && isGrabbing && grabbableCrate != null)
        {
            // resizes player and crate since crate is parented
            playerObj.transform.localScale *= 2;
        }
    }

    private Transform GetGrabbableCrate()
    {

        if (grabTrigger.GetComponent<GrabTriggerScript>().cratesTouched.Count > 0)
        {
            return grabTrigger.GetComponent<GrabTriggerScript>().cratesTouched[0].transform;
        }
        else
        {
            return null;
        }
    }
}