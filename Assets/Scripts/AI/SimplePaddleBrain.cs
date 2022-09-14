public class SimplePaddleBrain : PaddleBrain
{
    private GameView gameView;

    public SimplePaddleBrain(GameView gameView)
    {
        this.gameView = gameView;
    }

    public InputParams Act(float deltaTime)
    {
        var inputHorizontal = MoveToFollowBall();
        var inputFire = FireIfEnemyInSights();
        return new InputParams(inputHorizontal, inputFire);
    }

    private float MoveToFollowBall()
    {
        throw new System.NotImplementedException();
    }

    private bool FireIfEnemyInSights()
    {
        throw new System.NotImplementedException();
    }
}