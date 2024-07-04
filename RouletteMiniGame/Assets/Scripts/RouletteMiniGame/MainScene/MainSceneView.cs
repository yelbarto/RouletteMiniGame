using UnityEngine;
using UnityEngine.UI;
using Util.SceneNavigation;

namespace RouletteMiniGame.MainScene
{
    public class MainSceneView : MonoBehaviour
    {
        [SerializeField] private GameObject mainSceneParent;
        [SerializeField] private Button personalizedButton;

        private void Awake()
        {
            SceneNavigator.Instance.RegisterToSceneChange(OnSceneChanged, SceneType.MainScene);
            personalizedButton.onClick.AddListener(OnPlayButtonClicked);
        }

        private void OnSceneChanged()
        {
            ChangeMainSceneState(true);
        }

        private void ChangeMainSceneState(bool state)
        {
            mainSceneParent.SetActive(state);
            personalizedButton.interactable = state;
        }

        private void OnPlayButtonClicked()
        {
            SceneNavigator.Instance.ChangeScene(SceneType.Roulette);
            ChangeMainSceneState(false);
        }

        private void OnDestroy()
        {
            personalizedButton.onClick.RemoveListener(OnPlayButtonClicked);
        }
    }
}
