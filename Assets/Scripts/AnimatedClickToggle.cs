using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Ensures this component always has a Slider attached
[RequireComponent(typeof(Slider))]
public class AnimatedClickToggle : MonoBehaviour, IPointerClickHandler
{
    // Slider component that represents the switch handle position
    [SerializeField] private Slider slider;

    // All UI graphics that should dim when the switch is OFF
    [SerializeField] private Graphic[] dimmableGraphics;

    // Duration of the handle slide animation (in seconds)
    [SerializeField, Range(0.02f, 0.5f)]
    private float animationDuration = 0.12f;

    // Brightness multiplier when the switch is OFF
    [SerializeField, Range(0.1f, 1f)]
    private float offBrightnessMultiplier = 0.5f;

    // Called when user clicks the switch
    public UnityEvent<bool> onValueChanged;

    // Function that returns the external value (SettingsManager)
    private Func<bool> externalGetter;

    // Reference to the currently running animation coroutine
    private Coroutine animationRoutine;

    // Stores original colors so dimming is reversible
    private GraphicColor[] cachedColors;

    // Helper struct for color caching
    private struct GraphicColor
    {
        public Graphic graphic;
        public Color originalColor;
    }

    // Called when the component is reset in the Inspector
    private void Reset()
    {
        // Automatically assign the Slider on the same GameObject
        slider = GetComponent<Slider>();
    }

    // Called once when the object is created
    private void Awake()
    {
        // Safety check in case Reset() was not called
        if (slider == null)
            slider = GetComponent<Slider>();

        // Configure the slider to behave like a switch
        slider.minValue = 0f;          // OFF position
        slider.maxValue = 1f;          // ON position
        slider.wholeNumbers = false;   // Smooth animation
        slider.interactable = false;   // Disable dragging, click-only

        // If no graphics were assigned, collect all child graphics automatically
        if (dimmableGraphics == null || dimmableGraphics.Length == 0)
            dimmableGraphics = GetComponentsInChildren<Graphic>(true);

        // Cache original colors for all dimmable graphics
        cachedColors = new GraphicColor[dimmableGraphics.Length];
        for (int i = 0; i < dimmableGraphics.Length; i++)
        {
            cachedColors[i] = new GraphicColor
            {
                graphic = dimmableGraphics[i],
                originalColor = dimmableGraphics[i] != null
                    ? dimmableGraphics[i].color
                    : Color.white
            };
        }
    }

    // Called whenever the object becomes active
    private void OnEnable()
    {
        // When menu opens show correct state
        SyncFromExternal(false);
    }

    // Bind switch to SettingsManager value
    public void Bind(Func<bool> getter)
    {
        externalGetter = getter;

        // Immediately sync UI with external value
        SyncFromExternal(false);
    }

    // Update switch UI from SettingsManager value
    public void SyncFromExternal(bool animated)
    {
        // Do nothing if no external value is bound
        if (externalGetter == null) return;

        // Apply external value to the switch
        SetState(externalGetter.Invoke(), animated, false);
    }

    // Called automatically by Unity when the user clicks the UI element
    public void OnPointerClick(PointerEventData eventData)
    {
        // Toggle current state
        SetState(!IsOn(), true, true);
    }

    // Returns true if the switch is currently ON
    private bool IsOn()
    {
        return slider.value >= 0.5f;
    }

    // Applies a new ON/OFF state to the switch
    private void SetState(bool on, bool animated, bool sendEvent)
    {
        // Target slider value for ON or OFF
        float targetValue = on ? 1f : 0f;

        // Stop any running animation
        if (animationRoutine != null)
            StopCoroutine(animationRoutine);

        // Animate or instantly apply value
        if (animated)
            animationRoutine = StartCoroutine(AnimateTo(targetValue));
        else
            slider.value = targetValue;

        // Update visual brightness
        ApplyBrightness(on);

        // Notify listeners only when triggered by user
        if (sendEvent)
            onValueChanged?.Invoke(on);
    }

    // Smoothly animates the slider handle to the target value
    private IEnumerator AnimateTo(float target)
    {
        float startValue = slider.value;
        float elapsed = 0f;

        // Prevent division by zero
        float duration = Mathf.Max(0.0001f, animationDuration);

        while (elapsed < duration)
        {
            // Use unscaled time so animation works during pause
            elapsed += Time.unscaledDeltaTime;

            // Move slider value
            slider.value = Mathf.Lerp(startValue, target, elapsed / duration);

            yield return null;
        }

        // Snap to exact target
        slider.value = target;
        animationRoutine = null;
    }

    // Applies brightness based on ON/OFF state
    private void ApplyBrightness(bool on)
    {
        float multiplier = on ? 1f : offBrightnessMultiplier;

        for (int i = 0; i < cachedColors.Length; i++)
        {
            if (cachedColors[i].graphic == null) continue;

            Color baseColor = cachedColors[i].originalColor;

            // Multiply RGB channels, keep alpha unchanged
            cachedColors[i].graphic.color = new Color(
                baseColor.r * multiplier,
                baseColor.g * multiplier,
                baseColor.b * multiplier,
                baseColor.a
            );
        }
    }
}
