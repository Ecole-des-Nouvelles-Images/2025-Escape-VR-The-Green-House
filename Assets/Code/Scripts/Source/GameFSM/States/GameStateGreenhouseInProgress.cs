using System;
using System.Collections;
using Code.Scripts.Source.Managers;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Code.Scripts.Source.Gameplay.GreenHouse;
using UnityEngine.Assertions;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

namespace Code.Scripts.Source.GameFSM.States
{
    [Serializable]
    public class GameStateGreenhouseInProgress : GameBaseState
    {
        public Action OnPlantGrown;
        public bool PuzzleSolved { get; private set; }

        [SerializeField] private List<PlantSlot> _plantSlots = new(3);
         
        [SerializeField] private List<string> _correctPlants;
        [SerializeField]private Transform _PlantSlotContainer;
        private GameStateManager _ctx;
        private Action<GameBaseState, bool, bool> OnPuzzleSolved;
        private XRGrabInteractable _fuse;
        
        private bool _initialized = false;

        public override void EnterState(GameStateManager context)
        {
            base.EnterState(context);

            Debug.Log($"[GameStateGreehouseInProgress] Initialized state = {_initialized}");
            
            _initialized = false;
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
                PuzzleSolved = true;
                _fuse.enabled = true;

            }
        }
        
        
        private void Initialize(GameStateManager ctx)
        {
            ctx.StopAllCoroutines();
            Coroutine c = ctx.StartCoroutine(GetPlantSlots());
            
            _initialized = true;
        }


        private IEnumerator GetPlantSlots()
        {
            while (SceneLoader.Instance.ActiveScene.name != "Greenhouse")
            {
                yield return null;
            }

            Debug.Log("[GameStateGreehouseInProgress] Active scene valid");

            if (!_PlantSlotContainer)
            {
                _PlantSlotContainer = GameObject.FindGameObjectWithTag("PlantSlotContainer").transform;
                _fuse = _PlantSlotContainer.GetComponentInChildren<XRGrabInteractable>();
                
                _fuse.enabled = false;

                if (!_PlantSlotContainer)
                    Debug.LogError("[GameStateGreehouseInProgress] Could not find plant slot container");
            }

            if (_plantSlots.Count <= 0)
            {
                foreach (Transform child in _PlantSlotContainer)
                {
                    PlantSlot slot = child.GetComponent<PlantSlot>();
                    if (slot)
                        _plantSlots.Add(slot);
                }
            }
        }
    }
}
