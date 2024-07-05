using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using RouletteMiniGame.Domain;
using RouletteMiniGame.RouletteScene.RouletteArea.View;
using RouletteMiniGame.RouletteScene.WalletArea;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using Util.Addressables.Runtime;
using Util.SceneNavigation;
using Object = UnityEngine.Object;

namespace RouletteMiniGame.RouletteScene.RouletteArea
{
    public class RoulettePresenter : IDisposable
    {
        private readonly RouletteDataSO _rouletteDataSO;
        private readonly AddressablesFacade _addressablesFacade;
        private readonly CancellationTokenSource _lifetimeCts;
        private readonly WalletRepository _walletRepository;
        private readonly WalletPresenter _walletPresenter;
        private readonly WinView _winView;

        private readonly List<RouletteItemPresenter> _rouletteItems = new ();
        private readonly Spinner _spinner;

        private AsyncOperationHandle<GameObject> _rouletteUiLoadTask;
        private RouletteUiView _rouletteUiView;

        public RoulettePresenter(WalletPresenter walletPresenter, RouletteDataSO rouletteDataSO, 
            AddressablesFacade addressablesFacade, WalletRepository walletRepository, WinView winView,
            Transform uiParent)
        {
            _rouletteDataSO = rouletteDataSO;
            _addressablesFacade = addressablesFacade;
            _walletRepository = walletRepository;
            _walletPresenter = walletPresenter;
            _winView = winView;
            
            _lifetimeCts = new CancellationTokenSource();
            _spinner = new Spinner();
            SceneNavigator.Instance.RegisterToSceneChange(OnSceneChanged, SceneType.Roulette);
            LoadRouletteUiAsync(uiParent).Forget();
        }

        private async UniTask LoadRouletteUiAsync(Transform uiParent)
        {
            var loadTask = await _addressablesFacade
                .LoadAssetAsync<GameObject>(_rouletteDataSO.RouletteUiName, _lifetimeCts.Token);
            if (loadTask.Status != AsyncOperationStatus.Succeeded) return;
            _rouletteUiLoadTask = loadTask;
            _rouletteUiView = Object.Instantiate(_rouletteUiLoadTask.Result, uiParent).GetComponent<RouletteUiView>();
            _rouletteUiView.OnWalletButtonClicked += OpenWalletCalled;
            _rouletteUiView.OnSpinClicked += SpinRoulette;
            _rouletteUiView.OnUiClosed += CloseRouletteScene;
            GenerateItems(_rouletteDataSO.ItemOrderList);
        }

        private void GenerateItems(List<InventoryType> orderedInventories)
        {
            var counter = 0;
            foreach (var inventoryType in orderedInventories)
            {
                _rouletteItems.Add(new RouletteItemPresenter(_addressablesFacade, inventoryType, counter,
                    _rouletteUiView.RouletteItems[counter], _rouletteUiView.JumpTargetPosition));
                counter++;
            }
        }

        private void SpinRoulette()
        {
            SpinRouletteAsync().Forget();
        }

        private async UniTask SpinRouletteAsync()
        {
            var finalBoxIndex = await _spinner.SpinAsync(_rouletteItems, _rouletteUiView.InitialSpinSpeed, 
                _rouletteUiView.MinLoopRange, _rouletteUiView.DecelerationRate);
            await CollectRewardAsync(finalBoxIndex);
            if (IsAllItemsCollected())
            {
                ResetRoulette();
            }
            _rouletteUiView.ChangeSpinState(false);
        }

        private void ResetRoulette()
        {
            foreach (var item in _rouletteItems)
            {
                item.Dispose();
            }
            _rouletteItems.Clear();
            GenerateItems(_rouletteDataSO.ItemOrderList);
            
            _rouletteUiView.CloseUi();
        }

        private bool IsAllItemsCollected()
        {
            return _rouletteItems.All(i => i.IsCollected());
        }

        private async UniTask CollectRewardAsync(int itemIndex)
        {
            var item = _rouletteItems[itemIndex];
            await item.CollectItemAsync();
            _walletRepository.AddInventory(item.GetInventoryType(), 1);
            await PlayAfterCollectionAnimationAsync();
            await ShowWinUi(item.GetSprite());
        }

        private async UniTask ShowWinUi(Sprite sprite)
        {
            _winView.OpenWinUi(sprite);
            await UniTask.WaitUntil(() => !_winView.IsOpen, cancellationToken: _lifetimeCts.Token);
        }

        protected virtual UniTask PlayAfterCollectionAnimationAsync()
        {
            return UniTask.CompletedTask;
        }

        private void CloseRouletteScene()
        {
            SceneNavigator.Instance.ChangeScene(SceneType.MainScene);
        }

        private void OnSceneChanged()
        {
            OnSceneChangedAsync().Forget();
        }

        private async UniTask OnSceneChangedAsync()
        {
            if (_rouletteUiView == null)
            {
                await UniTask.WaitUntil(() => _rouletteUiView != null, cancellationToken: _lifetimeCts.Token);
            }
            _rouletteUiView.OpenRouletteUi();
        }

        private void OpenWalletCalled()
        {
            _walletPresenter.OpenWallet();
        }

        public void Dispose()
        {
            SceneNavigator.Instance.UnregisterFromSceneChange(OnSceneChanged, SceneType.Roulette);
            _lifetimeCts.Cancel();
            if (_rouletteUiView == null) return;
            _rouletteUiView.OnWalletButtonClicked -= OpenWalletCalled;
            _rouletteUiView.OnSpinClicked -= SpinRoulette;
            _rouletteUiView.OnUiClosed -= CloseRouletteScene;
        }
    }
}