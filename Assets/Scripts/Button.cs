using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    [SerializeField] GameObject doorObj;
    [SerializeField] Sprite pressedButton;
    [SerializeField] float doorSpeed;
    private bool isMoving;
    private float endPositionY;

    Vector3 pressedCollider = new Vector3(22.7f, 1f, 1f);

    // Start is called before the first frame update
    void Start()
    {
        isMoving = false;
        // door moves up its entire length
        endPositionY = doorObj.transform.position.y + GetComponent<Renderer>().bounds.size.y;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("end position: " + endPositionY);
        // change later so it moves up relative to orientation
        if(isMoving && doorObj.transform.position.y < endPositionY)
        {
            Vector3 newPosition = doorObj.transform.position;
            newPosition.y += (Time.deltaTime * doorSpeed);
            doorObj.transform.position = newPosition;
        }
        else
        {
            isMoving = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Crate>() is Crate)
        {
            //Debug.Log("crate collided with button");
            GetComponent<SpriteRenderer>().sprite = pressedButton;
            GetComponent<BoxCollider2D>().size = pressedCollider;
            isMoving = true;
        }
        // play button sfx
        // activate door                
    }    
}
