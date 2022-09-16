public class SimplePaddleBrain : PaddleBrain
{
    private const float BallFollowPositionOffsetPercentage = 0.05f;

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
        var ballX = gameView.Ball.transform.position.x;
        var myTransform = paddle.transform;
        var myX = myTransform.position.x;
        var myScaleX = myTransform.lossyScale.x;
        var offsetX = BallFollowPositionOffsetPercentage * myScaleX;
        var mySpeed = paddle.Speed;
        return (ballX - myX + offsetX) / mySpeed / deltaTime;
    }

    private bool FireIfEnemyAhead()
    {
        var myX = paddle.transform.position.x;
        var enemyPaddle = gameView.GetPaddle(paddle.Owner.GetEnemy());
        var enemyTransform = enemyPaddle.transform;
        var enemyPositionX = enemyTransform.position.x;
        var enemyScaleX = enemyTransform.lossyScale.x;
        var minX = enemyPositionX - 0.5f * enemyScaleX;
        var maxX = enemyPositionX + 0.5f * enemyScaleX;
        return myX >= minX && myX <= maxX;
    }
}