using APICalls;
using Gameplay;
using UnityEngine;
using Webmobril.AudioManager;

[DisallowMultipleComponent]
public class LetterGameAudioHolder : MonoBehaviour
{
    [SerializeField] private AudioClip musicClip;
    [SerializeField] private AudioClip endOfRound;
    [SerializeField] private AudioClip[] TileSounds;
    [SerializeField] private AudioClip[] TileToTraySounds;
    [SerializeField] private AudioClip endOfRoundNoScore;

    [SerializeField] private AudioClip undo;

    // [SerializeField]  private AudioClip gameplayBonus;
    [SerializeField] private AudioClip UndoAll;
    [SerializeField] private AudioClip popupOpen;

    [SerializeField] private AudioClip TwoXLAudio;
    [SerializeField] private AudioClip TwoXWAudio;
    [SerializeField] private AudioClip ThreeXLAudio;
    [SerializeField] private AudioClip ThreeXWAudio;
    [SerializeField] private AudioClip tossTile;


    [SerializeField] private GridController _gridController;

    private void OnValidate()
    {
        _gridController = FindObjectOfType<GridController>();
    }

    public static LetterGameAudioHolder Instance;

    private void Start()
    {
        Instance = this;
        //AudioManager.Instance.PlayMusic(musicClip);
    }

    public void PlayTileSound()
    {
        AudioClip tileSound = TileSounds[_gridController.GetAddedNewLetterCount - 1];
        LogSystem.LogEvent("tileSound {0}", tileSound.name);
        AudioManager.Instance.PlaySound(tileSound);
    }

    public void PlayTileSound(int index)
    {
        AudioClip tileSound = TileSounds[index];
        LogSystem.LogEvent("tileSound {0}", tileSound.name);
        AudioManager.Instance.PlaySound(tileSound);
    }

    public void PlayTileToTraySound()
    {
        AudioClip tileSound = TileToTraySounds[_gridController.GetAddedNewLetterCount];
        LogSystem.LogEvent("tileSound {0}", tileSound.name);
        AudioManager.Instance.PlaySound(tileSound);
    }

    public void PlayUndoSound()
    {
        AudioManager.Instance.PlaySound(undo);
    }

    public void PlayUndoAllSound()
    {
        AudioManager.Instance.PlaySound(UndoAll);
    }

    public void PlayTossSound()
    {
        AudioManager.Instance.PlaySound(tossTile);
    }

    public void PlayEndOfRoundSound(bool hasScore)
    {
        if (hasScore)
        {
            AudioManager.Instance.PlaySound(endOfRound);
        }
        else
        {
            AudioManager.Instance.PlaySound(endOfRoundNoScore);
        }
    }

    public void PlayBonusSound(PowerUpType powerUpType)
    {
        AudioClip audioClip = null;
        switch (powerUpType)
        {
            case PowerUpType.TwoXL:
                audioClip = TwoXLAudio;
                break;

            case PowerUpType.TwoXW:
                audioClip = TwoXWAudio;
                break;

            case PowerUpType.ThreeXL:
                audioClip = ThreeXLAudio;
                break;

            case PowerUpType.ThreeXW:
                audioClip = ThreeXWAudio;
                break;
        }


        AudioManager.Instance.PlaySound(audioClip);
    }
}