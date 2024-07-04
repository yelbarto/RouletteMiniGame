using System.Collections.Generic;
using System.Linq;

namespace RouletteMiniGame.RouletteScene.WalletArea
{
    public class WalletModel
    {
        public List<InventoryModel> WalletData { get; } = new();

        public WalletModel(Dictionary<InventoryType, int> wallet)
        {
            UpdateWalletData(wallet);
        }

        public void UpdateWalletData(Dictionary<InventoryType, int> wallet)
        {
            foreach (var (type, inventoryAmount) in wallet)
            {
                var currentInventory =
                    WalletData.FirstOrDefault(i => i.InventoryStaticData.InventoryType == type);
                if (currentInventory == null)
                {
                    WalletData.Add(new InventoryModel(InventoryDataProvider.Instance.GetInventoryStaticData(type),
                        inventoryAmount));
                }
                else
                {
                    currentInventory.UpdateAmount(inventoryAmount);
                }
            }
        }
    }
}