using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

namespace Code.Scripts.Source.Gameplay.Lounge
{
    public class BookPuzzle : MonoBehaviour
    {
        public bool PuzzleSolved = false; 
        [SerializeField] private List<XRSocketInteractor> _bookSockets = new List<XRSocketInteractor>(5);
        [SerializeField] private List<string> _correctBookPlacement = new List<string>(5);
        [SerializeField] private  bool _fusePlugged = false;

        private void OnEnable()
        {
            //GameStateLoungePhase2.OnSocketChanged += CheckPuzzle;
            //GameStateLoungePhase2.OnFusePlugged += PlugFuseCheck;
        }

        private void OnDisable()
        {
            //GameStateLoungePhase2.OnSocketChanged -= CheckPuzzle;
           // GameStateLoungePhase2.OnFusePlugged -= PlugFuseCheck;
        }

        private void CheckPuzzle()
        {
            Debug.Log("Checking Puzzle");
            if (PuzzleSolved || !_fusePlugged) return;

            for (int i = 0; i < _bookSockets.Count; i++)
            {
                if (!_bookSockets[i].hasSelection) return;

                GameObject selected = _bookSockets[i].GetOldestInteractableSelected().transform.gameObject;
                Book book = selected.GetComponent<Book>();

                if (book == null || book.BookName != _correctBookPlacement[i]) return;
            }

            PuzzleSolved = true;
            Debug.Log(" Puzzle terminé");
        }

        private void PlugFuseCheck(bool fuseIsPlugged)
        {
            _fusePlugged = fuseIsPlugged;
        }
        
    }
}
