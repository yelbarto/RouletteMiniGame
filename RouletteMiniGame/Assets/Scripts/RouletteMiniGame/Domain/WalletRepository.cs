using System;
using System.Collections.Generic;
using RouletteMiniGame.RouletteScene;

namespace RouletteMiniGame.Domain
{
    public class WalletRepository
    {
        private readonly Dictionary<InventoryType, int> _wallet = new();
        private Action _inventoryChangeAction;

        public void RegisterToInventoryChange(Action callback)
        {
            _inventoryChangeAction += callback;
        }
        public void UnregisterFromInventoryChange(Action callback)
        {
            _inventoryChangeAction -= callback;
        }
        
        public void AddInventory(InventoryType idx, int amount)
        {
            if (!_wallet.TryAdd(idx, amount))
                _wallet[idx] += amount;
            _inventoryChangeAction?.Invoke();
        }

        public Dictionary<InventoryType, int> GetInventoryData()
        {
            return _wallet;
        }
    }
}
