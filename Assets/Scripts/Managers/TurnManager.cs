using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class TurnManager : MonoBehaviour
{
    public static TurnManager instance;
    public int currentPlayerTurn;
    private int offense = 0;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        StartTurnGameplay(offense);        
    }
    public void StartTurnGameplay(int playerID)
    {
        currentPlayerTurn = playerID;
        PlayerManager.instance.AssignTurn(currentPlayerTurn);
        StartTurn();
    }
    public void StartTurn()
    {
        CardManager.instance.ProcessStartTurn(currentPlayerTurn);
        TacticManager.instance.ProcessStartTurn(currentPlayerTurn);
        GameplayUIController.instance.UpdateCurrentPlayerTurn(currentPlayerTurn);
        PlayerManager.instance.AssignTurn(currentPlayerTurn);
    }
    public void StartLayers()
    {
        CardManager.instance.ProcessStartLayers();
        TacticManager.instance.ProcessStartLayers();
        EndTurn();
    }
    public void EndTurn()
    {
        CardManager.instance.ProcessEndTurn();
        TacticManager.instance.ProcessEndTurn();
        StartTurn();
    }
    public void SwitchPossesion()
    {
        if (offense == 0) offense = 1;
        else offense = 0;
        StartTurnGameplay(offense);
    }
   
}
