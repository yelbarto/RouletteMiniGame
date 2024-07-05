using RouletteMiniGame.RouletteScene.CommonComponents;
using UnityEngine;
using UnityEngine.UI;
using Util.UI;

namespace RouletteMiniGame.RouletteScene.RouletteArea.View
{
    public class WinView : MonoBehaviour
    {
        [SerializeField] private InventoryContainerComponent itemContainer;
        [SerializeField] private GameObject winUiParent;
        [SerializeField] private Button closeButton;
        
        public bool IsOpen { get; private set; }
        
        private void Awake()
        {
            closeButton.onClick.AddListener(Close);
        }

        private void Close()
        {
            ChangeOpenState(false);
        }

        public void OpenWinUi(Sprite collectedItemSprite)
        {
            ChangeOpenState(true);
            itemContainer.SetUp(collectedItemSprite, 1);
        }

        private void ChangeOpenState(bool isOpen)
        {
            InputBlockerComponent.Instance.ChangeInteractable(!isOpen);
            winUiParent.SetActive(isOpen);
            IsOpen = isOpen;
        }

        private void OnDestroy()
        {
            closeButton.onClick.RemoveListener(Close);
        }
    }
}