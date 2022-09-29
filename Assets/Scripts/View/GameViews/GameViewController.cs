using System;
using System.Collections.Generic;
using Logic.Brains;
using Logic.Core;
using UnityEngine;
using Util.CoroutineRunner;
using Util.GenericSingletons;
using View.Audio;
using View.UI;

namespace View.GameViews
{
    public class GameViewController : GenericMonoSingleton<GameViewController>
    {
        [SerializeField] private Transform gameBoundsTransform;

        [SerializeField] private Transform paddedBoundsTransform;

        [SerializeField] private BallView ball;

        [SerializeField] private List<PaddleView> paddles;

        [SerializeField] private AudioClipSettings musicSettings;

        [SerializeField] private AudioClipSettings dieSoundSettings;

        private Vector3 topPaddleStartingPosition;

        private Vector3 bottomPaddleStartingPosition;

        private Player player1;

        private Player player2;

        private GameFlow gameFlow;

        public GameState GameState => gameFlow.GameState;

        public AudioManager AudioManager => AudioManager.Instance;

        public GameBounds GameBounds { get; private set; }

        public BallView Ball => ball;

        private PaddleView BottomPaddle => paddles.Find(x => x.IsABottom);

        private PaddleView TopPaddle => paddles.Find(x => !x.IsABottom);

        private void Start()
        {
            GameBounds = new GameBounds(gameBoundsTransform, paddedBoundsTransform, true, true, false);
            topPaddleStartingPosition = TopPaddle.transform.position;
            bottomPaddleStartingPosition = BottomPaddle.transform.position;
            gameFlow = new GameFlow(GameUI.Instance, CoroutineRunner.Instance);
            gameFlow.StartMainMenu();
            AudioManager.PlayAudio(musicSettings);
        }

        private Player DetermineOwner(PaddleView paddle)
        {
            return paddle.IsABottom ? player1 : player2;
        }

        private IPaddleBrain DetermineBrain(PaddleView paddle)
        {
            var player = paddle.Owner;
            var brain = (IPaddleBrain)Activator.CreateInstance(player.Brain);
            brain.Initialize(GameUI.Instance, this, paddle);
            return brain;
        }

        public PaddleView GetPaddle(Player owner)
        {
            return paddles.Find(x => x.Owner == owner);
        }

        public PaddleView GetEnemyPaddle(Player owner)
        {
            return paddles.Find(x => x.Owner != owner);
        }

        public void BulletHitPaddle(PaddleView paddle)
        {
            var player = paddle.Owner;
            var previousLives = player.CurrentLives;
            player.LoseHealth();
            ReactToPlayerStatus(player, previousLives);
        }

        public void BallHitBottom()
        {
            var player = BottomPaddle.Owner;
            var previousLives = player.CurrentLives;
            player.LoseLife();
            ReactToPlayerStatus(player, previousLives);
        }

        public void BallHitTop()
        {
            var player = TopPaddle.Owner;
            var previousLives = player.CurrentLives;
            player.LoseLife();
            ReactToPlayerStatus(player, previousLives);
        }

        private void ReactToPlayerStatus(Player player, int previousLives)
        {
            if (GetPaddle(player).IsABottom)
            {
                GameUI.Instance.UpdateBottomPlayerLife(player);
            }
            else
            {
                GameUI.Instance.UpdateTopPlayerLife(player);
            }

            if (player.CurrentLives != previousLives)
            {
                ClearBullets();
                AudioManager.PlayAudio(dieSoundSettings);
                if (player.IsDead)
                {
                    gameFlow.GameOver(GetEnemyPlayer(player));
                }
                else
                {
                    gameFlow.PrepareServe(GetPaddle(player), ball);
                }
            }
        }

        private Player GetEnemyPlayer(Player player)
        {
            return player == player1 ? player2 : player1;
        }

        private void ClearBullets()
        {
            var bullets = GetComponentsInChildren<BulletView>();
            foreach (var bullet in bullets)
            {
                bullet.Kill();
            }
        }

        public void StartGame(string player1Name, Type player1Brain, string player2Name, Type player2Brain)
        {
            player1 = new Player(player1Name, player1Brain);
            player2 = new Player(player2Name, player2Brain);

            foreach (var paddle in paddles)
            {
                paddle.Owner = DetermineOwner(paddle);
                paddle.Brain = DetermineBrain(paddle);
            }

            BottomPaddle.transform.position = bottomPaddleStartingPosition;
            TopPaddle.transform.position = topPaddleStartingPosition;

            var topPlayer = TopPaddle.Owner;
            var bottomPlayer = BottomPaddle.Owner;
            GameUI.Instance.Initialize(topPlayer, bottomPlayer);

            gameFlow.PrepareServe(GetPaddle(player1), ball);
        }
    }
}