public class DoNothingBrain : PaddleBrain
{
    public void Initialize(GameUI gameUI, GameView gameView, PaddleView paddle) { }

    public InputParams Act(float deltaTime)
    {
        return new InputParams();
    }
}