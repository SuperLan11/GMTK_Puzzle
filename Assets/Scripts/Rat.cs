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
        Vector3 newPosition = transform.position;
        if(facingRight)
        {
            newPosition.x += (Time.deltaTime * speed);
        }
        else
        {
            newPosition.x -= (Time.deltaTime * speed);
        }
        transform.position = newPosition;
    }
    private void OnCollisionEnter(Collision collision)
    {
        facingRight = !facingRight;
    }

    private void OnTriggerEnter(Collider collision)
    {
        facingRight = !facingRight;
    }
}
