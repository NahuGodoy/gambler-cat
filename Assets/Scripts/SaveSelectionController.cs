using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Events;


public class SaveSelectionController : MonoBehaviour
{
    [SerializeField] private Button[] slotButtons = new Button[SaveManager.SlotCount];
    [SerializeField] private TMP_Text[] slotLabels = new TMP_Text[SaveManager.SlotCount];
    [SerializeField] private Button[] deleteButtons = new Button[SaveManager.SlotCount];
    [SerializeField] private GameObject deleteConfirmationPanel;
    [SerializeField] private TMP_Text deleteConfirmationLabel;
    [SerializeField] private Button confirmDeleteButton;
    [SerializeField] private Button cancelDeleteButton;
    [SerializeField] private string tutorialSceneName = "EscenarioTutorial";
    [SerializeField] private string lobbySceneName = "LobbyTest";

    private static readonly Color CardBackground = new Color(0.075f, 0.045f, 0.105f, 0.96f);
    private static readonly Color CasinoGold = new Color(1f, 0.72f, 0.22f, 1f);
    private static readonly Color CasinoGoldDark = new Color(0.62f, 0.38f, 0.08f, 1f);
    private static readonly Color CasinoRed = new Color(0.72f, 0.08f, 0.12f, 1f);
    private static readonly Color CasinoText = new Color(0.96f, 0.92f, 0.82f, 1f);

    private UnityAction[] selectActions;
    private UnityAction[] deleteActions;
    private int pendingDeleteSlot = -1;

    private void OnEnable()
    {
        ApplyCasinoStyle();
        BindButtonListeners();
        if (SaveManager.Instance != null)
        {
            SaveManager.Instance.ProfilesChanged += Refresh;
        }

        Refresh();
    }

    private void OnDisable()
    {
        UnbindButtonListeners();
        if (SaveManager.Instance != null)
        {
            SaveManager.Instance.ProfilesChanged -= Refresh;
        }
    }

    public void SelectSlot(int slotIndex)
    {
        PlayerPowerUpInventory.LimpiarEstadosPersistentes();
        SaveManager.Instance.SelectSlot(slotIndex);
        string destination = SaveManager.Instance.ActiveSave.tutorialCompleted
            ? lobbySceneName
            : tutorialSceneName;
        SceneManager.LoadScene(destination);
    }
    public void ResetSlot(int slotIndex)
    {
        pendingDeleteSlot = slotIndex;
        if (deleteConfirmationPanel != null)
        {
            deleteConfirmationPanel.SetActive(true);
        }

        if (deleteConfirmationLabel != null)
        {
            deleteConfirmationLabel.text = $"<size=44><color=#FFD779>¿BORRAR LA PARTIDA {slotIndex + 1}?</color></size>\n<size=28>Esta acción no se puede deshacer.</size>";
        }
    }

    public void ConfirmResetSlot()
    {
        if (pendingDeleteSlot < 0 || SaveManager.Instance == null)
        {
            return;
        }

        SaveManager.Instance.ResetSlot(pendingDeleteSlot);
        pendingDeleteSlot = -1;
        if (deleteConfirmationPanel != null)
        {
            deleteConfirmationPanel.SetActive(false);
        }

        Refresh();
    }

    public void CancelResetSlot()
    {
        pendingDeleteSlot = -1;
        if (deleteConfirmationPanel != null)
        {
            deleteConfirmationPanel.SetActive(false);
        }
    }

    private void BindButtonListeners()
    {
        if (deleteConfirmationPanel != null)
        {
            deleteConfirmationPanel.SetActive(false);
        }

        selectActions = new UnityAction[SaveManager.SlotCount];
        deleteActions = new UnityAction[SaveManager.SlotCount];
        for (int slotIndex = 0; slotIndex < SaveManager.SlotCount; slotIndex++)
        {
            int capturedSlotIndex = slotIndex;
            selectActions[slotIndex] = () => SelectSlot(capturedSlotIndex);
            deleteActions[slotIndex] = () => ResetSlot(capturedSlotIndex);

            if (slotIndex < slotButtons.Length && slotButtons[slotIndex] != null)
            {
                slotButtons[slotIndex].onClick.AddListener(selectActions[slotIndex]);
            }

            if (slotIndex < deleteButtons.Length && deleteButtons[slotIndex] != null)
            {
                deleteButtons[slotIndex].onClick.AddListener(deleteActions[slotIndex]);
            }
        }

        if (confirmDeleteButton != null)
        {
            confirmDeleteButton.onClick.AddListener(ConfirmResetSlot);
        }

        if (cancelDeleteButton != null)
        {
            cancelDeleteButton.onClick.AddListener(CancelResetSlot);
        }
    }

    private void UnbindButtonListeners()
    {
        if (selectActions != null)
        {
            for (int slotIndex = 0; slotIndex < selectActions.Length; slotIndex++)
            {
                if (slotIndex < slotButtons.Length && slotButtons[slotIndex] != null)
                {
                    slotButtons[slotIndex].onClick.RemoveListener(selectActions[slotIndex]);
                }

                if (slotIndex < deleteButtons.Length && deleteButtons[slotIndex] != null)
                {
                    deleteButtons[slotIndex].onClick.RemoveListener(deleteActions[slotIndex]);
                }
            }
        }

        if (confirmDeleteButton != null)
        {
            confirmDeleteButton.onClick.RemoveListener(ConfirmResetSlot);
        }

        if (cancelDeleteButton != null)
        {
            cancelDeleteButton.onClick.RemoveListener(CancelResetSlot);
        }
    }

    public void Refresh()
    {
        if (SaveManager.Instance == null)
        {
            return;
        }

        for (int slotIndex = 0; slotIndex < SaveManager.SlotCount; slotIndex++)
        {
            SaveData save = SaveManager.Instance.GetSave(slotIndex);
            if (slotIndex < slotLabels.Length && slotLabels[slotIndex] != null)
            {
                slotLabels[slotIndex].text = BuildSlotLabel(slotIndex, save);
            }
        }
    }

    private static string BuildSlotLabel(int slotIndex, SaveData save)
    {
        if (!save.created)
        {
            return $"<size=52><color=#FFD779>PARTIDA {slotIndex + 1}</color></size>\n\n<size=36>Nueva partida</size>";
        }

        string achievementsState = $"<b>LOGROS</b>  <color=#FFD779>{save.ContarLogros()}/6</color>";
        string collectiblesState = $"<b>COLECCIONABLES</b>  <color=#FFD779>{save.ContarEasterEggs()}/6</color>";
        string chipsState = $"<b>FICHAS</b>  <color=#FFD779>{save.totalFichas}</color>";
        return $"<size=52><color=#FFD779>PARTIDA {slotIndex + 1}</color></size>\n\n{achievementsState}\n\n{collectiblesState}\n\n{chipsState}";
    }

    private void ApplyCasinoStyle()
    {
        AddSceneTitle();
        for (int slotIndex = 0; slotIndex < slotButtons.Length; slotIndex++)
        {
            if (slotIndex < slotButtons.Length && slotButtons[slotIndex] != null)
            {
                Image slotImage = slotButtons[slotIndex].targetGraphic as Image;
                if (slotImage != null)
                {
                    slotImage.color = CardBackground;
                    Outline slotOutline = slotButtons[slotIndex].GetComponent<Outline>();
                    if (slotOutline == null)
                    {
                        slotOutline = slotButtons[slotIndex].gameObject.AddComponent<Outline>();
                    }
                    slotOutline.effectColor = CasinoGoldDark;
                    slotOutline.effectDistance = new Vector2(5f, -5f);
                }

                ColorBlock colors = slotButtons[slotIndex].colors;
                colors.normalColor = CardBackground;
                colors.highlightedColor = new Color(0.18f, 0.105f, 0.22f, 1f);
                colors.pressedColor = CasinoGoldDark;
                colors.selectedColor = new Color(0.14f, 0.09f, 0.16f, 1f);
                slotButtons[slotIndex].colors = colors;
            }

            if (slotIndex < slotLabels.Length && slotLabels[slotIndex] != null)
            {
                slotLabels[slotIndex].color = CasinoText;
                slotLabels[slotIndex].fontSize = 38f;
                slotLabels[slotIndex].fontStyle = FontStyles.Bold;
                slotLabels[slotIndex].alignment = TextAlignmentOptions.Midline;
                slotLabels[slotIndex].lineSpacing = 18f;
            }

            if (slotIndex < deleteButtons.Length && deleteButtons[slotIndex] != null)
            {
                Image deleteImage = deleteButtons[slotIndex].targetGraphic as Image;
                if (deleteImage != null)
                {
                    deleteImage.color = CasinoRed;
                }

                ColorBlock colors = deleteButtons[slotIndex].colors;
                colors.normalColor = CasinoRed;
                colors.highlightedColor = new Color(0.95f, 0.18f, 0.22f, 1f);
                colors.pressedColor = new Color(0.45f, 0.03f, 0.06f, 1f);
                deleteButtons[slotIndex].colors = colors;

                TMP_Text deleteText = deleteButtons[slotIndex].GetComponentInChildren<TMP_Text>();
                if (deleteText != null)
                {
                    deleteText.color = Color.white;
                    deleteText.fontStyle = FontStyles.Bold;
                }
            }
        }

        if (deleteConfirmationPanel != null)
        {
            Image panelImage = deleteConfirmationPanel.GetComponent<Image>();
            if (panelImage != null)
            {
                panelImage.color = new Color(0.09f, 0.035f, 0.07f, 0.98f);
            }
        }

        if (deleteConfirmationLabel != null)
        {
            deleteConfirmationLabel.color = CasinoText;
            deleteConfirmationLabel.fontStyle = FontStyles.Bold;
        }

        StyleConfirmationButton(confirmDeleteButton, CasinoRed, Color.white);
        StyleConfirmationButton(cancelDeleteButton, CasinoGoldDark, CasinoText);

        Transform panelTransform = null;
        Canvas[] canvases = FindObjectsByType<Canvas>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (Canvas canvas in canvases)
        {
            Transform found = canvas.transform.Find("Panel");
            if (found != null)
            {
                panelTransform = found;
                break;
            }
        }

        if (panelTransform != null)
        {
            Image background = panelTransform.GetComponent<Image>();
            if (background != null)
            {
                background.color = new Color(0.025f, 0.015f, 0.045f, 0.96f);
            }
        }
    }

    private static void StyleConfirmationButton(Button button, Color background, Color textColor)
    {
        if (button == null)
        {
            return;
        }

        Image buttonImage = button.targetGraphic as Image;
        if (buttonImage != null)
        {
            buttonImage.color = background;
        }

        ColorBlock colors = button.colors;
        colors.normalColor = background;
        colors.highlightedColor = background * 1.25f;
        colors.pressedColor = background * 0.72f;
        colors.selectedColor = background;
        button.colors = colors;

        TMP_Text buttonText = button.GetComponentInChildren<TMP_Text>();
        if (buttonText != null)
        {
            buttonText.color = textColor;
            buttonText.fontStyle = FontStyles.Bold;
        }
    }

    private void AddSceneTitle()
    {
        Canvas[] canvases = FindObjectsByType<Canvas>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (Canvas canvas in canvases)
        {
            if (canvas.transform.Find("SceneTitle_SaveSelection") != null)
            {
                return;
            }
        }

        if (canvases.Length == 0)
        {
            return;
        }

        GameObject titleObject = new GameObject("SceneTitle_SaveSelection", typeof(RectTransform), typeof(CanvasRenderer), typeof(TextMeshProUGUI));
        titleObject.transform.SetParent(canvases[0].transform, false);

        RectTransform titleRect = titleObject.GetComponent<RectTransform>();
        titleRect.anchorMin = new Vector2(0.5f, 1f);
        titleRect.anchorMax = new Vector2(0.5f, 1f);
        titleRect.pivot = new Vector2(0.5f, 1f);
        titleRect.anchoredPosition = new Vector2(0f, -35f);
        titleRect.sizeDelta = new Vector2(900f, 100f);

        TMP_Text titleText = titleObject.GetComponent<TextMeshProUGUI>();
        titleText.text = "<color=#FFD779>ELIGE TU PARTIDA</color>";
        titleText.fontSize = 54f;
        titleText.fontStyle = FontStyles.Bold;
        titleText.alignment = TextAlignmentOptions.Center;
        titleText.color = CasinoText;
        titleText.raycastTarget = false;
    }
}