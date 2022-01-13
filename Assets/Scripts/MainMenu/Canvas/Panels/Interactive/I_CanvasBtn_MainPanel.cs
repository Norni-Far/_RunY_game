using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class I_CanvasBtn_MainPanel : MonoBehaviour
{
    public delegate void Delegats();
    public event Delegats eventNewGame;

    #region forButton
    public void btnJustGo()
    {

    }

    public void btnNewGame()
    {
        eventNewGame?.Invoke();
    }

    public void btnContinue()
    {

    }

    public void btnChalenges()
    {

    }
    public void btnPlayGame()
    {

    }
    #endregion
}
