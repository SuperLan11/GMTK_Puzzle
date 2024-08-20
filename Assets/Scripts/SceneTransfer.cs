using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransfer : MonoBehaviour
{
    [SerializeField] string targetSceneName;
    Animator anim;

    void Awake()
    {
        anim = GameObject.FindGameObjectWithTag("Fade").GetComponent<Animator>();
    }
    public void Load()
    {
        Debug.Log("loading scene");
        StartCoroutine(LoadScene());
    }
    IEnumerator LoadScene()
    {
        anim.SetTrigger("Fade");
        yield return new WaitForSecondsRealtime(1f);
        SceneManager.LoadSceneAsync(targetSceneName).completed += _ => FindObjectOfType<PlayerScript>().TeleportToCheckpoint();
    }
}
