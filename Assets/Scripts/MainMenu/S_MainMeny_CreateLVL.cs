using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class S_MainMeny_CreateLVL : MonoBehaviour
{
    //
    [SerializeField] private Decoding S_Decoding;
    [SerializeField] private S_CircleOfCreateWorld S_CircleOfCreateWorld;
    //

    private LineRenderer line;
    private S_MeshCreate S_MeshCreate;

    [SerializeField] GameObject PlaceOFCreateWorld;
    [SerializeField] GameObject PrefabLineOfCreateWorld;
    [SerializeField] GameObject PrefabPlatoGroundForpoligon;

    [SerializeField] GameObject[] prefabs_Grass = new GameObject[2];
    [SerializeField] GameObject[] prefabs_Tree_Summer = new GameObject[4];

    private Vector3 LastPosition = new Vector3(0, 0, -0.1f);
    private GameObject InstLine;

    private int SizeLvl;

    private float x = 0;
    private float y = 0;

    private int Dop = 1;
    private int i = 0;

    [SerializeField] private float Dec_Y = 0;
    [SerializeField] private float Dec_B = 0;
    [SerializeField] private float Dec_C = 0;
    [SerializeField] private bool ChangeOn;

    private void Start()
    {
        string KeyForCreate = "Y40b10c24";  // ввод ключа мира "Y50b20c20"
        //
        // Y = 60 - 100 - угол наклона, чем больше тем больше угол     /100
        // B = 40 - 80  - вариации скалы     /10
        // C = 17 - 28 -  шершавость сскалы (16 нет углов, 28 максимум углов )                                                /10
        S_Decoding.DecodingKey(KeyForCreate);

        Dec_Y = S_Decoding.y_up;
        Dec_B = S_Decoding.b;
        Dec_C = S_Decoding.c;

        StartCoroutine(ChangesOFWorld());
        S_CircleOfCreateWorld.EventCreateWorld += StartGenerateMeshWorld;
    }

    IEnumerator ChangesOFWorld()
    {
        bool t = false;

        while (true)
        {
            yield return new WaitUntil(() => ChangeOn);

            Dec_B = Random.Range(4.0f, 8.0f);
            Dec_C = Random.Range(1.7f, 2.8f);

            if (Dec_Y < 0.41f)
                t = false;
            else if (Dec_Y > 0.80f)
                t = true;

            if (!t)
            {
                Dec_Y += 0.04f;
            }
            else
            {
                Dec_Y -= 0.02f;
            }


            ChangeOn = false;
        }

    }

    public void StartGenerateMeshWorld(GameObject CircleObject)
    {
        InstLine = Instantiate(PrefabLineOfCreateWorld, new Vector3(0, 0, -0.1f), transform.rotation, PlaceOFCreateWorld.transform);

        line = InstLine.GetComponent<LineRenderer>();
        S_MeshCreate = InstLine.GetComponent<S_MeshCreate>();

        line.positionCount = 100;  // установка размера карты хоть до бессконечности !!
        SizeLvl = line.positionCount; // размер карты 

        // Создание линиии
        CreateLineWorld(CircleObject);
        //

        // добавления коллайдера на линию
        CreatePoligonCollider();
        //

        ChangeOn = true;
    }


    // построение уровня 
    #region CreateLVL
    private void CreateLineWorld(GameObject CircleObject)
    {
        GameObject Inst = Instantiate(PrefabPlatoGroundForpoligon, LastPosition, transform.rotation, PlaceOFCreateWorld.transform);

        // каждые 1000 размеров - плато
        line.SetPosition(0, LastPosition);

        for (int i = 1; i < SizeLvl; i++)
        {
            line.SetPosition(i, new Vector3(x = Bias() + check_X_(), y += Dec_Y, -0.1f));   // y_up - повышение линии
            Dop++;

            if (SizeLvl - 2 == i)
            {
                //перемещение CircleOfCreate  на пред последнее место
                LastPosition = new Vector3(x, y, -0.1f);
                CircleObject.transform.position = new Vector3(x, y, -0.1f);
            }

            if (SizeLvl - 1 == i)
            {
                // последняя линия  должна быть -10, для Poligona
                line.SetPosition(i, new Vector3(x + SizeLvl / 2, y - 100, -0.1f));
            }
        }


        // Grafics
        for (int i = 0; i < line.positionCount - 1; i++)
        {
            CreateGrassStaticObjects(line.GetPosition(i), line.GetPosition(i + 1));
            CreateGrassStaticObjects((line.GetPosition(i) + line.GetPosition(i + 1)) / 2, line.GetPosition(i + 1));

            int rnd = Random.Range(0, 101);
            if (rnd <= 5)
                CreateTreeStaticObjects(line.GetPosition(i));

        }

    }
    private void CreateTreeStaticObjects(Vector3 PointOfCreate)
    {
        GameObject Inst;
        Inst = Instantiate(prefabs_Tree_Summer[Random.Range(0, 4)], PointOfCreate, transform.rotation, InstLine.transform);
    }


    private void CreateGrassStaticObjects(Vector3 PointOfCreate, Vector3 Direction)
    {
        int rnd = UnityEngine.Random.Range(0, 2);
        GameObject Inst;

        switch (rnd)
        {
            case 0:
                Inst = Instantiate(prefabs_Grass[0], PointOfCreate, transform.rotation, InstLine.transform);
                MoveEnemyStatic.DirectionTowards(Inst, Direction);
                break;

            case 1:
                Inst = Instantiate(prefabs_Grass[1], PointOfCreate, transform.rotation, InstLine.transform);
                MoveEnemyStatic.DirectionTowards(Inst, Direction);
                break;

            default:
                Inst = Instantiate(prefabs_Grass[2], PointOfCreate, transform.rotation, InstLine.transform);
                MoveEnemyStatic.DirectionTowards(Inst, Direction);
                break;
        }
    }

    private void CreatePoligonCollider()
    {
        var Pos = new Vector3[line.positionCount];
        line.GetPositions(Pos);

        var Poligon = line.GetComponent<PolygonCollider2D>();  // колайдер полигона 
        Poligon.points = Pos.Select(p => (Vector2)p).ToArray();

        // CreateMesh
        S_MeshCreate.Start_S_CreateMesh(Pos);
    }

    // смещение по иксу
    private float Bias() // 2.7 - статическое смещение
    {
        return (Dop + x) / 2.1f;
    }


    private float check_X_()
    {
        i++;
        return ((Mathf.Abs(Mathf.Sin(i - 40)) * 10) + Dec_B) % Dec_C; // 2,4;
    }
    #endregion
}
