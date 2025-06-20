using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Code.Scripts.Source.Gameplay.GreenHouse
{
    public class PlantPuzzle : MonoBehaviour
    {
        public bool PuzzleSolved { get; private set; }

        [SerializeField] private List<PlantSlot> _plantSlots;
        [SerializeField] private List<string> _correctPlants;

        private void CheckPuzzle()
        {
            if (PuzzleSolved) return;

            foreach (PlantSlot slot in _plantSlots)
            {
                if (!slot.PlantGrowed)
                    return;
            }

            List<string> grownPlants = new();

            foreach (PlantSlot slot in _plantSlots)
                grownPlants.Add(slot.GetPlantLatinName());

            bool allCorrect = new HashSet<string>(grownPlants).SetEquals(_correctPlants);

            if (allCorrect)
                PuzzleSolved = true;
        }
    }
}
