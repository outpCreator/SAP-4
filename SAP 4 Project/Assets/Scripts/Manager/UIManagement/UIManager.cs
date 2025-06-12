using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;


    enum InputType { KeyboardMouse, Gamepad }
    InputType currentInput = InputType.KeyboardMouse;

    [Header("UI Menus")]
    public List<GameObject> menus = new List<GameObject>();

    [Header("UI Components")]
    public List<GameObject> InputsInfo = new List<GameObject>();
    [SerializeField] Sprite gamepadInputsSprite;
    [SerializeField] Sprite keyboardInputsSprite;

    public List<GameObject> MovementInfo = new List<GameObject>();
    [SerializeField] Sprite gamepadMovementSprite;
    [SerializeField] Sprite keyboardMovementSprite;

    public List<GameObject> CameraMovementInfo = new List<GameObject>();
    [SerializeField] Sprite gamepadCameraMovementSprite;
    [SerializeField] Sprite keyboardCameraMovementSprite;

    public List<GameObject> QuickSelectInfo = new List<GameObject>();
    [SerializeField] Sprite gamepadQuickSelectSprite;
    [SerializeField] Sprite keyboardQuickSelectSprite;

    public List<GameObject> OpenMenuInfo = new List<GameObject>();
    [SerializeField] Sprite gamepadOpenMenuSprite;
    [SerializeField] Sprite keyboardOpenMenuSprite;

    public List<GameObject> ChangeTargetInfo = new List<GameObject>();
    [SerializeField] Sprite gamepadChangeTargetSprite;
    [SerializeField] Sprite keyboardChangeTargetSprite;

    public List<GameObject> QuickCraftInfo = new List<GameObject>();
    [SerializeField] Sprite gamepadQuickCraftSprite;
    [SerializeField] Sprite keyboardQuickCraftSprite;

    public List<GameObject> UseItemInfo = new List<GameObject>();
    [SerializeField] Sprite gamepadUseItemSprite;
    [SerializeField] Sprite keyboardUseItemSprite;

    public List<GameObject> CancelInfo = new List<GameObject>();
    [SerializeField] Sprite gamepadCancelSprite;
    [SerializeField] Sprite keyboardCancelSprite;

    public List<GameObject> UsePotionInfo = new List<GameObject>();
    [SerializeField] Sprite gamepadUsePotionSprite;
    [SerializeField] Sprite keyboardUsePotionSprite;

    public List<GameObject> InteractionInfo = new List<GameObject>();
    [SerializeField] Sprite gamepadInteractionSprite;
    [SerializeField] Sprite keyboardInteractionSprite;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        CheckInputs();

    }

    public void SetCallerActive(GameObject caller)
    {
        foreach (GameObject menu in menus)
        {
            menu.SetActive(false);
            menu.SetActive(caller);
        }
    }

    void CheckInputs()
    {
        if (Gamepad.current != null && Gamepad.current.allControls.Any(x => x is ButtonControl button && button.isPressed && !x.synthetic))
        {
            if (currentInput != InputType.Gamepad)
            {
                UpdateUI(InputType.Gamepad);
            }
        }

        if ((Keyboard.current != null && Keyboard.current.anyKey.wasPressedThisFrame) || (Mouse.current != null && MouseInputDetection()))
        {
            if (currentInput != InputType.KeyboardMouse)
            {
                UpdateUI(InputType.KeyboardMouse);
            }
        }
    }

    void UpdateUI(InputType type)
    {
        currentInput = type;

        bool isGamepad = type == InputType.Gamepad;

        UpdateSprites(InputsInfo,           isGamepad ? gamepadInputsSprite :           keyboardInputsSprite);
        UpdateSprites(MovementInfo,         isGamepad ? gamepadMovementSprite :         keyboardMovementSprite);
        UpdateSprites(CameraMovementInfo,   isGamepad ? gamepadCameraMovementSprite :   keyboardCameraMovementSprite);
        UpdateSprites(QuickSelectInfo,      isGamepad ? gamepadQuickSelectSprite :      keyboardQuickSelectSprite);
        UpdateSprites(OpenMenuInfo,         isGamepad ? gamepadOpenMenuSprite :         keyboardOpenMenuSprite);
        UpdateSprites(ChangeTargetInfo,     isGamepad ? gamepadChangeTargetSprite :     keyboardChangeTargetSprite);
        UpdateSprites(QuickCraftInfo,       isGamepad ? gamepadQuickCraftSprite :       keyboardQuickCraftSprite);
        UpdateSprites(UseItemInfo,          isGamepad ? gamepadUseItemSprite :          keyboardUseItemSprite);
        UpdateSprites(CancelInfo,           isGamepad ? gamepadCancelSprite :           keyboardCancelSprite);
        UpdateSprites(UsePotionInfo,        isGamepad ? gamepadUsePotionSprite :        keyboardUsePotionSprite);
        UpdateSprites(InteractionInfo,      isGamepad ? gamepadInteractionSprite :      keyboardInteractionSprite);
    }

    void UpdateSprites(List<GameObject> elements, Sprite sprite)
    {
        foreach (var obj in elements)
        {
            var image = obj.GetComponent<UnityEngine.UI.Image>();
            if (image != null)
            {
                image.sprite = sprite;
            }
        }
    }

    bool MouseInputDetection()
    {
        var mouse = Mouse.current;
        if (mouse == null) return false;

        return mouse.leftButton.wasPressedThisFrame || mouse.rightButton.wasPressedThisFrame || mouse.middleButton.wasPressedThisFrame || mouse.scroll.ReadValue() != Vector2.zero || mouse.delta.ReadValue() != Vector2.zero;
    }
}
