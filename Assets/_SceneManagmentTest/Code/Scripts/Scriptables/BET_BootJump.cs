using UnityEngine;

[CreateAssetMenu(fileName = "newBootJump", menuName = "Scriptable Object/Boot Jump")]
public class BET_BootJump : ScriptableObject
{
	public enum JumpLengthType { Value, Target }
	public JumpLengthType LengthType = JumpLengthType.Target;
	public float LengthValue = 1;
	[Range(0, 1)] public float TargetRedirection = 1;
    public float AnticipationDuration = 0.5f;
	public float JumpDuration = 2f;
	public float MaxHeight = 2f;
	public float LandingDuration = 0.5f;
	public AnimationCurve Progression = AnimationCurve.Linear(0, 0, 1, 1);
	public AnimationCurve Height = AnimationCurve.Constant(0, 1, 1);
}
