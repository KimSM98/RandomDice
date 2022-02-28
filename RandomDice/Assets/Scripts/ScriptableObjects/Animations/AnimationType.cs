using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Animation.Core
{
    [CreateAssetMenu(menuName = "Animation Asset")]
    public class AnimationType : ScriptableObject
    {
        public Sprite[] sprites;
        public float fps;
    }

}
