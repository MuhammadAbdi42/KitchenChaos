using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class HeartsUI : MonoBehaviour
{
    [SerializeField] Transform healthyHeartTemplate;
    [SerializeField] Transform brokenHeartTemplate;
    [SerializeField] Transform container;
    private void Awake()
    {
        healthyHeartTemplate.gameObject.SetActive(false);
        brokenHeartTemplate.gameObject.SetActive(false);
    }
    private void Start()
    {
        GameManager.Instance.OnHeartLost += GameManager_OnHeartLost;
        HandleEvent();
    }

    private void GameManager_OnHeartLost(object sender, EventArgs e)
    {
        HandleEvent();
    }
    private void HandleEvent()
    {
        foreach (Transform child in container)
        {
            if (child == healthyHeartTemplate || child == brokenHeartTemplate) continue;
            Destroy(child.gameObject);
        }
        for (int i = 0; i < GameManager.Instance.GetHeartsLeft(); i++)
        {
            Transform healthyHeart = Instantiate(healthyHeartTemplate, container);
            healthyHeart.gameObject.SetActive(true);
        }
        for (int i = 0; i < GameManager.Instance.playerHeartsMax - GameManager.Instance.GetHeartsLeft(); i++)
        {
            Transform brokenHeart = Instantiate(brokenHeartTemplate, container);
            brokenHeart.gameObject.SetActive(true);
        }
    }

}
