using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public void ButtonWin()
    {
        GameController.Instance.winCondition = true;
        GameController.Instance.CheckCondition();
    }

    public void ButtonLose()
    {
        GameController.Instance.loseCondition = true;
        GameController.Instance.CheckCondition();
    }
}
