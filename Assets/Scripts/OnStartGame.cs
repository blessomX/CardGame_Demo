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
    }
}
