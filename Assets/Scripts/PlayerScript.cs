using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour, SizeObject
{
    [SerializeField] GameObject playerObj;
    [SerializeField] float moveSpeed;
    [SerializeField] private Vector2 throwVector;
    [SerializeField] private float jumpStrength;
    [SerializeField] float grabRange;
    [SerializeField] GameObject grabTrigger;
    [SerializeField] private GameObject grabbedObject;
    [SerializeField] bool isTriggerVisible;
    // 1: small, 2: medium, 3: large
    private Animator animator;

    private bool facingRight;
    private bool isGrabbing;    

    // Start is called before the first frame update
    void Start()
    {
        size = 1;
        ResizeBy(1);
        animator = GetComponentInChildren<Animator>();

        facingRight = true;
        isGrabbing = false;
        
        Renderer grabTriggerRenderer = grabTrigger.GetComponent<Renderer>();
        // make grabTriggerRenderer invisible
        if (!isTriggerVisible)            
            grabTriggerRenderer.enabled = false;
    }

    public void ResizeBy(int sizeDiff)
    {
        size += sizeDiff;
        float scale = (float)(size) / (size - sizeDiff);
        if (isGrabbing)
        {
            grabbedObject.transform.localScale /= scale;
        }
        float oldSize = GetComponent<BoxCollider2D>().size.y;
        float newSize = oldSize * scale;
        float adjustment = (newSize - oldSize) / 2;
        transform.position += transform.TransformVector(Vector2.up * adjustment);
        transform.localScale *= scale;
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

        animator.SetBool("isMoving", xDirection != 0);
        
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
                //flip x direction of throw vector if player is facing left
                Vector2 correctedThrowVector = throwVector;
                if (!facingRight)
                {
                    correctedThrowVector.x *= -1;
                }
                
                //throw crate
                grabbedObject.GetComponent<Rigidbody2D>().AddForce(correctedThrowVector);
                grabbedObject = null;
            }
        }

        // size up
        
        if (Input.GetKeyDown(KeyCode.E) && ((SizeObject)this).CanExpand())
        {
            ResizeBy(1);
            //if the crate they are holding is at max size, undo the resize
            if (isGrabbing && grabbedObject.GetComponent<SizeObject>().CanExpand())
            {
                grabbedObject.GetComponent<SizeObject>().ResizeBy(1);
            }
        }
        //size down
        else if (Input.GetKeyDown(KeyCode.Q) && ((SizeObject)this).CanShrink())
        {
            ResizeBy(-1);
            //if the crate they are holding is at max size, undo the resize
            if (isGrabbing && grabbedObject.GetComponent<SizeObject>().CanShrink())
            {
                grabbedObject.GetComponent<SizeObject>().ResizeBy(-1);
            }
        }
        
        //jump
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
    
    //list of objects that the player is touching that are floors
    List<GameObject> floorContacts = new List<GameObject>();
    
    private bool IsTouchingFloor()
    {
        return floorContacts.Count > 0;
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        //we consider an object a floor if the normal is mostly up
        //the normal vector is the vector perpendicular to the surface of the object
        //it points out from the object
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
        return 3;
    }

    public int GetMinSize()
    {
        return 1;
    }

    public int size { get; set; }

}