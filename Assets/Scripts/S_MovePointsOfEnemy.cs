using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_MovePointsOfEnemy : MonoBehaviour
{
    [SerializeField] private float Move = 10;
    [SerializeField] private Vector2 offset;
    private Transform Player;

    void LateUpdate()
    {
        if (Player != null)
        {
            Vector3 target;

            target = new Vector3(Player.position.x + offset.x, Player.position.y + offset.y, transform.position.z);

            Vector3 currentPosition = Vector3.Lerp(transform.position, target, Move * Time.deltaTime);
            transform.position = currentPosition;
        }
    }

    public void FindPlayer()
    {
        Player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }
}
