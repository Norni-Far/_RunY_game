using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_Canvas_upMeuForCodeAndPlay : MonoBehaviour
{
    [Header("Scripts")]
    [SerializeField] private S_CanvasMainMenu S_Canvas;
    [SerializeField] private I_CanvasBtn_NewGamePanel I_newGamePanel;

    [SerializeField] private SpringJoint2D springJoint;

    [Header("Second")]
    [SerializeField] private GameObject SecondTable;
    [SerializeField] private SpringJoint2D springJoint_1;
    Rigidbody2D rb_1;


    private RectTransform firstUpTransform;
    private RectTransform SecondUpTransform;

    private Vector3 StartPositionFirst;
    private Vector3 StartPositionSecond;

    Rigidbody2D rb;
    // для первой 
    private float startDistance;
    private float startDampingRatio;
    private float startFrequence;

    // для второй
    private float startDistance_1;
    private float startDampingRatio_1;
    private float startFrequence_1;


    private void Awake()
    {
        // подписки
        S_Canvas.closeAllPanels += closePanel;
        I_newGamePanel.event_ChooseBack += closePanel;
        I_newGamePanel.event_ChooseOnelvlGame += openPanel;
        I_newGamePanel.event_ChooseTwolvlGame += openPanel;
        I_newGamePanel.event_ChooseThreelvlGame += openPanel;
        I_newGamePanel.event_ChooseHardcorelvlGame += openPanel;
    }

    void Start()
    {
        startDistance = springJoint.distance;
        startDampingRatio = springJoint.dampingRatio;
        startFrequence = springJoint.frequency;

        startDistance_1 = springJoint_1.distance;
        startDampingRatio_1 = springJoint_1.dampingRatio;
        startFrequence_1 = springJoint_1.frequency;

        firstUpTransform = gameObject.GetComponent<RectTransform>();
        SecondUpTransform = SecondTable.GetComponent<RectTransform>();

        StartPositionFirst = firstUpTransform.anchoredPosition;
        StartPositionSecond = SecondUpTransform.anchoredPosition;

        rb = GetComponent<Rigidbody2D>();
        rb_1 = SecondTable.GetComponent<Rigidbody2D>();
    }

    public void StartGame()
    {
        print("Go");
    }

    public void openPanel()
    {
        rb.simulated = true;
        rb_1.simulated = true;
    }

    public void closePanel()
    {
        StartCoroutine(startClosepanel());
    }

    #region ClosePanel
    IEnumerator startClosepanel()
    {
        springJoint.distance = 2;
        springJoint.dampingRatio = 1;
        springJoint.frequency = 10;

        springJoint_1.distance = 2;
        springJoint_1.dampingRatio = 1;
        springJoint_1.frequency = 10;

        yield return new WaitForSeconds(1f);

        rb.simulated = false;
        rb_1.simulated = false;

        firstUpTransform.anchoredPosition = StartPositionFirst;
        SecondUpTransform.anchoredPosition = StartPositionSecond;

        springJoint.distance = startDistance;
        springJoint.dampingRatio = startDampingRatio;
        springJoint.frequency = startFrequence;

        springJoint_1.distance = startDistance_1;
        springJoint_1.dampingRatio = startDampingRatio_1;
        springJoint_1.frequency = startFrequence_1;
    }
    #endregion
}
