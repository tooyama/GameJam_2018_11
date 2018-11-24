using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour 
{
    [SerializeField]
    bool changeCamera = true;
    [SerializeField]
    float changeTime = 5.0f;

    CameraManager cameraManager;
    RectTransform ui;

    PlayerManager[] playerManagers;

    int targetId = 0;
    float countTime = 0;
    float raitoOfUI;

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
            }
        }
	}
	
	void Update () 
    {
        if (changeCamera)
        {
            countTime += Time.deltaTime;

            ui.localScale = new Vector3((changeTime - countTime) * raitoOfUI, ui.localScale.y, ui.localScale.z);

            if (changeTime < countTime)
            {
                countTime = 0;

                targetId = ~targetId;
                cameraManager.ChangeParent(playerManagers[Mathf.Abs(targetId)].transform);
            }
        }
	}
}
