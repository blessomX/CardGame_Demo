using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class SpriteQueue : MonoBehaviour
{
    public float MinCardDistance;
    public float MaxCardDistance;
    public bool Hidden;
    public bool Selected;
    public float DifferceSizePercentOfAvailableAndNoCards;

    void Update()
    {
        ChildrenCentering();
    }

    void ChildrenCentering()
    {
        var AvailableCards = this.GetComponentsInChildren<SpriteRenderer>().Select(x => x.gameObject).Where(x => x.tag == "Card" && !x.GetComponent<CardObject>().NoAvailable).ToArray();
        var NotAvailableCards = this.GetComponentsInChildren<SpriteRenderer>().Select(x => x.gameObject).Where(x => x.tag == "Card" && x.GetComponent<CardObject>().NoAvailable).ToArray();
        var AllCards = this.GetComponentsInChildren<SpriteRenderer>().Select(x => x.gameObject).Where(x => x.tag == "Card").ToArray();
        var QueueBorders = this.GetComponentsInChildren<SpriteRenderer>().Select(x => x.gameObject).Where(x => x.tag == "QueueBorder").ToArray();

        if (AllCards.Length > 0 && QueueBorders.Length > 1)
        {
            var CardWidth = AllCards[0].transform.localToWorldMatrix.m00 * 2;
            var MaxCalcSize = (CardWidth) * AllCards.Length;
            var SizeQueue = Math.Abs(QueueBorders[0].transform.localPosition.x) + Math.Abs(QueueBorders[1].transform.localPosition.x);
            var AvailablePercent = (float)AvailableCards.Length / (AvailableCards.Length + NotAvailableCards.Length) * 100;
            var DistinctAvailable = Math.Abs(MaxCalcSize * AvailablePercent / 100) - (SizeQueue* AvailablePercent/100);
            var DistinctNoAvailable = Math.Abs(MaxCalcSize - (MaxCalcSize * AvailablePercent / 100)) - (SizeQueue - (SizeQueue * AvailablePercent / 100));

            var MaxCardAvaiableDistanceBuf = 0f;
            var MaxCardNoAvaiableDistanceBuf = 0f;
            if (DistinctAvailable > 0)
            {
                MaxCardAvaiableDistanceBuf = (DistinctAvailable / AvailableCards.Length) * -1f;
            }
            if (DistinctNoAvailable > 0)
            {
                MaxCardNoAvaiableDistanceBuf = (DistinctNoAvailable / NotAvailableCards.Length) * -1f;
            }
            if (AvailableCards.Length > 0 && NotAvailableCards.Length > 0)
            {
                var MaxUpSize = (CardWidth + MaxCardNoAvaiableDistanceBuf) * NotAvailableCards.Length;
                var AvaiblePercentUpValue = ((MaxCardAvaiableDistanceBuf * AvailableCards.Length) * (100 + DifferceSizePercentOfAvailableAndNoCards) / 100);
                MaxCardNoAvaiableDistanceBuf += (AvaiblePercentUpValue - MaxCardAvaiableDistanceBuf * AvailableCards.Length) / NotAvailableCards.Length;
                AvaiblePercentUpValue = ((MaxCardAvaiableDistanceBuf * AvailableCards.Length) * (DifferceSizePercentOfAvailableAndNoCards) / 100);
                MaxCardAvaiableDistanceBuf = AvaiblePercentUpValue / AvailableCards.Length;
                MaxCalcSize = (CardWidth + MaxCardAvaiableDistanceBuf) * AvailableCards.Length;
                DistinctAvailable = Math.Abs(MaxCalcSize) - (SizeQueue * AvailablePercent / 100);
                if (DistinctAvailable > 0)
                {
                    MaxCardAvaiableDistanceBuf = (DistinctAvailable / AvailableCards.Length) * -1f;
                }
            }
            var CenteredCardDistance = (MaxCardAvaiableDistanceBuf * AvailablePercent) / 100 + (MaxCardNoAvaiableDistanceBuf * (100 - AvailablePercent)) / 100;
            var Offset = SizeQueue / 2 * -1 - (SizeQueue / 2 * -1 + ((CardWidth + CenteredCardDistance) * AllCards.Length) / 2);
            var StartX = 0f;
            int i = 0;
            foreach (GameObject Card in AllCards)
            {
                if(!Card.GetComponent<CardObject>().NoAvailable)
                {
                    if (i == 0)
                        StartX = Offset + (CardWidth + MaxCardAvaiableDistanceBuf) / 2;
                    Card.transform.localPosition = new Vector3(StartX, 0, 0);
                    Card.GetComponent<SpriteRenderer>().sortingOrder = i;
                    var Position = new Vector3(Card.transform.position.x, Card.transform.position.y, Card.transform.position.z - 0.1f);
                    Card.transform.position = Position;
                    StartX += (CardWidth + MaxCardAvaiableDistanceBuf);
                }
                else
                {
                    if (i == 0)
                        StartX = Offset + (CardWidth + MaxCardNoAvaiableDistanceBuf) / 2;
                    Card.transform.localPosition = new Vector3(StartX, 0, 0);
                    Card.GetComponent<SpriteRenderer>().sortingOrder = i;
                    var Position = new Vector3(Card.transform.position.x, Card.transform.position.y, Card.transform.position.z);
                    Card.transform.position = Position;
                    StartX += (CardWidth + MaxCardNoAvaiableDistanceBuf);
                }
                i++;
            }
        }
    }
}
