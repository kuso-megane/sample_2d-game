using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static int gameState_static;

    //this.gameStateの種類
    public static int state_playing = 0;
    public static int state_clear = 1;
    public static int state_over = 2;
    public static int state_end = 3;


    public GameObject mainImage;
    public Sprite gameOverSprite;
    public Sprite gameClearSprite;
    public GameObject panel;
    public GameObject restartButton;
    public GameObject nextButton;
    
    private Image titleImage;

    // Start is called before the first frame update
    void Start()
    {
        this.Invoke("InactiveImage", 1.0f);
        this.panel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
       
    }


    void InactiveImage()
    {
        this.mainImage.SetActive(false);
    }
}
