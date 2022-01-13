using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class I_CanvasBtn_NewGamePanel : MonoBehaviour
{

    public delegate void Delegats();
    public event Delegats event_ChooseOnelvlGame;
    public event Delegats event_ChooseTwolvlGame;
    public event Delegats event_ChooseThreelvlGame;
    public event Delegats event_ChooseHardcorelvlGame;
    public event Delegats event_ChooseBack;

    public delegate void GenerateKey(int lvl);
    public event GenerateKey event_NeedToGenerateKey;

    #region forButton
    public void OnelvlGame()
    {
        event_ChooseOnelvlGame?.Invoke();
        event_NeedToGenerateKey?.Invoke(0);
    }
    public void TwolvlGame()
    {
        event_ChooseTwolvlGame?.Invoke();
        event_NeedToGenerateKey?.Invoke(1);
    }

    public void ThreelvlGame()
    {
        event_ChooseThreelvlGame?.Invoke();
        event_NeedToGenerateKey?.Invoke(2);
    }
    public void HardcorelvlGame()
    {
        event_ChooseHardcorelvlGame?.Invoke();
        event_NeedToGenerateKey?.Invoke(3);
    }

    public void Back()
    {
        event_ChooseBack?.Invoke();
    }
    #endregion
}
