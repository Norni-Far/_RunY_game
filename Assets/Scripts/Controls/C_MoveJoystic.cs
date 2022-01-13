using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_MoveJoystic : MonoBehaviour
{
    //
    public S_Triangle S_Triangle;
    //

    [SerializeField] private GameObject Hungle;

    [SerializeField] private float MaxPosition = 128;
    [SerializeField] private float FloatTimeToupdateJomp = 0.2f;
    private bool f = false;

    private void Start()
    {

    }

    public void UpdateTriangle(S_Triangle Triangle)
    {
        S_Triangle = Triangle;
        Hungle.transform.localPosition = Vector3.zero;
        StartCoroutine(StartCheking());
    }

    IEnumerator StartCheking()
    {
        while (true)
        {
            if (S_Triangle != null)
            {
                if (Hungle.transform.localPosition.x > 5 || Hungle.transform.localPosition.x < -5 ||
               Hungle.transform.localPosition.y > 5 || Hungle.transform.localPosition.y < -5)
                {
                    S_Triangle.ForceOfJojstik = CheckForceOFJoistick(Hungle.transform.localPosition.x, Hungle.transform.localPosition.y);
                    S_Triangle.Jump();
                }
            }

            yield return new WaitForSeconds(0.2f);
        }
    }

    private float CheckForceOFJoistick(float x, float y)
    {
        float Forse = 0;

        if (Mathf.Abs(x) > Mathf.Abs(y))
            Forse = Mathf.Abs(x);
        else
            Forse = Mathf.Abs(y);

        Forse = (Forse * 100 / MaxPosition) / 100;

        return Forse;
    }


}
