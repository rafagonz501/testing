using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using Unity.VisualScripting;
using System;

public class CardController : MonoBehaviour,
    IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public Card
        card;
    public Image
        cardSprite, image;
    public TextMeshProUGUI
        cardName, cardPosition, cardPower, energyCost;
    private Transform
        originalParent;
    public List<int>
        powerList = new List<int>();
    public int
        linePower = 0, athPower = 0, dmPower = 0; 
        
    

    public void Initialize(Card card, int ownerID)
    {
        this.card = new Card(card)
        {
            ownerID = ownerID
        };
        cardSprite.sprite = card.cardSprite;
        cardName.text = card.cardName;
        cardPosition.text = card.cardPosition;
        cardPower.text = card.cardPower.ToString();
        energyCost.text = card.energyCost.ToString();
        originalParent = transform.parent;
    }
    private void Awake()
    {
        image = GetComponent<Image>();
        powerList.Add(0);
        powerList.Add(0);
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (originalParent.name == $"Player{card.ownerID + 1}Play") /*|| 
            TurnManager.instance.currentPlayerTurn != card.ownerID)*/
        {

        }
        else
        {
            transform.SetParent(transform.root);
            image.raycastTarget = false;
        }

    }
    public void OnPointerUp(PointerEventData eventData)
    {
        if (originalParent.name == $"Player{card.ownerID + 1}Play") /*|
            TurnManager.instance.currentPlayerTurn != card.ownerID)*/
        {

        }
        else
        {
            image.raycastTarget = true;
            AnalyzePointerUp(eventData);
        }
    }
    private void AnalyzePointerUp(PointerEventData eventData)
    {
        //---LEFT/L---//        
        if (eventData.pointerEnter != null &&
            eventData.pointerEnter.name == $"Player{card.ownerID + 1}PlayL" &&
            card.cardPosition == "Left")
        {
            PlayCardPosition(eventData);
            PlayerManager.instance.AddPower(card.ownerID, card.cardPower);
            PlayerManager.instance.AddLinePower(card.ownerID, card.cardPower);
            linePower += card.cardPower;
        }

        //---CENTER/ATH---//
        else if (eventData.pointerEnter != null &&
            eventData.pointerEnter.name == $"Player{card.ownerID + 1}PlayC" &&
            card.cardPosition == "Center")
        {
            PlayCardPosition(eventData);
            PlayerManager.instance.AddPower(card.ownerID, card.cardPower);
            PlayerManager.instance.AddAthPower(card.ownerID, card.cardPower);
            athPower += card.cardPower;

        }

        //---RIGHT/DM---//
        else if (eventData.pointerEnter != null &&
            eventData.pointerEnter.name == $"Player{card.ownerID + 1}PlayR" &&
            card.cardPosition == "Right")
        {
            PlayCardPosition(eventData);
            PlayerManager.instance.AddPower(card.ownerID, card.cardPower);
            PlayerManager.instance.AddDmPower(card.ownerID, card.cardPower);
            dmPower += card.cardPower;

        }
        else
            ReturnToHand();

    }
    public void PlayCardPosition(PointerEventData eventD)
    {
        if (PlayerManager.instance.FindPlayerByID(card.ownerID).energy >= card.energyCost &&
            PlayerManager.instance.FindPlayerByID(card.ownerID).playedCard == false)
        {
            PlayCard(eventD.pointerEnter.transform);
            card.isPlayed = true;
            PlayerManager.instance.SpendEnergy(card.ownerID, card.energyCost);            
            PlayerManager.instance.FindPlayerByID(card.ownerID).playedCard = true;
        }
        else
            ReturnToHand();
    }


    private void PlayCard(Transform playArea)
    {
        transform.SetParent(playArea);
        transform.localPosition = Vector3.zero;
        //originalParent = playArea;        
        CardManager.instance.PlayCard(this, card.ownerID);
        PlayerManager.instance.FindPlayerByID(card.ownerID).playedCard = true;       
        if(card.ownerID == 0)
            powerList[0] = PlayerManager.instance.FindPlayerByID(card.ownerID).power;
        else
            powerList[1] = PlayerManager.instance.FindPlayerByID(card.ownerID).power;
    }
    private void ReturnToHand()
    {
        card.isPlayed = false;
        transform.SetParent(originalParent);
        transform.localPosition = Vector3.zero;
        powerList.Remove(PlayerManager.instance.FindPlayerByID(card.ownerID).power);

    }
    public void OnDrag(PointerEventData eventData)
    {
        if (card.isPlayed) PlayerManager.instance.FindPlayerByID(card.ownerID).playedCard = false;
        if (transform.parent == originalParent) return;
        transform.position = eventData.position;
    }

    public void CompareCards()
    {
        if (PlayerManager.instance.FindPlayerByID(0).playedCard && PlayerManager.instance.FindPlayerByID(1).playedCard)
        {
            int player1Power = powerList[0];
            int player2Power = powerList[1];
            Debug.Log($"Player's 1 power is: {player1Power} Player's 2 power is: {player2Power}");
        }
    }

}
