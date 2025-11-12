using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

namespace Code.Scripts.Source.XR
{
    [RequireComponent(typeof(XRGrabInteractable))]
    public class AutoReleaseOnDistance : MonoBehaviour
    {
        [Tooltip("Distance max autorisée entre la main et le collider d’interaction avant drop")]
        public float maxGrabDistance = 0.175f;

        XRGrabInteractable _grabInteractable;
        IXRSelectInteractor _currentSelectInteractor;
        XRBaseInteractor _currentBaseInteractor;
        Collider _referenceCollider;
        bool _readyToCheck = false;

        void Awake()
        {
            _grabInteractable = GetComponent<XRGrabInteractable>();

            if (_grabInteractable.colliders != null && _grabInteractable.colliders.Count > 0)
            {
                _referenceCollider = _grabInteractable.colliders[0];
            }
            else
            {
                _referenceCollider = GetComponent<Collider>();
            }

            _grabInteractable.selectEntered.AddListener(OnGrab);
            _grabInteractable.selectExited.AddListener(OnRelease);
        }

        void OnDestroy()
        {
            _grabInteractable.selectEntered.RemoveListener(OnGrab);
            _grabInteractable.selectExited.RemoveListener(OnRelease);
        }

        void OnGrab(SelectEnterEventArgs args)
        {
            _currentSelectInteractor = args.interactorObject;
            _currentBaseInteractor = args.interactorObject as XRBaseInteractor;

            _readyToCheck = false;
            StartCoroutine(EnableCheckNextFrame());
        }

        IEnumerator EnableCheckNextFrame()
        {
            yield return null;
            _readyToCheck = true;
        }

        void OnRelease(SelectExitEventArgs args)
        {
            _currentSelectInteractor = null;
            _currentBaseInteractor = null;
            _readyToCheck = false;
        }

        void Update()
        {
            if (!_readyToCheck || _currentBaseInteractor == null || _referenceCollider == null)
                return;

            float dist = Vector3.Distance(
                _currentBaseInteractor.transform.position,
                _referenceCollider.bounds.center
            );

            if (dist > maxGrabDistance)
            {
                _grabInteractable.interactionManager
                    .SelectExit(_currentSelectInteractor, (IXRSelectInteractable)_grabInteractable);

                _currentSelectInteractor = null;
                _currentBaseInteractor = null;
                _readyToCheck = false;
            }
        }
    }
}
