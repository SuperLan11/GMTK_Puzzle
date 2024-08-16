using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [SerializeField] GameObject playerObj;
    [SerializeField] float moveSpeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {        
        Vector3 newPosition = playerObj.transform.position;
        if (Input.GetKey(KeyCode.RightArrow))
        {
            newPosition.x += (moveSpeed * Time.deltaTime);
            playerObj.transform.position = newPosition;
        }
        else if(Input.GetKey(KeyCode.LeftArrow))
        {
            newPosition.x -= (moveSpeed * Time.deltaTime);
            playerObj.transform.position = newPosition;
        }
    }
}
