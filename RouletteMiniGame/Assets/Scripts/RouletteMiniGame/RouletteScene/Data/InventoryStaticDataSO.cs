using System.Collections.Generic;
using UnityEngine;

namespace RouletteMiniGame.RouletteScene.Data
{
    [CreateAssetMenu(fileName = "InventoryData", menuName = "RouletteGame/InventoryData")]
    public class InventoryStaticDataSO : ScriptableObject
    {
        [SerializeField] private List<InventoryStaticData> inventoryList;

        public List<InventoryStaticData> InventoryList => inventoryList;
    }
}