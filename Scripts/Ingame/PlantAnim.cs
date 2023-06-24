using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace U1W.MoonPlant
{
    /// <summary>
    /// プラントのアニメーションを制御する
    /// </summary>
    public class PlantAnim : MonoBehaviour
    {
        void Start()
        {
            transform.DORotate(new Vector3(3, -45, 7.5f), 7f, RotateMode.Fast).From(new Vector3(-3, -45, -7.5f))
                .SetLoops(-1, LoopType.Yoyo);
        }
    }
}