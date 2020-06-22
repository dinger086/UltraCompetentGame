using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
	[SerializeField]
	Transform player;

	Vector3 cv;

    // Update is called once per frame
    void Update()
    {
		if (player != null)
		{
			//Make Camera focus on player
			Vector3 Pos = player.position;
			Pos.z = -10f;
			transform.position = Vector3.SmoothDamp(transform.position, Pos, ref cv, 1f);
		}
		
	}
}
