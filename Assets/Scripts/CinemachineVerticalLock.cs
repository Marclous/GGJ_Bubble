using UnityEngine;
using Cinemachine;

/// <summary>
/// A Cinemachine extension that locks the camera's X position, 
/// allowing only vertical (Y) movement.
/// </summary>
[ExecuteAlways]
[SaveDuringPlay]
[AddComponentMenu("Cinemachine/Extensions/VerticalLock")]
public class CinemachineVerticalLock : CinemachineExtension
{
    // If checked, the camera will lock its X position to the initial X of the virtual camera.
    [SerializeField] private bool lockXToInitial = true;

    // If not using initial X, you can manually specify an X coordinate here.
    [SerializeField] private float lockedX = 0f;

    private float initialX; 

    protected override void OnEnable()
    {
        base.OnEnable();

        // Capture the initial X position (if we have a valid VirtualCamera reference).
        if (VirtualCamera != null)
        {
            initialX = VirtualCamera.State.RawPosition.x;
        }
        else
        {
            initialX = transform.position.x;
        }
    }

    protected override void PostPipelineStageCallback(
        CinemachineVirtualCameraBase vcam, 
        CinemachineCore.Stage stage, 
        ref CameraState state, 
        float deltaTime)
    {
        // Only modify the position at the Body stage
        if (stage == CinemachineCore.Stage.Body)
        {
            Vector3 newPos = state.RawPosition;

            // Lock the X coordinate to either the initial X or a custom lockedX
            newPos.x = lockXToInitial ? initialX : lockedX;

            // Assign the modified position back
            state.RawPosition = newPos;
        }
    }
}