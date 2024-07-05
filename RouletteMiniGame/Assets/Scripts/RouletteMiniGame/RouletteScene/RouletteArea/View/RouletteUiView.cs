using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using Util.UI;

namespace RouletteMiniGame.RouletteScene.RouletteArea.View
{
    public abstract class RouletteUiView : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] protected Button spinButton;
        [SerializeField] private Button walletButton;
        [SerializeField] private Button closeButton;
        [SerializeField] private GameObject rouletteParent;
        [SerializeField] private List<RouletteItemView> rouletteItems;

        [Header("Spin Parameters")] 
        [SerializeField] private float initialSpinSpeed = 0.05f; // Faster initial speed
        [SerializeField] private float decelerationRate = 0.002f; // Slower deceleration rate
        [SerializeField] private Vector2Int minLoopRange = new(1, 3);

        [NonSerialized] public Action OnWalletButtonClicked;
        [NonSerialized] public Action OnSpinClicked;
        [NonSerialized] public Action OnUiClosed;

        public List<RouletteItemView> RouletteItems => rouletteItems;
        public float InitialSpinSpeed => initialSpinSpeed;
        public float DecelerationRate => decelerationRate;
        public Vector2Int MinLoopRange => minLoopRange;
        public Vector3 JumpTargetPosition => walletButton.transform.position;

        private CancellationTokenSource _lifetimeCts;
        private bool _isSpinning;
        private bool _isSet;

        private void Awake()
        {
            walletButton.onClick.AddListener(() => OnWalletButtonClicked?.Invoke());
            closeButton.onClick.AddListener(CloseUi);
            spinButton.onClick.AddListener(Spin);
            _lifetimeCts = new CancellationTokenSource();
        }

        public void OpenRouletteUi()
        {
            rouletteParent.SetActive(true);
        }

        private void Spin()
        {
            if (_isSpinning)
                throw new Exception("Can't call spin while spinning");
            ChangeSpinState(true);
            OnSpinClicked?.Invoke();
        }

        public virtual void ChangeSpinState(bool state)
        {
            _isSpinning = state;
            InputBlockerComponent.Instance.ChangeInteractable(state);
        }

        public void CloseUi()
        {
            OnUiClosed?.Invoke();
            rouletteParent.SetActive(false);
        }
        
        private void OnDestroy()
        {
            walletButton.onClick.RemoveAllListeners();
            walletButton.onClick.RemoveListener(CloseUi);
            spinButton.onClick.RemoveListener(Spin);
            _lifetimeCts.Cancel();
        }
    }
}
