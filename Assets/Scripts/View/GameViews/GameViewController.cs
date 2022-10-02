using System;
using Logic.Brains;
using Logic.Core;
using Logic.GameBounds;
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

        [SerializeField] private PaddleView bottomPaddle;

        [SerializeField] private PaddleView topPaddle;

        [SerializeField] private AudioClipSettings musicSettings;

        [SerializeField] private AudioClipSettings dieSoundSettings;

        [SerializeField] private AudioClipSettings pingSoundSettings;

        [SerializeField] private AudioClipSettings pongSoundSettings;

        private Vector3 bottomPaddleStartingPosition;

        private Vector3 topPaddleStartingPosition;

        private Player player1;

        private Player player2;

        private GameFlow gameFlow;

        public GameState GameState => gameFlow.GameState;

        public AudioManager AudioManager => AudioManager.Instance;

        public GameBoundsFactory GameBoundsFactory { get; private set; }

        public BallView Ball => ball;

        private void Start()
        {
            GameBoundsFactory = new GameBoundsFactory(gameBoundsTransform.position, gameBoundsTransform.lossyScale,
                paddedBoundsTransform.lossyScale, true, true, false);
            bottomPaddleStartingPosition = bottomPaddle.transform.position;
            topPaddleStartingPosition = topPaddle.transform.position;
            gameFlow = new GameFlow(GameUI.Instance, CoroutineRunner.Instance);
            gameFlow.StartMainMenu();
            AudioManager.PlayAudio(musicSettings);
        }

        private Player DetermineOwner(PaddleView paddle)
        {
            return paddle == bottomPaddle ? player1 : player2;
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
            return topPaddle.Owner == owner ? topPaddle : bottomPaddle.Owner == owner ? bottomPaddle : null;
        }

        public PaddleView GetEnemyPaddle(Player owner)
        {
            return topPaddle.Owner == owner ? bottomPaddle : bottomPaddle.Owner == owner ? topPaddle : null;
        }

        public void BulletHitPaddle(PaddleView paddle)
        {
            var player = paddle.Owner;
            var previousLives = player.CurrentLives;
            player.LoseHealth();
            ReactToPlayerStatus(player, previousLives);
        }

        public void BallHitPaddle(PaddleView paddle)
        {
            AudioManager.PlayAudio(paddle.Owner == player1 ? pingSoundSettings : pongSoundSettings);
        }

        public void BallHitBottom()
        {
            var player = bottomPaddle.Owner;
            var previousLives = player.CurrentLives;
            player.LoseLife();
            ReactToPlayerStatus(player, previousLives);
        }

        public void BallHitTop()
        {
            var player = topPaddle.Owner;
            var previousLives = player.CurrentLives;
            player.LoseLife();
            ReactToPlayerStatus(player, previousLives);
        }

        private void ReactToPlayerStatus(Player player, int previousLives)
        {
            if (player == bottomPaddle.Owner)
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

            topPaddle.Owner = DetermineOwner(topPaddle);
            bottomPaddle.Owner = DetermineOwner(bottomPaddle);

            topPaddle.Brain = DetermineBrain(topPaddle);
            bottomPaddle.Brain = DetermineBrain(bottomPaddle);

            bottomPaddle.transform.position = bottomPaddleStartingPosition;
            topPaddle.transform.position = topPaddleStartingPosition;

            var topPlayer = topPaddle.Owner;
            var bottomPlayer = bottomPaddle.Owner;
            GameUI.Instance.Initialize(topPlayer, bottomPlayer);

            gameFlow.PrepareServe(GetPaddle(player1), ball);
        }
    }
}