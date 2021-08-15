using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadNextSceneWithTransition : MonoBehaviour
{
    public Animator SceneTransitionAnimator;
    private bool Clicked;

    public void LoadScene()
    {
        if(Clicked)
            return;
        
        
        Clicked = true;
        StartCoroutine(LoadSceneWithTransition(SceneManager.GetActiveScene().buildIndex + 1));
    }
    IEnumerator LoadSceneWithTransition(int sceneIndex)
    {
        yield return new WaitForSeconds(1f);
        SceneTransitionAnimator.SetTrigger("transit");
        yield return new WaitForSeconds(1f);

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);
            yield return null;
        }
    }
}
