using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using RouletteMiniGame.RouletteScene.Data;
using RouletteMiniGame.RouletteScene.RouletteArea.RouletteItemStates;
using RouletteMiniGame.RouletteScene.RouletteArea.View;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.U2D;
using Util.Addressables.Runtime;

namespace RouletteMiniGame.RouletteScene.RouletteArea
{
    public class RouletteItemPresenter : IDisposable
    {
        private readonly AddressablesFacade _addressablesFacade;
        private readonly CancellationTokenSource _lifetimeCts;
        private readonly RouletteItemView _rouletteItemView;
        private readonly RouletteItemModel _itemModel;

        private AsyncOperationHandle<SpriteAtlas> _loadOperation;
        private CancellationTokenSource _highlightCts;

        public RouletteItemPresenter(AddressablesFacade addressablesFacade, InventoryType type, int index,
            RouletteItemView rouletteItemView, Vector3 jumpTarget)
        {
            _addressablesFacade = addressablesFacade;
            _rouletteItemView = rouletteItemView;

            _itemModel = new RouletteItemModel(type, index);
            _lifetimeCts = new CancellationTokenSource();
            SetUpViewAsync(type, jumpTarget).Forget();
        }

        public bool IsCollected()
        {
            return _itemModel.State.Type is RouletteItemState.Collected;
        }

        public InventoryType GetInventoryType()
        {
            return _itemModel.Type;
        }

        public Sprite GetSprite()
        {
            return _itemModel.Sprite;
        }

        private async UniTask SetUpViewAsync(InventoryType inventoryType, Vector3 jumpTarget)
        {
            await _itemModel.ChangeState(RouletteItemStateFactory.Create(RouletteItemState.Idle, _rouletteItemView),
                _lifetimeCts.Token);
            var inventoryData = InventoryDataProvider.Instance.GetInventoryStaticData(inventoryType);
            var task = await _addressablesFacade
                .LoadAssetAsync<SpriteAtlas>(inventoryData.AtlasName, _lifetimeCts.Token);
            if (task.Status != AsyncOperationStatus.Succeeded) return;
            _loadOperation = task;
            var sprite = task.Result.GetSprite(inventoryData.SpriteName);
            _rouletteItemView.SetUp(sprite, jumpTarget);
            _itemModel.SetSprite(sprite);
        }

        #region ItemStates

        public async UniTask HighlightItemAsync()
        {
            _highlightCts?.Cancel();
            _highlightCts = new CancellationTokenSource();
            var previousState = _itemModel.State.Type;
            await _itemModel.ChangeState(RouletteItemStateFactory.Create(RouletteItemState.Highlighted, _rouletteItemView),
                _lifetimeCts.Token);
            await _itemModel.ChangeState(RouletteItemStateFactory.Create(previousState, _rouletteItemView),
                _lifetimeCts.Token);
        }

        public async UniTask SelectItemAsync()
        {
            _highlightCts?.Cancel();
            await _itemModel.ChangeState(RouletteItemStateFactory.Create(RouletteItemState.Selected, _rouletteItemView),
                _lifetimeCts.Token);
        }

        public async UniTask CollectItemAsync()
        {
            await _itemModel.ChangeState(RouletteItemStateFactory.Create(RouletteItemState.Collected, _rouletteItemView),
                _lifetimeCts.Token);
        }

        #endregion

        public void Dispose()
        {
            _highlightCts?.Cancel();
            _lifetimeCts.Cancel();
            _rouletteItemView.ClearView();
            if (_loadOperation.Result != null)
                _addressablesFacade.Release(_loadOperation);
        }
    }
}