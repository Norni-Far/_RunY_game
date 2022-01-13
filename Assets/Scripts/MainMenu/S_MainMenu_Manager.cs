using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class S_MainMenu_Manager : MonoBehaviour
{
    [Header("Scripts")]
    [SerializeField] private S_CanvasMainMenu S_CanvasMainMenu;
    [SerializeField] private I_CanvasBtn_NewGamePanel I_NewGame;
    [SerializeField] private Ads_UnityReclama AdsUnity;
    [SerializeField] private Save_Json Save;

    [Header("UI")]
    [SerializeField] private GameObject btn_forNewTriangle;
    [SerializeField] private Text text_ForColNewTriangles;
    [SerializeField] private Text text_ForSeeKey;
    [SerializeField] private GameObject panelOfloadingGame;
    [SerializeField] private Text text_forNowColTriangle;
    [SerializeField] private Text[] text_forCompleteResalts = new Text[4];

    public delegate void MainMenu_save(int Coltriangles);
    public event MainMenu_save eventSave_mainMenu;

    public delegate void PropertiesGame_save(int lvl, string Key);
    public event PropertiesGame_save eventSave_PropertiesGame_save;

    [Header("Objects")]
    [SerializeField] private Transform PointOfCreate;
    [SerializeField] private GameObject Prefab_Triangle;

    public List<GameObject> ListOfTriangles = new List<GameObject>();

    private int TrianglesCount = 4;
    private int MaxTriangleCount = 20;

    // для записи
    private string KeyGame = "";
    private int lvl = 0;

    private void Awake()
    {
        I_NewGame.event_NeedToGenerateKey += GenerateKey;
        panelOfloadingGame.GetComponent<S_PanelOfLoadinng>().event_LoadingGame += gotoGame;
        S_CanvasMainMenu.event_needMoreTrianglesOnTheWorld += ShowRewerdedAds;
        AdsUnity.event_rewardedVideoIsResult += ResaltsRewardedAddNewTriangles;
        S_CanvasMainMenu.event_JustGoGame += GenerateKeyForJustGoGame;
        S_CanvasMainMenu.event_JustGoGame += gotoGame;
        S_CanvasMainMenu.event_JustGoGame += onPanelOfLoading;
    }

    private void Start()
    {
        // выгрузка сохранений внутри самого скрипта Save
        //Save.StartLoading();
        TrianglesCount = Save.LoadMainMenu.Col_TrianglessRun;
        checkMaxCoutnOfTrianlgle();
        StartCoroutine(StartCreate());

        // загрузка отметок пройденых уровней 
        LoadMarkOfLevels();
    }

    private void LoadMarkOfLevels()
    {
        // выгрузка из массива 
        for (int i = 0; i < text_forCompleteResalts.Length; i++)
        {
            text_forCompleteResalts[i].text = "x" + Save.Load_CompliteResults.lvl_Complete_e_m_h_hc[i].ToString();
        }
    }

    // create triangles
    IEnumerator StartCreate()
    {
        yield return new WaitForSeconds(1f);

        while (true)
        {
            checkMaxCoutnOfTrianlgle();

            if (ListOfTriangles.Count < TrianglesCount)
            {
                yield return new WaitForSeconds(0.7f);
                GameObject Inst = Instantiate(Prefab_Triangle, new Vector3(PointOfCreate.position.x, PointOfCreate.position.y, 0), PointOfCreate.rotation);
                Inst.GetComponent<Rigidbody2D>().AddForce(new Vector2(8, -8), ForceMode2D.Impulse);
                ListOfTriangles.Add(Inst);
                Inst.GetComponent<S_MainMenuTriangle>().S_Manager = this;
            }
            else
                yield return new WaitForSeconds(0.5f);
        }
    }

    // Y = 60 - 100 - угол наклона, чем больше тем больше угол     /100
    // B = 40 - 80  - вариации скалы     /10
    // C = 16 - 28 -  шершавость сскалы (16 нет углов, 28 максимум углов )

    #region GenericKey
    private void GenerateKey(int _lvl)
    {
        lvl = _lvl;
        KeyGame = "";

        int y = 0;
        int b = 0;
        int c = 0;

        switch (lvl)
        {
            case 0:
                y = Random.Range(63, 71);
                b = Random.Range(40, 81);
                c = Random.Range(16, 21);

                KeyGame = $"Y{y}B{b}C{c}";
                break;

            case 1:
                y = Random.Range(71, 81);
                b = Random.Range(40, 81);
                c = Random.Range(21, 25);

                KeyGame = $"Y{y}B{b}C{c}";
                break;

            case 2:
            case 3:

                y = Random.Range(81, 99);
                b = Random.Range(40, 81);
                c = Random.Range(25, 29);

                KeyGame = $"Y{y}B{b}C{c}";
                break;

            default:
                break;
        }

        text_ForSeeKey.text = KeyGame;
    }

    private void GenerateKeyForJustGoGame()
    {
        int lvl_ = Random.Range(0, 3); ;
        KeyGame = "";

        lvl = lvl_;
        int y = 0;
        int b = 0;
        int c = 0;

        switch (lvl)
        {
            case 0:
                y = Random.Range(63, 71);
                b = Random.Range(40, 81);
                c = Random.Range(16, 21);

                KeyGame = $"Y{y}B{b}C{c}";
                break;

            case 1:
                y = Random.Range(71, 81);
                b = Random.Range(40, 81);
                c = Random.Range(21, 25);

                KeyGame = $"Y{y}B{b}C{c}";
                break;

            case 2:

                y = Random.Range(81, 99);
                b = Random.Range(40, 81);
                c = Random.Range(25, 29);

                KeyGame = $"Y{y}B{b}C{c}";
                break;

            case 3:
                break;

            default:
                break;
        }

    }
    #endregion

    // включение загрузки игры
    public void onPanelOfLoading()
    {
        panelOfloadingGame.SetActive(true);
    }

    // вызов с панели загрузки, через анимацию
    private void gotoGame()
    {
        Save.Save_forPropertiesForDificultGame(lvl, KeyGame);
        SceneManager.LoadScene(1);
    }

    // ADS
    private void ShowRewerdedAds()
    {
        AdsUnity.ShowRewardedAds();
    }

    private void checkMaxCoutnOfTrianlgle()
    {
        text_forNowColTriangle.text = (ListOfTriangles.Count).ToString();
        text_ForColNewTriangles.text = (TrianglesCount + "/" + MaxTriangleCount).ToString();

        if (TrianglesCount >= MaxTriangleCount)
        {
            btn_forNewTriangle.GetComponent<Button>().enabled = false;
        }
    }

    private void ResaltsRewardedAddNewTriangles(bool Ok)
    {
        if (Ok)
            TrianglesCount++;

        Save.Save_forMainMeny(TrianglesCount);
    }

}
