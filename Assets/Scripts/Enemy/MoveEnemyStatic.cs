using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MoveEnemyStatic
{
    private static string Target = "Player";

    //public static bool PlayerIsLive;  // для пушек, чтобы знать мёрт игрок или жив

    public static GameObject Player;

    public static void DirectionTowards(GameObject gameObject, GameObject Target)
    {
        if (Target != null)
        {
            var direction = (Target.transform.position - gameObject.transform.position).normalized;
            var euler = gameObject.transform.eulerAngles;
            euler.z = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            gameObject.transform.eulerAngles = euler;

        }
    }

    public static void DirectionTowards(GameObject gameObject, Vector3 Target)
    {
        if (Target != null)
        {
            var direction = (Target - gameObject.transform.position).normalized;
            var euler = gameObject.transform.eulerAngles;
            euler.z = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            gameObject.transform.eulerAngles = euler;

        }
    }
    public static GameObject FindPlayer()
    {
        Player = GameObject.FindGameObjectWithTag(Target)?.gameObject;  // ? - проверка на null

        return Player;
    }


}
