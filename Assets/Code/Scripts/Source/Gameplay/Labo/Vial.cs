using UnityEngine;

namespace Code.Scripts.Source.Gameplay.Labo
{
    public enum PotionType
    {
        Tulénium,
        Yzora,
        Morazium,
        Veridox,
        Agent,
        Base
    }

    public class Vial : MonoBehaviour
    {
        [SerializeField] private PotionType _type;

        public PotionType Type => _type;
        public PotionProperties Properties { get; private set; }
        
        private void Awake()
        {
            Properties = new PotionProperties(_type);
        }
    }
}
