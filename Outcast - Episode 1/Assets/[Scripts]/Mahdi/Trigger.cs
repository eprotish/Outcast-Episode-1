using UnityEngine;
using UnityEngine.SceneManagement;

public class Trigger : MonoBehaviour
{
    
    [SerializeField] private GameObject Controller;
    private string SceneName;

    private void Start()
    {
        SceneName = SceneManager.GetActiveScene().name;
    }

    void OnTriggerEnter2D(Collider2D hit)
    {
        if (hit.gameObject.tag != "Player" || hit.isTrigger)
        {
            return;
        }

        if(SceneName == "Scene 2")
             Controller.GetComponent<Scene2>().CheckTrigger(this.name);

        if(SceneName == "Scene 3-1 SF - Dream")
            Controller.GetComponent<Scene3SFDream>().CheckTrigger(this.name);

        if(SceneName == "Scene 3-1 FF - Dream")
            Controller.GetComponent<Scene3Dream>().CheckTrigger(this.name);

        if(SceneName == "Scene 3 SF")
            Controller.GetComponent<Scene3SF>().CheckTrigger(this.name);
    }

    private void OnTriggerExit2D(Collider2D hit)
    {
        if (hit.gameObject.tag != "Player" || hit.isTrigger)
        {
            return;
        }
        

        if (SceneName == "Scene 3-1 FF - Dream")
            Controller.GetComponent<Scene3Dream>().CheckTriggerExit(this.name);
    }
}
