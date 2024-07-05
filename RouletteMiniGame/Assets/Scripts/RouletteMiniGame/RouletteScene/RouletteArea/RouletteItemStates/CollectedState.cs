using System.Threading;
using Cysharp.Threading.Tasks;
using RouletteMiniGame.RouletteScene.RouletteArea.View;

namespace RouletteMiniGame.RouletteScene.RouletteArea.RouletteItemStates
{
    public class CollectedState : RouletteItemStateBase
    {
        public CollectedState(RouletteItemView rouletteItemView) : base(rouletteItemView)
        {
            Type = RouletteItemState.Collected;
        }
        
        public override bool CanChangeState(RouletteItemState stateToChange)
        {
            return stateToChange is RouletteItemState.Highlighted;
        }

        public override async UniTask EnterState(CancellationToken token)
        {
            await RouletteItemView.CollectAsync(token);
        }
    }
}