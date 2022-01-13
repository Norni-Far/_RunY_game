using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class S_FinishForHero : MonoBehaviour
{
    public delegate void Delegats();
    public event Delegats event_Finish;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
            event_Finish?.Invoke();
    }
}
