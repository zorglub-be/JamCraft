using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TriggerEvent : MonoBehaviour
{
    [SerializeField] private string[] _withTags;    
    [SerializeField] private bool _ignoreFriends;    
    [SerializeField] private bool _ignoreFoes;
    [SerializeField] private bool _ignoreTriggers;    
    [SerializeField] private bool _ignoreColliders;
    public GameObjectEvent OnTriggerEntered;
    public GameObjectEvent OnTriggerStay;
    public GameObjectEvent OnTriggerExit;
    private HashSet<GameObject> _alreadyEntered = new HashSet<GameObject>();
    private HashSet<GameObject> _alreadyExited = new HashSet<GameObject>();
    private HashSet<GameObject> _alreadyStayed = new HashSet<GameObject>();

    private void Update()
    {
        _alreadyEntered.Clear();
        _alreadyExited.Clear();
        _alreadyStayed.Clear();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        HandleTrigger(other, OnTriggerEntered, _alreadyEntered);
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        HandleTrigger(other, OnTriggerStay, _alreadyStayed);
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        HandleTrigger(other, OnTriggerExit, _alreadyExited);
    }

    private void HandleTrigger(Collider2D otherCollider, GameObjectEvent callback, HashSet<GameObject> alreadyColliding)
    {
        if(ShouldIgnoreColliderType(otherCollider))
            return;
        var obj = otherCollider?.attachedRigidbody?.gameObject;
        if (obj == null)
            obj = otherCollider.gameObject;
        if (ShouldIgnore(otherCollider, obj, alreadyColliding) == false)
        {
            alreadyColliding.Add(obj);
            callback?.Invoke(obj);
        }
    }
    private bool ShouldIgnoreColliderType(Collider2D other)
    {
        
        if (_ignoreTriggers && other.isTrigger)
        {
            return true;
        }
        return _ignoreColliders && other.isTrigger == false;
    }
    private bool ShouldIgnore(Collider2D otherCollider, GameObject other, HashSet<GameObject> alreadyColliding)
    {
        if (_ignoreFoes || _ignoreFriends)
        {
            //we check if the collider is friendly so some colliders on an object can potentially not trigger while
            //others can (avoids detection triggers from enemy layers to set off unwanted behaviours)
            var isFriend = GameState.Instance.AreFriendly(gameObject, otherCollider.gameObject);
            if (isFriend && _ignoreFriends || !isFriend && _ignoreFoes)
                return true;
        }
        if (alreadyColliding.Contains(other))
            return true;
        if (_withTags.Length ==0 || _withTags.Contains(other.tag))
            return false;
        return true;
    }
}