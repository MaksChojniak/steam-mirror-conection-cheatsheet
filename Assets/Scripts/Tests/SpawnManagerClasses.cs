using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManagerClasses : MonoBehaviour
{
    [SerializeField] private GameObject managerClasses;
    private string TAG = "ClassesManager";

    void Start()
    {
        if(GameObject.FindGameObjectWithTag(TAG) == null)
        {
            GameObject manager = Instantiate(managerClasses);
            DontDestroyOnLoad(manager);
        }
    }
}
