using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMashine : MonoBehaviour
{
    //
    [Header("Scripts")]
    [SerializeField] private GameManager GameManager;
    [SerializeField] private S_DistanceOfWay S_DistanceOfWay;
    [SerializeField] private Save_Json Save;
    private S_EnemyMove S_EnemyMove;
    //
    // Уровни
    [SerializeField] private int lvl;
    [SerializeField]
    private int[] chanseOfOrdaLVL = new int[]
    { 5,
            15,
            20,
            30
        };
    [SerializeField]
    private int[] colOfOrdaLVL = new int[]
    {
        2,
        3,
        4,
        5
    };
    //

    [SerializeField] private Transform PlaceOfCreate;
    [SerializeField] private Transform[] TargetPlace = new Transform[10]; // места назначения 

    [SerializeField] private GameObject PrefabEnemy;
    [SerializeField] private GameObject[] PrefabsWeapons = new GameObject[10];

    public List<GameObject> EnemyList = new List<GameObject>(); // Враги в реальном времени
    //public List<int> RandPosition = new List<int>();
    public GameObject[] RandPosition = new GameObject[20];

    [SerializeField] public float TimeToCreateNow = 0;
    [SerializeField] private int MinTimeCreaet = 5;
    [SerializeField] private int MaxTimeCreate = 30;

    //[SerializeField] private int MaxRandomWeapons = 5;
    [SerializeField]
    private int[,,] p_Of_Weapons_lvl_0 = new int[,,]
    { 
        { // первый уровень  0
        {0,56},    // пулемётчик
        {55,81},   // ракетчик
        {80,91},   // минёр
        {90,96},   // антигравитан
        {95,101}   // клоны
        }, // второй уровень  1
        {
        {0,31},    // пулемётчик
        {30,61},   // ракетчик
        {60,81},   // минёр
        {80,91},   // антигравитан
        {90,101}   // клоны
        }, // третий уровень   2
        {
        {0,26},    // пулемётчик
        {25,51},   // ракетчик
        {50,76},   // минёр
        {75,86},   // антигравитан
        {85,101}   // клоны
        },  // хардкор  3
        {
        {0,21},    // пулемётчик
        {20,41},   // ракетчик
        {40,61},   // минёр
        {60,81},   // антигравитан
        {80,101}   // клоны
        }
    };


    public bool startGame = true;
    public bool PlayerIsLive = false;


    private void Awake()
    {
        GameManager.event_aboutPlayer += CheckPlayer;
    }

    private void LoadingProperties()
    {
        lvl = Save.LoadPropertiesForDificultGame.lvl;
    }

    public void StartEnemyMashine()
    {
        LoadingProperties();
        Chooselvl();

        if (startGame)
            StartCoroutine(StartCreate());
    }

    private void CheckPlayer(bool Player) => PlayerIsLive = Player;

    private void Chooselvl()
    {
        switch (lvl)
        {
            case 0:
                MinTimeCreaet = 10;
                MaxTimeCreate = 30;
                break;

            case 1:
                MinTimeCreaet = 5;
                MaxTimeCreate = 30;
                break;

            case 2:
                MinTimeCreaet = 5;
                MaxTimeCreate = 20;
                break;

            case 3:
                MinTimeCreaet = 5;
                MaxTimeCreate = 20;
                break;
        }

    }


    IEnumerator StartCreate()
    {
        while (true)
        {
            if (PlayerIsLive)
            {
                // зависимотсь пройденного пути и времени создания врагов 
                TimeToCreateNow = MinTimeCreaet + (((MaxTimeCreate - MinTimeCreaet) * S_DistanceOfWay.CheckNeedMoveDistance_Percent() / 100));

                yield return new WaitForSeconds(TimeToCreateNow);

                if (rndForOrda() <= chanseOfOrdaLVL[lvl])
                {
                    for (int i = 0; i < colOfOrdaLVL[lvl]; i++)
                    {
                        if (EnemyList.Count < TargetPlace.Length && PlayerIsLive)
                        {
                            CreateEnemy();
                        }
                    }
                }
                else
                {
                    if (EnemyList.Count < TargetPlace.Length && PlayerIsLive)
                    {
                        CreateEnemy();
                    }
                }
            }
            else
            {
                yield return new WaitUntil(() => PlayerIsLive);
            }
        }
    }

    private int rndForOrda()
    {
        return UnityEngine.Random.Range(0, 101);
    }

    private void CreateEnemy()
    {
        GameObject Inst = Instantiate(PrefabEnemy, PlaceOfCreate.position, PlaceOfCreate.rotation);
        SettingsEnemy(Inst);
    }

    #region ChossseSettings
    private void SettingsEnemy(GameObject Inst)
    {
        EnemyList.Add(Inst);
        S_EnemyMove = Inst.GetComponent<S_EnemyMove>();
        S_EnemyMove.PlacementWeapons(PrefabsWeapons[ChooseWeapons()]);
        S_EnemyMove.PlacementTarget(TargetPlace[ChoosePlace(Inst, S_EnemyMove)]);
    }
    private int ChooseWeapons()
    {
        int change = Random.Range(0, 101);

        for (int i = 0; i < p_Of_Weapons_lvl_0.GetLength(1); i++)
        {
            if (change >= p_Of_Weapons_lvl_0[lvl, i, 0] && change <= p_Of_Weapons_lvl_0[lvl, i, 1])
            {
                change = i;
                return change;
            }

        }
        return change;
    }

    private int ChoosePlace(GameObject Inst, S_EnemyMove s_EnemyMove)
    {
        int change = 0;

        change = Random.Range(0, TargetPlace.Length);

        while (RandPosition[change] != null)
        {
            change++;
            if (change == TargetPlace.Length)
                change = 0;
        }

        RandPosition[change] = Inst;
        s_EnemyMove.NumberOfPosition = change;

        return change;
    }

    #endregion
}


