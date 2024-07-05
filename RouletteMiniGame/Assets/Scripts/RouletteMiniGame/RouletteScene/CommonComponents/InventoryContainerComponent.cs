using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RouletteMiniGame.RouletteScene.CommonComponents
{
    public class InventoryContainerComponent : MonoBehaviour
    {
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private Image inventoryImage;
        [SerializeField] private TMP_Text inventoryText;

        public void SetUp(Sprite sprite, int amount)
        {
            inventoryImage.sprite = sprite;
            inventoryText.text = $"x{amount}";
        }

        public void SetState(bool state)
        {
            canvasGroup.alpha = state ? 1 : 0;
        }
    }
}