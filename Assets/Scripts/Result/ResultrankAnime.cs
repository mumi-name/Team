using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class ResultrankAnime : MonoBehaviour
{
    public TextMeshProUGUI rankText;
    public Image rankImage;

    public Sprite rankS;
    public Sprite rankA;
    public Sprite rankB;
    public Sprite rankC;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnRankAnimation()
    {
        string currentRank = rankText.text;

        switch (currentRank)
        {
            case "S": 
                rankImage.sprite = rankS;
                //Debug.Log("rankImageに" + currentRank + "の画像が入りました！");
                break;
            case "A": 
                rankImage.sprite = rankA;
                //Debug.Log("rankImageに" + currentRank + "の画像が入りました！");
                break;
            case "B": 
                rankImage.sprite = rankB;
                //Debug.Log("rankImageに" + currentRank + "の画像が入りました！");
                break;
            case "C": 
                rankImage.sprite = rankC;
                //Debug.Log("rankImageに" + currentRank + "の画像が入りました！");
                break;
        }
    }
}
