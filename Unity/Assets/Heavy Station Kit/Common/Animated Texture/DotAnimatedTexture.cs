using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Sequence item Class
[System.Serializable]
public class dotATSequence : System.Object {
	public int totalFrames = 0;
	public int firstFrame = 0;
	public int startingFrame = 0;
	public bool randomly = false; 
}

[ExecuteInEditMode]
public class DotAnimatedTexture : MonoBehaviour {
	#if UNITY_EDITOR
	[ReadOnlyAttribute] 
	#endif
	public int materialTotalFrames;
	public int activeSequence = 0;
	public List<dotATSequence> sequences;
	public int FPS = 10;
	// Private properties
	private Renderer _renderer = null;
	private int materialTileRows = 0;
	private int materialTileCols = 0;
	#if UNITY_EDITOR
	private int prevWarning = 0;
	#endif
	private int prevIndex = -1;

	void Start(){
		Init();
	}

	void Init(){
		if (_renderer == null) { _renderer = GetComponent<Renderer> ();	}
		Material rendererMaterial = (_renderer == null) ? null : _renderer.sharedMaterial;
		if (rendererMaterial == null) {
			materialTileRows = materialTileCols = 0;
		} else {
			Vector2 _size = rendererMaterial.mainTextureScale;
			materialTileRows = (_size.y > 0) ? Mathf.RoundToInt (1.0f / _size.y) : 1;
			materialTileCols = (_size.x > 0) ? Mathf.RoundToInt (1.0f / _size.x) : 1;
		}
		#if UNITY_EDITOR
		if (materialTileRows * materialTileCols == 0) {	Debug.LogWarning (transform.name + ": Material was not set or its 'Tile' value is incorrect");}
		#endif
	}

	void Update() {
		if (!Application.isPlaying) { Init (); }
		// Validate frame grid size
		materialTotalFrames = materialTileRows * materialTileCols;
		if ( materialTotalFrames == 0) {
			#if UNITY_EDITOR
			if (prevWarning != 1) {	Debug.LogWarning (transform.name + ": Incorrect parameters 'Row Count' or 'Col Count'");prevWarning = 1;}
			#endif
			return;
		} 
		// Validate active sequence
		if ((sequences == null) || (activeSequence + 1 > sequences.Count)) { // !2017-07-09 
			#if UNITY_EDITOR
			if (prevWarning != 2) {	Debug.LogWarning (transform.name + ": Incorrect parameter 'Active Sequence' or sequences was not set");prevWarning = 2;}
			#endif
			return;
		}
		// Init parameters
		var totalFrames = sequences [activeSequence].totalFrames;
		var startingFrame = sequences [activeSequence].startingFrame;
		var firstFrame = sequences [activeSequence].firstFrame;
		var randomly = sequences [activeSequence].randomly;
		// Validate active sequence parameters
		if ((totalFrames == 0) || (totalFrames + firstFrame > materialTileCols * materialTileRows)) {
			#if UNITY_EDITOR
			if (prevWarning != 3) {	Debug.LogWarning (transform.name + ": Incorrect parameter 'Total Frames' or/and 'Base Frame' for sequence #" + activeSequence);prevWarning = 3;}
			#endif
			return;
		}
		if (_renderer == null) {
			#if UNITY_EDITOR
			if (prevWarning != 4) {	Debug.LogWarning (transform.name + ": Renderer component was not found");prevWarning = 4;}
			#endif
		} else {
			// Size of frame
			Vector2 size = new Vector2 (1.0f / materialTileCols, 1.0f / materialTileRows);
			// Calculate newIndex
			int newIndex = firstFrame;  
			if (Application.isPlaying) {
				newIndex += ((int)(Time.time * FPS) + startingFrame) % totalFrames;
			} else {
				newIndex += startingFrame % totalFrames;
				Vector2 _size = _renderer.sharedMaterial.mainTextureScale;
				int _total = 0;
				if( _size.x * _size.y != 0 ){
					_total = Mathf.RoundToInt(1.0f / (_size.x * _size.y));
				};
				#if UNITY_EDITOR
				if(_total==0){Debug.LogWarning (transform.name + ": Material was not set or its 'Tile' value is incorrect");}
				#endif
				materialTotalFrames = _total;
			}
			if (newIndex != prevIndex) {
				prevIndex = newIndex;
				if(randomly){ newIndex = Mathf.RoundToInt (Random.value * (totalFrames - 1)); }
				// Current offset
				Vector2 offset = new Vector2 ((newIndex % materialTileCols) * size.x, (1.0f - size.y) - ((int)(newIndex / materialTileCols)) * size.y);
				// Set offset and size for texture
				if (Application.isPlaying) {
					_renderer.material.mainTextureOffset = offset;
				} else {
					#if UNITY_EDITOR
					Material rendererMaterial = Material.Instantiate(_renderer.sharedMaterial);
					rendererMaterial.name = _renderer.sharedMaterial.name;
					rendererMaterial.mainTextureOffset = offset;
					_renderer.material = rendererMaterial;
					#endif
				}	

			}

		}

	}

}
