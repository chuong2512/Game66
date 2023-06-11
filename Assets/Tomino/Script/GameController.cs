using UnityEngine;
using Tomino;

public class GameController : MonoBehaviour
{
    public Camera currentCamera;
    public Game game;
    public BoardView boardView;
    public PieceView nextPieceView;
    public ScoreView scoreView;
    public LevelView levelView;
    public AlertView alertView;
    public SettingsView settingsView;
    public AudioPlayer audioPlayer;
    public GameObject screenButtons;
    public AudioSource musicAudioSource;

    private UniversalInput universalInput;

    private void Awake()
    {
        HandlePlayerSettings();
        Settings.ChangedEvent += HandlePlayerSettings;
    }

    void Start()
    {
        Board board = new Board(10, 20);

        boardView.SetBoard(board);
        nextPieceView.SetBoard(board);

        universalInput = new UniversalInput(new KeyboardInput(), boardView.touchInput);

        game = new Game(board, universalInput);
        game.FinishedEvent += OnGameFinished;
        game.PieceFinishedFallingEvent += audioPlayer.PlayPieceDropClip;
        game.PieceRotatedEvent += audioPlayer.PlayPieceRotateClip;
        game.PieceMovedEvent += audioPlayer.PlayPieceMoveClip;
        game.Start();

        scoreView.game = game;
        levelView.game = game;
    }

    public void OnPauseButtonTap()
    {
        game.Pause();
        ShowPauseView();
    }

    public void OnMoveLeftButtonTap()
    {
        game.SetNextAction(PlayerAction.MoveLeft);
    }

    public void OnMoveRightButtonTap()
    {
        game.SetNextAction(PlayerAction.MoveRight);
    }

    public void OnMoveDownButtonTap()
    {
        game.SetNextAction(PlayerAction.MoveDown);
    }

    public void OnFallButtonTap()
    {
        game.SetNextAction(PlayerAction.Fall);
    }

    public void OnRotateButtonTap()
    {
        game.SetNextAction(PlayerAction.Rotate);
    }

    void OnGameFinished()
    {
        alertView.SetTitle(ConstantGame.Text.GameFinished);
        alertView.AddButton(ConstantGame.Text.PlayAgain, game.Start, audioPlayer.PlayNewGameClip);
        alertView.Show();
    }

    void Update()
    {
        game.Update(Time.deltaTime);
    }

    void ShowPauseView()
    {
        alertView.SetTitle(ConstantGame.Text.GamePaused);
        alertView.AddButton(ConstantGame.Text.Resume, game.Resume, audioPlayer.PlayResumeClip);
        alertView.AddButton(ConstantGame.Text.NewGame, game.Start, audioPlayer.PlayNewGameClip);
       // alertView.AddButton(ConstantGame.Text.Settings, ShowSettingsView, audioPlayer.PlayResumeClip);
        alertView.Show();
    }

    void ShowSettingsView()
    {
        settingsView.Show(ShowPauseView);
    }

    void HandlePlayerSettings()
    {
        screenButtons.SetActive(true);
        boardView.touchInput.Enabled = false;
        musicAudioSource.gameObject.SetActive(Settings.MusicEnabled);
    }
}
