using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ControllerManager : MonoBehaviour
{
    [SerializeField]
    Vector2 rumbleAngle = Vector2.zero;

    static readonly Joycon.Button[] m_buttons =Enum.GetValues(typeof(Joycon.Button)) as Joycon.Button[];

    List<Joycon> m_joycons;
    Joycon joyconL;
    Joycon joyconR;
    Joycon.Button? pressedButtonL;
    Joycon.Button? pressedButtonR;

    void Start()
    {
        m_joycons = JoyconManager.Instance.j;

        if (m_joycons == null || m_joycons.Count <= 0) return;

        joyconL = m_joycons.Find(c => c.isLeft);
        joyconR = m_joycons.Find(c => !c.isLeft);
    }

    public void Rumble(float force)
    {
        joyconR.SetRumble(rumbleAngle.x, rumbleAngle.y, force, 200);
        joyconL.SetRumble(rumbleAngle.x, rumbleAngle.y, force, 200);
    }

    public Vector2 GetStick(bool isRight)
    {
        if (isRight)
        {
            return new Vector2(joyconR.GetStick()[1], joyconR.GetStick()[0]);
        }
        else
        {
            return new Vector2(-joyconL.GetStick()[1], -joyconL.GetStick()[0]);
        }
    }

    public String GetButtonName(bool isRight)
    {
        if (isRight)
        {
            return pressedButtonR.ToString();
        }
        else
        {
            return pressedButtonL.ToString();
        }
    }

    void Update()
    {
        pressedButtonL = null;
        pressedButtonR = null;

        if (m_joycons == null || m_joycons.Count <= 0) return;

        foreach (var button in m_buttons)
        {
            if (joyconL.GetButton(button))
            {
                pressedButtonL = button;
            }
            if (joyconR.GetButton(button))
            {
                pressedButtonR = button;
            }
        }
    }
}