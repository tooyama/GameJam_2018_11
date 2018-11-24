﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour 
{
    [SerializeField]
    bool isChangeCamera = true;
    [SerializeField]
    bool isRumble = true;
    [SerializeField]
    float changeTime = 5.0f;

    CameraManager cameraManager;
    ControllerManager controllerManager;
    RectTransform ui;

    PlayerManager[] playerManagers;

    int targetId = 0;
    float countTime = 0;
    float raitoOfUI;
    float defaultDis;

	void Start () 
    {
        cameraManager = GameObject.Find("Main Camera").GetComponent<CameraManager>();

        ui = GameObject.Find("UI/Timer").GetComponent<RectTransform>();
        raitoOfUI = ui.localScale.x/changeTime;

        playerManagers = GetComponentsInChildren<PlayerManager>();
        foreach(PlayerManager p in playerManagers)
        {
            if (p.GetId() == 0)
            {
                cameraManager.ChangeParent(p.transform);
                p.SetMove(true);
            }
        }

        controllerManager = GetComponent<ControllerManager>();
        defaultDis = Vector3.Distance(playerManagers[0].transform.position, playerManagers[1].transform.position);

        if (isRumble)
        {
            StartCoroutine(WaitForRumble());
        }
	}
	
	void Update () 
    {
        if (isChangeCamera)
        {
            countTime += Time.deltaTime;

            ui.localScale = new Vector3((changeTime - countTime) * raitoOfUI, ui.localScale.y, ui.localScale.z);

            if (changeTime < countTime)
            {
                countTime = 0;

                playerManagers[Mathf.Abs(targetId)].SetMove(false);

                targetId = ~targetId;
                cameraManager.ChangeParent(playerManagers[Mathf.Abs(targetId)].transform);

                playerManagers[Mathf.Abs(targetId)].SetMove(true);
            }
        }
	}

    IEnumerator WaitForRumble()
    {
        yield return new WaitForSeconds(2.0f);

        float dis = Vector3.Distance(playerManagers[0].transform.position, playerManagers[1].transform.position);

        if (20.0f < (defaultDis - dis))
        {
            controllerManager.Rumble(3.0f);
        }
        else if (15.0f < (defaultDis - dis))
        {
            controllerManager.Rumble(1.0f);
        }
        else if (7.5f < (defaultDis - dis))
        {
            controllerManager.Rumble(0.1f);
        }

        StartCoroutine(WaitForRumble());
    }
}
