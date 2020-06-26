using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingRecipe : MonoBehaviour
{
	public Toggle toggle;
	public Recipe recipe;
	public Text ingredients;

	private void Awake()
	{
		toggle.onValueChanged.AddListener((bool b) =>{ UpdateRecipe(b); });

		if (toggle.isOn)
		{
			UpdateRecipe(true);
			
		}
	}

	private void UpdateRecipe(bool b)
	{
		if (b)
		{
			ingredients.text = "";
			for (int i = 0; i < recipe.ItemCount; i++)
			{
				ingredients.text += recipe.GetNumber(i).ToString() + " " + recipe.GetItem(i).name + "\n";
			}
		}
	}

	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
