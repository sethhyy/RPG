using UnityEngine;
using UnityEngine.UI;

public class InteractableZone : MonoBehaviour
{
    public GameObject interactButton; 
    public GameObject dictionaryPopup;


    void Start()
    {
        interactButton.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player detected! Showing button.");
            interactButton.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            interactButton.SetActive(false);
            // Hindi na natin isasara ang dictionary dito
        }
    }


    public void OnInteractButtonPressed()
    {
        
            dictionaryPopup.SetActive(true);
            interactButton.SetActive(false);
        
    }

    public void CloseDictionary()
    {
        dictionaryPopup.SetActive(false);
        interactButton.SetActive(true);
    }

}