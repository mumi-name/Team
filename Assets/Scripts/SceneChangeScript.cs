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
                SceneManager.LoadScene(sceneName);
        }
    }
}
