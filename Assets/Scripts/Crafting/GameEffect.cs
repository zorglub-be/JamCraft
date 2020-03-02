using System.Runtime.InteropServices;
using UnityEngine;

public abstract class GameEffect : ScriptableObject, IGameEffect
{
    protected static GameState gameState;

    public void LoadGameState()
    {
        if (gameState == null)
        {
            gameState = FindObjectOfType<GameState>();            
        }
    }

    public abstract void Execute();
}