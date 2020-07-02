using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnginePanel : MonoBehaviour
{
	public ItemData targetItem;
	public int requiredNumber;
	public GameObject itemPrefab;
	List<GameObject> items = new List<GameObject>();

	public delegate void VictoryHandler();
	public event VictoryHandler VictoryAchieved;

	[FMODUnity.EventRef]
	public string insertPart;

	[FMODUnity.EventRef]
	public string startEngine;
	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public void Add(ItemData item)
	{
		GameObject go = Instantiate(itemPrefab, transform);
		items.Add(go);
		go.GetComponentInChildren<Text>().text = item.name;

		FMODUnity.RuntimeManager.PlayOneShot(insertPart);

		if (items.Count == requiredNumber)
		{
			FMODUnity.RuntimeManager.PlayOneShot(startEngine);
			if (VictoryAchieved != null)
			{
				VictoryAchieved();
			}
		}
	}
}
