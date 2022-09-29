using System;
using System.Collections.Generic;
using System.Linq;
using Logic.Brains;
using TMPro;
using UnityEngine;
using Util.Extensions;
using Util.PersistedVariables;
using View.Audio;
using View.GameViews;

namespace View.UI
{
    public class MainMenuUI : MonoBehaviour
    {
        private static readonly PersistedString PersistedPlayer1Name = new("Player1Name");

        private static readonly PersistedString PersistedPlayer2Name = new("Player2Name");

        private static readonly PersistedInt PersistedPlayer1BrainIndex = new("Player1BrainIndex");

        private static readonly PersistedInt PersistedPlayer2BrainIndex = new("Player2BrainIndex");

        private static readonly PersistedBool PersistedSoundEnabledValue = new("SoundEnabled");

        private static readonly PersistedBool PersistedMusicEnabledValue = new("MusicEnabled");

        [SerializeField] private TMP_InputField player1NameInput;

        [SerializeField] private TMP_Dropdown player1BrainDropdown;

        [SerializeField] private TMP_InputField player2NameInput;

        [SerializeField] private TMP_Dropdown player2BrainDropdown;

        [SerializeField] private UIButton startGameButton;

        [SerializeField] private UIButton soundEnabledButton;

        [SerializeField] private TMP_Text soundEnabledText;

        [SerializeField] private UIButton musicEnabledButton;

        [SerializeField] private TMP_Text musicEnabledText;

        private List<Type> brainTypes;

        private void Start()
        {
            brainTypes = typeof(IPaddleBrain).GetImplementingClasses().ToList();
            var brainNames = brainTypes.ConvertAll(x => new TMP_Dropdown.OptionData(x.Name));
            player1BrainDropdown.options = brainNames;
            player1BrainDropdown.value = brainNames.FindIndex(x => x.text == nameof(LocalPlayerInput));
            player2BrainDropdown.options = brainNames;
            player2BrainDropdown.value = brainNames.FindIndex(x => x.text == nameof(SlowerPaddleBrain));
            ApplySoundEnabledValue(true);
            ApplyMusicEnabledValue(true);
            LoadPlayerPrefs();
            startGameButton.OnRelease += OnClickStartGameButton;
            soundEnabledButton.OnRelease += OnClickSoundEnabledButton;
            musicEnabledButton.OnRelease += OnClickMusicEnabledButton;
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

            var loadedSoundEnabledValue = PersistedSoundEnabledValue.Get();
            if (loadedSoundEnabledValue.HasValue) ApplySoundEnabledValue(loadedSoundEnabledValue.Value);

            var loadedMusicEnabledValue = PersistedMusicEnabledValue.Get();
            if (loadedMusicEnabledValue.HasValue) ApplyMusicEnabledValue(loadedMusicEnabledValue.Value);
        }

        private void ApplySoundEnabledValue(bool soundEnabled)
        {
            AudioManager.Instance.SoundEnabled = soundEnabled;
            soundEnabledText.text = soundEnabled ? "MUTE SOUND" : "UNMUTE SOUND";
        }

        private void ApplyMusicEnabledValue(bool musicEnabled)
        {
            AudioManager.Instance.MusicEnabled = musicEnabled;
            musicEnabledText.text = musicEnabled ? "MUTE MUSIC" : "UNMUTE MUSIC";
        }

        private void OnClickStartGameButton()
        {
            SavePlayerPrefs();
            var player1Brain = brainTypes[player1BrainDropdown.value];
            var player2Brain = brainTypes[player2BrainDropdown.value];
            GameViewController.Instance.StartGame(player1NameInput.text, player1Brain, player2NameInput.text, player2Brain);
            gameObject.SetActive(false);
        }

        private void OnClickSoundEnabledButton()
        {
            ApplySoundEnabledValue(!AudioManager.Instance.SoundEnabled);
        }

        private void OnClickMusicEnabledButton()
        {
            ApplyMusicEnabledValue(!AudioManager.Instance.MusicEnabled);
        }

        private void SavePlayerPrefs()
        {
            PersistedPlayer1Name.Set(player1NameInput.text);
            PersistedPlayer2Name.Set(player2NameInput.text);
            PersistedPlayer1BrainIndex.Set(player1BrainDropdown.value);
            PersistedPlayer2BrainIndex.Set(player2BrainDropdown.value);
            PersistedSoundEnabledValue.Set(AudioManager.Instance.SoundEnabled);
            PersistedMusicEnabledValue.Set(AudioManager.Instance.MusicEnabled);
        }
    }
}