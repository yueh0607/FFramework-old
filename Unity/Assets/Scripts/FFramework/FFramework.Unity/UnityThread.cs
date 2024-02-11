using Assets.Scripts.FFramework;
using FFramework;
using UnityEngine;

public class UnityThread : MonoBehaviour
{

    void Start()
    {
        UpdateThread.FrameCall += LifeCycleEx.SendUpdate;
    }

    public readonly FVirtualThread UpdateThread = new FVirtualThread();
    private void Update()
    {
        UpdateThread.FrameCall?.Invoke(Time.deltaTime);
    }

    public readonly FVirtualThread FixedUpdateThread = new FVirtualThread();
    private void FixedUpdate()
    {
        FixedUpdateThread.FrameCall?.Invoke(Time.fixedDeltaTime);
    }

#if UNITY_EDITOR
    public readonly FVirtualThread GizmosUpdateThread = new FVirtualThread();
    private void OnDrawGizmos()
    {
        GizmosUpdateThread.FrameCall?.Invoke(Time.deltaTime);
    }
#endif

}
