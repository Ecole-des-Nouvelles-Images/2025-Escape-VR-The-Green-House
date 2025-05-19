using UnityEngine;

namespace Code.Scripts.Source.XR
{
  public interface IRespawnable
  {
      public void Respawn();
      public void OnTriggerExit(Collider other);
  }
}
