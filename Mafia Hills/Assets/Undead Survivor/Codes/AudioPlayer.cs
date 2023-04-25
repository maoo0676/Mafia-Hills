using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    public void Select()
    {
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Select);
    }
}
