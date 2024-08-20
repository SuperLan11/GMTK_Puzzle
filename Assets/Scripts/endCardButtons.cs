using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class endCardButtons : MonoBehaviour
{
    // Start is called before the first frame update
    public Animator fade;

    public void replay() {
        fade.Play("FadeOut");
        StartCoroutine(delayedLoad(2));
    }

    public void returnToMain(){
        fade.Play("FadeOut");
        StartCoroutine(delayedLoad(0));
}

    IEnumerator delayedLoad(int scene) {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(scene);
    }
}
