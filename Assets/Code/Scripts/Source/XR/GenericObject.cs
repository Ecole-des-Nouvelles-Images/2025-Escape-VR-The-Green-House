using UnityEngine;

namespace Code.Scripts.Source.XR
{

    public abstract class GenericObject : MonoBehaviour, IRespawnable
    {
        [SerializeField] private float _resetDelay = 5f;
        private Vector3 _initialPosition;
        private Quaternion _initialRotation;
        private Rigidbody _rigidbody;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _initialPosition = transform.position;
            _initialRotation = transform.rotation;
        }

        public void Respawn()
        {
            _rigidbody.angularVelocity = Vector3.one;
            transform.position = _initialPosition;
            transform.rotation = _initialRotation;
        }
        
        public void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("GameArea"))
            {
                Invoke(nameof(Respawn),_resetDelay);
            }
        }


      
    }
}
