using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;

public class PlayerMovementController : NetworkBehaviour
{
    public float speed = 0.1f;
    public GameObject playerModel;



    void Start()
    {
        playerModel.SetActive(false);    
    }

    void Update()
    {
        Movement();

        if (SceneManager.GetActiveScene().name == "GameScene")
        {
            if (playerModel.activeSelf == false)
            {
                SetPosition();
                playerModel.SetActive(true);
            }

            if (hasAuthority)
            {
                Movement();
            }
        }
    }


    public void SetPosition()
    {
        transform.position = new Vector3(Random.Range(-6,6), 0.8f, Random.Range(-6, 6));
    }

    public void Movement()
    {
        float xDirection = Input.GetAxis("Horizontal");
        float zDirection = Input.GetAxis("Vertical");

        Vector3 moveDirection = new Vector3(xDirection, 0.0f ,zDirection);

        transform.position += moveDirection * speed;
    }
}
