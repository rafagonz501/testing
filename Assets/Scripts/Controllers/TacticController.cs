using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using Unity.VisualScripting;
using System;

public class TacticController : MonoBehaviour,
    IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public Tactic
        tactic;
    public Image
        tacticSprite, image;
    public TextMeshProUGUI
        tacticName, tacticPower, tacticMultiplier, tacticType,
        tacticVeryStrong, tacticStrong, tacticNeutral, tacticWeak, tacticVeryWeak,
        layerID;
    private Transform
        originalParent;   
    public int
        tPower = 0;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    public void Initialize(Tactic tactic, int ownerID)
    {
        this.tactic = new Tactic(tactic)
        {
            ownerID = ownerID
        };
        tacticSprite.sprite = tactic.tacticSprite;
        tacticName.text = tactic.tacticName;
        tacticPower.text = tactic.tacticPower.ToString();
        tacticMultiplier.text = tactic.tacticMultiplier.ToString();
        tacticType.text = tactic.tacticType.ToString();
        tacticVeryStrong.text = tactic.tacticVeryStrong.ToString();
        tacticStrong.text = tactic.tacticStrong.ToString();
        tacticNeutral.text = tactic.tacticNeutral.ToString();
        tacticWeak.text = tactic.tacticWeak.ToString();
        tacticVeryWeak.text = tactic.tacticVeryWeak.ToString();
        layerID.text = tactic.tacticLayerID.ToString();
        originalParent = transform.parent;
    }  
    public void OnPointerDown(PointerEventData eventData)
    {
        if (originalParent.name != $"P{tactic.ownerID + 1}play")
        {
            transform.SetParent(transform.root);
            image.raycastTarget = false;
            tacticSprite.raycastTarget = false;
        }
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        if (originalParent.name != $"P{tactic.ownerID + 1}platy") 
        {
            image.raycastTarget = true;
            tacticSprite.raycastTarget = true;
            AnalyzeTactics(eventData);
        }
    }
    private void AnalyzeTactics(PointerEventData eventData)
    {
        if (eventData.pointerEnter != null &&
            eventData.pointerEnter.name == $"P{tactic.ownerID + 1}Tactic")
        {
            PlayTacticPosition(eventData);
            PlayerManager.instance.AddPower(tactic.ownerID, tactic.tacticPower);
            PlayerManager.instance.AddTacticPower(tactic.ownerID, tactic.tacticPower);
            tPower += tactic.tacticPower;
        }
       
        else
            ReturnToHand();

    }
    public void PlayTacticPosition(PointerEventData eventD)
    {        
        PlayTactic(eventD.pointerEnter.transform);
        tactic.isPlayed = true;
        PlayerManager.instance.FindPlayerByID(tactic.ownerID).playedTactic = true;
       
    }
    private void PlayTactic(Transform playArea)
    {
        transform.SetParent(playArea);
        transform.localPosition = Vector3.zero;
        TacticManager.instance.PlayTactic(this, tactic.ownerID);                  
    }
    private void ReturnToHand()
    {
        tactic.isPlayed = false;
        transform.SetParent(originalParent);
        transform.localPosition = Vector3.zero;
        TacticManager.instance.ReturnToHand(this, tactic.ownerID);
        PlayerManager.instance.RemovePower(tactic.ownerID, tactic.tacticPower);
    }
    public void OnDrag(PointerEventData eventData)
    {
        if (tactic.isPlayed) PlayerManager.instance.FindPlayerByID(tactic.ownerID).playedTactic = false;
        if (transform.parent == originalParent) return;
        transform.position = eventData.position;
    }
}
