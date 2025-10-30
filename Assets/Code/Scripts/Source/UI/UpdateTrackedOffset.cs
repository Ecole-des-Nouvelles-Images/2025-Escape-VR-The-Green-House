using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.UI;

namespace Code.Scripts.Source.UI
{
    [RequireComponent(typeof(LazyFollow))]
    public class UpdateTrackedOffset : MonoBehaviour
    {
        private LazyFollow _lazyFollowComponent;
        private XRGrabInteractable _interactable;

        private void Awake()
        {
            _lazyFollowComponent = GetComponent<LazyFollow>();
        }

        public void OnGrab()
        {
            _lazyFollowComponent.targetOffset = _interactable.GetTargetPose().position;
        }
    }
}
