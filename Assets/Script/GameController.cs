using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameController : SimpleSingleton<GameController>
{
    [SerializeField] TextMeshProUGUI textDay;
    [SerializeField] TextMeshProUGUI textCurrentColor;

    private int currentDay;
    public bool isPlayerTurn;

    public IState currentState;

    public List<Unit> allyUnit = new List<Unit>();
    public List<Unit> enemyUnit = new List<Unit>();
    public List<Building> allyBuilding = new List<Building>();
    public List<Building> enemyBuilding = new List<Building>();
    public List<Building> neutralBuilding = new List<Building>();

    public Sprite[] hpSprites;

    public bool winCondition = false;
    public bool loseCondition = false;
    public int enemyDestroyed = 0;

    void Start()
    {
        StartGame();
    }

    void Update()
    {
        if (GameStateManager.Instance.IsState(GameState.Gameplay))
        {
            currentState.OnExecute();
        }
    }

    public void ChangeState(IState newState)
    {
        if (currentState != null)
        {
            currentState.OnExit();
        }

        currentState = newState;

        if (currentState != null)
        {
            currentState.OnEnter();
        }
    }

    public void StartGame()
    {
        GameStateManager.Instance.ChangeState(GameState.Gameplay);
        winCondition = false;
        loseCondition = false;
        enemyDestroyed = 0;
        currentDay = 0;
        UpdateTextDay();
        UpdateTextColor();
        StartPlayerTurn();
    }

    public void StartPlayerTurn()
    {
        Debug.Log("Player Turn");
        isPlayerTurn = true;
        currentDay++;
        UpdateTextDay();
        UpdateTextColor();

        foreach (Unit u in allyUnit)
        {
            if (UnitControl.Instance.IsHealing(u))
            {
                u.GetDamage(-20f);
            }
        }

        UIGamePlay.Instance.buttonEnd.SetActive(true);

        ChangeState(new FreeState());
    }

    public void StartEnemyTurn()
    {
        Debug.Log("Enemy Turn");
        isPlayerTurn = false;
        UpdateTextColor();

        foreach (Unit u in enemyUnit)
        {
            if (UnitControl.Instance.IsHealing(u))
            {
                u.GetDamage(-20f);
            }
        }

        StartCoroutine(AIAuto.Instance.ExecuteTurn());
    }

    public void CheckCondition()
    {
        if (winCondition)
        {
            GameStateManager.Instance.ChangeState(GameState.Pause);
            SavePlayerStage();
            UIController.Instance.UIWin.Open();
        }
        if (loseCondition)
        {
            GameStateManager.Instance.ChangeState(GameState.Pause);
            UIController.Instance.UILose.Open();
        }
    }

    public void SavePlayerStage()
    {
        int currentStage = SceneManager.GetActiveScene().buildIndex;
        if (PlayerData.Instance.GetCurrentStage() <= currentStage)
        {
            PlayerData.Instance.SetCurrentStage(currentStage + 1);
            PlayerData.Instance.SaveCurrentStage();
        }
    }

    public float GetDamage(Unit attackUnit, Unit targetUnit)
    {
        int baseValue = UnitData.Instance.damageValue[(int)attackUnit.unitType, (int)targetUnit.unitType];
        TerrainType targetTerrain = GridManager.Instance.GetTerrainType(targetUnit.GetUnitPos().x, targetUnit.GetUnitPos().y);
        return baseValue * (attackUnit.GetHp() / 100f) * ((10 - EnvironmentData.Instance.defenseValue[(int)targetTerrain]) / 10f);
    }

    public void RefreshPosition(List<Unit> unitList)
    {
        foreach (Unit u in unitList)
        {
            u.GetComponent<MapElement>().PlaceObjectOnNewGrid(u.transform.position);
        }
    }

    public int GetCurrentDay()
    {
        return currentDay;
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
