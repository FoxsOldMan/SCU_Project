using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAndCameraController : MonoBehaviour    
{
    public bool isActive = true;

    public Camera PlayerCamObj;
    private ThirdAndFirstPersonCamera playercam;

    public GameObject PlayerObj;
    private PlayerStateAndMovement player;


    // Start is called before the first frame update
    void Start()
    {
        playercam = PlayerCamObj.GetComponent<ThirdAndFirstPersonCamera>();
        player = PlayerObj.GetComponent<PlayerStateAndMovement>();
    }

    // Update is called once per frame
    void Update()
    {

        if (isActive && Time.timeScale > 0)
        {
            player.Move(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), Input.GetKey(KeyCode.LeftShift));
            player.Rotate(Input.GetAxis("Mouse X"));

            player.Jump();

            if (Input.GetMouseButtonDown(0))
                player.UseItem();

            //if (Input.GetKeyDown(KeyCode.H))
            //{
            //    player.GotHit();
            //}
        }

    }

    private void LateUpdate()
    {
        if (isActive)
        {
            playercam.RotateWithTarget(Input.GetAxis("Mouse Y"), PlayerObj);

        }

    }

}
