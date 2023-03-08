using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class CardManager : MonoBehaviour
{
    public static CardManager instance;
    public List<Card>
        cards = new List<Card>();
    public List<int>
        p1Deck = new List<int>(),
        p2Deck = new List<int>();
    public Transform
        p1Hand,
        p2Hand;
    public CardController
        cardPrefab;
    public List<CardController>
        p1Cards = new List<CardController>(),
        p1HandCards = new List<CardController>(),

        p2Cards = new List<CardController>(),
        p2HandCards = new List<CardController>();
    public List<GameObject>
        layer1 = new List<GameObject>(),
        layer2 = new List<GameObject>(),
        layer3 = new List<GameObject>();
    public GameObject
        p1L1, p1L2, p1L3, p1Ath1, p1Ath2, p1Flex, p1DM, p1Tactic,
         p2L1, p2L2, p2L3, p2Ath1, p2Ath2, p2Flex, p2DM, p2Tactic;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        GenerateCards();
    }
    private void GenerateCards()
    {
        foreach (Card card in cards) if (card.ownerID == 0)
            {
                CardController newCard = Instantiate(cardPrefab, p1Hand);
                newCard.transform.localPosition = Vector3.zero;
                newCard.Initialize(card, 0);
                p1HandCards.Add(newCard);
            }
        foreach (Card card in cards) if (card.ownerID == 1)
            {
                CardController newCard = Instantiate(cardPrefab, p2Hand);
                newCard.transform.localPosition = Vector3.zero;
                newCard.Initialize(card, 1);
                p2HandCards.Add(newCard);
            }
    }
    public void PlayCard(CardController card, int ID)
    {
        if (ID == 0)
        {
            p1Cards.Add(card);
            p1HandCards.Remove(card);
        }
        else
        {
            p2Cards.Add(card);
            p2HandCards.Remove(card);
        }
    }
    public void ReturnToHand(CardController card, int ID)
    {
        if (ID == 0)
        {
            p1Cards.Remove(card);
            p1HandCards.Add(card);
        }
        else
        {
            p2Cards.Remove(card);
            p2HandCards.Add(card);
        }
    }

    public void ProcessStartTurn(int ID)
    {
        List<CardController> cards = new List<CardController>();

        cards.AddRange(p1HandCards);
        cards.AddRange(p2HandCards);
    }

    public void ProcessStartLayers()  //Compare cards on Layers 1, 2 and 3, take tactic difference into account, create structure for abilities
    {
        if (LayerDifference(layer1) > 0)
        {
            
            if (LayerDifference(layer2) > 0)
            {
                Debug.Log("offense wins");
                if (LayerDifference(layer3) > 0)
                {
                    Debug.Log("offense wins");
                }
                else
                    Yardage(layer3);
            }
            else
                Yardage(layer2);
        }
        else
            Yardage(layer1);
    }
    private int LayerDifference(List<GameObject> layer)
    {
        int p1LayPow = 0;
        int p2LayPow = 0;
        int layerDiff;
        layer.Clear();


        GetP1TacticPositions(p1Tactic, TacticManager.instance.p1Tactics);
        foreach (GameObject position in layer)
        {
            p1LayPow += AddPositionPower(position, p1Cards);
        }
        Debug.Log(layer);
        layer.Clear();

        p1LayPow += AddTacticPower(p1Tactic, TacticManager.instance.p1Tactics);


        GetP2TacticPositions(p2Tactic, TacticManager.instance.p2Tactics);
        foreach (GameObject position in layer)
        {
            p2LayPow += AddPositionPower(position, p2Cards);
        }
        Debug.Log(layer);
        layer.Clear();

        p2LayPow += AddTacticPower(p2Tactic, TacticManager.instance.p2Tactics);

        layerDiff = 0;
        layerDiff = p1LayPow - p2LayPow;
        Debug.Log("p1 " + p1LayPow);
        Debug.Log("p2 " + p2LayPow);
        Debug.Log("ldif " + layerDiff);
        Debug.Log(TacticManager.instance.tactics);
        Debug.Log("------------");
        return layerDiff;
    }

    private void Yardage(List<GameObject> layer)
    {
        if (layer == layer1)
        {
            if (LayerDifference(layer1) >= -4 && LayerDifference(layer1) <= 0)
            {
                Debug.Log("No gain");
                //display no gain message and update down
            }
            else if (LayerDifference(layer1) >= -8 && LayerDifference(layer1) <= -5)
            {
                PlayerManager.instance.AdvanceYards(1, 5);
            }
            else if (LayerDifference(layer1) <= -9)
            {
                //if running
                
                Fumble();
                //if passing
                Interception();
            }
        }
        if (layer == layer2)
        {
            if (LayerDifference(layer1) >= -4 && LayerDifference(layer1) <= 0)
            {
                if (LayerDifference(layer1) >= -1 && LayerDifference(layer1) <= 0)
                    Debug.Log("Incomplete if passing play");
                //display message and update down
                else
                    Debug.Log("Short gain 1-3");
            }
            else if (LayerDifference(layer1) >= -8 && LayerDifference(layer1) <= -5)
            {
                Debug.Log("Incomplete or short gain 1");
            }
            else if (LayerDifference(layer1) <= -10)
            {
                Debug.Log("Interception or fumble");
            }
        }
        if (layer == layer3)
        {
            if (LayerDifference(layer1) >= -5 && LayerDifference(layer1) <= 0)
            {
                Debug.Log("Difference layer 2 + Difference layer1");
            }
            else if (LayerDifference(layer1) >= -10 && LayerDifference(layer1) <= -6)
            {
                Debug.Log("difference layer 2 - short gain 3-6 ");
            }
            else if (LayerDifference(layer1) >= -19 && LayerDifference(layer1) <= -11)
            {
                Debug.Log("difference layer 2 - medium gain 6-15");
            }
            else if (LayerDifference(layer1) <= -20)
            {
                Debug.Log("Interception or fumble");
            }
        }

    }

    private void Fumble()
    {
        TurnManager.instance.SwitchPossesion();
        Debug.Log("display fumble message");
    }
    private void Interception()
    {
        TurnManager.instance.SwitchPossesion();
        Debug.Log("display Interception message");
    }

    private int AddPositionPower(GameObject position, List<CardController> playerCards)// Get power from cards
    {
        int power = 0;
        foreach (CardController card in playerCards)
        {
            if (card.transform.parent.name == position.name)
            {
                string pNumber = card.cardPower.text;
                power = int.Parse(pNumber);
                string stamina = card.stamina.text;
                if (stamina == "0") power = 0;
                if (position == null) power = 0;
                return power;
            }

        }
        return power;
    }

    private int AddTacticPower(GameObject position, List<TacticController> tactics)// Get power from cards
    {
        int power = 0;
        foreach (TacticController tactic in tactics)
        {
            if (tactic.transform.parent.name == position.name)
            {
                string number = tactic.tacticPower.text;
                power = int.Parse(number);
                return power;
            }
        }
        return power;
    }
    private void AddToHand(Transform playerHand, int ID)
    {
        int RandomCard = UnityEngine.Random.Range(0, this.cards.Count);
        CardController newCard = Instantiate(cardPrefab, playerHand);
        newCard.transform.localPosition = Vector3.zero;
        newCard.Initialize(this.cards[RandomCard], ID);
        if (ID == 0)
            p1HandCards.Add(newCard);
        else
            p2HandCards.Add(newCard);
    }


    public void ProcessEndTurn()
    {
        if (p1L1.transform.childCount > 0 && p1L2.transform.childCount > 0 && p1L3.transform.childCount > 0
            && p1Ath1.transform.childCount > 0 && p1Ath2.transform.childCount > 0
            && p1Flex.transform.childCount > 0
            && p1DM.transform.childCount > 0)
        {
            if (p2L1.transform.childCount > 0 && p2L2.transform.childCount > 0 && p2L3.transform.childCount > 0
            && p2Ath1.transform.childCount > 0 && p2Ath2.transform.childCount > 0
            && p2Flex.transform.childCount > 0
            && p2DM.transform.childCount > 0)
            {

            }
        }
    }
    public void GetP1TacticPositions(GameObject position, List<TacticController> tactics)
    {
        layer1.Clear();
        layer2.Clear();
        layer3.Clear();

        foreach (TacticController tactic in tactics)
        {
            string layer = tactic.layerID.text; //change layerID for name as it will be clearer to read the switch

            switch (layer)
            {
                case "1":
                    layer1.Add(p1L1);
                    layer1.Add(p1L2);
                    layer1.Add(p1L3);
                    layer1.Add(p1Ath2);
                    layer1.Add(p1DM);

                    layer2.Add(p1Ath1);

                    layer3.Add(p1Flex);
                    break;

                case "2":
                    layer1.Add(p1L1);
                    layer1.Add(p1L2);
                    layer1.Add(p1L3);
                    layer3.Add(p1Ath1);
                    layer2.Add(p1Ath2);
                    layer2.Add(p1Flex);
                    layer3.Add(p1DM);
                    break;

                case "3":
                    layer1.Add(p1L1);
                    layer1.Add(p1L2);
                    layer1.Add(p1L3);
                    layer1.Add(p1Ath1);
                    layer2.Add(p1Ath2);
                    layer2.Add(p1Flex);
                    layer3.Add(p1DM);
                    break;

                case "4":
                    layer2.Add(p1L1);
                    layer1.Add(p1L2);
                    layer1.Add(p1L3);
                    layer2.Add(p1Ath1);
                    layer3.Add(p1Ath2);
                    layer2.Add(p1Flex);
                    layer3.Add(p1DM);
                    break;

                case "5":
                    layer1.Add(p1L1);
                    layer1.Add(p1L2);
                    layer1.Add(p1L3);
                    layer2.Add(p1Ath1);
                    layer3.Add(p1Ath2);
                    layer3.Add(p1Flex);
                    layer1.Add(p1DM);
                    break;

                case "6":
                    layer1.Add(p1L1);
                    layer1.Add(p1L2);
                    layer1.Add(p1L3);
                    layer2.Add(p1Ath1);
                    layer1.Add(p1Ath2);
                    layer2.Add(p1Flex);
                    layer3.Add(p1DM);
                    break;

                case "7":
                    layer1.Add(p1L1);
                    layer1.Add(p1L2);
                    layer1.Add(p1L3);
                    layer2.Add(p1Ath1);
                    layer3.Add(p1Ath2);
                    layer2.Add(p1Flex);
                    layer1.Add(p1DM);
                    break;

                case "8":
                    layer1.Add(p1L1);
                    layer1.Add(p1L2);
                    layer1.Add(p1L3);
                    layer2.Add(p1Ath1);
                    layer2.Add(p1Ath2);
                    layer2.Add(p1Flex);
                    layer2.Add(p1DM);
                    break;

                default:
                    layer1.Add(p1L1);
                    layer1.Add(p1L2);
                    layer1.Add(p1L3);
                    layer3.Add(p1Ath1);
                    layer2.Add(p1Ath2);
                    layer2.Add(p1Flex);
                    layer2.Add(p1DM);
                    break;
            }
        }
    }
    public void GetP2TacticPositions(GameObject position, List<TacticController> tactics)
    {
        layer1.Clear();
        layer2.Clear();
        layer3.Clear();

        foreach (TacticController tactic in tactics)
        {
            string layer = tactic.layerID.text; //change layerID for name as it will be clearer to read the switch

            switch (layer)
            {
                case "1":
                    layer1.Add(p2L1);
                    layer1.Add(p2L2);
                    layer1.Add(p2L3);
                    layer1.Add(p2Ath2);
                    layer1.Add(p2DM);

                    layer2.Add(p2Ath1);

                    layer3.Add(p2Flex);
                    break;

                case "2":
                    layer1.Add(p2L1);
                    layer1.Add(p2L2);
                    layer1.Add(p2L3);
                    layer3.Add(p2Ath1);
                    layer2.Add(p2Ath2);
                    layer2.Add(p2Flex);
                    layer3.Add(p2DM);
                    break;

                case "3":
                    layer1.Add(p2L1);
                    layer1.Add(p2L2);
                    layer1.Add(p2L3);
                    layer1.Add(p2Ath1);
                    layer2.Add(p2Ath2);
                    layer2.Add(p2Flex);
                    layer3.Add(p2DM);
                    break;

                case "4":
                    layer2.Add(p2L1);
                    layer1.Add(p2L2);
                    layer1.Add(p2L3);
                    layer2.Add(p2Ath1);
                    layer3.Add(p2Ath2);
                    layer2.Add(p2Flex);
                    layer3.Add(p2DM);
                    break;

                case "5":
                    layer1.Add(p2L1);
                    layer1.Add(p2L2);
                    layer1.Add(p2L3);
                    layer2.Add(p2Ath1);
                    layer3.Add(p2Ath2);
                    layer3.Add(p2Flex);
                    layer1.Add(p2DM);
                    break;

                case "6":
                    layer1.Add(p2L1);
                    layer1.Add(p2L2);
                    layer1.Add(p2L3);
                    layer2.Add(p2Ath1);
                    layer1.Add(p2Ath2);
                    layer2.Add(p2Flex);
                    layer3.Add(p2DM);
                    break;

                case "7":
                    layer1.Add(p2L1);
                    layer1.Add(p2L2);
                    layer1.Add(p2L3);
                    layer2.Add(p2Ath1);
                    layer3.Add(p2Ath2);
                    layer2.Add(p2Flex);
                    layer1.Add(p2DM);
                    break;

                case "8":
                    layer1.Add(p2L1);
                    layer1.Add(p2L2);
                    layer1.Add(p2L3);
                    layer2.Add(p2Ath1);
                    layer2.Add(p2Ath2);
                    layer2.Add(p2Flex);
                    layer2.Add(p2DM);
                    break;

                default:
                    layer1.Add(p2L1);
                    layer1.Add(p2L2);
                    layer1.Add(p2L3);
                    layer3.Add(p2Ath1);
                    layer2.Add(p2Ath2);
                    layer2.Add(p2Flex);
                    layer2.Add(p2DM);
                    break;
            }
        }
    }
}