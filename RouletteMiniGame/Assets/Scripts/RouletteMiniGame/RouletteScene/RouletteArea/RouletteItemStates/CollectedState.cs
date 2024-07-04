namespace RouletteMiniGame.RouletteScene.RouletteArea.RouletteItemStates
{
    public class CollectedState : RouletteItemStateBase
    {
        public CollectedState()
        {
            Type = RouletteItemState.Collected;
        }
        
        public override bool CanChangeState(RouletteItemState stateToChange)
        {
            return stateToChange is RouletteItemState.Highlighted;
        }
    }
}