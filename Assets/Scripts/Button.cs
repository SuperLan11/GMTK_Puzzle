using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    [SerializeField] GameObject doorObj;
    [SerializeField] Sprite pressedButton;
    [SerializeField] float doorSpeed;
    private bool doorIsMoving;
    private float endPositionY;
    private bool pressed;

    Vector3 pressedCollider = new Vector3(22.7f, 1f, 1f);

    // Start is called before the first frame update
    void Start()
    {
        doorIsMoving = false;
        pressed = false;
        // door moves up its entire length
        endPositionY = doorObj.transform.position.y + doorObj.GetComponent<Renderer>().bounds.size.y;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("end position: " + endPositionY);
        // change later so it moves up relative to orientation
        if(doorIsMoving && doorObj.transform.position.y < endPositionY)
        {            
            Vector3 newPosition = doorObj.transform.position;
            newPosition.y += (Time.deltaTime * doorSpeed);
            doorObj.transform.position = newPosition;
        }
        else
        {
            doorIsMoving = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("button collision");

        if (pressed)
            return;

        bool isCrate = collision.gameObject.GetComponent<Crate>() is Crate;
        bool isRat = collision.gameObject.GetComponent<Rat>() is Rat;

        if (isCrate || isRat)
        {            
            pressed = true;
            GetComponent<SpriteRenderer>().sprite = pressedButton;
            GetComponent<BoxCollider2D>().size = pressedCollider;
            doorIsMoving = true;            

            GetComponent<AudioSource>().Play();            
        }                    
    }    
}
