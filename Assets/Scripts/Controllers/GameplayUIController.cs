using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class GameplayUIController : MonoBehaviour
{
    public static GameplayUIController instance;
    public TextMeshProUGUI currentPlayerTurn;
    public Button player1EndTurn, player2EndTurn;

    private void Awake()
    {
        instance = this;
        SetUpButtons();
    }

    private void SetUpButtons()
    {
        bool player1ready = false;
        bool player2ready = false;

        player1EndTurn.onClick.AddListener(() =>
        {
            player1ready = true;
            if (player1ready && player2ready)
            {
                TurnManager.instance.EndTurn();
                player1ready = false;
                player2ready = false;
            }
        });

        player2EndTurn.onClick.AddListener(() =>
        {
            player2ready = true;
            if (player1ready && player2ready)
            {
                TurnManager.instance.EndTurn();
                player1ready = false;
                player2ready = false;
            }
        });
    }

    public void UpdateCurrentPlayerTurn(int ID)
    {
        currentPlayerTurn.gameObject.SetActive(true);
        currentPlayerTurn.text = $"Player {ID + 1}'s turn";
        StartCoroutine(BlinkCurrentPlayerTurn());
    }

    private IEnumerator BlinkCurrentPlayerTurn()
    {
        yield return new WaitForSeconds(0.5f);
        currentPlayerTurn.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        currentPlayerTurn.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        currentPlayerTurn.gameObject.SetActive(false);
    }
}
