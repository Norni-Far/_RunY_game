using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class S_CircleOfCreateWorld : MonoBehaviour
{
    public delegate void DelegatsOfCreate(GameObject circle);
    public event DelegatsOfCreate EventCreateWorld;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" ||
            collision.gameObject.tag == "DeadWheel")
            EventCreateWorld?.Invoke(gameObject);
    }




}
