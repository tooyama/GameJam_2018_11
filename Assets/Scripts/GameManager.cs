using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour 
{
    [SerializeField]
    bool isRumble = true;
    [SerializeField]
    float changeTime = 5.0f;

    CameraManager cameraManager;
    ControllerManager controllerManager;
    RectTransform ui;
    Text resultText;

    PlayerManager[] playerManagers;

    int targetId = 0;
    float countTime = 0;
    float raitoOfUI;
    float defaultDis;
    bool isStart = false;
    bool isEnd = false;
    bool isChangeCamera = false;

	void Start () 
    {
        cameraManager = GameObject.Find("Main Camera").GetComponent<CameraManager>();

        ui = GameObject.Find("UI/Timer").GetComponent<RectTransform>();
        resultText = GameObject.Find("UI/Text").GetComponent<Text>();
        raitoOfUI = ui.localScale.x/changeTime;

        playerManagers = GetComponentsInChildren<PlayerManager>();
        foreach(PlayerManager p in playerManagers)
        {
            if (p.Id == 0)
            {
                cameraManager.ChangeParent(p.transform);
            }
        }

        controllerManager = GetComponent<ControllerManager>();
        defaultDis = Vector3.Distance(playerManagers[0].transform.position, playerManagers[1].transform.position);

        if (isRumble)
        {
            StartCoroutine(WaitForRumble());
        }
	}

    public bool End 
    {
        get 
        {
            return isEnd;
        }

        set
        {
            isEnd = value;

            if (isEnd)
            {
                if (targetId == 0)
                {
                    resultText.text = "1P Win !";
                }
                else
                {
                    resultText.text = "2P Win !";
                }
            }
        }
    }

	void Update () 
    {
        if(controllerManager.GetButtonName(true) == "PLUS")
        {
            if(!isStart)
            {
                isStart = true;
                isChangeCamera = true;
                resultText.text = "";

                foreach (PlayerManager p in playerManagers)
                {
                    if (p.Id == 0)
                    {
                        p.CanMove = true;
                    }
                }
            }
        }

        if (isChangeCamera && !isEnd)
        {
            countTime += Time.deltaTime;

            ui.localScale = new Vector3((changeTime - countTime) * raitoOfUI, ui.localScale.y, ui.localScale.z);

            if (changeTime < countTime)
            {
                countTime = 0;

                playerManagers[Mathf.Abs(targetId)].CanMove = false;

                targetId = ~targetId;
                cameraManager.ChangeParent(playerManagers[Mathf.Abs(targetId)].transform);

                playerManagers[Mathf.Abs(targetId)].CanMove = true;
            }
        }


	}

    IEnumerator WaitForRumble()
    {
        yield return new WaitForSeconds(2.0f);

        if (!isEnd)
        {
            float dis = Vector3.Distance(playerManagers[0].transform.position, playerManagers[1].transform.position);

            if (defaultDis*0.9f < (defaultDis - dis))
            {
                controllerManager.Rumble(3.0f);
            }
            else if (defaultDis * 0.7f  < (defaultDis - dis))
            {
                controllerManager.Rumble(1.0f);
            }
            else if (defaultDis * 0.4f  < (defaultDis - dis))
            {
                controllerManager.Rumble(0.1f);
            }

            StartCoroutine(WaitForRumble());
        }
    }
}
