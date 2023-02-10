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

        cards.AddRange(player1Cards);
        cards.AddRange(player2Cards);
        

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
      
       
    }
    private int CheckPosition(string type)//Checks layer of all players
    {
        string cardName = "k";
        foreach (Transform child in player1PlayLeft.transform)
        {
            cardName = child.name;
        }

        if (cardName == type)
        {
            int power = int.Parse(UIManager.instance.player1LinePower.text);
            return power;
        }
        else
        {
            return 0;
        }
    }

    private int AddPositionPower(GameObject position)
    {
        string cardName;
        foreach (Transform child in position.transform)
        {           
            cardName = child.name;
        }

        CheckPosition( "Line");
        CheckPosition( "Line");

        CheckPosition( "Ath");
        CheckPosition( "Ath");

        CheckPosition("Dm");
        CheckPosition("Dm");
        return 0;
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
                CompareCards();
            }
        }
    }

    private void CompareCards()
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
    }
}
