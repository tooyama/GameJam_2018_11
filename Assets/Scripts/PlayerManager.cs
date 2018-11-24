using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour 
{
    [SerializeField]
    int id = 0;

    public int GetId()
    {
        return id;
    }
}
