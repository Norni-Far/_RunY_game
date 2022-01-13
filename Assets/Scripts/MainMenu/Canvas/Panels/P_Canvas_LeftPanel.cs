using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
public class P_Canvas_LeftPanel : MonoBehaviour
{
    [Header("Scripts")]
    [SerializeField] private S_CanvasMainMenu S_Canvas;

    [Header("Springs")]
    [SerializeField] private SpringJoint2D upJoint;
    [SerializeField] private SpringJoint2D downJoint;

    private RectTransform leftTransform;
    private Vector3 Startposition;
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
    }

    void Start()
    {
        startDistance = upJoint.distance;
        startDampingRatio = upJoint.dampingRatio;
        startFrequence = upJoint.frequency;

        leftTransform = gameObject.GetComponent<RectTransform>();
        Startposition = leftTransform.anchoredPosition;
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
        upJoint.distance = 2;
        downJoint.distance = 2;

        upJoint.dampingRatio = 1;
        downJoint.dampingRatio = 1;

        upJoint.frequency = 10;
        downJoint.frequency = 10;

        yield return new WaitForSeconds(1f);

        rb.simulated = false;
        leftTransform.anchoredPosition = Startposition;

        upJoint.distance = startDistance;
        downJoint.distance = startDistance;

        upJoint.dampingRatio = startDampingRatio;
        downJoint.dampingRatio = startDampingRatio;

        upJoint.frequency = startFrequence;
        downJoint.frequency = startFrequence;
    }
    #endregion
}
