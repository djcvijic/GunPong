using JetBrains.Annotations;
using Logic.Core;
using View.GameViews;
using View.UI;

namespace Logic.Brains
{
    [UsedImplicitly]
    public class LocalPlayerInput : IPaddleBrain
    {
        private GameUI gameUI;

        public void Initialize(GameUI gameUI, GameViewController gameViewController, PaddleView paddle)
        {
            this.gameUI = gameUI;
        }

        public InputParams Act(float deltaTime)
        {
            return gameUI.InputParams;
        }
    }
}