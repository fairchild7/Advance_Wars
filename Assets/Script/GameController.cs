using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textDay;
    [SerializeField] TextMeshProUGUI textCurrentColor;
    [SerializeField] UnitControl unitControl;

    private int currentDay;
    public bool isPlayerTurn;

    void Start()
    {
        currentDay = 0;
        StartGame();
    }

    void Update()
    {
        
    }

    private void StartGame()
    {
        UpdateTextDay();
        UpdateTextColor();
        StartPlayerTurn();
    }

    private void StartPlayerTurn()
    {
        Debug.Log("Player Turn");
        isPlayerTurn = true;
        currentDay++;
        UpdateTextDay();
        UpdateTextColor();
    }

    public void StartEnemyTurn()
    {
        Debug.Log("Enemy Turn");
        isPlayerTurn = false;
        UpdateTextColor();
        StartCoroutine(EndEnemyTurn(5f));
    }

    private IEnumerator EndEnemyTurn(float time)
    {
        yield return new WaitForSeconds(time);
        StartPlayerTurn();
    }

    private void UpdateTextDay()
    {
        textDay.text = "Day " + currentDay;
    }

    private void UpdateTextColor()
    {
        if (isPlayerTurn)
        {
            textCurrentColor.text = "Red Turn";
        }
        else
        {
            textCurrentColor.text = "Blue Turn";
        }
    }
}
