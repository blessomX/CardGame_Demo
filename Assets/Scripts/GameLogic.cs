using MarchingBytes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameLogic : MonoBehaviour
{
    static readonly public int IdleAwait = 0;
    static readonly public int MovementStart = 1;
    static readonly public int MovementEnd = 2;
    static readonly public string[] Lears = { "S", "H", "D", "C"};
    static readonly public string NotDefinedLear = "AllLear";
    public string NowLear;
    public DeckObject Deck;
    public int State = -1;
    int Score = 0;
    public GameObject ScoreText;
    public bool GameStop = false;

    public delegate void WaitFunc();

    public delegate GameLogic GameLogicSetup();

    private void Start()
    {
        Deck = GameObject.FindGameObjectWithTag("Deck").GetComponent<DeckObject>();
    }

    System.Random r = new System.Random();
    public void UpdateTable()
    {
        if(State == -1)
        {
            NowLear = Lears[r.Next(0, 3)];
            State = IdleAwait;
        }
        if (State == MovementEnd)
        {
            foreach (CardObject card in Deck.Deck)
            {
                if(card.OnTable && card.enabled)
                {
                    WaitFunc Remover = delegate ()
                    {
                        EasyObjectPool.instance.ReturnObjectToPool(card.gameObject);
                        NowLear = Lears[r.Next(0, 4)];
                        State = IdleAwait;
                        Score++;
                        ScoreText.GetComponent<Text>().text = Score.ToString();
                    };
                    StartCoroutine(wait(2f, Remover));
                    Deck.Deck.Remove(card);
                    break;
                }
            }
        }
    }

    public static IEnumerator SmoothMove(GameObject Card, Vector3 startPos, Transform endPos, float time, GameLogic gameLogicSetup)
    {
        float currTime = 0;
        do
        {
            Card.transform.position = Vector3.Lerp(startPos, endPos.position, currTime / time);
            currTime += Time.deltaTime;
            yield return null;
        }
        while (currTime <= time);
        Card.gameObject.transform.SetParent(endPos);
        var bufLogic = Card.GetComponent<CardObject>().gameLogic;
        Card.GetComponent<CardObject>().gameLogic = gameLogicSetup;
        if(bufLogic != null)
            Card.GetComponent<CardObject>().gameLogic.State = MovementEnd;
        Card.GetComponent<CardObject>().gameLogic.UpdateTable();
    }

    public static IEnumerator wait(float waitTime, WaitFunc func, int Repeat = 1)
    {
        for(int i = 0; i < Repeat; i++)
        {
            yield return new WaitForSeconds(waitTime);
            if (func != null)
                func();
        }
    }
}
