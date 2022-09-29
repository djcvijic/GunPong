using Logic.Core;
using View.GameViews;
using View.UI;

namespace Logic.Brains
{
    public interface IPaddleBrain
    {
        void Initialize(GameUI gameUI, GameViewController gameViewController, PaddleView paddle);

        InputParams Act(float deltaTime);
    }
}