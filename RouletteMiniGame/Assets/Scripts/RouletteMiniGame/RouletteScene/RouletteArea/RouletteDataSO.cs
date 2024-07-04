using System.Collections.Generic;
using UnityEngine;

namespace RouletteMiniGame.RouletteScene.RouletteArea
{
    [CreateAssetMenu(fileName = "RouletteData", menuName = "RouletteGame/RouletteData")]
    public class RouletteDataSO : ScriptableObject
    {
        [SerializeField] private List<InventoryType> itemOrderList;
        [SerializeField] private string rouletteUiName;

        public List<InventoryType> ItemOrderList => itemOrderList;
        public string RouletteUiName => rouletteUiName;
    }
}