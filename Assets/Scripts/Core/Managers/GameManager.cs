using System;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class GameManager : Singleton<GameManager>
{
    protected override void Awake()
    {
        base.Awake();
        QueenManager.OnQueenAddedEvent += OnQueenAdded;
    }

    private void OnQueenAdded(Queen queen)
    {
        if (CheckWinCondition())
        {
            // WIN
        }
    }

    public bool CheckWinCondition()
    {
        List<Queen> queens = QueenManager.Instance.Queens;

        if (queens.Count != GridManager.Instance.GridSize)
            return false;

        foreach (var queen in queens)
        {
            if (queen.Conflicts.Count > 0)
                return false;
        }

        return true;
    }
}