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
        cardName, cardPosition, cardPower, stamina, energyCost;
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
        stamina.text = card.stamina.ToString();
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
        if (originalParent.name != $"Player{card.ownerID + 1}Play")
        {
            transform.SetParent(transform.root);
            image.raycastTarget = false;
        }
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        if (originalParent.name != $"Player{card.ownerID + 1}Play")
        {
            image.raycastTarget = true;
            AnalyzeCards(eventData);
        }
    }
    private void AnalyzeCards(PointerEventData eventData)
    {
        if (card.stamina <= 0) card.cardPower = 0;
        //---L1---//        
        if (eventData.pointerEnter != null &&
            eventData.pointerEnter.name == $"P{card.ownerID + 1}L1" &&
            (card.cardPosition == "Flex" || card.cardPosition == "Line"))
        {
            PlayCardPosition(eventData);
            PlayerManager.instance.AddPower(card.ownerID, card.cardPower);
            PlayerManager.instance.AddLinePower(card.ownerID, card.cardPower);
            linePower += card.cardPower;
        }
        else if (eventData.pointerEnter != null &&
           eventData.pointerEnter.name == $"P{card.ownerID + 1}L2" &&
           (card.cardPosition == "Flex" || card.cardPosition == "Line"))
        {
            PlayCardPosition(eventData);
            PlayerManager.instance.AddPower(card.ownerID, card.cardPower);
            PlayerManager.instance.AddLinePower(card.ownerID, card.cardPower);
            linePower += card.cardPower;
        }
        else if (eventData.pointerEnter != null &&
           eventData.pointerEnter.name == $"P{card.ownerID + 1}L3" &&
           (card.cardPosition == "Flex" ||card.cardPosition == "Line"))
        {
            PlayCardPosition(eventData);
            PlayerManager.instance.AddPower(card.ownerID, card.cardPower);
            PlayerManager.instance.AddLinePower(card.ownerID, card.cardPower);
            linePower += card.cardPower;
        }

        //---ATH---//
        else if (eventData.pointerEnter != null &&
            eventData.pointerEnter.name == $"P{card.ownerID + 1}Ath1" &&
            (card.cardPosition == "Flex" || card.cardPosition == "Ath" ))
        {
            PlayCardPosition(eventData);
            PlayerManager.instance.AddPower(card.ownerID, card.cardPower);
            PlayerManager.instance.AddAthPower(card.ownerID, card.cardPower);
            athPower += card.cardPower;
        }        
        else if (eventData.pointerEnter != null &&
            eventData.pointerEnter.name == $"P{card.ownerID + 1}Ath2" &&
            (card.cardPosition == "Flex" || card.cardPosition == "Ath"))
        {
            PlayCardPosition(eventData);
            PlayerManager.instance.AddPower(card.ownerID, card.cardPower);
            PlayerManager.instance.AddAthPower(card.ownerID, card.cardPower);
            athPower += card.cardPower;
        }
        //---FLEX---//

        else if (eventData.pointerEnter != null &&
            eventData.pointerEnter.name == $"P{card.ownerID + 1}Flex" &&
            (card.cardPosition == "Flex" || card.cardPosition == "Ath" || card.cardPosition == "Line"))
        {
            PlayCardPosition(eventData);
            PlayerManager.instance.AddPower(card.ownerID, card.cardPower);
            PlayerManager.instance.AddAthPower(card.ownerID, card.cardPower);
            athPower += card.cardPower;
        }

        //---DM---//
        else if (eventData.pointerEnter != null &&
            eventData.pointerEnter.name == $"P{card.ownerID + 1}DM" &&
            card.cardPosition == "DM")
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
        if (PlayerManager.instance.FindPlayerByID(card.ownerID).energy >= card.energyCost /*&&
            PlayerManager.instance.FindPlayerByID(card.ownerID).playedCard == false*/)
        {
            PlayCard(eventD.pointerEnter.transform);
            card.isPlayed = true;           
            //PlayerManager.instance.FindPlayerByID(card.ownerID).playedCard = true;
        }
        else
            ReturnToHand();
    }
    private void PlayCard(Transform playArea)
    {
        transform.SetParent(playArea);
        transform.localPosition = Vector3.zero;        
        CardManager.instance.PlayCard(this, card.ownerID);               
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
        if (transform.parent == originalParent) return;
        transform.position = eventData.position;
    }  
}
