using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_Attacklojstick : MonoBehaviour
{
    //
    private S_Triangle S_Triangle;
    //

    [SerializeField] private GameObject Hungle;

    private Vector3 Direction;
    private float Target;
    private bool TargetOn;
    void Start()
    {

    }

    public void UpdateTriangle(S_Triangle Triangle)
    {
        S_Triangle = Triangle;
    }

    void Update()
    {
        if (Hungle.transform.localPosition != new Vector3(0, 0, 0))
        {
            TargetOn = true;
            Direction = new Vector3(-Hungle.transform.localPosition.x, Hungle.transform.localPosition.y);
            S_Triangle.Attack_Direction(Direction, TargetOn);
        }

        if (Hungle.transform.localPosition == new Vector3(0, 0, 0) && TargetOn)
        {
            TargetOn = false;
            S_Triangle.Attack_Fire();
        }
    }

}
