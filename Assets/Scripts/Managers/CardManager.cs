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

    public void ProcessStartTurn(int ID)
    {
        List<CardController> cards = new List<CardController>();

        cards.AddRange(p1HandCards);
        cards.AddRange(p2HandCards);
        /*  
          if (p1HandCards.Count <= 5)        
              AddToHand(p1Hand, 0);     

          if (p2HandCards.Count <= 5)        
              AddToHand(p2Hand, 1);  */
    }

    public void ProcessStartLayers()  //Compare cards on Layers 1, 2 and 3, take tactic difference into account, create structure for abilities
    {
        //tactic comparison       
        int p1Layer1Power = 0;
        int p2Layer1Power = 0;
        int l1Difference = 0;
        GetP1TacticPositions(p1Tactic, TacticManager.instance.p1Tactics);
        foreach (GameObject position in layer1)
        {
            p1Layer1Power += AddPositionPower(position, p1Cards);
        }
        Debug.Log(layer1);

        layer1.Clear();
        GetP2TacticPositions(p2Tactic, TacticManager.instance.p2Tactics);
        foreach (GameObject position in layer1)
        {
            p2Layer1Power += AddPositionPower(position, p2Cards);
        }
        Debug.Log(layer1);

        layer1.Clear();

        l1Difference = p1Layer1Power - p2Layer1Power;
        Debug.Log(p1Layer1Power);
        Debug.Log(p2Layer1Power);
        Debug.Log(l1Difference);


        int p1TacticPower = AddTacticPower(p1Tactic, TacticManager.instance.p1Tactics);
        int p2TacticPower = AddTacticPower(p2Tactic, TacticManager.instance.p2Tactics);
        int tacticDifference = p1TacticPower - p2TacticPower;



        /*l1Difference = CompareLayers(p1L1, p2L1) + CompareLayers(p1L2, p2L2) + CompareLayers(p1L3, p2L3);
        if (l1Difference <= 0)
            Debug.Log("Defense stopped attacl");
        else
        {
            Debug.Log("Attack moves forward");
            l2Difference = CompareLayers(p1Ath1, p2Ath1) + CompareLayers(p1DM, p2DM) + CompareLayers(p1Flex, p2Flex);
            if (l2Difference <= 0)
                Debug.Log("Defense stopped attacl");
            else
            {
                Debug.Log("Attack moves forward");
                l3Difference = CompareLayers(p1Ath2, p2Ath2) + CompareLayers(p1Ath2, p2Ath2);
                if (l3Difference <= 0)
                    Debug.Log("Defense stopped attacl");
                else
                {
                    Debug.Log("Attack Won");
                }
            }
        }*/
    }
    
    public string CheckPosition(GameObject position)//Get name of cards in positions
    {
        string childName = "null";

        foreach (Transform child in position.transform)
        {
            childName = child.name;
        }

        return childName;
    }

    private int AddPositionPower(GameObject position, List<CardController> playerCards)// Get power from cards
    {
        int power = 0;
        foreach (CardController card in playerCards)
        {
            if (card.name.ToString() == CheckPosition(position)){
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
            if (tactic.name.ToString() == CheckPosition(position))
            {
                string number = tactic.tacticPower.text;
                power = int.Parse(number);
                return power;
            }
        }
        return power;
    }

    private int CompareLayers(GameObject layer1, GameObject layer2)//check layers difference
    {
        int player1Power = 0;
        player1Power = AddPositionPower(layer1, p1Cards);
        if (layer1 == null) player1Power = 0;
        int player2Power = 0;
        player2Power = AddPositionPower(layer2, p2Cards);
        if (layer1 == null) player2Power = 0;


        int difference = player1Power - player2Power; // missing tactic impact
        return difference;
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
        foreach (TacticController tactic in tactics)
        {
            if (tactic.name == CheckPosition(position))
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
    }
    public void GetP2TacticPositions(GameObject position, List<TacticController> tactics)
    {
        foreach (TacticController tactic in tactics)
        {
            if (tactic.name == CheckPosition(position))
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
}
