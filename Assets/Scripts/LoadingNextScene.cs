using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingNextScene : MonoBehaviour
{
    //씬 비동기 로딩...
    //로딩 진행상황 표시(슬라이더)

    public int sceneNumber = 2;
    public Slider loadingBar;
    IEnumerator TransitionNextScene(int num)
    {
        AsyncOperation ao = SceneManager.LoadSceneAsync(num); //씬 비동기 로드
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
