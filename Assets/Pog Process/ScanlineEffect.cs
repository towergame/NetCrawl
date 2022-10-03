using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering.Universal.PostProcessing;
using UnityEngine.Scripting;


// Mercilessly cannibalised from the tutorial on https://github.com/yahiaetman/URPCustomPostProcessingStack
// lowkey kinda bullshit how I have to use a package to do what's natively supported in HDRP for like 2 years now thanks unity
[System.Serializable, VolumeComponentMenu("CustomPostProcess/Scanline")]
public class ScanlineEffect : VolumeComponent
{
	[Tooltip("Controls the distance between the lines.")]
	public ClampedIntParameter lines = new ClampedIntParameter(0, 0, 10000);
	[Tooltip("Controls the distance between the horizontal lines.")]
	public ClampedIntParameter rows = new ClampedIntParameter(0, 0, 10000);
}

[CustomPostProcess("Scanline", CustomPostProcessInjectionPoint.BeforePostProcess), Preserve]
public class ScanlineEffectRenderer : CustomPostProcessRenderer
{
	private ScanlineEffect m_VolumeComponent;

	private Material m_Material;

	static class ShaderIDs
	{
		internal readonly static int Input = Shader.PropertyToID("_MainTex");
		internal readonly static int Lines = Shader.PropertyToID("_Lines");
		internal readonly static int Rows = Shader.PropertyToID("_Rows");
	}

	public override bool visibleInSceneView => false;

	public override ScriptableRenderPassInput input => ScriptableRenderPassInput.Color;

	[Preserve]
	public override void Initialize()
	{
		m_Material = CoreUtils.CreateEngineMaterial("Hidden/PostProcess/Scanline");
	}

	[Preserve]
	public override bool Setup(ref RenderingData renderingData, CustomPostProcessInjectionPoint injectionPoint)
	{
		var stack = VolumeManager.instance.stack;
		m_VolumeComponent = stack.GetComponent<ScanlineEffect>();
		return m_VolumeComponent.lines.value > 0;
	}

	[Preserve]
	public override void Render(CommandBuffer cmd, RenderTargetIdentifier source, RenderTargetIdentifier destination, ref RenderingData renderingData, CustomPostProcessInjectionPoint injectionPoint)
	{
		if (m_Material != null)
		{
			m_Material.SetFloat(ShaderIDs.Lines, m_VolumeComponent.lines.value);
			m_Material.SetFloat(ShaderIDs.Rows, m_VolumeComponent.rows.value);
		}
		cmd.SetGlobalTexture(ShaderIDs.Input, source);
		CoreUtils.DrawFullScreen(cmd, m_Material, destination);
	}
}