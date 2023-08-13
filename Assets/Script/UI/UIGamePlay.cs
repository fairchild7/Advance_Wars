using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGamePlay : SimpleSingleton<UIGamePlay>
{
    public GameObject buttonFire;
    public GameObject buttonWait;
    public GameObject buttonCapture;
    public GameObject buttonCancel;
    public GameObject buttonEnd;
    public UnitPanel unitPanel;
    public EnvironmentPanel environmentPanel;
    public GameObject gameProgressPanel;

    void Start()
    {
        HideAllButtons();
        HideAllPanels();
        buttonEnd.SetActive(false);
    }

    public void HideAllButtons()
    {
        buttonFire.SetActive(false);
        buttonWait.SetActive(false);
        buttonCapture.SetActive(false);
        buttonCancel.SetActive(false);
    }

    public void HideAllPanels()
    {
        unitPanel.gameObject.SetActive(false);
        environmentPanel.gameObject.SetActive(false);
    }

    public void ShowEnvironmentPanel(int x, int y)
    {
        environmentPanel.gameObject.SetActive(true);
        environmentPanel.UpdateEnvironmentPanel(GridManager.Instance.GetTerrainType(x, y));
    }

    public void ShowUnitPanel(Unit selectedUnit)
    {
        unitPanel.gameObject.SetActive(true);
        unitPanel.UpdateUnitPanel(selectedUnit);
    }

    public void HideEnvironmentPanel()
    {
        environmentPanel.gameObject.SetActive(false);
    }

    public void HideUnitPanel()
    {
        unitPanel.gameObject.SetActive(false);
    }

    public void Open()
    {
        gameObject.SetActive(true);
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }
}
