using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class S_CanvasMainMenu : MonoBehaviour
{
    [Header("Scripts")]
    [SerializeField] private S_PanelOfLoadinng S_Loading;
    [SerializeField] private I_CanvasBtn_MainPanel I_MainPanel;
    [SerializeField] private I_CanvasBtn_NewGamePanel I_NewGamePanel;

    [Header("PanelsOfMenu")]
    [SerializeField] private GameObject LeftMenu;
    [SerializeField] private GameObject UpMenu;
    [SerializeField] private GameObject PlanetMenu;

    [Header("PanelsOfSecondLeftMenu")]
    [SerializeField] private GameObject _main_Menu;
    [SerializeField] private GameObject _newGame_Menu;

    public delegate void Delegats();
    public event Delegats EndloadingGames;
    public event Delegats closeAllPanels;
    public event Delegats event_needMoreTrianglesOnTheWorld;
    public event Delegats event_JustGoGame;

    private void SuSubscribe()
    {
        //I_NewGamePanel.event_ChooseOnelvlGame += one_lvlIsWasChoosen;

        I_MainPanel.eventNewGame += ChangePanelMenu_MainToNewGame;
        S_Loading.event_LoadingGame += LoadGame;  // для панели загрузки 
        I_NewGamePanel.event_ChooseBack += ChangePanelMenu_NewGameToMainmenu;
    }

    void Start()
    {
        SuSubscribe();
        StartCoroutine(StartMainMenu());
    }

    IEnumerator StartMainMenu()
    {
        yield return new WaitForSeconds(0.5f);

        EndloadingGames?.Invoke();
    }

    //public void endLoadGame()
    //{
    //    EndloadingGames?.Invoke();
    //}

    public void btn_NeedmoreTriangles()
    {
        event_needMoreTrianglesOnTheWorld?.Invoke();
    }


    #region SettingsEndOthers(from_Events_)
    
    private void ChangePanelMenu_MainToNewGame()
    {
        _main_Menu.SetActive(false);
        _newGame_Menu.SetActive(true);
    }

    private void ChangePanelMenu_NewGameToMainmenu()
    {
        _main_Menu.SetActive(true);
        _newGame_Menu.SetActive(false);
    }

    #endregion

    public void LoadGame()
    {
        closeAllPanels?.Invoke();
    }

    public void justGo() // через кнопку Go!
    {
        closeAllPanels?.Invoke();
        event_JustGoGame?.Invoke();
    }

}
