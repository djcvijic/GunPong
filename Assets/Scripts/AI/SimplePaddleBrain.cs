public class SimplePaddleBrain : PaddleBrain
{
    private readonly GameView gameView;

    private readonly PaddleView paddle;

    public SimplePaddleBrain(GameView gameView, PaddleView paddle)
    {
        this.gameView = gameView;
        this.paddle = paddle;
    }

    public InputParams Act(float deltaTime)
    {
        var inputHorizontal = MoveToFollowBall(deltaTime);
        var inputFire = FireIfEnemyAhead();
        return new InputParams(inputHorizontal, inputFire);
    }

    private float MoveToFollowBall(float deltaTime)
    {
        var t = paddle.transform;
        var myX = t.position.x;
        var ballX = gameView.Ball.transform.position.x;
        var mySpeed = paddle.Speed;
        return (ballX - myX) / mySpeed / deltaTime;
    }

    private bool FireIfEnemyAhead()
    {
        var myX = paddle.transform.position.x;
        var enemyPaddle = gameView.GetEnemyPaddle(paddle);
        var enemyTransform = enemyPaddle.transform;
        var enemyPositionX = enemyTransform.position.x;
        var enemyScaleX = enemyTransform.lossyScale.x;
        var minX = enemyPositionX - 0.5f * enemyScaleX;
        var maxX = enemyPositionX + 0.5f * enemyScaleX;
        return myX >= minX && myX <= maxX;
    }
}