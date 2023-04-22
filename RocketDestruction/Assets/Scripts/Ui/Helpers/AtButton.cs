using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace RocketDestruction.Ui.Helpers
{
    public class AtButton : Button
    {
        private Vector3 _defaultScale;

        protected override void Awake()
        {
            base.Awake();
            _defaultScale = transform.localScale;
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);
            if (interactable == false) return;
            transform.DOScale(_defaultScale * 0.95f, 0.1f);
        }


        public override void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerUp(eventData);
            if (interactable == false) return;
            transform.DOScale(_defaultScale, 0.1f);
        }
    }
}