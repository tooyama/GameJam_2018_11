using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour 
{
    CameraManager cameraManager;
    PlayerManager[] playerManagers;

    int targetId = 0;

	void Start () 
    {
        cameraManager = GameObject.Find("Main Camera").GetComponent<CameraManager>();
        playerManagers = GetComponentsInChildren<PlayerManager>();

        foreach(PlayerManager p in playerManagers)
        {
            if (p.GetId() == 0)
            {
                cameraManager.ChangeParent(p.transform);
            }
        }
	}
	
	void Update () 
    {
        if(Input.GetKeyDown(KeyCode.Z))
        {
            targetId = ~targetId;
            cameraManager.ChangeParent(playerManagers[Mathf.Abs(targetId)].transform);
        }
	}
}
