using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [SerializeField] GameObject playerObj;
    [SerializeField] float moveSpeed;
    [SerializeField] float grabRange;
    [SerializeField] GameObject grabTrigger;
    [SerializeField] bool isTriggerVisible;
    private Animator animator;

    private bool facingRight;
    private bool isGrabbing;    
    private List<Transform> crateTransforms = new List<Transform>();

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

        facingRight = true;
        isGrabbing = false;
        GetCrates();
        
        Renderer grabTriggerRenderer = grabTrigger.GetComponent<Renderer>();
        // make grabTriggerRenderer invisible
        if (!isTriggerVisible)            
            grabTriggerRenderer.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {        
        Vector3 newPosition = playerObj.transform.position;
        Vector3 newTriggerPosition = grabTrigger.transform.position;

        Renderer playerRenderer = GetComponent<Renderer>();      

        // move player right
        if (Input.GetKey(KeyCode.D))
        {
            newPosition.x += (moveSpeed * Time.deltaTime);            
            playerObj.transform.position = newPosition;

            newTriggerPosition.x += (moveSpeed * Time.deltaTime);
            // flip player and grab trigger when turning
            if (!facingRight)            
                playerObj.transform.eulerAngles = new Vector3(0, 0, 0);                     
            facingRight = true;

            animator.SetBool("isMoving", true);
        }
        // move player left
        else if(Input.GetKey(KeyCode.A))
        {
            newPosition.x -= (moveSpeed * Time.deltaTime);
            playerObj.transform.position = newPosition;

            newTriggerPosition.x -= (moveSpeed * Time.deltaTime);
            if (facingRight)            
                playerObj.transform.eulerAngles = new Vector3(0, 180, 0);             
            facingRight = false;

            animator.SetBool("isMoving", true);
        }
        else
        {
            animator.SetBool("isMoving", false);
        }

        // when new levels are added, call GetCrates here on entering new level 

        // gets a crate in grab range, if any
        // does not account for multiple crates within range
        Transform grabbableCrate = GetGrabbableCrate();

        // flip grab toggle
        if (Input.GetKeyDown(KeyCode.Space) && grabbableCrate != null)
        {
            isGrabbing = !isGrabbing;

            if (isGrabbing)
                grabbableCrate.SetParent(playerObj.transform);
            else
                // unparents crate from player
                grabbableCrate.SetParent(null);
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
        // get rid of transform list later
        foreach(Transform crateTransform in crateTransforms)
        {            
            if (GrabTriggerScript.crateGrabbable)
                return crateTransform;
        }
        //Debug.Log("No crates in range");
        return null;
    }
    

    private List<Transform> GetCrates()
    {
        GameObject[] gameObjs = GameObject.FindObjectsOfType<GameObject>();
        crateTransforms.Clear();
        foreach (GameObject gameObj in gameObjs)
        {
            if(gameObj.name.Contains("Crate"))
            {
                crateTransforms.Add(gameObj.transform);
            }
        }
        return crateTransforms;
    }
}