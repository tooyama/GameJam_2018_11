using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour 
{
    Vector3 defaultAngle;

    void Start()
    {
        defaultAngle = transform.eulerAngles;
    }

    public void ChangeParent(Transform parent)
    {
        transform.parent = parent;
        Vector3 pos = parent.transform.position;
        transform.position = new Vector3(pos.x,transform.position.y,pos.z);
    }

	void FixedUpdate () 
    {
        transform.eulerAngles = defaultAngle;
	}
}
