using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayingClockUI : MonoBehaviour
{
    [SerializeField] private Image timerClockImage;
    private void Start()
    {
        timerClockImage.fillAmount = 1f;
    }
    private void Update()
    {
        if (GameManager.Instance.IsGamePlaying())
        {
            timerClockImage.fillAmount = GameManager.Instance.GetGamePlayingTimerNormalized();
        }
    }
}
