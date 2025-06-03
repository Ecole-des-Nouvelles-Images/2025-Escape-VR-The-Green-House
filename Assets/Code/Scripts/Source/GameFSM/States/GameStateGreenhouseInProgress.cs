using System;
using System.Collections;
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

        [SerializeField] private List<PlantSlot> _plantSlots = new(3);
         
        [SerializeField] private List<string> _correctPlants;
        [SerializeField]private Transform _PlantSlotContainer;
        private GameStateManager _ctx;
        private Action<GameBaseState, bool, bool> OnPuzzleSolved;
        
        private bool _initialized;

        public override void EnterState(GameStateManager context)
        {
            base.EnterState(context);

            if (!_initialized)
                Initialize(context);
            
            
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
                OnPuzzleSolved.Invoke(_ctx.GameStates.GreenhouseResolved, false, false);
            }
        }
        
        
        private void Initialize(GameStateManager ctx)
        {
            ctx.StartCoroutine(GetPlantSlots());
            _initialized = true;
        }
        
        
        private IEnumerator GetPlantSlots()
        {
            while (SceneLoader.Instance.ActiveScene.name != "Lounge")
            {
                yield return null;
            }

            if (!_PlantSlotContainer)
            {
              _PlantSlotContainer = GameObject.FindGameObjectWithTag("PlantSlotContainer").transform;
              //  _biblioAnimator = _bookSocketsContainer.parent.GetComponent<Animator>();

                if (!_PlantSlotContainer)
                    throw new Exception("[GameStateLoungePhase2] Book sockets container not found.");
            }

            if (_plantSlots.Count == 0)
            {
                foreach (Transform child in _PlantSlotContainer)
                    _plantSlots.Add(child.GetComponent<PlantSlot>());
            }
        }
        
    }
}
