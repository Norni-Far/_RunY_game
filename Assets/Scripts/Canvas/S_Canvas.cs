using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class S_Canvas : MonoBehaviour
{
    // TEST
    public Text Test_Text_MaxFall;
    public Text Test_Text_Hp;
    //

    public delegate void Delegats(bool Flag);
    public event Delegats event_Flag;

    [SerializeField] private GameManager GameManager;

    [Header("Canvas")]
    [SerializeField] private GameObject JoystickAttack;
    [SerializeField] private GameObject JoystickMove;
    [SerializeField] private GameObject btn_Flag;
    [SerializeField] private Image image_Flag;
    [SerializeField] private RectTransform FlagOfDistance;
    [SerializeField] private RectTransform HeroOfDistance;


    [Header("Objects")]
    [SerializeField] private GameObject Flag;
    [SerializeField] private GameObject Triangle;

    private float MaxFall;
    private bool HeroisLive;
    private bool FlagIsSet;
    private bool isFinish;
    private bool tuchFlag;

    private void Awake()
    {
        GameManager.event_aboutPlayer += HeroIs;
        GameManager.event_needControl += JoystickOn;
        GameManager.event_finish += Finish;
    }

    private void HeroIs(bool player)
    {
        HeroisLive = player;

        if (HeroisLive && !isFinish)
        {
            Triangle = GameObject.FindGameObjectWithTag("Player").gameObject;
            JoystickOn();
            FlagOn();
        }
        else
        {
            JoystickOff();
            FlagOff();
        }
    }

    public void Finish()
    {
        isFinish = true;
    }

    public void ButtonSetFlag()
    {
        if (!tuchFlag)
        {
            tuchFlag = true;
            Flag.SetActive(false);
            Flag.transform.position = Triangle.transform.position;
            Flag.SetActive(true);
            event_Flag?.Invoke(true);
            StartCoroutine(StartWait());
            image_Flag.color = new Color(1, 1, 1, 0.2f);

            FlagOfDistance.position = new Vector2(HeroOfDistance.position.x, FlagOfDistance.position.y);
        }
    }

    IEnumerator StartWait()
    {
        yield return new WaitForSeconds(30f);
        image_Flag.color = new Color(1, 1, 1, 1);
        tuchFlag = false;
    }

    public void JoystickOn()
    {
        JoystickAttack.SetActive(true);
        JoystickMove.SetActive(true);
    }

    public void JoystickOff()
    {
        JoystickAttack.SetActive(false);
        JoystickMove.SetActive(false);
    }
    public void FlagOff() => btn_Flag.SetActive(false);

    public void FlagOn() => btn_Flag.SetActive(true);


    ///// TEST  /////// 

    public void UpdateMaxFall(float Max)
    {
        if (MaxFall < Max)
            MaxFall = Max;

        Test_Text_MaxFall.text = "MaxFall: " + MaxFall;
    }

    public void UpdateMaxFall(int Hp)
    {
        Test_Text_Hp.text = "Hp: " + Hp;
    }

}
