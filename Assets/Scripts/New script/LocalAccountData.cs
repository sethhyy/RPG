using UnityEngine;

public static class LocalAccountData
{
    // =========================
    // FLAGS
    // =========================
    public static bool HasCharacter(string email)
    {
        return PlayerPrefs.GetInt(email + "_HasCharacter", 0) == 1;
    }

    public static void SetHasCharacter(string email)
    {
        PlayerPrefs.SetInt(email + "_HasCharacter", 1);
    }

    // =========================
    // PLAYER NAME
    // =========================
    public static void SavePlayerName(string email, string name)
    {
        PlayerPrefs.SetString(email + "_PlayerName", name);
    }

    public static string LoadPlayerName(string email)
    {
        return PlayerPrefs.GetString(email + "_PlayerName", "");
    }

    // =========================
    // GENDER
    // =========================
    public static void SaveGender(string email, string gender)
    {
        PlayerPrefs.SetString(email + "_Gender", gender);
    }

    public static string LoadGender(string email)
    {
        return PlayerPrefs.GetString(email + "_Gender", "Lalake");
    }

    // =========================
    // CHARACTER GENDER (optional)
    // =========================
    public static void SaveCharacterGender(string email, string characterGender)
    {
        PlayerPrefs.SetString(email + "_CharacterGender", characterGender);
    }

    public static string LoadCharacterGender(string email)
    {
        return PlayerPrefs.GetString(email + "_CharacterGender", "Lalake");
    }

    // =========================
    // COLOR
    // =========================
    public static void SaveColor(string email, Color color)
    {
        PlayerPrefs.SetFloat(email + "_ColorR", color.r);
        PlayerPrefs.SetFloat(email + "_ColorG", color.g);
        PlayerPrefs.SetFloat(email + "_ColorB", color.b);
        PlayerPrefs.SetFloat(email + "_ColorA", color.a);
    }

    public static Color LoadColor(string email)
    {
        return new Color(
            PlayerPrefs.GetFloat(email + "_ColorR", 1f),
            PlayerPrefs.GetFloat(email + "_ColorG", 1f),
            PlayerPrefs.GetFloat(email + "_ColorB", 1f),
            PlayerPrefs.GetFloat(email + "_ColorA", 1f)
        );
    }

    // =========================
    // HEARTS
    // =========================
    public static void SaveHearts(string email, int currentHearts, int maxHearts)
    {
        PlayerPrefs.SetInt(email + "_CurrentHearts", currentHearts);
        PlayerPrefs.SetInt(email + "_MaxHearts", maxHearts);
    }

    public static void LoadHearts(string email, out int current, out int max)
    {
        max = PlayerPrefs.GetInt(email + "_MaxHearts", 5);
        current = PlayerPrefs.GetInt(email + "_CurrentHearts", max);
    }

    // =========================
    // SAVE ALL AT ONCE
    // =========================
    public static void SaveAll(string email, PlayerData data)
    {
        SetHasCharacter(email);
        SavePlayerName(email, data.PlayerName);
        SaveGender(email, data.Gender);
        SaveCharacterGender(email, data.CharacterGender);
        SaveColor(email, data.ClothingColor);
        SaveHearts(email, data.CurrentHearts, data.MaxHearts);

        PlayerPrefs.Save();
    }

    // =========================
    // LOAD INTO PLAYERDATA
    // =========================
    public static PlayerData LoadPlayerData(string email)
    {
        PlayerData data = new PlayerData
        {
            PlayerName = LoadPlayerName(email),
            Gender = LoadGender(email),
            CharacterGender = LoadCharacterGender(email),
            ClothingColor = LoadColor(email)
        };

        LoadHearts(email, out int current, out int max);
        data.MaxHearts = max;
        data.CurrentHearts = current;

        return data;
    }
}


