using System;
using Code.Scripts.Source.Gameplay.Hall;
using UnityEngine;

public class PadLockFollowPlayer : MonoBehaviour
{
   private float _distance; 
   private GameObject _cameraPlayer;

   private void Awake()
   {
      _cameraPlayer = Camera.main.gameObject;
   }

   private void Update()
   {
      _distance = (_cameraPlayer.transform.position - gameObject.transform.position).sqrMagnitude;
      
      if (_distance > 1f ) return;
      gameObject.transform.LookAt(_cameraPlayer.transform);
   }
}
