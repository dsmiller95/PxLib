using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

namespace PxLib.Dotween
{
    public static class DOTweenModulePxShader
    {
        public static TweenerCore<float, float, FloatOptions> DOFade(this PxShaderController target, float endValue, float duration)
        {
            TweenerCore<float, float, FloatOptions> t = DOTween.To(() => target.Opacity, x => target.Opacity = x, endValue, duration);
            t.SetTarget(target);
            return t;
        }
        
        public static TweenerCore<float, float, FloatOptions> DOEffectScale(this PxShaderEffect target, float endValue, float duration)
        {
            TweenerCore<float, float, FloatOptions> t = DOTween.To(() => target.EffectScale, x => target.EffectScale = x, endValue, duration);
            t.SetTarget(target);
            return t;
        }

        public static TweenerCore<T1, T1, TPlugOptions> SnapOnKill<T1, TPlugOptions>(this TweenerCore<T1, T1, TPlugOptions> target) 
            where TPlugOptions : struct, IPlugOptions
        {
            return target.OnKill(() => target.setter(target.endValue));
        }
    }
}