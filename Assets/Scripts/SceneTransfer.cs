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
    public void Load(bool newLevel=false)
    {
        Debug.Log("loading scene");
        StartCoroutine(LoadScene(newLevel));
    }
    IEnumerator LoadScene(bool newLevel)
    {
        anim.SetTrigger("Fade");
        yield return new WaitForSecondsRealtime(1f);
        SceneManager.LoadSceneAsync(targetSceneName).completed += _ =>
        {
            if (!newLevel)
                FindObjectOfType<PlayerScript>().TeleportToCheckpoint();
        };
    }
}
