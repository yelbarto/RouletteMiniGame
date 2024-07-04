using UnityEngine;

namespace RouletteMiniGame.RouletteScene.WalletArea
{
    public class InventoryModel
    {
        public int Amount { get; private set; }
        public Sprite Sprite { get; private set; }
        public InventoryStaticData InventoryStaticData { get; private set; }
        
        public InventoryModel(InventoryStaticData inventoryStaticData, int amount)
        {
            InventoryStaticData = inventoryStaticData;
            Amount = amount;
        }

        public void UpdateAmount(int currentAmount)
        {
            Amount = currentAmount;
        }

        public void SetSprite(Sprite sprite)
        {
            Sprite = sprite;
        }
    }
}