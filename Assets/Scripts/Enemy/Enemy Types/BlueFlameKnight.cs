using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueFlameKnight : Enemy
{
    [SerializeField] private AudioClip grabStabSFX;

    public void PlayGrabStabSFX()
    {
        SoundFXManager.instance.PlaySoundFXClip(grabStabSFX, transform, 0.5f);
    }
}
