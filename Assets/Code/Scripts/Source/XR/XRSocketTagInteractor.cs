using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

namespace Code.Scripts.Source.XR
{
    public class XRSocketTagInteractor : XRSocketInteractor
    {
      [SerializeField] private string _socketTargetTag;

      public override bool CanHover(IXRHoverInteractable interactable)
      {
          return base.CanHover(interactable) && interactable.transform.CompareTag(_socketTargetTag);
      }

      public override bool CanSelect(IXRSelectInteractable interactable)
      {
          return base.CanSelect(interactable) && interactable.transform.CompareTag(_socketTargetTag);
      }
    }
}
