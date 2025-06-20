using System;
using System.Collections;
using Code.Scripts.Source.Managers;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Code.Scripts.Source.Gameplay.GreenHouse;
using Code.Scripts.Utils;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

namespace Code.Scripts.Source.GameFSM.States
{
    [Serializable]
    public class GameStateGreenhouseInProgress : GameBaseState
    {
        private static readonly int Sleep = Animator.StringToHash("Sleep");

        public override GameStatesIndex StateIndex { get; protected set; } = GameStatesIndex.GameStateGreenhouseInProgress;

        public Action OnPlantGrown;
        public bool PuzzleSolved { get; private set; }

        [SerializeField] private List<PlantSlot> _plantSlots = new(3);
        [SerializeField] private List<Animator> _ivyAnimators;
        [SerializeField] private List<string> _correctPlants;
        [SerializeField] private Transform _plantSlotContainer;

        private GameStateManager _ctx;
        private Action<GameBaseState, bool, bool> _onPuzzleSolved;
        private XRGrabInteractable _fuse;

        private bool _initialized = false;

        public override void EnterState(GameStateManager context)
        {
            CustomLogger.LogInfo($"[GameStateGreehouseInProgress] Initialized state = {_initialized}");

            _initialized = false;
            if (!_initialized)
                Initialize(context);

            _ctx = context;
            _onPuzzleSolved += context.SwitchState;
            OnPlantGrown += CheckPuzzle;
        }

        public override void UpdateState(GameStateManager context)
        {
        }

        public override void ExitState(GameStateManager context)
        {
            OnPlantGrown -= CheckPuzzle;
            _onPuzzleSolved -= context.SwitchState;
        }

        private void CheckPuzzle()
        {
            List<string> grownPlants = new();
            bool anyNotGrown = false;

            if (PuzzleSolved) return;

            foreach (PlantSlot slot in _plantSlots)
            {
                if (!slot.PlantGrowed) {
                    anyNotGrown = true;
                    break;
                }
            }

            if (anyNotGrown) return;

            foreach (PlantSlot slot in _plantSlots)
                grownPlants.Add(slot.GetPlantLatinName());

            bool allCorrect = new HashSet<string>(grownPlants).SetEquals(_correctPlants);

            if (allCorrect)
            {
                _onPuzzleSolved.Invoke(_ctx.GameStates.GreenhouseResolved, false, false);
                PuzzleSolved = true;
                _fuse.enabled = true;
                _fuse.GetComponent<Rigidbody>().isKinematic = false;

                foreach (Animator ivyAnimator in _ivyAnimators)
                    ivyAnimator.SetTrigger(Sleep);
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
            while (SceneLoader.Instance.ActiveScene.name != "Greenhouse") yield return null;

            if (!_plantSlotContainer)
            {
                _plantSlotContainer = GameObject.FindGameObjectWithTag("PlantSlotContainer").transform;
                _fuse = _plantSlotContainer.GetComponentInChildren<XRGrabInteractable>();

                _fuse.enabled = false;

                if (!_plantSlotContainer)
                    CustomLogger.LogInfo("[GameStateGreehouseInProgress] Could not find plant slot container");
            }

            if (_plantSlots.Count <= 0)
                foreach (Transform child in _plantSlotContainer)
                {
                    PlantSlot slot = child.GetComponent<PlantSlot>();
                    if (slot) _plantSlots.Add(slot);

                    Animator ivyAnimator = child.GetComponent<Animator>();
                    if (ivyAnimator) _ivyAnimators.Add(ivyAnimator);
                }
        }
    }
}
