using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public static TurnManager instance;
    public int currentPlayerTurn;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        StartTurnGameplay(0);
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
        GameplayUIController.instance.UpdateCurrentPlayerTurn(currentPlayerTurn);
        PlayerManager.instance.AssignTurn(currentPlayerTurn);
    }
    public void StartLayers()
    {
        CardManager.instance.ProcessStartLayers();
        EndTurn();
    }
    public void EndTurn()
    {
        CardManager.instance.ProcessEndTurn();
        StartTurn();
    }
}
