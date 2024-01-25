using DG.Tweening;
using UnityEngine;

public class UIBaseTheme : ScriptableObject
{
    #region Animation types

    public void BubblyBounceAnim(Transform buttonTransform) {
        float time = 0.3f;
        buttonTransform.localScale = 0.8f * buttonTransform.localScale;
        buttonTransform.DOScale(1f, time).SetEase(Ease.OutBounce);
    }

    public void SwayYAnim(Transform buttonTransform) {
        buttonTransform.DOMoveY(buttonTransform.position.y - 20f, 1f).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
    }

    #endregion
}
