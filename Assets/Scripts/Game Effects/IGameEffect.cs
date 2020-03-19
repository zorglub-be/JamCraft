using System;
using UnityEngine;

public interface IGameEffect
{
    // Executes a game effect and calls the callback delegate when finished
    void Execute(GameObject source, Action callback = null);
}