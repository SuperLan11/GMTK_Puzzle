using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseListener : MonoBehaviour
{
    bool paused = false;
    Animator anim;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            paused = !paused;
            anim.SetTrigger("Fade");

            if (paused)
                Time.timeScale = 0f;
            else
                Time.timeScale = 1f;
        }
    }
}
