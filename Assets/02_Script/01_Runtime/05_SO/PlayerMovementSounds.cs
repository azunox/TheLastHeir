using UnityEngine;


[CreateAssetMenu(fileName = "Player Stats", menuName = "Player Stats/PlayerMovementSounds")]
public class PlayerMovementSounds : ScriptableObject
{
    public AudioClip playerLandingSound;
    
    public AudioClip[] playerFootstepSounds;
    
    [Range(0,1)] public float footStepVolume;

}