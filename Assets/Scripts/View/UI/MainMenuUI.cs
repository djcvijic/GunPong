using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    private static readonly PersistedString PersistedPlayer1Name = new("Player1Name");

    private static readonly PersistedString PersistedPlayer2Name = new("Player2Name");

    private static readonly PersistedInt PersistedPlayer1BrainIndex = new("Player1BrainIndex");

    private static readonly PersistedInt PersistedPlayer2BrainIndex = new("Player2BrainIndex");

    [SerializeField] private TMP_InputField player1NameInput;

    [SerializeField] private TMP_Dropdown player1BrainDropdown;

    [SerializeField] private TMP_InputField player2NameInput;

    [SerializeField] private TMP_Dropdown player2BrainDropdown;

    [SerializeField] private UIButton startGameButton;

    private List<Type> brainTypes;

    private void Start()
    {
        brainTypes = typeof(PaddleBrain).GetImplementingClasses().ToList();
        var brainNames = brainTypes.ConvertAll(x => new TMP_Dropdown.OptionData(x.Name));
        player1BrainDropdown.options = brainNames;
        player1BrainDropdown.value = brainNames.FindIndex(x => x.text == nameof(LocalPlayerInput));
        player2BrainDropdown.options = brainNames;
        player2BrainDropdown.value = brainNames.FindIndex(x => x.text == nameof(SlowerPaddleBrain));
        LoadPlayerPrefs();
        startGameButton.OnRelease += OnClickStartGameButton;
    }

    private void LoadPlayerPrefs()
    {
        var loadedPlayer1Name = PersistedPlayer1Name.Get();
        if (!loadedPlayer1Name.IsNullOrEmpty()) player1NameInput.text = loadedPlayer1Name;

        var loadedPlayer2Name = PersistedPlayer2Name.Get();
        if (!loadedPlayer2Name.IsNullOrEmpty()) player2NameInput.text = loadedPlayer2Name;

        var loadedPlayer1BrainIndex = PersistedPlayer1BrainIndex.Get();
        if (loadedPlayer1BrainIndex.HasValue) player1BrainDropdown.value = loadedPlayer1BrainIndex.Value;

        var loadedPlayer2BrainIndex = PersistedPlayer2BrainIndex.Get();
        if (loadedPlayer2BrainIndex.HasValue) player2BrainDropdown.value = loadedPlayer2BrainIndex.Value;
    }

    private void OnClickStartGameButton()
    {
        SavePlayerPrefs();
        var player1Brain = brainTypes[player1BrainDropdown.value];
        var player2Brain = brainTypes[player2BrainDropdown.value];
        GameView.Instance.StartGame(player1NameInput.text, player1Brain, player2NameInput.text, player2Brain);
        gameObject.SetActive(false);
    }

    private void SavePlayerPrefs()
    {
        PersistedPlayer1Name.Set(player1NameInput.text);
        PersistedPlayer2Name.Set(player2NameInput.text);
        PersistedPlayer1BrainIndex.Set(player1BrainDropdown.value);
        PersistedPlayer2BrainIndex.Set(player2BrainDropdown.value);
    }
}