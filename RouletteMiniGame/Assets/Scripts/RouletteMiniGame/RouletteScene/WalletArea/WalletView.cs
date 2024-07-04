using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RouletteMiniGame.RouletteScene.WalletArea
{
    public class WalletView : MonoBehaviour
    {
        [SerializeField] private List<InventoryContainerComponent> inventoryContainerComponents;
        [SerializeField] private GameObject walletParent;
        [SerializeField] private Button closeButton;

        private void Awake()
        {
            closeButton.onClick.AddListener(() => walletParent.SetActive(false));
        }

        public void Open(Dictionary<Sprite, int> allInventoryContainerData)
        {
            var count = 0;
            foreach (var inventoryContainerData in allInventoryContainerData)
            {
                inventoryContainerComponents[count].SetState(true);
                inventoryContainerComponents[count].SetUp(inventoryContainerData.Key, inventoryContainerData.Value);
                count++;
            }

            for (var i = count; i < inventoryContainerComponents.Count; i++)
            {
                inventoryContainerComponents[i].SetState(false);
            }
            walletParent.SetActive(true);
        }

        private void OnDestroy()
        {
            closeButton.onClick.RemoveAllListeners();
        }
    }
}