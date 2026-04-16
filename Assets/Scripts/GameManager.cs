using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public PlayerData playerData = new PlayerData();
    public string LoggedInEmail;
    public string lastExit;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            if (playerData == null) playerData = new PlayerData();
        }
        else
        {
            // If this object also hosts other gameplay/UI scripts, do not destroy the whole GameObject.
            int other = 0;
            foreach (var mb in GetComponents<MonoBehaviour>())
            {
                if (mb != null && mb != this)
                    other++;
            }
            if (other == 0)
                Destroy(gameObject);
            else
                Destroy(this);
        }
    }

    public void ClearExit() { lastExit = ""; }
    public void SetExit(string exitName) { lastExit = exitName; }
}