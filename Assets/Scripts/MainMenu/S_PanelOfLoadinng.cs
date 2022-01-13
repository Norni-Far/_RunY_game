using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_PanelOfLoadinng : MonoBehaviour
{

    public delegate void Delegats();
    public event Delegats event_LoadingGame;
    public event Delegats event_LoadingMainMenu;

    public void LoadingGame()
    {
        event_LoadingGame?.Invoke();
    }

    public void LoadingMainMenu()
    {
        event_LoadingMainMenu?.Invoke();
    }

}
