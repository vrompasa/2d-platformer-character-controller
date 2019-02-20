using UnityEngine;

[System.Serializable]
public class ControllerParameters
{
    [Tooltip("Whether the game object should be affected by gravity.")]
	public bool Flying;

    [Tooltip("Strength of gravity.")]
    public float Gravity = -28f;
    
    [Tooltip("What is the maximum velocity that can be reached?")]
    public Vector2 MaxVelocity = new Vector2(float.MaxValue, float.MaxValue);
   
    [Tooltip("The maximum speed that can be achieved by moving")]
    public float MaxSpeed = 3f;
    
    [Tooltip("The acceleration value while grounded.")]
    public float AccelerationOnGround = 20f;
    
    [Tooltip("The acceleration value while in the air.")]
    public float AccelerationInAir = 5f;
}