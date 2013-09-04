using UnityEngine;
using System.Collections;

using Spil;

namespace Spil{
	
	public class IgaPanel : MonoBehaviour {
		private bool wasViewed = false;
		public SpilUnity spilUnity;
		public Texture2D texture;
		public string link;
		public string adId;
		
		// Use this for initialization
		void Start () {
			renderer.material.mainTexture = texture;
			renderer.material.mainTextureScale = new Vector2(-1,-1);
			float w = texture.width;
			float h = texture.height;
			float sx = 1;
			float sz = 1;
			
			if(w > h){
				sz = h/w;
			}else{
				sx = w/h;
			}
			
			transform.localScale = new Vector3(sx, 1, sz);
		}
		
		// Update is called once per frame
		void Update () {
			Vector3 point = Vector3.zero;
			if(Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.Android){
				foreach(Touch t in Input.touches){
					if(t.phase == TouchPhase.Ended){
						point = new Vector3(t.position.x, t.position.y, 0);
						break;
					}
				}
			}else{ //Editor!
				if(Input.GetMouseButtonUp(0))
					point = Input.mousePosition;	
			}
			
			if(point != Vector3.zero)
				if(checkHit(point))
					Application.OpenURL(link);
		}
	
		private bool checkHit(Vector3 point){
			Ray ray = Camera.main.ScreenPointToRay(point);
			RaycastHit hit = new RaycastHit();
			if(Physics.Raycast(ray, out hit)){
				if(hit.collider == collider){
					return true;	
				}
			} 
			return false;
		}
		
		void OnBecameVisible(){
			if(!wasViewed && spilUnity != null){
				spilUnity._AdMarkAsShown(adId);
				wasViewed = true;
			}
		}
	}
}