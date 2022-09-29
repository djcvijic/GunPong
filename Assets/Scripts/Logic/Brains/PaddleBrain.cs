using Logic.Core;
using View.GameViews;
using View.UI;

namespace Logic.Brains
{
    public interface PaddleBrain
    {
        void Initialize(GameUI gameUI, GameView gameView, PaddleView paddle);

        InputParams Act(float deltaTime);
    }
}