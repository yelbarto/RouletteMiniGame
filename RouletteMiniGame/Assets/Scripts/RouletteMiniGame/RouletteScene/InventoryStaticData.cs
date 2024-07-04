using System;
using UnityEngine;

namespace RouletteMiniGame.RouletteScene
{
    [Serializable]
    public class InventoryStaticData
    {
        [SerializeField] private InventoryType inventoryType;
        [SerializeField] private string atlasName;
        [SerializeField] private string spriteName;

        public InventoryType InventoryType => inventoryType;

        public string AtlasName => atlasName;

        public string SpriteName => spriteName;
    }
}