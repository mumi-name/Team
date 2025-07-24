using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneChangeScript : MonoBehaviour
{
   public string sceneName="";
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Title"))
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}
