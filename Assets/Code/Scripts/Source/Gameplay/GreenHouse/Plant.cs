using UnityEngine;

namespace Code.Scripts.Source.Gameplay.GreenHouse
{
    public class Plant : MonoBehaviour
    {
        [SerializeField] private PlantSlot _plantSlot;
        private Animator _animator;
        private bool _isCut;
        
        private void Awake() {
            _animator = GetComponent<Animator>();
            _plantSlot = GetComponentInParent<PlantSlot>();
        }

        private void OnTriggerEnter(Collider other) 
        {
            if (!other.CompareTag("Shears")) return;
            if (_isCut) return;
           
            _plantSlot.OnPlantCut?.Invoke();
            CutPlant();
        }

        private void CutPlant()
        {
            _isCut = true;
            _animator.SetTrigger("Cut");
           Destroy(gameObject,2f);
        }
    
    }
}
