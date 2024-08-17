using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour, SizeObject
{
    [SerializeField] GameObject playerObj;
    [SerializeField] float moveSpeed;
    [SerializeField] private float jumpStrength;
    [SerializeField] float grabRange;
    [SerializeField] GameObject grabTrigger;
    [SerializeField] private GameObject grabbedObject;
    [SerializeField] bool isTriggerVisible;
    // 0: small, 1: medium, 2: large
    private Animator animator;

    private bool facingRight;
    private bool isGrabbing;    

    // Start is called before the first frame update
    void Start()
    {
        size = 1;
        animator = GetComponentInChildren<Animator>();

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

        // size up
        if (Input.GetKeyDown(KeyCode.E) && ((SizeObject)this).CanExpand())
        {
            size++;
            // resizes player and crate since crate is parented
            playerObj.transform.localScale *= 2;
            playerObj.transform.position +=
                transform.TransformVector(new Vector2(0,
                    GetComponent<BoxCollider2D>().size.y / 4));
            if (isGrabbing && !grabbedObject.GetComponent<SizeObject>().CanExpand())
            {
                grabbedObject.transform.localScale /= 2;
            }
            else if (isGrabbing)
            {
                grabbedObject.GetComponent<Crate>().size++;
            }
            
        }
        //size down
        else if (Input.GetKeyDown(KeyCode.Q) && ((SizeObject)this).CanShrink())
        {
            size--;
            playerObj.transform.localScale /= 2;
            playerObj.transform.position -=
                transform.TransformVector(new Vector2(0,
                    GetComponent<BoxCollider2D>().size.y / 2));
            if (isGrabbing && !grabbedObject.GetComponent<SizeObject>().CanShrink())
            {
                grabbedObject.transform.localScale *= 2;
            }
            else if (isGrabbing)
            {
                grabbedObject.GetComponent<Crate>().size--;
            }
        }

        if (Input.GetKeyDown(KeyCode.J) && IsTouchingFloor())
        {
            GetComponent<Rigidbody2D>().AddForce(Vector2.up * jumpStrength, ForceMode2D.Impulse);
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

    List<GameObject> floorContacts = new List<GameObject>();
    
    private bool IsTouchingFloor()
    {
        return floorContacts.Count > 0;
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.contacts[0].normal.y > 0.9)
        {
            floorContacts.Add(other.gameObject);
        }
    }
    
    private void OnCollisionExit2D(Collision2D other)
    {
        //this won't throw if item isn't found
        floorContacts.Remove(other.gameObject);
    }

    public int GetMaxSize()
    {
        return 2;
    }

    public int GetMinSize()
    {
        return 0;
    }

    public int size { get; set; }

}