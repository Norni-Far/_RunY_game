using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Decoding: MonoBehaviour
{
    [HideInInspector] public float y_up;
    [HideInInspector] public float b;
    [HideInInspector] public float c;

    public delegate void DecodingMethod(string Code);
    public event DecodingMethod event_DecodingMethod;

    private string[] a = new string[3];

    public void DecodingKey(string Key)
    {
        Key.ToUpper();
        int g = 0;

        for (int i = 0; i < Key.Length; i++)
        {
            switch (Key[i].ToString().ToUpper())
            {
                case "Y":
                    i++;
                    g = 0;
                    break;

                case "B":
                    i++; ;
                    g = 1;
                    break;

                case "C":
                    i++;
                    g = 2;
                    break;
            }

            a[g] += Key[i];
        }

        y_up = Convert.ToSingle(a[0]) / 100;
        b = Convert.ToSingle(a[1]) / 10;
        c = Convert.ToSingle(a[2]) / 10;
    }

}

