using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingNextScene : MonoBehaviour
{
    //�� �񵿱� �ε�...
    //�ε� �����Ȳ ǥ��(�����̴�)

    public int sceneNumber = 2;
    public Slider loadingBar;
    IEnumerator TransitionNextScene(int num)
    {
        AsyncOperation ao = SceneManager.LoadSceneAsync(num); //�� �񵿱� �ε�
        ao.allowSceneActivation = false;

        while (!ao.isDone)
        {
            loadingBar.value = ao.progress;

            if(ao.progress >= 0.9f)
            {
                ao.allowSceneActivation = true;
            }
            yield return null;
        }
    }
    void Start()
    {
        StartCoroutine(TransitionNextScene(sceneNumber));
    }

}
