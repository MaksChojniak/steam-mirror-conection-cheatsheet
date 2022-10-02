using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
public class SpawnManagerClasses : MonoBehaviour
{
    [SerializeField] Text input;

    [Space(9)]
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
    
    public void SetClasses()
    {
        var manager = GameObject.FindGameObjectWithTag(TAG).GetComponent<ManagerClasses>();
        manager.SetClasses(int.Parse(input.text));
    }
}
