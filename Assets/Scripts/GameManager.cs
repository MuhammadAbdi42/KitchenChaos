using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    private enum State
    {
        WaitingToStart,
        CountdownToStart,
        GamePlaying,
        GameOver,
    }
    private State state;
    private float waitingToStartTimer = 1f;
    private float countdownToStartTimer = 3f;
    private float gamePlayingTimerMax = 600f;
    public int playerHeartsMax = 3;
    public int playerHearts;
    private float gamePlayingTimer;
    public event EventHandler OnGameStateChanged;
    public event EventHandler OnHeartLost;
    private void Awake()
    {
        Instance = this;
        state = State.WaitingToStart;
        playerHearts = playerHeartsMax;
    }
    private void Update()
    {
        switch (state)
        {
            case State.WaitingToStart:
                waitingToStartTimer -= Time.deltaTime;
                if (waitingToStartTimer <= 0f)
                {
                    state = State.CountdownToStart;
                    OnGameStateChanged?.Invoke(this, EventArgs.Empty);
                }
                break;
            case State.CountdownToStart:
                countdownToStartTimer -= Time.deltaTime;
                if (countdownToStartTimer <= 0f)
                {
                    gamePlayingTimer = gamePlayingTimerMax;
                    state = State.GamePlaying;
                    OnGameStateChanged?.Invoke(this, EventArgs.Empty);

                }
                break;
            case State.GamePlaying:
                gamePlayingTimer -= Time.deltaTime;
                if (gamePlayingTimer <= 0f)
                {
                    state = State.GameOver;
                    OnGameStateChanged?.Invoke(this, EventArgs.Empty);

                }
                break;
            case State.GameOver:
                break;
        }
        Debug.Log(state);
    }
    public bool IsCountdownToStartActive()
    {
        return state == State.CountdownToStart;
    }
    public bool IsGamePlaying()
    {
        return state == State.GamePlaying;
    }
    public bool IsGameOver()
    {
        return state == State.GameOver;
    }
    public float GetCountdownTimer()
    {
        return countdownToStartTimer;
    }
    public float GetGamePlayingTimerNormalized()
    {
        return (gamePlayingTimer / gamePlayingTimerMax);
    }
    public void SetStateToGameOver()
    {
        state = State.GameOver;
        OnGameStateChanged?.Invoke(this, EventArgs.Empty);
    }
    public void DeliveryFailedInTime()
    {
        playerHearts--;
        if (playerHearts == 0)
        {
            SetStateToGameOver();
        }
        OnHeartLost?.Invoke(this, EventArgs.Empty);
    }
    public int GetHeartsLeft()
    {
        return playerHearts;
    }
}
