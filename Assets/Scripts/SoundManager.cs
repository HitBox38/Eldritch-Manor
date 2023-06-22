using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// TODO: active the sound in-game
public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioSource SFX;
    [SerializeField] private AudioSource AreaSFX;
    [Header("SOUNDS")]
    [SerializeField] private AudioClip walkAudioClip;
    [SerializeField] private AudioClip jumpAudioClip;
    [SerializeField] private AudioClip glassesOnAudioClip;
    [SerializeField] private AudioClip glassesOffAudioClip;
    [SerializeField] private AudioClip itemCollectionAudioClip;
    [SerializeField] private AudioClip marbleRollAudioClip;
    [SerializeField] private AudioClip marbleImpactAudioClip;
    [SerializeField] private AudioClip marbleFireAudioClip;
    [SerializeField] private AudioClip marbleClickAudioClip;
    [SerializeField] private AudioClip marbleReloadAudioClip;
    [SerializeField] private AudioClip resetLeverFunctionalAudioClip;
    [SerializeField] private AudioClip resetLeverCrackedAudioClip;
    [SerializeField] private AudioClip resetLeverBrokenAudioClip;
    [SerializeField] private AudioClip resetLeverFailAudioClip;
    [SerializeField] private AudioClip roomRotationAudioClip;
    [SerializeField] private AudioClip buttonPressedAudioClip;
    [SerializeField] private AudioClip scissorsUsedAudioClip;

    public static SoundManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
            Destroy(gameObject);
    }

    public void PlaySound(SoundTypes sound)
    {
        switch (sound)
        {
            case SoundTypes.Walk:
                SFX.clip = walkAudioClip;
                break;
            case SoundTypes.Jumping:
                SFX.clip = jumpAudioClip;
                break;
            case SoundTypes.Running:
                SFX.clip = walkAudioClip;
                break;
            case SoundTypes.GlassesOn:
                SFX.clip = glassesOnAudioClip;
                break;
            case SoundTypes.GlassesOff:
                SFX.clip = glassesOffAudioClip;
                break;
            case SoundTypes.ItemCollection:
                SFX.clip = itemCollectionAudioClip;
                break;
            case SoundTypes.MarbleRoll:
                SFX.clip = marbleRollAudioClip;
                break;
            case SoundTypes.MarbleImpact:
                SFX.clip = marbleImpactAudioClip;
                break;
            case SoundTypes.MarbleFire:
                SFX.clip = marbleFireAudioClip;
                break;
            case SoundTypes.MarbleClick:
                SFX.clip = marbleClickAudioClip;
                break;
            case SoundTypes.MarbleReload:
                SFX.clip = marbleReloadAudioClip;
                break;
            case SoundTypes.ResetLeverFunctional:
                SFX.clip = resetLeverFunctionalAudioClip;
                break;
            case SoundTypes.ResetLeverCracked:
                SFX.clip = resetLeverCrackedAudioClip;
                break;
            case SoundTypes.ResetLeverBroken:
                SFX.clip = resetLeverBrokenAudioClip;
                break;
            case SoundTypes.ResetLeverFail:
                SFX.clip = resetLeverFailAudioClip;
                break;
            case SoundTypes.RoomRotation:
                SFX.clip = roomRotationAudioClip;
                break;
            case SoundTypes.ButtonPressed:
                SFX.clip = buttonPressedAudioClip;
                break;
            case SoundTypes.ScissorsUsed:
                SFX.clip = scissorsUsedAudioClip;
                break;
            default:
                SFX.clip = null;
                break;
        }

        SFX.Play();
    }

    public enum SoundTypes
    {
        Walk,
        Jumping,
        Running,
        GlassesOn,
        GlassesOff,
        ItemCollection,
        MarbleRoll,
        MarbleImpact,
        MarbleFire,
        MarbleClick,
        MarbleReload,
        ResetLeverFunctional,
        ResetLeverCracked,
        ResetLeverBroken,
        ResetLeverFail,
        RoomRotation,
        ButtonPressed,
        ScissorsUsed,
    }
}
