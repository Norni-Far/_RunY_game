using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Save_Json : MonoBehaviour
{
    [SerializeField] private S_MainMenu_Manager S_mainMenuManager;

    public Save_PropertiesForDificultGame LoadPropertiesForDificultGame;
    public Save_MainMenu LoadMainMenu;
    public Save_CompleteResult Load_CompliteResults;

    public bool LoadAllInf = true;

    private void Awake()
    {
        if (SceneManager.sceneCount == 0)
        {
            S_mainMenuManager.eventSave_mainMenu += Save_forMainMeny;
        }

        LoadAll();
    }

    public void LoadAll()
    {

#if UNITY_ANDROID
        if (!File.Exists(Application.persistentDataPath + "PropertiesForDificultGame.json"))
            Save_forPropertiesForDificultGame(0, "Y60b48c24");

        if (!File.Exists(Application.persistentDataPath + "MainMenu.json"))
            Save_forMainMeny(5);

        if (!File.Exists(Application.persistentDataPath + "CompleteResult.json"))
            Save_forCompleteResult(false, 1);
#endif
#if UNITY_EDITOR
        if (!File.Exists(Application.dataPath + "PropertiesForDificultGame.json"))
            Save_forPropertiesForDificultGame(0, "Y60b48c24");

        if (!File.Exists(Application.dataPath + "MainMenu.json"))
            Save_forMainMeny(5);

        if (!File.Exists(Application.dataPath + "CompleteResult.json"))
            Save_forCompleteResult(false, 1);
#endif

        // выгрузка информации 
        Load_forPropertiesForDificultGame();
        Load_forMainMenu();
        Load_forCompliteResults();

        LoadAllInf = true;
    }


    #region Save

    //
    public void Save_forPropertiesForDificultGame(int lvl, string Key)
    {
        // создаю экземпляр класса который нужно сохранить
        Save_PropertiesForDificultGame DataZapic = new Save_PropertiesForDificultGame();

        // изменяю данные
        DataZapic.lvl = lvl;
        DataZapic.KeyForStage = Key;

        // преобразую их в JsonFile
        string jsonZapic = JsonUtility.ToJson(DataZapic);
        // сохраняю
#if UNITY_ANDROID
        File.WriteAllText(Application.persistentDataPath + "PropertiesForDificultGame.json", jsonZapic);
#endif

#if UNITY_EDITOR
        File.WriteAllText(Application.dataPath + "PropertiesForDificultGame.json", jsonZapic);
#endif
    }


    //
    public void Save_forMainMeny(int Coltriangles)
    {
        // создаю экземпляр класса который нужно сохранить
        Save_MainMenu DataZapic = new Save_MainMenu();

        // изменяю данные
        DataZapic.Col_TrianglessRun = Coltriangles;

        // преобразую их в JsonFile
        string jsonZapic = JsonUtility.ToJson(DataZapic);

        // сохраняю
#if UNITY_ANDROID
        File.WriteAllText(Application.persistentDataPath + "MainMenu.json", jsonZapic);
#endif

#if UNITY_EDITOR
        File.WriteAllText(Application.dataPath + "MainMenu.json", jsonZapic);
#endif
    }




    public void Save_forCompleteResult(bool lvlISComplete, int num_lvl)
    {

        // создаю экземпляр класса который нужно сохранить
        Save_CompleteResult DataZapic = new Save_CompleteResult();
        if (Load_CompliteResults != null)
            DataZapic = Load_CompliteResults;

        // изменяю данные
        DataZapic.lvlIsComplete = lvlISComplete;
        if (lvlISComplete)
            DataZapic.lvl_Complete_e_m_h_hc[num_lvl]++;

        // преобразую их в JsonFile
        string jsonZapic = JsonUtility.ToJson(DataZapic);

        // сохраняю в  json файл
#if UNITY_ANDROID
        File.WriteAllText(Application.persistentDataPath + "CompleteResult.json", jsonZapic);
#endif

#if UNITY_EDITOR
        File.WriteAllText(Application.dataPath + "CompleteResult.json", jsonZapic);
#endif
    }
    #endregion

    #region Load
    public void Load_forPropertiesForDificultGame()
    {
        string jsonZapic2 = "";
        // ReadAllVeraibles
#if UNITY_ANDROID
        jsonZapic2 = File.ReadAllText(Application.persistentDataPath + "PropertiesForDificultGame.json");
#endif

#if UNITY_EDITOR
        jsonZapic2 = File.ReadAllText(Application.dataPath + "PropertiesForDificultGame.json");
#endif

        // сохраняю прочитаное в экземпляр класса
        LoadPropertiesForDificultGame = JsonUtility.FromJson<Save_PropertiesForDificultGame>(jsonZapic2);
    }

    public void Load_forMainMenu()
    {
        // читаю все переменные
        string jsonZapic2 = "";

#if UNITY_ANDROID
        jsonZapic2 = File.ReadAllText(Application.persistentDataPath + "MainMenu.json");
#endif

#if UNITY_EDITOR
        jsonZapic2 = File.ReadAllText(Application.dataPath + "MainMenu.json");
#endif
        // сохраняю прочитаное в экземпляр класса
        LoadMainMenu = JsonUtility.FromJson<Save_MainMenu>(jsonZapic2);
    }

    public void Load_forCompliteResults()
    {
        // читаю все переменные
        string jsonZapic2 = "";

#if UNITY_ANDROID
        jsonZapic2 = File.ReadAllText(Application.persistentDataPath + "CompleteResult.json");
#endif

#if UNITY_EDITOR
        jsonZapic2 = File.ReadAllText(Application.dataPath + "CompleteResult.json");
#endif
        // сохраняю прочитаное в экземпляр класса
        Load_CompliteResults = JsonUtility.FromJson<Save_CompleteResult>(jsonZapic2);

    }

    #endregion





    #region SaveClass
    public class Save_MainMenu
    {
        public int Col_TrianglessRun = 5; // кол-во бегущих треугольников (за рекламу)
    }

    public class Save_PropertiesForDificultGame
    {
        public string KeyForStage; // сгенерированный ключ уровня
        public int lvl = 0; // выбранный уровень для загрузки
    }

    public class Save_CompleteResult
    {
        public bool lvlIsComplete; // пометка, для сохранения пройденного уровня 

        public byte[] lvl_Complete_e_m_h_hc = new byte[4]; // пройденное количество раз уровень (0 easy; 1 middle; 2 hard; 3 hardcore)
    }

    // для  продолжения 
    public class Save_ContinueGame
    {
        public string KeyForStage; // сгенерированный ключ уровня
        public Transform StartPositionForContinue;   // позиция установленного флага
        public int HealthHero;   // оставшиеся жизни игрока
        public GameObject[] enemyMashins = new GameObject[30]; // все вражеские машины, которые остались на сцене 
    }

    #endregion

}