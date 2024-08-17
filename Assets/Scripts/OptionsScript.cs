using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OptionsScript : MonoBehaviour
{
    public void BackClick()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
