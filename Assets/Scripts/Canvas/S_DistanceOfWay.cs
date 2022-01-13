using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class S_DistanceOfWay : MonoBehaviour
{
    // Scene
    [SerializeField] private float fullDistance;
    [SerializeField] private float nowDistance;
    [SerializeField] private float passedDistance_Percent;

    [SerializeField] private Transform RealTransformPlayer;
    [SerializeField] private Transform Target;
    [SerializeField] private Transform StartPositionPlayer;

    // Canvas
    [SerializeField] private float fullDistanceCanvas;

    [SerializeField] private RectTransform StartPoint;
    [SerializeField] private RectTransform Finishpoint;
    [SerializeField] private RectTransform HeroPoint;

    private bool StartCheck;
    public float HieghtOfTargetTowardsY = 0;

    public void Start()
    {
        fullDistanceCanvas = CheckFullDistanceCanvas();
    }

    public void CheckDistance()
    {
        CheckPlayer();

        Target.transform.position = new Vector3(RealTransformPlayer.position.x, HieghtOfTargetTowardsY, 0);

        fullDistance = CheckFullDistance();

        StartCheck = true;
    }

    public void CheckPlayer()
    {
        RealTransformPlayer = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    private void Update()
    {
        if (StartCheck && RealTransformPlayer != null)
        {
            Target.position = new Vector3(RealTransformPlayer.position.x, Target.position.y, 0);
            nowDistance = Vector3.Distance(RealTransformPlayer.position, Target.position);
            passedDistance_Percent = CheckPassedDistance_Percent();
            HeroPoint.position = new Vector3(CheckPositionHeroOnCanvas(), HeroPoint.position.y);
        }
    }

    private float CheckFullDistance()
    {
        return Vector3.Distance(StartPositionPlayer.position, Target.position);
    }
    private float CheckFullDistanceCanvas()
    {
        return Mathf.Abs(StartPoint.position.x) + Finishpoint.position.x;
        //return Vector3.Distance(StartPoint.position, Finishpoint.position);
    }

    public float CheckPassedDistance_Percent()
    {
        return Mathf.Abs((fullDistance - nowDistance) * 100 / fullDistance);
    }

    public float CheckNeedMoveDistance_Percent() // для врагов (пройдено)
    {
        return Mathf.Abs(nowDistance * 100 / fullDistance);
    }

    private float CheckPositionHeroOnCanvas()
    {
        return StartPoint.position.x + (passedDistance_Percent * fullDistanceCanvas / 100);
        //return fullDistanceCanvas * passedDistance_Percent / 100;
    }
}
