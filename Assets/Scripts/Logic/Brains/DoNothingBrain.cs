using JetBrains.Annotations;
using Logic.Core;
using View.GameViews;
using View.UI;

namespace Logic.Brains
{
    [UsedImplicitly]
    public class DoNothingBrain : IPaddleBrain
    {
        public void Initialize(GameUI gameUI, GameView gameView, PaddleView paddle) { }

        public InputParams Act(float deltaTime)
        {
            return new InputParams();
        }
    }
}