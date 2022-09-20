using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    private static string PersistedPlayer1Name
    {
        get => PlayerPrefs.HasKey("Player1Name") ? PlayerPrefs.GetString("Player1Name") : null;
        set
        {
            if (value != null) PlayerPrefs.SetString("Player1Name", value);
            else PlayerPrefs.DeleteKey("Player1Name");
        }
    }

    private static string PersistedPlayer2Name
    {
        get => PlayerPrefs.HasKey("Player2Name") ? PlayerPrefs.GetString("Player2Name") : null;
        set
        {
            if (value != null) PlayerPrefs.SetString("Player2Name", value);
            else PlayerPrefs.DeleteKey("Player2Name");
        }
    }

    private static int? PersistedPlayer1BrainIndex
    {
        get => PlayerPrefs.HasKey("Player1BrainIndex") ? PlayerPrefs.GetInt("Player1BrainIndex") : null;
        set
        {
            if (value.HasValue) PlayerPrefs.SetInt("Player1BrainIndex", value.Value);
            else PlayerPrefs.DeleteKey("Player1BrainIndex");
        }
    }

    private static int? PersistedPlayer2BrainIndex
    {
        get => PlayerPrefs.HasKey("Player2BrainIndex") ? PlayerPrefs.GetInt("Player2BrainIndex") : null;
        set
        {
            if (value.HasValue) PlayerPrefs.SetInt("Player2BrainIndex", value.Value);
            else PlayerPrefs.DeleteKey("Player2BrainIndex");
        }
    }

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
        player2BrainDropdown.value = brainNames.FindIndex(x => x.text != nameof(LocalPlayerInput));
        LoadPlayerPrefs();
        startGameButton.OnRelease += OnClickStartGameButton;
    }

    private void LoadPlayerPrefs()
    {
        var loadedPlayer1Name = PersistedPlayer1Name;
        if (!loadedPlayer1Name.IsNullOrEmpty()) player1NameInput.text = loadedPlayer1Name;

        var loadedPlayer2Name = PersistedPlayer2Name;
        if (!loadedPlayer2Name.IsNullOrEmpty()) player2NameInput.text = loadedPlayer2Name;

        var loadedPlayer1BrainIndex = PersistedPlayer1BrainIndex;
        if (loadedPlayer1BrainIndex.HasValue) player1BrainDropdown.value = loadedPlayer1BrainIndex.Value;

        var loadedPlayer2BrainIndex = PersistedPlayer2BrainIndex;
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
        PersistedPlayer1Name = player1NameInput.text;
        PersistedPlayer2Name = player2NameInput.text;
        PersistedPlayer1BrainIndex = player1BrainDropdown.value;
        PersistedPlayer2BrainIndex = player2BrainDropdown.value;
    }
}