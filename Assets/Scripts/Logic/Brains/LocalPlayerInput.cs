using Logic.Core;
using View.GameViews;
using View.UI;

namespace Logic.Brains
{
    public class LocalPlayerInput : PaddleBrain
    {
        private GameUI gameUI;

        public void Initialize(GameUI gameUI, GameView gameView, PaddleView paddle)
        {
            this.gameUI = gameUI;
        }

        public InputParams Act(float deltaTime)
        {
            return gameUI.InputParams;
        }
    }
}