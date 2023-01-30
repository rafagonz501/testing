using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public TextMeshProUGUI player1Yards, player2Yards, player1Energy, player2Energy;
    public Transform energyScreen;
    public GameEndUIController gameEndUI;
    private void Awake()
    {
        instance = this; 
    }

    public void GameFinished(Player winner)
    {
        gameEndUI.gameObject.SetActive(true);
        gameEndUI.Initialize(winner);
    }
    public void UpdateValues(Player player1, Player player2)
    {
        UpdateYards(player1.yards, player2.yards);
        UpdateEnergy(player1.energy, player2.energy);
    }
    public void UpdateYards(int player1Yards, int player2Yards)
    {
        this.player1Yards.text = player1Yards.ToString();
        this.player2Yards.text = player2Yards.ToString();
    }
    public void UpdateEnergy(int player1Energy, int player2Energy)
    {
        this.player1Energy.text = player1Energy.ToString();
        this.player2Energy.text = player2Energy.ToString();
    }

    
}
