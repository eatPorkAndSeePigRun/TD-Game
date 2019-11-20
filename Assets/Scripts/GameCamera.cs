﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCamera : MonoBehaviour
{
    public static GameCamera Instance = null;

    protected float m_distance = 15;
    protected Vector3 m_rot = new Vector3(-55, 180, 0);
    protected float m_moveSpeed = 60;
    protected float m_vx = 0;
    protected float m_vy = 0;

    protected Transform m_transform;
    protected Transform m_cameraPoint;

    private void Awake()
    {
        Instance = this;
        m_transform = this.transform;
    }

    private void Start()
    {
        m_cameraPoint = CameraPoint.Instance.transform;
        Follow();
    }

    private void LateUpdate()
    {
        Follow();
    }

    void Follow()
    {
        m_cameraPoint.eulerAngles = m_rot;
        m_transform.position = m_cameraPoint.TransformPoint(new Vector3(0, 0, m_distance));
        transform.LookAt(m_cameraPoint);
    }

    public void Control(bool mouse, float mx, float my)
    {
        if (!mouse)
            return;

        m_cameraPoint.eulerAngles = Vector3.zero;
        m_cameraPoint.Translate(-mx, 0, -my);
    }
}
