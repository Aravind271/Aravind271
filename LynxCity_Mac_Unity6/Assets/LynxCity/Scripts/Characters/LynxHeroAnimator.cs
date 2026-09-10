using UnityEngine;
using LynxCity.Player;

namespace LynxCity.Characters
{
    /// <summary>
    /// Procedural locomotion for the V2.1 authoring fallback. This deliberately animates a proper
    /// joint hierarchy (hips/knees/shoulders/elbows) so the test character reads as a person rather
    /// than sliding primitives. A production Animator Controller can replace this component later.
    /// </summary>
    public sealed class LynxHeroAnimator : MonoBehaviour
    {
        LynxMotor motor;
        Transform visual, pelvis, spine, chest, head;
        Transform lShoulder, rShoulder, lElbow, rElbow, lThigh, rThigh, lKnee, rKnee;
        Vector3 pelvisBase;
        float phase;
        bool bound;

        void Awake() => motor = GetComponent<LynxMotor>();

        public void Bind(Transform visualRoot)
        {
            visual = visualRoot;
            pelvis = Find("Rig/Root/Pelvis");
            spine = Find("Rig/Root/Pelvis/Spine");
            chest = Find("Rig/Root/Pelvis/Spine/Chest");
            head = Find("Rig/Root/Pelvis/Spine/Chest/Neck/Head");
            lShoulder = Find("Rig/Root/Pelvis/Spine/Chest/LeftShoulder");
            rShoulder = Find("Rig/Root/Pelvis/Spine/Chest/RightShoulder");
            lElbow = Find("Rig/Root/Pelvis/Spine/Chest/LeftShoulder/LeftElbow");
            rElbow = Find("Rig/Root/Pelvis/Spine/Chest/RightShoulder/RightElbow");
            lThigh = Find("Rig/Root/Pelvis/LeftThigh");
            rThigh = Find("Rig/Root/Pelvis/RightThigh");
            lKnee = Find("Rig/Root/Pelvis/LeftThigh/LeftKnee");
            rKnee = Find("Rig/Root/Pelvis/RightThigh/RightKnee");
            if (pelvis) pelvisBase = pelvis.localPosition;
            bound = pelvis && spine && chest && head && lShoulder && rShoulder && lThigh && rThigh && lKnee && rKnee;
        }

        Transform Find(string path) => visual ? visual.Find(path) : null;

        void LateUpdate()
        {
            if (!bound || motor == null) return;
            float speed01 = motor.CurrentSpeed01;
            float speed = motor.CurrentSpeed;
            float moving = Mathf.SmoothStep(0f, 1f, speed01);
            phase += Time.deltaTime * Mathf.Lerp(2.4f, 8.8f, Mathf.Clamp01(speed / 5.2f));

            float stride = Mathf.Sin(phase) * Mathf.Lerp(0f, 31f, moving);
            float opposing = Mathf.Sin(phase + Mathf.PI);
            float kneeL = Mathf.Max(0f, opposing) * 28f * moving;
            float kneeR = Mathf.Max(0f, -opposing) * 28f * moving;
            float arm = stride * 0.72f;
            float bob = Mathf.Abs(Mathf.Sin(phase * 2f)) * 0.018f * moving;
            float sway = Mathf.Sin(phase) * 0.012f * moving;

            pelvis.localPosition = pelvisBase + new Vector3(sway, bob, 0f);
            pelvis.localRotation = Quaternion.Euler(0f, Mathf.Sin(phase) * 2.4f * moving, Mathf.Sin(phase) * 1.8f * moving);
            spine.localRotation = Quaternion.Euler(1.0f * moving, -Mathf.Sin(phase) * 2.8f * moving, 0f);
            chest.localRotation = Quaternion.Euler(Mathf.Sin(Time.time * 1.8f) * (1f - moving) * 0.55f, Mathf.Sin(phase) * 3.2f * moving, 0f);
            head.localRotation = Quaternion.Euler(Mathf.Sin(Time.time * 0.73f) * 0.8f * (1f-moving), -Mathf.Sin(phase) * 1.4f * moving, 0f);

            lThigh.localRotation = Quaternion.Euler(stride, 0f, 0f);
            rThigh.localRotation = Quaternion.Euler(-stride, 0f, 0f);
            lKnee.localRotation = Quaternion.Euler(kneeL, 0f, 0f);
            rKnee.localRotation = Quaternion.Euler(kneeR, 0f, 0f);
            lShoulder.localRotation = Quaternion.Euler(-arm, 0f, -3f);
            rShoulder.localRotation = Quaternion.Euler(arm, 0f, 3f);
            if (lElbow) lElbow.localRotation = Quaternion.Euler(-8f - Mathf.Max(0f, -arm) * 0.20f, 0f, 0f);
            if (rElbow) rElbow.localRotation = Quaternion.Euler(-8f - Mathf.Max(0f, arm) * 0.20f, 0f, 0f);
        }
    }
}
