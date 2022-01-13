using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_CreateTriangle : MonoBehaviour
{

    public GameObject Triangle;
    public Transform PointofCreate;

    private GameObject Nowtriangle;

    bool Ok;
    public void Create()
    {
        if (Ok)
            Instantiate(Triangle, PointofCreate.transform.position, PointofCreate.transform.rotation);
    }

    private void Update()
    {
        Nowtriangle = GameObject.FindGameObjectWithTag("Player")?.gameObject;
        if (Nowtriangle == null)
            Ok = true;
        else
            Ok = false;
    }
}
