using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    private Vector3 offsetToAdd;
    private Vector3 oldPlayerPosition;
    // Start is called before the first frame update
    void Start()
    {
        Vector3 playerPosition = GameObject.Find("Player").transform.position;
        oldPlayerPosition = playerPosition;

        offsetToAdd = new Vector3(0, 0, 0);        
    }

    // called after player moves (I think)
    public void Update()
    {                
        Vector3 newPlayerPosition = FindObjectOfType<PlayerScript>().transform.position;
        
        // adding because player is in negative position?
        float xDistMoved = newPlayerPosition.x - oldPlayerPosition.x;
        float yDistMoved = newPlayerPosition.y - oldPlayerPosition.y;    

        // background moves in direction of player, but smaller amount for parallax
        offsetToAdd.x = (0.8f * xDistMoved);
        offsetToAdd.y = (0.8f * yDistMoved);
        offsetToAdd.z = 0;
        
        transform.position += offsetToAdd;        

        oldPlayerPosition = newPlayerPosition;
        
        // later: make background unable to offset if edge of camera reaches edge of background
    }
}
