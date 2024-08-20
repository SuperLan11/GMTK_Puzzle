using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    public Animator fade;

    public void BeginGame()
    {
        fade.Play("FadeOut");
        StartCoroutine(delayedLoad(2)) ;        
    }

    public void OpenOptions()
    {
        SceneManager.LoadScene("Options");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    IEnumerator delayedLoad(int scene) {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(2);
    }
}
