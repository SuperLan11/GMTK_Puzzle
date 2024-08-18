using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransfer : MonoBehaviour
{
    [SerializeField] string targetSceneName;
    Animator anim;

    public void Awake()
    {
        anim = GameObject.FindGameObjectWithTag("Fade").GetComponent<Animator>();
    }
    public void Load()
    {
        StartCoroutine(LoadScene());
    }
    IEnumerator LoadScene()
    {
        anim.SetTrigger("Fade");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(targetSceneName);
    }
}
