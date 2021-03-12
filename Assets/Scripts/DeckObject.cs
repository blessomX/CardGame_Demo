using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using MarchingBytes;

public class DeckObject : MonoBehaviour
{
    public List<CardObject> Deck;
    public int DeckSize;
    float MoveSpeed;
    GameObject QueuePosition;

    public void StartGame()
    {
        StartCoroutine(LateStart(0.5f));
        QueuePosition = GameObject.FindGameObjectWithTag("CardsQueue");
    }

    IEnumerator LateStart(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        DeckGenerate();
        StartCoroutine(CardsMove());
    }

    IEnumerator CardsMove()
    {
        foreach (CardObject card in Deck)
        {
            card.gameObject.transform.Translate(Vector3.forward * Time.deltaTime);
            StartCoroutine(GameLogic.SmoothMove(card.gameObject, card.gameObject.transform.position, QueuePosition.transform, 0.7f, QueuePosition.GetComponent<GameLogic>()));
            card.OnTable = false;
            card.Hidden = false;
            yield return GameLogic.wait(0.1f, null);
        }
    }

    System.Random r = new System.Random();
    void DeckGenerate()
    {
        for (int i = 0; i < DeckSize; i++)
        {
            var Lear = GameLogic.Lears[r.Next(0, 4)];
            var Value = r.Next(2, 2);
            var Card = EasyObjectPool.instance.GetObjectFromPool("CardPool", this.transform.position, Quaternion.identity).GetComponent<CardObject>();
            Card.Hidden = true;
            Card.CardLear = Lear;
            Card.CardValue = Value.ToString();
            Card.NoAvailable = Card.Grey;
            Deck.Add(Card);
        }
        QueuePosition.GetComponent<GameLogic>().UpdateTable();
    }
}
