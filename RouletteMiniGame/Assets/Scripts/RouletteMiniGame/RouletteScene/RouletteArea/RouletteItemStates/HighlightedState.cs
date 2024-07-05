using System.Threading;
using Cysharp.Threading.Tasks;
using RouletteMiniGame.RouletteScene.RouletteArea.View;

namespace RouletteMiniGame.RouletteScene.RouletteArea.RouletteItemStates
{
    public class HighlightedState : RouletteItemStateBase
    {
        public HighlightedState(RouletteItemView view) : base(view)
        {
            Type = RouletteItemState.Highlighted;
        }
        
        public override bool CanChangeState(RouletteItemState stateToChange)
        {
            return stateToChange is RouletteItemState.Idle or RouletteItemState.Selected or RouletteItemState.Collected;
        }

        public override async UniTask EnterState(CancellationToken token)
        {
            await RouletteItemView.HighlightAsync(token);
        }
    }
}