namespace RouletteMiniGame.RouletteScene.RouletteArea.RouletteItemStates
{
    public class IdleState : RouletteItemStateBase
    {
        public IdleState()
        {
            Type = RouletteItemState.Idle;
        }

        public override bool CanChangeState(RouletteItemState stateToChange)
        {
            return true;
        }
    }
}