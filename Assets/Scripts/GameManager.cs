﻿using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;
    public LayerMask m_groundlayer;

    public int m_wave = 1;
    public int m_waveMax = 10;
    public int m_life = 10;
    public int m_point = 30;

    Text m_txt_wave;
    Text m_txt_life;
    Text m_txt_point;
    Button m_but_try;

    bool m_isSelectedButton = false;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        UnityAction<BaseEventData> downAction = new UnityAction<BaseEventData>(OnButCreateDefenderDown);
        UnityAction<BaseEventData> upAction = new UnityAction<BaseEventData>(OnButCreateDefenderUp);

        EventTrigger.Entry down = new EventTrigger.Entry();
        down.eventID = EventTriggerType.PointerDown;
        down.callback.AddListener(downAction);

        EventTrigger.Entry up = new EventTrigger.Entry();
        up.eventID = EventTriggerType.PointerUp;
        up.callback.AddListener(upAction);

        foreach (Transform t in this.GetComponentsInChildren<Transform>())
        {
            if (t.name.CompareTo("wave") == 0)
            {
                m_txt_wave = t.GetComponent<Text>();
                SetWave(1);
            }
            else if (t.name.CompareTo("life") == 0)
            {
                m_txt_life = t.GetComponent<Text>();
                m_txt_life.text = string.Format("生命：<color=yellow>{0}</color>", m_life);
            }
            else if (t.name.CompareTo("point") == 0)
            {
                m_txt_point = t.GetComponent<Text>();
                m_txt_point.text = string.Format("铜钱：<color=yellow>{0}</color>", m_point);
            }
            else if (t.name.CompareTo("but_try") == 0)
            {
                m_but_try = t.GetComponent<Button>();
                m_but_try.onClick.AddListener(delegate ()
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                });
                m_but_try.gameObject.SetActive(false);
            }
            else if (t.name.Contains("but_player"))
            {
                EventTrigger trigger = t.gameObject.AddComponent<EventTrigger>();
                trigger.triggers = new List<EventTrigger.Entry>();
                trigger.triggers.Add(down);
                trigger.triggers.Add(up);
            }
        }
    }

    public void SetWave(int wave)
    {
        m_wave = wave;
        m_txt_wave.text = string.Format("波数：<color=yellow>{0}/{1}</color>", m_wave, m_waveMax);
    }

    public void SetDamage(int damage)
    {
        m_life -= damage;
        if (m_life <= 0)
        {
            m_life = 0;
            m_but_try.gameObject.SetActive(true);
        }
        m_txt_life.text = string.Format("生命：<color=yellow>{0}</color>", m_life);
    }

    public bool SetPoint(int point)
    {
        if (m_point + point < 0)
            return false;
        m_point += point;
        m_txt_point.text = string.Format("铜钱：<color=yellow>{0}</color>", m_point);
        return true;
    }

    void OnButCreateDefenderDown(BaseEventData data)
    {
        m_isSelectedButton = true;
    }

    void OnButCreateDefenderUp(BaseEventData data)
    {
        GameObject go = data.selectedObject;
    }

    private void Update()
    {
        if (m_isSelectedButton)
            return;

#if (UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR
        bool press = Input.touches.Length > 0 ? true : false;
        float mx = 0;
        float my = 0;
        if (press)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                mx = Input.GetTouch(0).deltaPosition.x * 0.01f;
                my = Input.GetTouch(0).deltaPosition.y * 0.01f;
            }
        }
#else
        bool press = Input.GetMouseButton(0);
        float mx = Input.GetAxis("Mouse X");
        float my = Input.GetAxis("Mouse Y");
#endif
        GameCamera.Instance.Control(press, mx, my);
    }
}
