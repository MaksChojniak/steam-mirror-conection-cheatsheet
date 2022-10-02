using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerClasses : MonoBehaviour
{
    public static int classes;

    [SerializeField] private int currValue;
    [SerializeField] private int value;
    [SerializeField] private bool update;

    void Update()
    {
        currValue = classes;

        if(update == true)
        {
            classes = value;
            update = false;
        }
    }


}
