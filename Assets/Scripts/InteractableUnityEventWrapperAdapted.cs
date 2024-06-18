/*
 * Copyright (c) Meta Platforms, Inc. and affiliates.
 * All rights reserved.
 *
 * Licensed under the Oculus SDK License Agreement (the "License");
 * you may not use the Oculus SDK except in compliance with the License,
 * which is provided at the time of installation or download, or which
 * otherwise accompanies this software in either electronic or hard copy form.
 *
 * You may obtain a copy of the License at
 *
 * https://developer.oculus.com/licenses/oculussdk/
 *
 * Unless required by applicable law or agreed to in writing, the Oculus SDK
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using AutoFill;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using HighlightPlus;

namespace Oculus.Interaction
{
    /// <summary>
    /// Exposes Unity events that broadcast state changes from an ‹see cref="IInteractableView"/› (an Interactable).
    /// </summary>
    public class InteractableUnityEventWrapperAdapted : MonoBehaviour
    {
        public ModifyTextNetwork modifyText;
        private Vector3 positionText;
        private Vector3 positioncolliderText;
       
        private Collider colider;
        [SerializeField] private HighlightEffect highlightEffect;
        [SerializeField] private StateChange stateChange;
        [SerializeField] private TMP_Text nameText;

        private GameObject tooltipInstance;
        private Pose offsetTooltipGrab;
        private Vector3 tooltipStartPos;
        private Pose _offsetPoseGrabber;
        private Transform animalTransform;
        private Vector3 _visualLocalPos;
        private Quaternion _visualLocalRot;
        private Vector3 _visualLocalScale;

        /// <summary>
        /// The ‹see cref="IInteractableView"/› (Interactable) component to wrap.
        /// </summary>
        [Tooltip("The IInteractableView (Interactable) component to wrap.")]
        
        [SerializeField, Interface(typeof(IInteractableView))]
        [Fill]
        private UnityEngine.Object _interactableView;
        private IInteractableView InteractableView;

        /// <summary>
        /// Raised when an Interactor hovers over the Interactable.
        /// </summary>
        [Tooltip("Raised when an Interactor hovers over the Interactable.")]
        [SerializeField]
        private UnityEvent _whenHover;

        /// <summary>
        /// Raised when the Interactable was being hovered but now it isn't.
        /// </summary>
        [Tooltip("Raised when the Interactable was being hovered but now it isn't.")]
        [SerializeField]
        private UnityEvent _whenUnhover;

        /// <summary>
        /// Raised when an Interactor selects the Interactable.
        /// </summary>
        [Tooltip("Raised when an Interactor selects the Interactable.")]
        [SerializeField]
        private UnityEvent _whenSelect;

        /// <summary>
        /// Raised when the Interactable was being selected but now it isn't.
        /// </summary>
        [Tooltip("Raised when the Interactable was being selected but now it isn't.")]
        [SerializeField]
        private UnityEvent _whenUnselect;

        /// <summary>
        /// Raised each time an Interactor hovers over the Interactable, even if the Interactable is already being hovered by a different Interactor.
        /// </summary>
        [Tooltip("Raised each time an Interactor hovers over the Interactable, even if the Interactable is already being hovered by a different Interactor.")]
        [SerializeField]
        private UnityEvent _whenInteractorViewAdded;

        /// <summary>
        /// Raised each time an Interactor stops hovering over the Interactable, even if the Interactable is still being hovered by a different Interactor.
        /// </summary>
        [Tooltip("Raised each time an Interactor stops hovering over the Interactable, even if the Interactable is still being hovered by a different Interactor.")]
        [SerializeField]
        private UnityEvent _whenInteractorViewRemoved;

        /// <summary>
        /// Raised each time an Interactor selects the Interactable, even if the Interactable is already being selected by a different Interactor.
        /// </summary>
        [Tooltip("Raised each time an Interactor selects the Interactable, even if the Interactable is already being selected by a different Interactor.")]
        [SerializeField]
        private UnityEvent _whenSelectingInteractorViewAdded;

        /// <summary>
        /// Raised each time an Interactor stops selecting the Interactable, even if the Interactable is still being selected by a different Interactor.
        /// </summary>
        [Tooltip("Raised each time an Interactor stops selecting the Interactable, even if the Interactable is still being selected by a different Interactor.")]
        [SerializeField]
        private UnityEvent _whenSelectingInteractorViewRemoved;

        #region Properties

        public UnityEvent WhenHover => _whenHover;
        public UnityEvent WhenUnhover => _whenUnhover;
        public UnityEvent WhenSelect => _whenSelect;
        public UnityEvent WhenUnselect => _whenUnselect;
        public UnityEvent WhenInteractorViewAdded => _whenInteractorViewAdded;
        public UnityEvent WhenInteractorViewRemoved => _whenInteractorViewRemoved;
        public UnityEvent WhenSelectingInteractorViewAdded => _whenSelectingInteractorViewAdded;
        public UnityEvent WhenSelectingInteractorViewRemoved => _whenSelectingInteractorViewRemoved;

        #endregion

        protected bool _started = false;

        protected virtual void Awake()
        {
            
            InteractableView = _interactableView as IInteractableView;
            InteractableView = GetComponent<RayInteractable>();
            
        }

        protected virtual void Start()
        {
            
            tooltipInstance = GameObject.Find("Tooltip");
            modifyText = GameObject.FindGameObjectWithTag("ZoneText").GetComponent<ModifyTextNetwork>();
            positionText = GameObject.FindGameObjectWithTag("ZoneText").GetComponent<Transform>().position;
            nameText = GameObject.FindGameObjectWithTag("ZoneText").GetComponentInChildren<TMP_Text>();
            colider = GetComponent<Collider>();
            animalTransform = GameObject.FindGameObjectWithTag("Respawn").transform;
            highlightEffect = GetComponent<HighlightEffect>();
            stateChange = GetComponentInParent<StateChange>();
            _visualLocalPos = animalTransform.localPosition;
            _visualLocalRot = animalTransform.localRotation;
            _visualLocalScale = animalTransform.localScale;
            

            this.BeginStart(ref _started);
            this.AssertField(InteractableView, nameof(InteractableView));
            this.EndStart(ref _started);


        }
    

        protected virtual void OnEnable()
        {
            if (_started)
            {
                InteractableView.WhenStateChanged += HandleStateChanged;
                InteractableView.WhenInteractorViewAdded += HandleInteractorViewAdded;
                InteractableView.WhenInteractorViewRemoved += HandleInteractorViewRemoved;
                InteractableView.WhenSelectingInteractorViewAdded += HandleSelectingInteractorViewAdded;
                InteractableView.WhenSelectingInteractorViewRemoved += HandleSelectingInteractorViewRemoved;
            }
        }

        protected virtual void OnDisable()
        {
            if (_started)
            {
                InteractableView.WhenStateChanged -= HandleStateChanged;
                InteractableView.WhenInteractorViewAdded -= HandleInteractorViewAdded;
                InteractableView.WhenInteractorViewRemoved -= HandleInteractorViewRemoved;
                InteractableView.WhenSelectingInteractorViewAdded -= HandleSelectingInteractorViewAdded;
                InteractableView.WhenSelectingInteractorViewRemoved -= HandleSelectingInteractorViewRemoved;
            }
        }
        
        private void HandleStateChanged(InteractableStateChangeArgs args)
        {
            switch (args.NewState)
            {
                case InteractableState.Normal:
                    if (args.PreviousState == InteractableState.Hover)
                    {

                        highlightEffect.highlighted = false;
                        nameText.text = "";
                        modifyText.OnChangeName(nameText.text);
                        ChangeTooltipPosition();
                        modifyText.OnChangePosition(tooltipInstance.transform.position);

                        stateChange.OnChangeHighlight(highlightEffect.highlighted);
                    }

                    break;
                case InteractableState.Hover:
                    if (args.PreviousState == InteractableState.Normal)
                    {
                        highlightEffect.highlighted = true;
                        nameText.text = gameObject.name;                  
                        modifyText.OnChangeName(nameText.text);
                        ChangeTooltipPosition();
                        modifyText.OnChangePosition(tooltipInstance.transform.position);

                        stateChange.OnChangeHighlight(highlightEffect.highlighted);

                    }
                    else if (args.PreviousState == InteractableState.Select)
                    {
                        _whenUnselect.Invoke();
                    }

                    break;
                case InteractableState.Select:
                    if (args.PreviousState == InteractableState.Hover)
                    {
                      
                    }

                    break;
            }
        }

        private void ChangeTooltipPosition()
        {
            
            if (tooltipInstance != null) tooltipInstance.transform.position = animalTransform.position - new Vector3(0 , 0.5f , 0.4f);
        }

        private void HandleInteractorViewAdded(IInteractorView interactorView)
        {
            WhenInteractorViewAdded.Invoke();
        }

        private void HandleInteractorViewRemoved(IInteractorView interactorView)
        {
            WhenInteractorViewRemoved.Invoke();
        }

        private void HandleSelectingInteractorViewAdded(IInteractorView interactorView)
        {
            WhenSelectingInteractorViewAdded.Invoke();
        }

        private void HandleSelectingInteractorViewRemoved(IInteractorView interactorView)
        {
            WhenSelectingInteractorViewRemoved.Invoke();
        }

        #region Inject

        public void InjectAllInteractableUnityEventWrapper(IInteractableView interactableView)
        {
            InjectInteractableView(interactableView);
        }

        public void InjectInteractableView(IInteractableView interactableView)
        {
            _interactableView = interactableView as UnityEngine.Object;
            InteractableView = interactableView;
        }

        #endregion
    }
}
