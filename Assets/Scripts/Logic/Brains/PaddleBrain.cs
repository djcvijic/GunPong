public interface PaddleBrain
{
    void Initialize(GameUI gameUI, GameView gameView, PaddleView paddle);

    InputParams Act(float deltaTime);
}