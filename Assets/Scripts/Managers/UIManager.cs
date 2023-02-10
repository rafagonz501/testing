using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public TextMeshProUGUI player1Yards, player2Yards, player1Energy, player2Energy, player1Power, player2Power,
    player1LinePower, player2LinePower, player1AthPower, player2AthPower, player1DmPower, player2DmPower;
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
        UpdatePower(player1.power, player2.power, player1.linePower, player2.linePower,
            player1.athPower, player2.athPower, player1.dmPower, player2.dmPower);
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
    private void UpdatePower(int player1Power, int player2Power, int player1LinePower, int player2LinePower,
        int player1AthPower, int player2AthPower, int player1DmPower, int player2DmPower)
    {
        this.player1Power.text = player1Power.ToString();
        this.player2Power.text = player2Power.ToString();
        this.player1LinePower.text = player1LinePower.ToString();
        this.player2LinePower.text = player2LinePower.ToString();
        this.player1AthPower.text = player1AthPower.ToString();
        this.player2AthPower.text = player2AthPower.ToString();
        this.player1DmPower.text = player1DmPower.ToString();
        this.player2DmPower.text = player2DmPower.ToString();
    }
}
