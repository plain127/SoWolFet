using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingARScene : MonoBehaviour
{
    public static string sceneName;
    public Slider loadingBar;
    public Text loadingGuage;

    void Start()
    {
        StartCoroutine("TransitionNextScene");
    }
    IEnumerator TransitionNextScene(string name)
    {
        AsyncOperation ao = SceneManager.LoadSceneAsync(name);
        ao.allowSceneActivation = false;

        while (!ao.isDone)
        {
            loadingBar.value = ao.progress;
            loadingGuage.text = (ao.progress * 100f).ToString() + "%";

            if (ao.progress >= 0.9f)
            {
                if (CharacterARAppearManager.placedObject != null)
                {
                    ao.allowSceneActivation = true;
                }
            }
            yield return null;
        }
    }
}
