using UnityEngine;

public class PlayerHUD : MonoBehaviour
{
    public Vector2 barSize = new Vector2(220f, 20f);

    private PlayerController playerController;

    private GUIStyle pickupStyle;

    void Awake()
    {
        playerController = GetComponent<PlayerController>();
    }

    void OnGUI()
    {
        DrawDot();

        if (playerController.CanPickup)
            DrawPickupPrompt();

        DrawBar(new Vector2(20f, 20f), playerController.HealthPercent, Color.red, "Health");
        DrawBar(new Vector2(20f, 50f), playerController.StaminaPercent, Color.yellow, "Stamina");
    }

    void DrawDot()
    {
        Rect outline = new Rect(Screen.width * 0.5f - 3f, Screen.height * 0.5f - 3f, 6f, 6f);
        Rect dot = new Rect(Screen.width * 0.5f - 2f, Screen.height * 0.5f - 2f, 4f, 4f);

        GUI.color = Color.black;
        GUI.DrawTexture(outline, Texture2D.blackTexture);

        GUI.color = Color.white;
        GUI.DrawTexture(dot, Texture2D.whiteTexture);
        GUI.color = Color.white;
    }

    void DrawPickupPrompt()
    {
        if (pickupStyle == null)
        {
            pickupStyle = new GUIStyle(GUI.skin.label);
            pickupStyle.fontSize = 18;
            pickupStyle.alignment = TextAnchor.MiddleCenter;
            pickupStyle.normal.textColor = Color.blue;
        }

        string prompt = $"Press {playerController.ButtonPrompt} to pick up";

        Rect promptPosition = new Rect(Screen.width * 0.5f - 100f, Screen.height * 0.5f + 20f, 200f, 30f);

        GUI.Label(promptPosition, prompt, pickupStyle);
    }

    void DrawBar(Vector2 position, float amount, Color color, string label)
    {
        Rect background = new Rect(position.x, position.y, barSize.x, barSize.y);
        Rect fill = new Rect(position.x + 2f, position.y + 2f, (barSize.x - 4f) * Mathf.Clamp01(amount), barSize.y - 4f);

        GUI.color = new Color(0f, 0f, 0f, 0.8f);
        GUI.DrawTexture(background, Texture2D.whiteTexture);

        GUI.color = color;
        GUI.DrawTexture(fill, Texture2D.whiteTexture);

        GUI.color = Color.black;
        GUI.Label(new Rect(position.x + 5f, position.y, barSize.x, barSize.y), label);
    }
}
