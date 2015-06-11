using UnityEngine;
using System.Collections;

public class BgScroller : MonoBehaviour {

	public float scrollSpeed;
	Renderer r;
	private Vector2 savedOffset;
	void Start () {
		r = GetComponent<Renderer>();
		savedOffset = r.sharedMaterial.GetTextureOffset("_MainTex");
	}

	void Update () {
		float x = Mathf.Repeat(Time.time * scrollSpeed,1);
//		Debug.Log(x);
		r.sharedMaterial.SetTextureOffset("_MainTex",new Vector2(x,savedOffset.y));
	}

	void OnDisable(){
		r.sharedMaterial.SetTextureOffset("_MainTex",savedOffset);
	}
}
