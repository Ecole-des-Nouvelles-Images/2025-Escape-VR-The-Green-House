using System;
using Code.Scripts.Source.Managers;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Code.Scripts.Source.Gameplay.GreenHouse;

namespace Code.Scripts.Source.GameFSM.States
{
    [Serializable]
    public class GameStateGreenhouseInProgress : GameBaseState
    {
        public static Action OnPlantGrown;
        public bool PuzzleSolved { get; private set; }

        [SerializeField] private List<PlantSlot> _plantSlots;
        [SerializeField] private List<string> _correctPlants;

        private GameStateManager _ctx;
        private Action<GameBaseState, bool, bool> OnPuzzleSolved;

        public override void EnterState(GameStateManager context)
        {
            base.EnterState(context);

            _ctx = context;
            OnPuzzleSolved += context.SwitchState;
            OnPlantGrown += CheckPuzzle;
        }

        public override void UpdateState(GameStateManager context)
        {

        }

        public override void ExitState(GameStateManager context)
        {
            OnPlantGrown -= CheckPuzzle;
            OnPuzzleSolved -= context.SwitchState;
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
                OnPuzzleSolved.Invoke(_ctx.GameStates.LoungePhase2, false, false);
            }
        }
    }
}
