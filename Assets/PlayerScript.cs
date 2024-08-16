using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [SerializeField] GameObject playerObj;
    [SerializeField] float moveSpeed;
    [SerializeField] float grabRange;

    private bool isGrabbing;    
    private List<Transform> crateTransforms = new List<Transform>();

    // Start is called before the first frame update
    void Start()
    {
        isGrabbing = false;
        GameObject[] gameObjs = GameObject.FindObjectsOfType<GameObject>();        

        foreach(GameObject gameObj in gameObjs)
        {
            if(gameObj.name.Contains("Crate"))
            {
                crateTransforms.Add(gameObj.transform);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {        
        Vector3 newPosition = playerObj.transform.position;

        // move player right
        if (Input.GetKey(KeyCode.D))
        {
            newPosition.x += (moveSpeed * Time.deltaTime);
            playerObj.transform.position = newPosition;
        }
        // move player left
        else if(Input.GetKey(KeyCode.A))
        {
            newPosition.x -= (moveSpeed * Time.deltaTime);
            playerObj.transform.position = newPosition;
        }
        
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
    }

    private Transform GetGrabbableCrate()
    {
        foreach(Transform crateTransform in crateTransforms)
        {
            if (CrateInGrabRange(playerObj.transform, crateTransform))
                return crateTransform;
        }
        Debug.Log("Issue finding grabbable crates");
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
}