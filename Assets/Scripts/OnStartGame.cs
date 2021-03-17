using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class OnStartGame : MonoBehaviour
{
    public GameObject GameLogicObj;
    public GameObject Deck;
    public GameObject Slider, LoadingPanel;
    public GameObject LandScapeBackground;
    public GameObject TextScreen;
    int Count = 0;

    void Start()
    {
        GameLogic.WaitFunc StartGame = delegate ()
        {
            Count++;
            Slider.GetComponent<Slider>().value = Count;
            if (Count == 4)
            {
                LoadingPanel.SetActive(false);
                Deck.GetComponent<DeckObject>().StartGame();
            }
        };
        StartCoroutine(GameLogic.wait(1f, StartGame, 4));
        if(IsTablet())
        {
            this.LandScapeBackground.SetActive(true);
        }
    }

    private void Update()
    {
        TextScreen.GetComponent<Text>().text = "width: " + Screen.width + " height: " + Screen.height + " dpi:" + Screen.dpi;
    }

    public static bool IsTablet()
    {
        
        float ssw;
        if (Screen.width > Screen.height) { ssw = Screen.width; } else { ssw = Screen.height; }

        if (ssw < 800) {
            Screen.autorotateToPortrait = true;
            Screen.autorotateToPortraitUpsideDown = true;
            Screen.autorotateToLandscapeLeft = false;
            Screen.autorotateToLandscapeRight = false;
            Screen.orientation = ScreenOrientation.Portrait;
            Screen.orientation = ScreenOrientation.AutoRotation;
            return false;
        }

        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.WindowsEditor)
        {
            float screenWidth = Screen.width / Screen.dpi;
            float screenHeight = Screen.height / Screen.dpi;
            float size = Mathf.Sqrt(Mathf.Pow(screenWidth, 2) + Mathf.Pow(screenHeight, 2));
            float aspectRatio = Mathf.Max(Screen.width, Screen.height) / Mathf.Min(Screen.width, Screen.height);
            if (size >= 6.5f) // && aspectRatio < 2f
            {
                Screen.autorotateToPortrait = false;
                Screen.autorotateToPortraitUpsideDown = false;
                Screen.autorotateToLandscapeLeft = true;
                Screen.autorotateToLandscapeRight = true;
                Screen.orientation = ScreenOrientation.LandscapeLeft;
                Screen.orientation = ScreenOrientation.AutoRotation;
                return true;
            }
        }
        Screen.autorotateToPortrait = true;
        Screen.autorotateToPortraitUpsideDown = true;
        Screen.autorotateToLandscapeLeft = false;
        Screen.autorotateToLandscapeRight = false;
        Screen.orientation = ScreenOrientation.Portrait;
        Screen.orientation = ScreenOrientation.AutoRotation;
        return false;
    }

}
