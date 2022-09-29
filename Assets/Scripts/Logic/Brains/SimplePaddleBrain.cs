using JetBrains.Annotations;
using Logic.Core;
using UnityEngine;
using View.GameViews;
using View.UI;

namespace Logic.Brains
{
    [UsedImplicitly]
    public class SimplePaddleBrain : IPaddleBrain
    {
        private const float BallFollowPositionOffsetPercentage = 0.05f;

        private GameViewController gameViewController;

        private PaddleView paddle;

        public void Initialize(GameUI gameUI, GameViewController gameViewController, PaddleView paddle)
        {
            this.gameViewController = gameViewController;
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
            var ballX = gameViewController.Ball.transform.position.x;
            var myTransform = paddle.transform;
            var myX = myTransform.position.x;
            var myScaleX = myTransform.lossyScale.x;
            var offsetX = BallFollowPositionOffsetPercentage * myScaleX;
            var mySpeed = paddle.Speed;
            var result = (ballX - myX + offsetX) / mySpeed / deltaTime;
            var resultVector = myTransform.rotation * new Vector3(result, 0f, 0f);
            var resultRotated = resultVector.x;
            return resultRotated;
        }

        private bool FireIfEnemyAhead()
        {
            if (paddle.IsBallAttached) return true;

            var myX = paddle.transform.position.x;
            var enemyPaddle = gameViewController.GetEnemyPaddle(paddle.Owner);
            var enemyTransform = enemyPaddle.transform;
            var enemyPositionX = enemyTransform.position.x;
            var enemyScaleX = enemyTransform.lossyScale.x;
            var minX = enemyPositionX - 0.5f * enemyScaleX;
            var maxX = enemyPositionX + 0.5f * enemyScaleX;
            return myX >= minX && myX <= maxX;
        }
    }
}