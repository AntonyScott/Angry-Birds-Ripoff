using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    [SerializeField] string _nextLevelName;

    Monster[] _monsters;

    void OnEnable()
    {
        _monsters = FindObjectsOfType<Monster>();
    }
    // Update is called once per frame
    void Update()
    {
        if (NoMoreMonsters())
            NextLevel();
    }

    void NextLevel()
    {
        SceneManager.LoadScene(_nextLevelName);
    }

    bool NoMoreMonsters()
    {
        foreach (var monster in _monsters)
        {
            if (monster.gameObject.activeSelf)
                return false;
        }
        return true;
    }
}
