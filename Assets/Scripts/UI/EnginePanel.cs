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

		if (items.Count == requiredNumber)
		{
			if (VictoryAchieved != null)
			{
				VictoryAchieved();
			}
		}
	}
}
