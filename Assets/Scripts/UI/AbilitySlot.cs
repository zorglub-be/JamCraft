using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class AbilitySlot : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private GameObject AbilityPopupMenuPrefab;
    public Item Item { get; private set; }
    public bool IsEmpty => Item == null;
    private GameObject objectToDestroy;
    
   
    public void SetItem(Item item)
    {
        Item = item;
        this.GetComponentInChildren<Image>().sprite = item.Icon;

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Scene _currentScene = SceneManager.GetActiveScene();
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("Inventory"));
        objectToDestroy =  Instantiate(AbilityPopupMenuPrefab , eventData.pressPosition * 2, Quaternion.identity);
        objectToDestroy.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform,false);
        Debug.Log(eventData.pointerPress);
        SceneManager.SetActiveScene(_currentScene);

    }
}
