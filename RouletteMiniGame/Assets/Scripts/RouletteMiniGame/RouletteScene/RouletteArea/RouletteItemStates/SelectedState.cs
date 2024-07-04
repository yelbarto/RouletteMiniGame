namespace RouletteMiniGame.RouletteScene.RouletteArea.RouletteItemStates
{
    public class SelectedState : RouletteItemStateBase
    {
        public SelectedState()
        {
            Type = RouletteItemState.Selected;
        }

        public override bool CanChangeState(RouletteItemState stateToChange)
        {
            return stateToChange is RouletteItemState.Idle or RouletteItemState.Collected;
        }
    }
}