using UnityEngine;
using UnityEngine.UI;

namespace Util.UI
{
    public class InputBlockerComponent : MonoSingleton<InputBlockerComponent>
    {
        [SerializeField] private Image raycastImage;
        
        public void ChangeInteractable(bool blockState)
        {
            raycastImage.raycastTarget = blockState;
        }
    }
}