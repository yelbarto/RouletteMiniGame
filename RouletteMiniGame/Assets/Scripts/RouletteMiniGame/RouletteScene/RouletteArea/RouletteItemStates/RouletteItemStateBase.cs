namespace RouletteMiniGame.RouletteScene.RouletteArea.RouletteItemStates
{
    public abstract class RouletteItemStateBase
    {
        public RouletteItemState Type;

        public abstract bool CanChangeState(RouletteItemState stateToChange);
    }
}