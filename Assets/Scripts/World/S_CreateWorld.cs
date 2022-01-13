using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class S_CreateWorld : MonoBehaviour
{
    //
    [SerializeField] private Decoding S_Decoding;
    [SerializeField] private S_DistanceOfWay s_DistanceOfWay;
    [SerializeField] private S_CircleOfCreateWorld S_CircleOfCreate;
    [SerializeField] private Save_Json Save;
    //

    private LineRenderer line;
    private S_MeshCreate S_MeshCreate;

    [Header("PlaceForCreate")]
    [SerializeField] GameObject PlaceOFCreateWorld;
    [SerializeField] Transform PlaceOFPlants;
    [SerializeField] Transform PlaceOFstones;
    [SerializeField] GameObject PrefabLineOfCreateWorld;
    [SerializeField] GameObject PrefabPlatoGroundForpoligon;

    [Header("Prefabs")]
    [SerializeField] GameObject[] prefabs_Grass = new GameObject[2];
    [SerializeField] GameObject[] prefabs_Stone_ = new GameObject[8];
    [SerializeField] GameObject[] prefabs_Tree_Summer = new GameObject[4];
    [SerializeField] GameObject[] prefabs_Tree_Fall = new GameObject[3];
    [SerializeField] GameObject[] prefabs_Tree_Winter = new GameObject[3];

    [Header("GradientColorForWorld")]
    [SerializeField] private Gradient summerColor;
    [SerializeField] private Gradient fallColor;
    [SerializeField] private Gradient winterColor;
    private Gradient needColorNow;

    private Vector3 LastPosition = new Vector3(0, 0, -0.1f);

    [SerializeField] private int SizeLvl;

    private float x = 0;
    private float y = 0;

    private int Dop = 1;

    [Header("Decoding")]
    [SerializeField] string KeyForCreate;
    [SerializeField] private float Dec_Y = 0;
    [SerializeField] private float Dec_B = 0;
    [SerializeField] private float Dec_C = 0;

    [Header("lvl")]
    [SerializeField] public int lvl = 0;
    [SerializeField] private float maxHieght;


    [Header("Random")]
    [SerializeField] private int i;
    [SerializeField] private int rStones;
    [SerializeField] private int rWorld;

    private void LoadingProperties()
    {
        KeyForCreate = Save.LoadPropertiesForDificultGame.KeyForStage; //"Y60b48c24";  // ввод ключа мира 
        lvl = Save.LoadPropertiesForDificultGame.lvl;
    }

    public void StartGenerateMeshWorld()  // запуск главным скриптом
    {
        LoadingProperties();

        switch (lvl)
        {
            case 0:
                needColorNow = summerColor;
                maxHieght = 800;
                break;

            case 1:
                needColorNow = fallColor;
                maxHieght = 900;
                break;

            case 2:
                needColorNow = winterColor;
                maxHieght = 1200; 
                break;

            case 3:
                needColorNow = fallColor;
                maxHieght = 2000;
                break;

            default:  // просто для защиты 
                needColorNow = fallColor;
                maxHieght = 6000;
                break;
        }

        s_DistanceOfWay.HieghtOfTargetTowardsY = maxHieght;

        // Y = 60 - 100 - угол наклона, чем больше тем больше угол     /100
        // B = 40 - 80  - вариации скалы     /10
        // C = 16 - 28 -  шершавость сскалы (16 нет углов, 28 максимум углов )
        S_Decoding.DecodingKey(KeyForCreate);

        Dec_Y = S_Decoding.y_up;
        Dec_B = S_Decoding.b;
        Dec_C = S_Decoding.c;

        S_CircleOfCreate.EventCreateWorld += GenerationWorld;
    }

    #region CreateLevelForMeshAndPoligon
    private void GenerationWorld(GameObject CircleObject)
    {
        GameObject Inst = Instantiate(PrefabLineOfCreateWorld, new Vector3(0, 0, -0.1f), transform.rotation, PlaceOFCreateWorld.transform);

        line = Inst.GetComponent<LineRenderer>();
        line.colorGradient = needColorNow;
        S_MeshCreate = Inst.GetComponent<S_MeshCreate>();

        line.positionCount = 100;  // установка размера карты хоть до бессконечности !!
        SizeLvl = line.positionCount; // размер карты 

        // Создание линиии
        CreateLineWorld(CircleObject);
        //

        // добавления коллайдера на линию
        CreatePoligonCollider();
        //
    }

    // построение уровня 
    private void CreateLineWorld(GameObject CircleOfCreate)
    {
        Instantiate(PrefabPlatoGroundForpoligon, LastPosition + new Vector3(0, 0, -0.2f), transform.rotation, PlaceOFCreateWorld.transform);

        // + цвет линии 
        // 1 лвл = зелёный
        // 2 лвл = коричневый 
        // 3 лвл = белый 

        // каждые 71 размеров - плато
        line.SetPosition(0, LastPosition);

        Vector3 Point;

        for (int i = 1; i < SizeLvl; i++)
        {
            if (i == 71)
                line.SetPosition(i, Point = new Vector3(x = Bias() + check_X_(), y += 0, -0.1f));   // y_up - повышение линии
            else
                line.SetPosition(i, Point = new Vector3(x = Bias() + check_X_(), y += Dec_Y, -0.1f));   // y_up - повышение линии
            Dop++;

            if (SizeLvl - 2 == i)
            {
                //перемещение CircleOfCreate  на пред последнее место
                LastPosition = new Vector3(x, y, -0.1f);
                CircleOfCreate.transform.position = new Vector3(x, y, -0.1f);
            }

            if (SizeLvl - 1 == i)
            {
                // последняя линия  должна быть -10, для Poligona
                line.SetPosition(i, new Vector3(x + SizeLvl / 2, y - SizeLvl, -0.1f));
            }
        }

        // Grafics
        for (int i = 0; i < line.positionCount - 1; i++)
        {
            if (line.GetPosition(i).y < maxHieght * 30 / 100) // если пройденно меньше 30 процентов
            {
                CreateGrassStaticObjects(line.GetPosition(i), line.GetPosition(i + 1));
                CreateGrassStaticObjects((line.GetPosition(i) + line.GetPosition(i + 1)) / 2, line.GetPosition(i + 1));

                if (UnityEngine.Random.Range(0, 10) < 2)
                    CreateTreeStaticObjects(line.GetPosition(i));
            }
            else if (line.GetPosition(i).y < maxHieght * 60 / 100)  // если пройденно меньше 60 процентов
            {
                CreateGrassStaticObjects(line.GetPosition(i), line.GetPosition(i + 1));

                if (UnityEngine.Random.Range(0, 10) < 2)
                    CreateTreeStaticObjects(line.GetPosition(i));

                if (random_forDildingLevel() < 3)
                    CreateStonesStaticObjects(line.GetPosition(i));
            }
            else
            {
                if (UnityEngine.Random.Range(0, 10) < 8)
                    CreateGrassStaticObjects(line.GetPosition(i), line.GetPosition(i + 1));

                if (random_forDildingLevel() < 8)
                    CreateStonesStaticObjects(line.GetPosition(i));

                if (UnityEngine.Random.Range(0, 10) < 1)
                    CreateTreeStaticObjects(line.GetPosition(i));
            }

        }
    }

    // Grafics 
    private void CreateGrassStaticObjects(Vector3 PointOfCreate, Vector3 Direction)
    {
        int rnd = UnityEngine.Random.Range(0, 2);
        GameObject Inst;

        switch (rnd)
        {
            case 0:
                Inst = Instantiate(prefabs_Grass[0], PointOfCreate, transform.rotation, PlaceOFPlants);
                MoveEnemyStatic.DirectionTowards(Inst, Direction);
                break;

            case 1:
                Inst = Instantiate(prefabs_Grass[1], PointOfCreate, transform.rotation, PlaceOFPlants);
                MoveEnemyStatic.DirectionTowards(Inst, Direction);
                break;

            default:
                Inst = Instantiate(prefabs_Grass[0], PointOfCreate, transform.rotation, PlaceOFPlants);
                MoveEnemyStatic.DirectionTowards(Inst, Direction);
                break;
        }
    }
    private void CreateTreeStaticObjects(Vector3 PointOfCreate)
    {
        GameObject Inst;

        if (lvl == 0)
            Inst = Instantiate(prefabs_Tree_Summer[UnityEngine.Random.Range(0, 4)], PointOfCreate, transform.rotation, PlaceOFPlants);
        else if (lvl == 1)
            Inst = Instantiate(prefabs_Tree_Fall[UnityEngine.Random.Range(0, 3)], PointOfCreate, transform.rotation, PlaceOFPlants);
        else if (lvl == 2)
            Inst = Instantiate(prefabs_Tree_Winter[UnityEngine.Random.Range(0, 3)], PointOfCreate, transform.rotation, PlaceOFPlants);
        else if (lvl == 3)
            Inst = Instantiate(prefabs_Tree_Fall[UnityEngine.Random.Range(0, 3)], PointOfCreate, transform.rotation, PlaceOFPlants);

        // в хардкоре всё подряд и разных цветов 
    }
    private void CreateStonesStaticObjects(Vector3 PointOfCreate)
    {
        GameObject Inst;

        switch (Mathf.RoundToInt(random_forStones() / 10))
        {
            case 0:
                Inst = Instantiate(prefabs_Stone_[0], PointOfCreate, transform.rotation, PlaceOFstones);
                Inst.transform.rotation = Quaternion.Euler(0, 0, 360 * random_forStones() / 100);
                break;

            case 1:
                Inst = Instantiate(prefabs_Stone_[1], PointOfCreate, transform.rotation, PlaceOFstones);
                Inst.transform.rotation = Quaternion.Euler(0, 0, 360 * random_forStones() / 100);
                break;
            case 2:
                Inst = Instantiate(prefabs_Stone_[2], PointOfCreate, transform.rotation, PlaceOFstones);
                Inst.transform.rotation = Quaternion.Euler(0, 0, 360 * random_forStones() / 100);
                break;

            case 3:
                Inst = Instantiate(prefabs_Stone_[3], PointOfCreate, transform.rotation, PlaceOFstones);
                Inst.transform.rotation = Quaternion.Euler(0, 0, 360 * random_forStones() / 100);
                break;

            case 5:
                Inst = Instantiate(prefabs_Stone_[5], PointOfCreate, transform.rotation, PlaceOFstones);
                Inst.transform.rotation = Quaternion.Euler(0, 0, 360 * random_forStones() / 100);
                break;
            case 6:
                Inst = Instantiate(prefabs_Stone_[6], PointOfCreate, transform.rotation, PlaceOFstones);
                Inst.transform.rotation = Quaternion.Euler(0, 0, 360 * random_forStones() / 100);
                break;

            case 7:
                Inst = Instantiate(prefabs_Stone_[7], PointOfCreate, transform.rotation, PlaceOFstones);
                Inst.transform.rotation = Quaternion.Euler(0, 0, 360 * random_forStones() / 100);
                break;

            default:
                break;
        }
    }

    private void CreatePoligonCollider()
    {
        var Pos = new Vector3[line.positionCount];
        line.GetPositions(Pos);

        var Poligon = line.GetComponent<PolygonCollider2D>();  // колайдер полигона 
        Poligon.points = Pos.Select(p => (Vector2)p).ToArray();

        S_MeshCreate.Start_S_CreateMesh(Pos);
    }

    #endregion

    #region Random

    // смещение по иксу 
    private float Bias() // 2.7 - статическое смещение
    {
        return (Dop + x) / 2.1f;
    }


    // "random" по иксу для построения линий 
    private float check_X_() //float x, float Dop)
    {
        i++;
        return ((Mathf.Abs(Mathf.Sin(i - 40)) * 10) + Dec_B) % Dec_C; // 2,4
    }


    // in 0 to *100
    // "random" для построения волунов
    private float random_forStones()
    {
        rStones++;
        print(Mathf.Sin(rStones + Mathf.PI * Dec_B - Dec_C) * 100);
        return Mathf.Sin(rStones + Mathf.PI * Dec_B - Dec_C) * 100;
        //return ((Mathf.Abs(Mathf.Sin(rStones - 40)) * 100) + Dec_B) % Dec_C; // 2,4
    }

    private int random_forDildingLevel()
    {
        rWorld++;
        return Convert.ToInt32(Math.Abs(Mathf.Round(Mathf.Sin(rWorld) * 1.1f * 10)));
    }

    #endregion

}
