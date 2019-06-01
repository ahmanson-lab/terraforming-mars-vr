using UnityEngine;
using System.Collections;

public class DotHskElevator2Events : DotHskElevatorEventsClass {

	public Material displayUpMat;
	public Material displayDnMat;
	public Material displayDnPlatformMat;
	private DotHskAtlasedMaterialControl displayUp;
	private DotHskAtlasedMaterialControl displayDn;
	private DotHskAtlasedMaterialControl displayDnPlatform;


	public override void OnFloorChange (int floor, bool willMove, int dir){
		if (elevator != null) {
			if (displayUp != null) { displayUp.setActiveFrame( elevator.floors [floor].floorNumber + 9 ); } // Display current floor
			if (displayDn != null) { displayDn.setActiveFrame( willMove ? 1 - dir : 1 ); } // Display move status
		}
	}

	public override void OnInit(){
		if ( (displayUpMat != null) && (displayDnMat != null) && (displayDnPlatformMat != null) ) {
			if (elevator.platformConsole != null) {
				initConsoleDisplays (elevator.platformConsole,true);
				if (displayDnPlatform != null) { displayDnPlatform.setActiveFrame (19); }
			}
			for (int i = 0; i < elevator.floors.Count; i++) {
				if (elevator.floors [i].console != null) {
					initConsoleDisplays (elevator.floors [i].console,false);
				}
			}
		}
	}

	private void initConsoleDisplays(DotHskElevator2Console console, bool isConsole){
		MeshRenderer[] meshes = console.transform.GetComponentsInChildren<MeshRenderer>();
		if (meshes != null) {
			foreach (MeshRenderer mesh in meshes) {
				switch(mesh.name){
				case "C_El_DisplUp":
					mesh.sharedMaterial = displayUpMat;
					if (displayUp == null) { displayUp = new DotHskAtlasedMaterialControl(mesh,true); }
					break;
				case  "C_El_DisplDn":
					if (isConsole) {
						mesh.sharedMaterial = displayDnPlatformMat;
						if (displayDnPlatform == null) {
							displayDnPlatform = new DotHskAtlasedMaterialControl (mesh, true);
						}
					} else {
						mesh.sharedMaterial = displayDnMat;
						if (displayDn == null) {
							displayDn = new DotHskAtlasedMaterialControl (mesh, true);
						}
					}
					break;
				}
			}
		}
	}

	public override void OnStart (){
		if ((elevator != null)&&(displayDnPlatform != null)) {
			displayDnPlatform.setActiveFrame( elevator.floors [elevator.targetFloor].floorNumber + 9 ); // Display target floor
		}
	}
	public override void OnStop (){
		if ((elevator != null)&&(displayDnPlatform!= null)) {
			displayDnPlatform.setActiveFrame( 19 ); // Display empty field
		}
	}
	public override void OnActivateConsole(int floor){}
	public override void OnDeactivateConsole(int floor){}

}

// Support class for atlased material control
public class DotHskAtlasedMaterialControl {
	private Renderer matRenderer;
	private int materialTileRows = 0;
	private int materialTileCols = 0;
	private Vector2 materialSize;
	private int totalFrames = 0;
	private int prevFrame = -1;
	private int activeFrame = 0;
	bool useSharedMaterial = true;

	public DotHskAtlasedMaterialControl(Renderer _renderer, bool _useSharedMaterial){
		matRenderer = _renderer;
		useSharedMaterial = _useSharedMaterial;
		if ( (matRenderer != null) && (matRenderer.sharedMaterial != null) ){
			materialSize  = matRenderer.sharedMaterial.mainTextureScale;
			materialTileRows = (materialSize.y > 0) ? Mathf.RoundToInt (1f / materialSize.y) : 1;
			materialTileCols = (materialSize.x > 0) ? Mathf.RoundToInt (1f / materialSize.x) : 1;
			totalFrames = materialTileRows * materialTileCols;
		}
	}

	public int getTotalFrames(){ return totalFrames; }

	public bool setActiveFrame(int frame){
		if ( (totalFrames == 0) || (prevFrame == frame) ) { return false; }
		activeFrame = (frame < 0) ? 0 :((frame < totalFrames) ? frame : totalFrames-1 );
		Vector2 offset = new Vector2 ((activeFrame % materialTileCols) * materialSize.x, (1.0f - materialSize.y) - ((int)(activeFrame / materialTileCols)) * materialSize.y);
		if (useSharedMaterial) {
			matRenderer.sharedMaterial.mainTextureOffset = offset;
		} else {
			Material rendererMaterial = Material.Instantiate (matRenderer.sharedMaterial);
			rendererMaterial.name = matRenderer.sharedMaterial.name;
			rendererMaterial.mainTextureOffset = offset;
			matRenderer.material = rendererMaterial;
		}
		prevFrame = activeFrame;
		return true;
	}

}
