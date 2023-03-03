using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class TacticManager : MonoBehaviour
{
    public static TacticManager instance;
    public List<Tactic> 
        tactics = new List<Tactic>();
    public List<int>
        p1Deck = new List<int>(),
        p2Deck = new List<int>();
    public Transform
        p1Hand,
        p2Hand;
    public TacticController
        tacticPrefab;
    public List<TacticController>       
        p1Tactics = new List<TacticController>(),
        p1HandTactics = new List<TacticController>(),
        p2Tactics = new List<TacticController>(),
        p2HandTactics = new List<TacticController>();
    public GameObject
       p1Tactic, p2Tactic;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        GenerateTactics();
    }
    private void GenerateTactics()
    {
        foreach (Tactic tactic in tactics) if (tactic.ownerID == 0)
            {
                TacticController newTactic = Instantiate(tacticPrefab, p1Hand);                
                newTactic.transform.localPosition = Vector3.zero;
                newTactic.Initialize(tactic, 0);
                p1HandTactics.Add(newTactic);                
            }
        foreach (Tactic tactic in tactics) if (tactic.ownerID == 1)
            {
                TacticController newTactic = Instantiate(tacticPrefab, p2Hand);
                newTactic.transform.localPosition = Vector3.zero;
                newTactic.Initialize(tactic, 1);
                p2HandTactics.Add(newTactic);
            }
    }
    public void PlayTactic(TacticController tactic, int ID)
    {
        if (ID == 0)
        {
            p1Tactics.Add(tactic);
            p1HandTactics.Remove(tactic);
        }
        else
        {
            p2Tactics.Add(tactic);
            p2HandTactics.Remove(tactic);
        }
    }
    public void ReturnToHand(TacticController tactic, int ID)
    {
        if (ID == 0)
        {
            p1Tactics.Remove(tactic);
            p1HandTactics.Add(tactic);
        }
        else
        {
            p2Tactics.Remove(tactic);
            p2HandTactics.Add(tactic);
        }
    }


    public void ProcessStartTurn(int ID)
    {
        List<TacticController> tactic = new List<TacticController>();

        tactic.AddRange(p1HandTactics);
        tactic.AddRange(p2HandTactics);        
    }

    public void ProcessStartLayers() 
    {            
            
    }
       
    private int AddPositionPower(GameObject position, List<TacticController> playerTactics)
    {
        int power = 0;
        foreach (TacticController tactic in playerTactics)
        {            
            string number = tactic.tacticPower.text;
            power = int.Parse(number);
            return power;            
        }
        return power;
    }

    private int CompareLayers(GameObject layer1, GameObject layer2)
    {
        int player1Power = 0;
        player1Power = AddPositionPower(layer1, p1Tactics);
        int player2Power = 0;
        player2Power = AddPositionPower(layer2, p2Tactics);

        int difference = player1Power - player2Power;
        return difference;
    }   
    public void ProcessEndTurn()
    {
        if (p1Tactic.transform.childCount > 0 )
        {
            if (p2Tactic.transform.childCount > 0 )
            {

            }
        }
    }    

}
