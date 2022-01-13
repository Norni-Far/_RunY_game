using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UI_DeadMenu : MonoBehaviour
{
    [SerializeField] private GameManager GameManager;
    private S_changeText s_ChangeText;

    public delegate void Delegats();
    public event Delegats event_tuchContinue;
    public event Delegats event_tuchRestart;
    public event Delegats event_tuchMainMenu;
    public event Delegats event_seeReclamaFoeGame;


    //[SerializeField] private GameManager GameManager;
    [SerializeField] private RectTransform PanelOfDead;
    [SerializeField] private Rigidbody2D Panel_DeadMenuRigidbody;

    [Header("UI")]
    [SerializeField] private Text text_forDead;
    [SerializeField] private Text text_forWin;
    [SerializeField] private Text text_lifes;
    [SerializeField] private GameObject panelOfDead;
    [SerializeField] private GameObject panelOfWin;

    private Vector3 Startposition;

    [SerializeField] private SpringJoint2D LeftSpring;
    [SerializeField] private SpringJoint2D RightSpring;

    private int NumberOFChangeSituation;
    private int Lifes;

    private void Awake()
    {
        GameManager.event_minusLife += CheckOstatokHP;
    }

    private void Start()
    {
        Startposition = PanelOfDead.localPosition;
        NumberOFChangeSituation = 0;
        s_ChangeText = gameObject.GetComponent<S_changeText>();
    }

    public void OpenMenu()
    {
        if (Lifes >= 5)
            text_forDead.text = s_ChangeText.chsngeSituation_start(0);
        else
            text_forDead.text = s_ChangeText.chsngeSituation_start(1);

        changeSituation();
        Panel_DeadMenuRigidbody.simulated = true;
    }

    public void openFinishMenu()
    {
        text_forWin.text = s_ChangeText.chsngeSituation_start(2);
        changeSituation();
        panelOfDead.SetActive(false);
        panelOfWin.SetActive(true);
        Panel_DeadMenuRigidbody.simulated = true;
    }

    public void changeSituation()
    {
        switch (NumberOFChangeSituation)
        {
            case 1:
            case 3:
                LeftSpring.distance = 17;
                RightSpring.distance = 17;

                LeftSpring.dampingRatio = 1f;
                RightSpring.dampingRatio = 1;

                LeftSpring.frequency = 1f;
                RightSpring.frequency = 1.5f;

                NumberOFChangeSituation++;
                break;

            default:
                LeftSpring.distance = 17;
                RightSpring.distance = 17;

                LeftSpring.dampingRatio = 1;
                RightSpring.dampingRatio = 1;

                LeftSpring.frequency = 1.5f;
                RightSpring.frequency = 1.5f;

                NumberOFChangeSituation++;
                break;

        }
    }

    private void CheckOstatokHP(int lifes)
    {
        Lifes = lifes;

        text_lifes.text = ("x" + Lifes).ToString();
        if (Lifes == 0)
        {
            text_lifes.color = Color.red;
        }
    }


    // Buttons
    public void btn_Continue()
    {
        if (Lifes > 0)
        {
            event_tuchContinue?.Invoke();
            StartCoroutine(ClosePanelDeadMenu());
        }
        else
        {
            event_seeReclamaFoeGame?.Invoke();
            event_tuchContinue?.Invoke();
            StartCoroutine(ClosePanelDeadMenu());
        }

    }
    public void btn_Restart()
    {
        event_tuchRestart?.Invoke();
        StartCoroutine(ClosePanelDeadMenu());
    }
    public void btn_MainMenu()
    {
        event_tuchMainMenu?.Invoke();
        StartCoroutine(ClosePanelDeadMenu());
    }

    IEnumerator ClosePanelDeadMenu()
    {

        LeftSpring.distance = 2;
        RightSpring.distance = 2;

        LeftSpring.dampingRatio = 1;
        RightSpring.dampingRatio = 1;

        LeftSpring.frequency = 10;
        RightSpring.frequency = 10;

        yield return new WaitForSeconds(0.5f);

        Panel_DeadMenuRigidbody.simulated = false;
        PanelOfDead.anchoredPosition = Startposition;
    }
}
