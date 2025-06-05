using System;
using Code.Scripts.Source.Managers;
using UnityEngine;

namespace Code.Scripts.Source.XR
{
    public class IvyInitializer : MonoBehaviour
    {
        private enum IvyType
        {
            Default,
            Lounge,
            GreenHouse,
        }

        [SerializeField] private IvyType _ivyType;
        private void Start()
        {
            gameObject.SetActive(false);

            if (_ivyType == IvyType.Lounge)
            {
                if (GameStateManager.Instance.CurrentState == GameStateManager.Instance.GameStates.HallResolved) 
                {
                    gameObject.SetActive(true);
                }
            }
            else if (_ivyType == IvyType.GreenHouse)
            {
                if (GameStateManager.Instance.CurrentState == GameStateManager.Instance.GameStates.LoungePhase1) 
                {
                    gameObject.SetActive(true);
                }
            }
        }
    }
}