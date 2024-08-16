using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [SerializeField] GameObject playerObj;
    [SerializeField] float moveSpeed;
    [SerializeField] float grabRange;
    [SerializeField] GameObject grabTrigger;

    private bool facingRight;
    private bool isGrabbing;    
    private List<Transform> crateTransforms = new List<Transform>();

    // Start is called before the first frame update
    void Start()
    {
        facingRight = true;
        isGrabbing = false;
        GetCrates();

        // make grabTriggerRenderer invisible
        Renderer grabTriggerRenderer = grabTrigger.GetComponent<Renderer>();
        //grabTriggerRenderer.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {        
        // flip grab trigger to player direction...

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
            playerObj.transform.localScale *= 2;
            grabbableCrate.transform.localScale *= 2;
        }
    }

    private Transform GetGrabbableCrate()
    {
        // get rid of transform list later
        foreach(Transform crateTransform in crateTransforms)
        {
            if (CrateInGrabRange(playerObj.transform, crateTransform))
                return crateTransform;
        }
        //Debug.Log("No crates in range");
        return null;
    }

    // measures distance BETWEEN player and crate transform, not from their centers
    // note this currently only measures x, not y distance between
    private bool CrateInGrabRange(Transform playerTransform, Transform crateTransform)
    {
        if(playerTransform == null || crateTransform == null)
        {
            return false;
        }

        float betweenDistance;

        Renderer playerRenderer = GetComponent<Renderer>();
        Renderer crateRenderer = crateTransform.GetComponent<Renderer>();

        if(crateTransform.position.x > playerTransform.position.x)
        {
            float playerRightEdgeX = playerTransform.position.x;
            playerRightEdgeX += (playerRenderer.bounds.size.x/2);

            float crateLeftEdgeX = crateTransform.position.x;
            crateLeftEdgeX -= (crateRenderer.bounds.size.x / 2);

            betweenDistance = (crateLeftEdgeX - playerRightEdgeX);           
        }
        else
        {
            float playerLeftEdgeX = playerTransform.position.x;
            playerLeftEdgeX -= (playerRenderer.bounds.size.x / 2);

            float crateRightEdgeX = crateTransform.position.x;
            crateRightEdgeX -= (crateRenderer.bounds.size.x / 2);

            betweenDistance = (playerLeftEdgeX - crateRightEdgeX);            
        }
        
        //Debug.Log("distance to crate edge: " + betweenDistance);
        if (betweenDistance < grabRange)
            return true;
        return false;
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