using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Scripts")]
    [SerializeField] private S_CreateWorld S_CreateWorld;
    [SerializeField] private EnemyMashine EnemyMashine;
    [SerializeField] private S_DistanceOfWay S_DistanceOfWay;
    [SerializeField] private S_MainCamera S_MainCamera;
    [SerializeField] private S_MovePointsOfEnemy S_MovePointsOfEnemy;
    [SerializeField] private UI_DeadMenu DeadMenu;
    [SerializeField] private Save_Json Save;
    [SerializeField] private S_Canvas S_Canvas;
    [SerializeField] private C_Attacklojstick C_Attacklojstick;
    [SerializeField] private C_MoveJoystic C_MoveJoystic;
    [SerializeField] private S_FinishForHero S_Finish;
    [SerializeField] private Ads_UnityReclama UnityAds;
    [SerializeField] private UI_infDamage UI_infDamage;

    [Header("Objects")]
    [SerializeField] private GameObject ArrayParent;
    [SerializeField] private GameObject prefabFlash;
    [SerializeField] private GameObject Triangle;
    [SerializeField] private Transform PointofCreateStart;
    [SerializeField] private Transform PointofCreateInFlag;
    [SerializeField] private AudioSource MusicFon;

    [Header("Variables")]
    [SerializeField] private bool FlagIsSet;
    [SerializeField] private bool PlayerIsLive;

    [Header("AboutHero")]
    [SerializeField] private GameObject Hero = null;
    [SerializeField] private S_Triangle HeroScript = null;

    public delegate void Delegats(bool Player);
    public event Delegats event_aboutPlayer;

    public delegate void Delegats_();
    public event Delegats_ event_needControl;
    public event Delegats_ event_finish;

    private bool isFinish;

    [Header("ForReclama")]
    [SerializeField] private int forReclamaDead = 9;

    public delegate void Lifes_Delegats(int forReclamaDead);
    public event Lifes_Delegats event_minusLife;


    private void Awake()
    {
        DeadMenu.event_tuchContinue += Create;
        DeadMenu.event_tuchMainMenu += MainMenu;
        DeadMenu.event_tuchRestart += Restart;
        DeadMenu.event_seeReclamaFoeGame += SeeReclama;
        S_Canvas.event_Flag += AboutFlag;
        S_Finish.event_Finish += GameIsOver_Finish;
    }

    void Start()
    {
        // Выгрузка сохранений d в самом скрипте  
        //Save.StartLoading();

        // Рендер уровня
        S_CreateWorld.StartGenerateMeshWorld();

        // Появление игрока
        Create();

        // Старт спавна Врагов
        EnemyMashine.StartEnemyMashine();
    }

    public void PlayerInTheScene(GameObject Player)
    {
        S_Triangle PlayerScript = Player.GetComponent<S_Triangle>();

        for (int i = 0; i < ArrayParent.transform.childCount; i++)
        {
            Destroy(ArrayParent.transform.GetChild(i).gameObject);
        }

        PlayerIsLive = true;

        // оповещение о создании игрока   
        event_aboutPlayer?.Invoke(true);
        //

        // Обновеление информации о новом игроке
        PlayerScript.event_PlayerisTakeDamage += UI_infDamage.DamageHero;
        S_DistanceOfWay.CheckDistance();
        S_MainCamera.FindPlayer();
        S_MovePointsOfEnemy.FindPlayer();

        C_Attacklojstick.UpdateTriangle(PlayerScript);
        C_MoveJoystic.UpdateTriangle(PlayerScript);
        //
    }

    public void PlayerIsDestroy(GameObject Player)
    {
        // отмена музыки 
        MusicFon.Stop();

        PlayerIsLive = false;

        // оповещение о гибели        
        event_aboutPlayer?.Invoke(false);
        //
        if (!isFinish)
        {
            S_MainCamera.FindHeart();
            DeadMenu.OpenMenu();
        }
        else
        {
            S_MainCamera.FindHeart();
            DeadMenu.openFinishMenu();
        }

    }

    private void SeeReclama()
    {
        Time.timeScale = 0;
        UnityAds.ShowSimpleAds();
    }

    private void GameIsOver_Finish()
    {
        // молния смерти 
        Instantiate(prefabFlash, Hero.transform.position, transform.rotation = Quaternion.Euler(0, 0, 0));
        isFinish = true;
        print("finish");

        // Сохранение уровня на котором умер 
        Save.Save_forCompleteResult(true, S_CreateWorld.lvl);
        //

        HeroScript.Health -= HeroScript.Health + 5000;
        event_finish?.Invoke();
    }

    // события из меню 
    private void AboutFlag(bool Flag) => FlagIsSet = Flag;

    public void Create() // для начала и продолжения 
    {
        if (!PlayerIsLive)
        {
            MusicFon.Play();

            Hero = null;
            HeroScript = null;

            // вывод управления на экран
            event_needControl?.Invoke();

            if (FlagIsSet)
            {
                Hero = Instantiate(Triangle, PointofCreateInFlag.transform.position, PointofCreateInFlag.transform.rotation);
            }
            else
            {
                Hero = Instantiate(Triangle, PointofCreateStart.transform.position, PointofCreateStart.transform.rotation);
            }
            HeroScript = Hero.GetComponent<S_Triangle>();
            HeroScript.event_PlayerisDead += PlayerIsDestroy;
            HeroScript.event_PlayerisLive += PlayerInTheScene;

            forReclamaDead--;
            event_minusLife?.Invoke(forReclamaDead);
        }
    }

    public void Restart()  // перезагрузка сцены 
    {
        SceneManager.LoadScene(1);
    }

    public void MainMenu()   // переход в меню
    {
        SceneManager.LoadScene(0);
    }



}
