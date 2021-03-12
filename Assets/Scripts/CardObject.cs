using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardObject : MonoBehaviour
{
    public string CardValue;
    public string CardLear;
    public bool Hidden;
    public bool Grey;
    public bool NoAvailable;
    private SpritePool SpritePool;
    public GameLogic gameLogic;
    public GameObject Table;
    public bool OnTable;

    void Start()
    {
        gameLogic = this.GetComponentInParent<GameLogic>();
        SpritePool = GameObject.FindGameObjectWithTag("SpritePool").GetComponent<SpritePool>();
        Table = GameObject.FindGameObjectWithTag("Table");
    }

    void Update()
    {
        if(this.enabled && !OnTable)
            if ((Hidden || gameLogic == null))
            {
                this.GetComponent<SpriteRenderer>().sprite = SpritePool.GetSprite("Back");
            }
            else
            {
                this.GetComponent<SpriteRenderer>().sprite = SpritePool.GetSprite(CardValue + CardLear);
                if (gameLogic.NowLear == CardLear || gameLogic.NowLear == GameLogic.NotDefinedLear)
                {
                    Grey = false;
                    NoAvailable = false;
                }
                else
                {
                    Grey = true;
                    NoAvailable = true;
                }
                if (Grey)
                {
                    this.GetComponent<SpriteRenderer>().color = new Color(0.4f, 0.4f, 0.4f, 1f);
                }
                else
                {
                    this.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
                }
            }
    }

    private void OnMouseDown()
    {
        if(!NoAvailable && gameLogic.State == GameLogic.IdleAwait && !gameLogic.GameStop)
        {
            this.gameObject.transform.SetParent(null);
            OnTable = true;
            gameLogic.State = GameLogic.MovementStart;
            StartCoroutine(GameLogic.SmoothMove(this.gameObject, this.gameObject.transform.position, Table.transform, 0.7f, gameLogic));
        }
    }
}
