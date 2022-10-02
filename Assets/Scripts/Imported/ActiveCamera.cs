using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;

public class ActiveCamera : NetworkBehaviour
{
    [SerializeField] private GameObject followCamera;


    public override void OnStartAuthority()
    {
        followCamera.SetActive(true);
    }

    /*void Update()
    {
        if(SceneManager.GetActiveScene() == SceneManager.GetSceneByBuildIndex(2) && followCamera.activeSelf == false)
        {
            followCamera.SetActive(true);
        }
    }*/
}
