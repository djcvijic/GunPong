using JetBrains.Annotations;
using Logic.Core;
using UnityEngine;
using View.GameViews;
using View.UI;

namespace Logic.Brains
{
    [UsedImplicitly]
    public class SlowerPaddleBrain : IPaddleBrain
    {
        private const float SpeedFactor = 0.5f;

        private const float BallFollowPositionOffsetPercentage = 0.05f;

        private GameView gameView;

        private PaddleView paddle;

        public void Initialize(GameUI gameUI, GameView gameView, PaddleView paddle)
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
            var result = (ballX - myX + offsetX) / mySpeed  / deltaTime;
            return Mathf.Clamp(result, -SpeedFactor, SpeedFactor);
        }

        private bool FireIfEnemyAhead()
        {
            if (paddle.IsBallAttached) return true;

            var myX = paddle.transform.position.x;
            var enemyPaddle = gameView.GetEnemyPaddle(paddle.Owner);
            var enemyTransform = enemyPaddle.transform;
            var enemyPositionX = enemyTransform.position.x;
            var enemyScaleX = enemyTransform.lossyScale.x;
            var minX = enemyPositionX - 0.5f * enemyScaleX;
            var maxX = enemyPositionX + 0.5f * enemyScaleX;
            return myX >= minX && myX <= maxX;
        }
    }
}