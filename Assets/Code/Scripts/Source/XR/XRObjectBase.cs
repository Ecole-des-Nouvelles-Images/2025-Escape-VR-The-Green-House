using UnityEngine;

namespace Code.Scripts.Source.XR
{

    public abstract class XRObjectBase : MonoBehaviour
    {
        [SerializeField] protected float _resetDelay = 5f;
        protected Vector3 _initialPosition;
        protected Quaternion _initialRotation;
        protected Rigidbody _rigidbody;

        protected virtual void Init()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _initialPosition = transform.position;
            _initialRotation = transform.rotation;
        }

        protected virtual void Respawn()
        {
            _rigidbody.angularVelocity = Vector3.one;
            transform.position = _initialPosition;
            transform.rotation = _initialRotation;
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("GameArea"))
            {
                Invoke(nameof(Respawn),_resetDelay);
            }
        }
    }
}
