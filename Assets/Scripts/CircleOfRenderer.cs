using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D), typeof(SpriteRenderer))]
public class CircleOfRenderer : MonoBehaviour
{
    private SpriteRenderer Sprite;

    private void Start()
    {
        Sprite = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "CFR")
        {
            Sprite.enabled = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "CFR")
        {
            Sprite.enabled = false;
        }
    }

}
