using TMPro;
using UnityEngine;

public class GameUi : MonoBehaviour
{
    public static GameUi instance;
    public CanvasUi _canvasUi;
    public ButtonUI _buttonUi;
    public TextUi _textUi;
    public InputFieldUi _inputFieldUi;
    public PrefabUi _Prefabs;
    public ImageUi _imageUi;
    
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        // Disable multitouch gestures
        Input.multiTouchEnabled = false;
    }
}