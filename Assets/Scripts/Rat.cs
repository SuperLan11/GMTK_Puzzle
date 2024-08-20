using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rat : SizeObject
{
    [SerializeField] float speed;    
    private bool facingRight;
    
    // Start is called before the first frame update
    void Start()
    {
        size = 1;
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

    public override int GetMaxSize()
    {
        return 1;
    }

    public override int GetMinSize()
    {
        return 1;
    }


    public override void ResizeBy(int sizeDiff)
    {
        throw new System.NotImplementedException();
    }
}
