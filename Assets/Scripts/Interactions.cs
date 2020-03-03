using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.UI;

public class Interactions : MonoBehaviour
{
    [SerializeField] private GameObject PopupTextPrefab;
    private GameObject objectToDestroy;
    private bool inTrigger = false;
    private void OnTriggerEnter2D(Collider2D collider)
    {
        inTrigger = true;
        objectToDestroy =  Instantiate(PopupTextPrefab, new Vector2(transform.position.x, transform.position.y + 3), Quaternion.identity);
    }
    
    private void OnTriggerExit2D(Collider2D collider)
    {
        inTrigger = false;
        Destroy(objectToDestroy);
    }

    private void Update()
    {
        if (inTrigger)
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                Destroy(objectToDestroy);
                Destroy(gameObject);
            }
        }
    }
}
