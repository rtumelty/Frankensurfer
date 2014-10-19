// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Material)]
	[HutongGames.PlayMaker.Tooltip("Sets a named float in a game object's material.")]
	public class SetMaterialFloat : FsmStateAction
	{
		[HutongGames.PlayMaker.Tooltip("The GameObject that the material is applied to.")]
		[CheckForComponent(typeof(Renderer))]
		public FsmOwnerDefault gameObject;

		[HutongGames.PlayMaker.Tooltip("GameObjects can have multiple materials. Specify an index to target a specific material.")]
		public FsmInt materialIndex;

		[HutongGames.PlayMaker.Tooltip("Alternatively specify a Material instead of a GameObject and Index.")]
		public FsmMaterial material;

		[RequiredField]
		[HutongGames.PlayMaker.Tooltip("A named float parameter in the shader.")]
		public FsmString namedFloat;
		
		[RequiredField]
		[HutongGames.PlayMaker.Tooltip("Set the parameter value.")]
		public FsmFloat floatValue;
		
		[HutongGames.PlayMaker.Tooltip("Repeat every frame. Useful if the value is animated.")]
		public bool everyFrame;

		public override void Reset()
		{
			gameObject = null;
			materialIndex = 0;
			material = null;
			namedFloat = "";
			floatValue = 0f;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoSetMaterialFloat();
			
			if (!everyFrame)
			{
				Finish();
			}
		}
		
		public override void OnUpdate ()
		{
			DoSetMaterialFloat();
		}
		
		void DoSetMaterialFloat()
		{
			if (material.Value != null)
			{
				material.Value.SetFloat(namedFloat.Value, floatValue.Value);
				return;
			}
			
			var go = Fsm.GetOwnerDefaultTarget(gameObject);
			if (go == null) return;

			if (go.renderer == null)
			{
				LogError("Missing Renderer!");
				return;
			}
			
			if (go.renderer.material == null)
			{
				LogError("Missing Material!");
				return;
			}
			
			if (materialIndex.Value == 0)
			{
				go.renderer.material.SetFloat(namedFloat.Value, floatValue.Value);
			}
			else if (go.renderer.materials.Length > materialIndex.Value)
			{
				var materials = go.renderer.materials;
				materials[materialIndex.Value].SetFloat(namedFloat.Value, floatValue.Value);
				go.renderer.materials = materials;			
			}	
		}
	}
}