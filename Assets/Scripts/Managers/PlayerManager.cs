using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;
    public List<Player> players = new List<Player>();

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        UIManager.instance.UpdateValues(players[0], players[1]);
    }

    internal void AssignTurn(int currentPlayerTurn)
    {
        foreach (Player player in players)
        {
            if (player.ID == currentPlayerTurn) player.isOffense = true;
            else player.isOffense = false;
        }
    }
    public void SwitchPossesion(int currentPlayerTurn)
    {
        foreach (Player player in players)
        {
            if (player.ID == currentPlayerTurn) player.isOffense = false;
            else player.isOffense = true;
        }
    }
    public void SpendEnergy(int ID, int energyCost)
    {
        Player player = FindPlayerByID(ID);
        player.energy -= energyCost;
        UIManager.instance.UpdateValues(players[0], players[1]);
    }
    public void AddEnergy(int ID, int energyCost)
    {
        Player player = FindPlayerByID(ID);
        player.energy += energyCost;
        UIManager.instance.UpdateValues(players[0], players[1]);
    }
    public void AddPower(int ID, int power)
    {
        Player player = FindPlayerByID(ID);
        player.power += power;
        UIManager.instance.UpdateValues(players[0], players[1]);
    }
    public void RemovePower(int ID, int power)
    {
        Player player = FindPlayerByID(ID);
        player.power -= power;
        UIManager.instance.UpdateValues(players[0], players[1]);
    }
    /* public void AddLinePower(int ID, int power)
     {
         Player player = FindPlayerByID(ID);
         player.linePower += power;
         UIManager.instance.UpdateValues(players[0], players[1]);
     }
     public void AddAthPower(int ID, int power)
     {
         Player player = FindPlayerByID(ID);
         player.athPower += power;
         UIManager.instance.UpdateValues(players[0], players[1]);
     }
     public void AddDmPower(int ID, int power)
     {
         Player player = FindPlayerByID(ID);
         player.dmPower += power;
         UIManager.instance.UpdateValues(players[0], players[1]);
     }*/
    public void AdvanceYards(int ID, int yardMove)
    {
        Player player = FindPlayerByID(ID);
        player.yards += yardMove;
        UIManager.instance.UpdateValues(players[0], players[1]);
    }

    private void PlayerLost(int ID)
    {
        UIManager.instance.GameFinished(ID == 0 ? FindPlayerByID(1) : FindPlayerByID(0));
    }

    public Player FindPlayerByID(int ID)
    {
        Player foundPlayer = null;
        foreach (Player player in players)
        {
            if (player.ID == ID)
                foundPlayer = player;
        }
        return foundPlayer;
    }

    internal void AddTacticPower(int ownerID, int tacticPower)
    {
       
    }
}
