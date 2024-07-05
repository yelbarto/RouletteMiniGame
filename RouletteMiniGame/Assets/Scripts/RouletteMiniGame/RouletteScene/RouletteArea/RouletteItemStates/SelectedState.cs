using System.Threading;
using Cysharp.Threading.Tasks;
using RouletteMiniGame.RouletteScene.RouletteArea.View;

namespace RouletteMiniGame.RouletteScene.RouletteArea.RouletteItemStates
{
    public class SelectedState : RouletteItemStateBase
    {
        public SelectedState(RouletteItemView view) : base(view)
        {
            Type = RouletteItemState.Selected;
        }

        public override bool CanChangeState(RouletteItemState stateToChange)
        {
            return stateToChange is RouletteItemState.Idle or RouletteItemState.Collected;
        }

        public override async UniTask EnterState(CancellationToken token)
        {
            await RouletteItemView.SelectAsync(token);
        }
    }
}