namespace RouletteMiniGame.RouletteScene.RouletteArea.RouletteItemStates
{
    public class HighlightedState : RouletteItemStateBase
    {
        public HighlightedState()
        {
            Type = RouletteItemState.Highlighted;
        }
        
        public override bool CanChangeState(RouletteItemState stateToChange)
        {
            return stateToChange is RouletteItemState.Idle or RouletteItemState.Selected or RouletteItemState.Collected;
        }
    }
}