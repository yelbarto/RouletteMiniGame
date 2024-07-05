using System.Threading;
using Cysharp.Threading.Tasks;
using RouletteMiniGame.RouletteScene.RouletteArea.View;

namespace RouletteMiniGame.RouletteScene.RouletteArea.RouletteItemStates
{
    public abstract class RouletteItemStateBase
    {
        public RouletteItemState Type;

        protected RouletteItemView RouletteItemView;

        protected RouletteItemStateBase(RouletteItemView rouletteItemView)
        {
            RouletteItemView = rouletteItemView;
        }
        public abstract bool CanChangeState(RouletteItemState stateToChange);
        public abstract UniTask EnterState(CancellationToken token);
    }
}