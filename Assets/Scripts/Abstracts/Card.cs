using Unity.VisualScripting;
using UnityEngine;
[System.Serializable]
public class Card
{
    public string cardName, cardPosition;
    public int cardPower, energyCost, ownerID;
    public Sprite cardSprite;
    public bool isPlayed;
    public Card(Card card)
    {
        cardName = card.cardName;
        energyCost = card.energyCost;
        cardPosition = card.cardPosition;
        cardPower = card.cardPower;
        cardSprite = card.cardSprite;
        isPlayed = card.isPlayed;
    }
}
