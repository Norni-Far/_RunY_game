using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_MainCamera : MonoBehaviour
{
    [SerializeField] private float dumping;
    [SerializeField] private Vector2 offset;
    [SerializeField] private Transform Player;

    [SerializeField] bool isMainMenu;
    void Start()
    {
        if (isMainMenu)
            FindWheel();
    }
    void LateUpdate()
    {
        if (Player != null)
        {
            Vector3 target;

            target = new Vector3(Player.position.x + offset.x, Player.position.y + offset.y, transform.position.z);

            Vector3 currentPosition = Vector3.Lerp(transform.position, target, dumping * Time.deltaTime);
            transform.position = currentPosition;
        }
    }

    public void FindPlayer()
    {
        Player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (Player != null)
            transform.position = FindTarget();
    }

    public void FindHeart()
    {
        Player = GameObject.FindGameObjectWithTag("Heart")?.transform;
        Player.transform.tag = "Untagged";
    }

    private Vector3 FindTarget()
    {
        return new Vector3(Player.position.x + offset.x, Player.position.y + offset.y, transform.position.z);
    }

    public void FindWheel()
    {
        Player = GameObject.FindGameObjectWithTag("DeadWheel")?.transform;
        //if (Player != null)
        //    transform.position = FindTarget();
    }
}
