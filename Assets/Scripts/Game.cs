using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    public static Game Instance;
    public GameObject m_Initializer;
    public LifeHeartContainer m_LifeHeartContainer;

    private GameObject _currentInitializer;
    private Vector3 _spawnPos;

    [SerializeField] public Text m_AbilityBoard;

    private void OnEnable()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    // Use this for initialization
    void Start()
    {
        _currentInitializer = Instantiate(m_Initializer, transform);
        RememberSpawnPos(_currentInitializer.transform.position);
        Application.targetFrameRate = 60;
        DontDestroyOnLoad(gameObject);
    }

    internal void RememberSpawnPos(Vector3 spawnPos)
    {
        _spawnPos = spawnPos;
    }

    public void Restart()
    {
        var curScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(curScene.name);

        DestroyImmediate(_currentInitializer);
        _currentInitializer = Instantiate(m_Initializer, transform);
        _currentInitializer.transform.position = _spawnPos;
        RememberSpawnPos(_spawnPos);
    }
}
