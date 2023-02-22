using Unity.VisualScripting;
using UnityEngine;
[System.Serializable]

public class Tactic 
{
    public string tacticName, tacticType, tacticVeryStrong, tacticStrong, tacticNeutral, tacticWeak, tacticVeryWeak;
    public int tacticPower, tacticMultiplier, ownerID,
        tacticLayerID;
    public bool isPlayed;
    public Sprite tacticSprite;
    public Tactic(Tactic tactic)
    {
        tacticName = tactic.tacticName;
        tacticPower = tactic.tacticPower;
        tacticMultiplier = tactic.tacticMultiplier;
        tacticType = tactic.tacticType;
        ownerID = tactic.ownerID;
        tacticVeryStrong = tactic.tacticVeryStrong;
        tacticStrong = tactic.tacticStrong;
        tacticNeutral = tactic.tacticNeutral;
        tacticWeak = tactic.tacticWeak;
        tacticVeryWeak = tactic.tacticVeryWeak;
        tacticLayerID = tactic.tacticLayerID;
        isPlayed = tactic.isPlayed;
        tacticSprite = tactic.tacticSprite;
    }
}

