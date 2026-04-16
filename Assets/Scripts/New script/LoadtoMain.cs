using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadtoMain : MonoBehaviour
{
    public bool Whichtoload;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (Whichtoload == true)
        {
            Invoke("Nextscene", 1f);
        } else
        {
            Invoke("LoadtoCharater", 1f);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Nextscene()
    {
        SceneManager.LoadScene("LabasNgBahay");
    }

    public void LoadtoCharater()
    {
        SceneManager.LoadScene("CharacterSelect");
    }
}
