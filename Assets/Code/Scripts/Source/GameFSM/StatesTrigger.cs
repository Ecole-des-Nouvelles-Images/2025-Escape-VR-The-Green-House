using System;
using Code.Scripts.Source.GameFSM;
using Code.Scripts.Source.Managers;
using UnityEngine;

public class StatesTrigger : MonoBehaviour
{
    private bool _isTriggered = false;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (!_isTriggered)
        {
            SwitchStates();
        }

    }

    private void SwitchStates()
    {
        Debug.Log("Switch States trigerred");
        GameStateManager.Instance.SwitchState( GameStateManager.Instance.GameStates.LoungeIntro); // à supprimer
        _isTriggered = true;
    }
}
