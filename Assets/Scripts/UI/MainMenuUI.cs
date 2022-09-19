using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
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
        startGameButton.OnRelease += OnClickStartGameButton;
    }

    private void OnClickStartGameButton()
    {
        var player1Brain = brainTypes[player1BrainDropdown.value];
        var player2Brain = brainTypes[player2BrainDropdown.value];
        GameView.Instance.StartGame(player1NameInput.text, player1Brain, player2NameInput.text, player2Brain);
        gameObject.SetActive(false);
    }
}