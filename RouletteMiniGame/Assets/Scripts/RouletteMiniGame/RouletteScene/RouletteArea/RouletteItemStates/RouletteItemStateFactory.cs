using System;

namespace RouletteMiniGame.RouletteScene.RouletteArea.RouletteItemStates
{
    public class RouletteItemStateFactory
    {
        public RouletteItemStateBase Create(RouletteItemState state)
        {
            return state switch
            {
                RouletteItemState.Idle => new IdleState(),
                RouletteItemState.Highlighted => new HighlightedState(),
                RouletteItemState.Selected => new SelectedState(),
                RouletteItemState.Collected => new CollectedState(),
                _ => throw new ArgumentOutOfRangeException(nameof(state), state, null)
            };
        }
    }
}