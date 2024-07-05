using System.Threading;
using Cysharp.Threading.Tasks;
using RouletteMiniGame.RouletteScene.RouletteArea.View;

namespace RouletteMiniGame.RouletteScene.RouletteArea.RouletteItemStates
{
    public class IdleState : RouletteItemStateBase
    {
        public IdleState(RouletteItemView rouletteItemView) : base(rouletteItemView)
        {
            Type = RouletteItemState.Idle;
        }

        public override bool CanChangeState(RouletteItemState stateToChange)
        {
            return true;
        }

        public override UniTask EnterState(CancellationToken token)
        {
            return UniTask.CompletedTask;
        }
    }
}