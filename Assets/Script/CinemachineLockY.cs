﻿using UnityEngine;
using Cinemachine;

/// <summary>
/// An add-on module for Cinemachine Virtual Camera that locks the camera's Y co-ordinate
/// </summary>
[ExecuteInEditMode] [SaveDuringPlay] [AddComponentMenu("")] // Hide in menu
public class CinemachineLockY : CinemachineExtension
{
	[Tooltip("Lock the camera's Y position to this value")]
	public float YPosition = 10;

	protected override void PostPipelineStageCallback(
		CinemachineVirtualCameraBase vcam,
		CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
	{
		if (enabled && stage == CinemachineCore.Stage.Body)
		{
			var pos = state.RawPosition;
			pos.y = YPosition;
			state.RawPosition = pos;
		}
	}
}