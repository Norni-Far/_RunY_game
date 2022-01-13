using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_Canvas_justGoPanel : MonoBehaviour
{
    [Header("Scripts")]
    [SerializeField] private S_CanvasMainMenu S_Canvas;
    [SerializeField] private I_CanvasBtn_MainPanel I_MainMenuPanel;
    [SerializeField] private I_CanvasBtn_NewGamePanel I_NewGamePanel;

    [SerializeField] private SpringJoint2D springJoint;

    private RectTransform justGoTransform;
    private Vector3 StartPosition;
    Rigidbody2D rb;

    // для пружин
    private float startDistance;
    private float startDampingRatio;
    private float startFrequence;

    private void Awake()
    {
        // подписки
        S_Canvas.EndloadingGames += openPanel;
        S_Canvas.closeAllPanels += closePanel;
        I_MainMenuPanel.eventNewGame += closePanel;
        I_NewGamePanel.event_ChooseBack += openPanel;
    }

    void Start()
    {
        startDistance = springJoint.distance;
        startDampingRatio = springJoint.dampingRatio;
        startFrequence = springJoint.frequency;

        justGoTransform = gameObject.GetComponent<RectTransform>();
        StartPosition = justGoTransform.anchoredPosition;
        rb = GetComponent<Rigidbody2D>();
    }
    public void openPanel()
    {
        rb.simulated = true;
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

        yield return new WaitForSeconds(1f);

        rb.simulated = false;
        justGoTransform.anchoredPosition = StartPosition;

        springJoint.distance = startDistance;
        springJoint.dampingRatio = startDampingRatio;
        springJoint.frequency = startFrequence;
    }

    #endregion
}
