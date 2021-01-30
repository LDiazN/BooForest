using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum GameStat
{
    Playing,
    GameOver,
    Victory,
};


public class GameStatus : MonoBehaviour
{
    public static GameStatus Instance { get; private set; }

    private GameStat _status = GameStat.Playing;
    public GameStat Status
    {
        get { return _status; }
    }

    public UnityEvent OnGameOver;
    public UnityEvent OnVictory;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogError("Mira maldito, esto es un singleton ." + gameObject.name);
        }
        Instance = this;
    }


    private void Start()
    {
        PlayerStatus.Instance.death.OnPlayerDeath.AddListener(GameOver);    
    }


    public void GameOver()
    {
        if (_status != GameStat.Playing)
            return;

        _status = GameStat.GameOver;
        OnGameOver.Invoke();
    }


    public void Victory()
    {
        if (_status != GameStat.Playing)
            return;

        _status = GameStat.Victory;
        OnVictory.Invoke();
    }
}
