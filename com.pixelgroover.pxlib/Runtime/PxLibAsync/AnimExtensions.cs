using System.Threading;
using Cysharp.Threading.Tasks;

namespace PxLib
{
    public static class AnimExtensions
    {
        public static UniTask PlayAnimAsync(
            this PxAnimator animator,
            Anim newAnim,
            PlayerLoopTiming timing = PlayerLoopTiming.Update,
            CancellationToken cancel = default)
        {
            if (newAnim == null) return UniTask.CompletedTask;
            if (animator.CurrentAnim == newAnim)
            {
                // we are already playing this animation, or it has played and completed
                return UniTask.CompletedTask;
            }
            animator.PlayAnim(newAnim);
            return WaitForCurrentAnimToCompleteOrChange(animator, timing, cancel);
        }
    
        public static async UniTask WaitForAnimToComplete(
            this PxAnimator animator,
            Anim anim,
            PlayerLoopTiming timing = PlayerLoopTiming.Update,
            CancellationToken cancel = default)
        {
            while (animator.AnimComplete == false || animator.CurrentAnim != anim)
            {
                await UniTask.Yield(timing, cancel);
            }
        }
    
        public static UniTask WaitForCurrentAnimToCompleteOrChange(this PxAnimator animator, PlayerLoopTiming timing = PlayerLoopTiming.Update, CancellationToken cancel = default)
        {
            var waitingAnim = animator.CurrentAnim;
            return WaitForAnimToComplete(animator, waitingAnim, timing, cancel);
        }
    }
}