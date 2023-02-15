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
        Player1Deck = new List<int>(),
        Player2Deck = new List<int>();
    public Transform
        player1Hand,
        player2Hand;
    public CardController
        cardControllerPrefab;
    public List<CardController>
        player1Cards = new List<CardController>(),
        player2Cards = new List<CardController>(),
        player1HandCards = new List<CardController>(),
        player2HandCards = new List<CardController>();
    public GameObject
        player1PlayLeft, player1PlayCenter, player1PlayRight,
        player2PlayLeft, player2PlayCenter, player2PlayRight;

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
            CardController newCard = Instantiate(cardControllerPrefab, player1Hand);
            newCard.transform.localPosition = Vector3.zero;
            newCard.Initialize(card, 0);
            player1HandCards.Add(newCard);
        }
    foreach (Card card in cards) if (card.ownerID == 1)
        {
            CardController newCard = Instantiate(cardControllerPrefab, player2Hand);
            newCard.transform.localPosition = Vector3.zero;
            newCard.Initialize(card, 1);
            player2HandCards.Add(newCard);                
        }
    }

    public void PlayCard(CardController card, int ID)
    {
        if (ID == 0)
        {
            player1Cards.Add(card);
            player1HandCards.Remove(card);
        }
        else
        {
            player2Cards.Add(card);
            player2HandCards.Remove(card);
        }
    }

    public void ProcessStartTurn(int ID)
    {
        List<CardController> cards = new List<CardController>();

        cards.AddRange(player1HandCards);
        cards.AddRange(player2HandCards);
        

        if (player1HandCards.Count <= 5)
        {
            AddToHand(player1Hand, 0);           
        }
        if (player2HandCards.Count <= 5)
        {
            AddToHand(player2Hand, 1);
        }
    }

    public void ProcessStartLayers()  //Compare cards on Layers 1, 2 and 3, take tactic difference into account, create structure for abilities
    {
        //tactic comparison
        //positions on layers
     

        int l1Difference = CompareLayers(player1PlayLeft, player2PlayLeft);
        if (l1Difference <= 0)        
            Debug.Log("Defense stopped attacl");
        else
            Debug.Log("Attack moves forward");
       

    }
    private string CheckPosition(GameObject position)//Get name of cards in positions
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
                string number = card.cardPower.text;                
                power = int.Parse(number);

                return power;
            }
        }              
        return power;
    }

    private int CompareLayers(GameObject layer1, GameObject layer2)//check layers difference
    {
        int player1Power = 0;
        player1Power = AddPositionPower(layer1, player1Cards);  
        int player2Power = 0;
        player2Power = AddPositionPower(layer2, player2Cards);

        int difference = player1Power - player2Power; // missing tactic impact
        return difference;     
    }

    private void AddToHand(Transform playerHand, int ID)
    {
        int RandomCard = UnityEngine.Random.Range(0, this.cards.Count);
        CardController newCard = Instantiate(cardControllerPrefab, playerHand);
        newCard.transform.localPosition = Vector3.zero;
        newCard.Initialize(this.cards[RandomCard], ID);
        if(ID==0)
            player1HandCards.Add(newCard);
        else
            player2HandCards.Add(newCard);
    }

    public void ProcessEndTurn()
    {     
        if (player1PlayLeft.transform.childCount > 0)
        {
            if (player2PlayLeft.transform.childCount > 0)
            {
                //CompareCards();
            }
        }
    }

    /*private void CompareCards()
    {
        int yardsAdvance = 0;

        if (PlayerManager.instance.FindPlayerByID(0).playedCard &&
            PlayerManager.instance.FindPlayerByID(1).playedCard)
        {
            if (GetComponent<CardController>().powerList[0] >= GetComponent<CardController>().powerList[1])
            {
                yardsAdvance = GetComponent<CardController>().powerList[0] - GetComponent<CardController>().powerList[1];
            }
            else
            {
                yardsAdvance = GetComponent<CardController>().powerList[1] - GetComponent<CardController>().powerList[0];
            }
            Debug.Log($"Mod yards by {yardsAdvance}");
        }
    }*/
}
