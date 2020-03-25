using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class TriggerEvent : MonoBehaviour
{
    [SerializeField] private string[] _withTags;    
    [SerializeField] private bool _ignoreFriends;    
    [SerializeField] private bool _ignoreFoes;
    public GameObjectEvent OnTriggerEntered;
    public GameObjectEvent OnTriggerStay;
    public GameObjectEvent OnTriggerExit;
    private HashSet<GameObject> _alreadyColliding = new HashSet<GameObject>();

    private void Update()
    {
        _alreadyColliding.Clear();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        HandleTrigger(other, OnTriggerEntered);
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        HandleTrigger(other, OnTriggerStay);
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        HandleTrigger(other, OnTriggerExit);
    }

    private void HandleTrigger(Collider2D other, GameObjectEvent callback)
    {
        var obj = other?.attachedRigidbody?.gameObject;
        if (obj == null)
            obj = other.gameObject;
        if (ShouldIgnore(obj) == false)
        {
            _alreadyColliding.Add(obj);
            callback?.Invoke(obj);
        }
    }

    private bool ShouldIgnore(GameObject other)
    {
        if (_ignoreFoes || _ignoreFriends)
        {
            var isFriend = GameState.Instance.AreFriendly(gameObject, other);
            if (isFriend && _ignoreFriends || !isFriend && _ignoreFoes)
                return true;
        }
        if (_alreadyColliding.Contains(other))
            return true;
        if (_withTags.Length ==0 || _withTags.Contains(other.tag))
            return false;
        return true;
    }
}