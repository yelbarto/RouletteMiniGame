using System;
using System.Threading;
using com.cyborgAssets.inspectorButtonPro;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using RouletteMiniGame.RouletteScene.RouletteArea.RouletteItemStates;
using UnityEngine;
using UnityEngine.UI;

namespace RouletteMiniGame.RouletteScene.RouletteArea
{
    public class RouletteItemView : MonoBehaviour
    {
        [SerializeField] private Image itemImage;
        [SerializeField] private Image shineImage;
        [SerializeField] private GameObject defaultBackground;
        [SerializeField] private GameObject collectedBackground;
        [SerializeField] private GameObject selectedBackground;
        [SerializeField] private Image tickImage;

        [Header("Highlight Parameters")] 
        [SerializeField] private float alphaDuration;
        [SerializeField] private Ease alphaEase;
        [Header("Select Parameters")] 
        [SerializeField] private float shineDuration;
        [SerializeField] private int shineLoopCount;
        [SerializeField] private Ease shineEase;
        [SerializeField] private float tickFillDuration;
        [SerializeField] private Ease tickFillEase;

        [Header("Collect Parameters")]
        [SerializeField] private float jumpDuration = 0.5f;
        [SerializeField] private float jumpPower = 2f;
        [SerializeField] private Ease jumpEase = Ease.InQuad;
        
        private Vector3 _jumpTarget;

        public void SetUp(Sprite itemSprite, Vector3 jumpTarget)
        {
            itemImage.sprite = itemSprite;
            _jumpTarget = jumpTarget;
        }
        
        public void ClearView()
        {
            itemImage.sprite = null;
            itemImage.gameObject.SetActive(true);
            tickImage.fillAmount = 0;
            collectedBackground.SetActive(false);
            selectedBackground.SetActive(false);
            defaultBackground.SetActive(true);
        }

        public async UniTask OnStateChanged(RouletteItemState state, CancellationToken token)
        {
            switch (state)
            {
                case RouletteItemState.Idle:
                    break;
                case RouletteItemState.Highlighted:
                    await HighlightAsync(token);
                    break;
                case RouletteItemState.Selected:
                    await SelectAsync(token);
                    break;
                case RouletteItemState.Collected:
                    await CollectAsync(token);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }

        private async UniTask HighlightAsync(CancellationToken token)
        {
            shineImage.DOFade(1, 0).ToUniTask(cancellationToken: token).Forget();
            await shineImage.DOFade(0, alphaDuration).SetEase(alphaEase)
                .ToUniTask(TweenCancelBehaviour.KillAndCancelAwait, token);
        }

        private async UniTask SelectAsync(CancellationToken token)
        {
            await shineImage.DOFade(1, shineDuration).SetEase(shineEase)
                .SetLoops(shineLoopCount, LoopType.Yoyo)
                .ToUniTask(TweenCancelBehaviour.KillAndCancelAwait, token);
            shineImage.DOFade(0, 0).ToUniTask(cancellationToken: token).Forget();
            selectedBackground.SetActive(true);
            await tickImage.DOFillAmount(1, tickFillDuration).SetEase(tickFillEase)
                .ToUniTask(TweenCancelBehaviour.KillAndCancelAwait, token);
        }

        private async UniTask CollectAsync(CancellationToken token)
        {
            collectedBackground.SetActive(true);
            await itemImage.transform.DOJump(_jumpTarget, jumpPower, 1, jumpDuration)
                .SetEase(jumpEase)
                .ToUniTask(TweenCancelBehaviour.KillAndCancelAwait, token);
            itemImage.gameObject.SetActive(false);
            itemImage.transform.localPosition = Vector3.zero;
        }

        #region Debug

        [ProButton]
        public void Highlight()
        {
            HighlightAsync(gameObject.GetCancellationTokenOnDestroy()).Forget();
        }

        [ProButton]
        public void Select()
        {
            SelectAsync(gameObject.GetCancellationTokenOnDestroy()).Forget();
        }

        [ProButton]
        public void Collect()
        {
            CollectAsync(gameObject.GetCancellationTokenOnDestroy()).Forget();
        }

        [ProButton]
        public void Reset()
        {
            itemImage.gameObject.SetActive(true);
            tickImage.fillAmount = 0;
            collectedBackground.SetActive(false);
            selectedBackground.SetActive(false);
            defaultBackground.SetActive(true);
        }

        #endregion
    }
}