using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneChangeScript : MonoBehaviour
{
   public string sceneName="";
    void Start()
    {
        
    }

    
    void Update()
    {
        if (Input.GetButtonDown("Title"))
        {
            if(TimerManager.instance != null)
            {
                TimerManager.instance.AllCountReset();           
            }
                SceneManager.LoadScene(sceneName);
        }
    }
}
