using Logic.Core;
using View.GameViews;
using View.UI;

namespace Logic.Brains
{
    public interface IPaddleBrain
    {
        void Initialize(GameUI gameUI, GameView gameView, PaddleView paddle);

        InputParams Act(float deltaTime);
    }
}