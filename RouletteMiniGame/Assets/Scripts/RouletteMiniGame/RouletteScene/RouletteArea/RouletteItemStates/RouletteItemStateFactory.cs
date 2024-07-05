using System;
using RouletteMiniGame.RouletteScene.RouletteArea.View;

namespace RouletteMiniGame.RouletteScene.RouletteArea.RouletteItemStates
{
    public static class RouletteItemStateFactory
    {
        public static RouletteItemStateBase Create(RouletteItemState state, RouletteItemView itemView)
        {
            return state switch
            {
                RouletteItemState.Idle => new IdleState(itemView),
                RouletteItemState.Highlighted => new HighlightedState(itemView),
                RouletteItemState.Selected => new SelectedState(itemView),
                RouletteItemState.Collected => new CollectedState(itemView),
                _ => throw new ArgumentOutOfRangeException(nameof(state), state, null)
            };
        }
    }
}