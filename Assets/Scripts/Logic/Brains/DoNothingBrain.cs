using Logic.Core;
using View.GameViews;
using View.UI;

namespace Logic.Brains
{
    public class DoNothingBrain : PaddleBrain
    {
        public void Initialize(GameUI gameUI, GameView gameView, PaddleView paddle) { }

        public InputParams Act(float deltaTime)
        {
            return new InputParams();
        }
    }
}