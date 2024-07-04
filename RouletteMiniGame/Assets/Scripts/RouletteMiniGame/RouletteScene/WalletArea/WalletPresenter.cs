using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using RouletteMiniGame.Domain;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.U2D;
using Util.Addressables.Runtime;

namespace RouletteMiniGame.RouletteScene.WalletArea
{
    public class WalletPresenter : IDisposable
    {
        private readonly WalletModel _walletModel;
        private readonly WalletView _walletView;
        private readonly WalletRepository _walletRepository;
        private readonly AddressablesFacade _addressablesFacade;
        private readonly CancellationTokenSource _lifetimeCts;
        private readonly List<AsyncOperationHandle<SpriteAtlas>> _loadedSpriteOperations = new();

        public WalletPresenter(WalletView walletView, WalletRepository walletRepository,
            AddressablesFacade addressablesFacade)
        {
            _addressablesFacade = addressablesFacade;
            _walletRepository = walletRepository;
            _walletView = walletView;

            _lifetimeCts = new CancellationTokenSource();
            _walletModel = new WalletModel(walletRepository.GetInventoryData());
            walletRepository.RegisterToInventoryChange(OnInventoryChanged);
        }

        private void OnInventoryChanged()
        {
            _walletModel.UpdateWalletData(_walletRepository.GetInventoryData());
        }

        public void OpenWallet()
        {
            OpenWalletAsync().Forget();
        }

        private async UniTask OpenWalletAsync()
        {
            var newItems = _walletModel.WalletData.Where(d => d.Sprite == null);
            foreach (var item in newItems)
            {
                await LoadInventorySprite(item);
            }

            _walletView.Open(_walletModel.WalletData.ToDictionary(k => k.Sprite, v => v.Amount));
        }

        private async UniTask LoadInventorySprite(InventoryModel inventoryModel)
        {
            var task = await _addressablesFacade.LoadAssetAsync<SpriteAtlas>(
                inventoryModel.InventoryStaticData.AtlasName,
                _lifetimeCts.Token);
            if (task.Status == AsyncOperationStatus.Succeeded)
            {
                _loadedSpriteOperations.Add(task);
                inventoryModel.SetSprite(task.Result.GetSprite(inventoryModel.InventoryStaticData.SpriteName));
            }
        }

        public void Dispose()
        {
            _walletRepository.UnregisterFromInventoryChange(OnInventoryChanged);
            _lifetimeCts.Cancel();
            foreach (var ops in _loadedSpriteOperations)
            {
                _addressablesFacade.Release(ops);
            }
        }
    }
}