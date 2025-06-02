using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.XR.Interaction.Toolkit.Interactors.Casters;

namespace Code.Scripts.Source.XR
{
    public class XRFarInteractorCursor : MonoBehaviour
    {
        [SerializeField] private NearFarInteractor _farInteractor;
        [SerializeField] private CurveInteractionCaster _curveInteractionCaster;
        [SerializeField] private GameObject _decalCursorPrefab;
    
        private GameObject _cursorInstance;
        private Vector3 _lastHitPoint;
        private RaycastHit _hit;
        private bool _isInteracting = false;
        private MeshRenderer _cursorRenderer;

        [SerializeField]  private float rayDistance;
        private void OnEnable()
        {
            _farInteractor.hoverEntered.AddListener(OnHoverEnter);
            _farInteractor.hoverExited.AddListener(OnHoverExit);
            _farInteractor.selectEntered.AddListener(OnSelectEnter);
            _farInteractor.selectExited.AddListener(OnSelectExit);
        }

        private void OnDisable()
        {
            _farInteractor.hoverEntered.RemoveListener(OnHoverEnter);
            _farInteractor.hoverExited.RemoveListener(OnHoverExit);
            _farInteractor.selectEntered.RemoveListener(OnSelectEnter);
            _farInteractor.selectExited.RemoveListener(OnSelectExit);
        }

        private void Start()
        {
       
            rayDistance = _curveInteractionCaster.castDistance;
            if (_farInteractor == null || _decalCursorPrefab == null)
            {
                return;
            }

            _cursorInstance = Instantiate(_decalCursorPrefab);
            _cursorInstance.SetActive(false);
            _cursorRenderer = _cursorInstance.GetComponent<MeshRenderer>();
        }

        private void Update()
        {
            if (_isInteracting) return;
            Ray ray = new Ray(transform.position, transform.forward);

            if (Physics.Raycast(ray, out _hit, rayDistance))
            {
                Vector3 adjustedPosition = _hit.point + _hit.normal * 0.001f;
            
                if (!_cursorInstance.activeSelf || Vector3.Distance(_lastHitPoint, adjustedPosition) > 0.001f)
                {
                    _cursorInstance.SetActive(true);
                    _cursorInstance.transform.position = adjustedPosition;
                    _cursorInstance.transform.rotation = Quaternion.LookRotation(-_hit.normal);
                    _lastHitPoint = adjustedPosition;
                }
            }
            else if (_cursorInstance.activeSelf)
            {
                _cursorInstance.SetActive(false);
            }
        }

        private void OnHoverEnter(HoverEnterEventArgs args)
        {
            if (_cursorRenderer != null)
                _cursorRenderer.material.color = Color.green;
        }

        private void OnHoverExit(HoverExitEventArgs args)
        {
            if (_cursorRenderer != null)
                _cursorRenderer.material.color = Color.white;
        }

        private void OnSelectEnter(SelectEnterEventArgs args)
        {
            _isInteracting = true;
            _cursorInstance.SetActive(false);
        }

        private void OnSelectExit(SelectExitEventArgs args)
        {
            _isInteracting = false;
        }
    
    
    }
}

