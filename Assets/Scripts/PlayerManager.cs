﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour 
{
    public static readonly float THRESHOLD = 0.1f;
    
    [SerializeField]
    int id = 0;
    [SerializeField]
    float speed = 1.0f;
    [SerializeField]
    string markObjectName;
    [SerializeField]
    string trapObjectName;
    [SerializeField]
    Transform[] startPositions; 

    GameManager gameManager;
    ControllerManager controllerManager;
    Animator animator;
    Rigidbody rb;
    GameObject markObject;
    GameObject trapObject;
    bool isRight;
    bool canMove = false;
    bool isBomb = false;

    void Start()
    {
        if(id == 0)
        {
            isRight = true;
        }
        else
        {
            isRight = false;
        }

        transform.position = startPositions[Random.Range(0, startPositions.Length)].position;

        gameManager = transform.parent.GetComponent<GameManager>();
        animator = GetComponent<Animator>();
        controllerManager = transform.parent.GetComponent<ControllerManager>();
        rb = GetComponent<Rigidbody>();
        markObject = Resources.Load(markObjectName) as GameObject;
        trapObject = Resources.Load(trapObjectName) as GameObject;
    }

    public int Id
    {
        get
        {
            return id;
        }
        set
        {
            id = value;
        }
    }

    public bool CanMove
    {
        get
        {
            return canMove;
        }
        set
        {
            canMove = value;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag.Equals("Player"))
        {
            canMove = false;
            gameManager.End = true;
        }

        if (other.tag.Equals("Bomb"))
        {
            if(other.GetComponent<BombManager>().Id != id)
            {
                Destroy(other.gameObject);
                isBomb = true;
                StartCoroutine(WaitForMove());
            }
        }
    }

    void Update()
    {
        if(isRight && controllerManager.GetButtonName(isRight).Equals("DPAD_RIGHT"))
        {
            Instantiate(markObject,transform.position,transform.rotation);
        }

        if (!isRight && controllerManager.GetButtonName(isRight).Equals("DPAD_LEFT"))
        {
            Instantiate(markObject, transform.position, transform.rotation);
        }

        if (isRight && controllerManager.GetButtonName(isRight).Equals("DPAD_UP"))
        {
            GameObject trap = Instantiate(trapObject, transform.position, trapObject.transform.rotation) as GameObject;
            trap.GetComponent<BombManager>().Id = id;
        }

        if (!isRight && controllerManager.GetButtonName(isRight).Equals("DPAD_DOWN"))
        {
            GameObject trap = Instantiate(trapObject, transform.position, trapObject.transform.rotation);
            trap.GetComponent<BombManager>().Id = id;
        }
    }

    void FixedUpdate()
    {
        if (canMove && !isBomb)
        {
            float x = controllerManager.GetStick(isRight).x;
            float z = controllerManager.GetStick(isRight).y;

            if (Mathf.Abs(x) > Mathf.Abs(z))
            {
                if (x < -THRESHOLD)
                {
                    transform.localEulerAngles = new Vector3(0, 90.0f, 0);
                }
                else if (THRESHOLD < x)
                {
                    transform.localEulerAngles = new Vector3(0, -90.0f, 0);
                }

                animator.SetFloat("MoveSpeed", x);
            }
            else
            {
                if (z < -THRESHOLD)
                {
                    transform.localEulerAngles = new Vector3(0, 180.0f, 0);
                }
                else if (THRESHOLD < z)
                {
                    transform.localEulerAngles = new Vector3(0, 0.0f, 0);
                }

                animator.SetFloat("MoveSpeed", z);
            }

            Vector3 distance = new Vector3(-x, 0, z);

            rb.velocity = distance * speed;
        }
    }

    IEnumerator WaitForMove()
    {
        yield return new WaitForSeconds(5.0f);

        isBomb = false;
    }
}
