using System.Linq;
using UnityEngine;
using Util;

namespace RouletteMiniGame.RouletteScene.Data
{
    public class InventoryDataProvider : MonoSingleton<InventoryDataProvider>
    {
        [SerializeField] private InventoryStaticDataSO inventoryStaticDataSO;

        public InventoryStaticData GetInventoryStaticData(InventoryType type)
        {
            return inventoryStaticDataSO.InventoryList.First(i => i.InventoryType == type);
        }
    }
}