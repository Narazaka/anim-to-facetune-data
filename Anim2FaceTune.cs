using UnityEngine;

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Narazaka.VRChat.Anim2FaceTune.Editor")]

namespace Narazaka.VRChat.Anim2FaceTune
{
    public class Anim2FaceTune : MonoBehaviour
    {
        [SerializeField] internal AnimationClip[] clips = new AnimationClip[0];
    }
}
