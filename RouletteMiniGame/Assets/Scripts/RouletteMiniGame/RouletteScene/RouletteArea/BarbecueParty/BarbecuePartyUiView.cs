namespace RouletteMiniGame.RouletteScene.RouletteArea.BarbecueParty
{
    public class BarbecuePartyUiView : RouletteUiView
    {
        public override void ChangeSpinState(bool state)
        {
            base.ChangeSpinState(state);
            spinButton.gameObject.SetActive(!state);
        }
    }
}