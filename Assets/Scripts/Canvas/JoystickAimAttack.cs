using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class JoystickAimAttack : MonoBehaviour, IPointerDownHandler, IDragHandler
{
    public GameObject Aim;
    public GameObject Hungle;

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        Transform Last = Aim.transform;
        Aim.transform.position = Hungle.transform.position;
        if (Aim.transform.position.x == 0 &&
            Aim.transform.position.y == 0)
            Aim.transform.position = Last.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Aim.transform.position = Hungle.transform.position;
    }
}
