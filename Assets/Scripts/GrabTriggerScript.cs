using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabTriggerScript : MonoBehaviour
{
    [SerializeField] public List<GameObject> cratesTouched;

    // Start is called before the first frame update
    void Start()
    {
        cratesTouched = new List<GameObject>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<Crate>() != null)
        {
            cratesTouched.Add(other.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<Crate>() != null)
        {
            cratesTouched.Remove(other.gameObject);
        }             
    }
}
