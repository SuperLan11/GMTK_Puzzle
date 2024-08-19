using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rat : MonoBehaviour
{
    [SerializeField] float speed;    
    private bool facingRight;
    
    // Start is called before the first frame update
    void Start()
    {
        facingRight = true;                
    }

    // Update is called once per frame
    void Update()
    {        
        // localScale.x is inverted by RatWallTrigger.cs
        if(transform.localScale.x > 0)
        {
            facingRight = true;
        }    
        else
        {
            facingRight = false;
        }
           
        if(facingRight)
        {            
            GetComponent<Rigidbody2D>().velocity = new Vector2(speed, GetComponent<Rigidbody2D>().velocity.y);
        }
        else
        {            
            GetComponent<Rigidbody2D>().velocity = new Vector2(-speed, GetComponent<Rigidbody2D>().velocity.y);
        }        
    }   
}
