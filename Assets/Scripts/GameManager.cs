using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    /*public BoxCollider2D box;
    public SpriteRenderer spr;
    bool jumpFlag = false;*/
    public List<OnOffBrock> brocks;
    public static GameManager instance;
    void Start()
    {
        instance = this;
        ON();
    }

    // Update is called once per frame
    void Update()
    {
        /*if (Input.GetKey(KeyCode.RightArrow))
        {
            if (jumpFlag) return;
            box.enabled=false;
            spr.color = new Color(255, 255, 255, 0);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            if (jumpFlag) return;
            box.enabled = true;
            spr.color = new Color(255, 255, 255, 255);
        }*/
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public void ON()
    {
        foreach (var brock in brocks)
        {
            if (brock.on)
            {
                brock.box.enabled = true;
                brock.spr.color= new Color(255, 255, 0, 255);
                Debug.Log("オン起動中");
            }
            else
            {
                brock.box.enabled = false;
                brock.spr.color = new Color(0, 0, 0, 0.4f);
               
            }
            brock.OnMove();
        }
        
    }
    public void OFF()
    {
        foreach (var brock in brocks)
        {
            if (brock.on)
            {
                
                brock.box.enabled = false;
                brock.spr.color = new Color(0, 0, 0, 0.4f);
                 Debug.Log("オフ起動中");
                
            }
            else
            {
                brock.box.enabled = true;
                brock.spr.color = new Color(255, 255, 0, 255);
            }
            brock.OffMove();
        }
    }

    public void ChangeOnOff(bool on)
    {
        foreach (var brock in brocks)
        {
            brock.on = !brock.on;
        }
        if (on) ON();
        else OFF();
    }
}
