using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Code.Scripts.Source.Gameplay.GreenHouse
{
   public class PlantPuzzle : MonoBehaviour
   {
     // public static Action OnPlantGrown;
      public bool PuzzleSolved { get; private set; }
   
      [SerializeField] private List<PlantSlot> _plantSlots;
      [SerializeField] private List<string> _correctPlants;

      private void OnEnable()
      {
        // OnPlantGrown += CheckPuzzle;
      }

      private void OnDisable()
      {
        // OnPlantGrown -= CheckPuzzle;
      }

      private void CheckPuzzle()
      {
         if (PuzzleSolved) return;
     
         if(_plantSlots.Any(slot => !slot.PlantGrowed)) return;
      
         var grownPlants = _plantSlots
            .Select(slot => slot.GetPlantLatinName())
            .ToList();

         bool allCorrect = new HashSet<string>(grownPlants).SetEquals(_correctPlants);

         if (allCorrect)
         {
            // puzlle solved
            Debug.Log("puzzle slved");
            PuzzleSolved = true;
         }
      }

   

   }
}
