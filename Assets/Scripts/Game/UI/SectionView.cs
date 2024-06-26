using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class SectionView : MonoBehaviour
    {
        [SerializeField] private float _textTopPadding;
        [SerializeField] private float _imageTopPadding;
        [SerializeField] private Image _image;
        [SerializeField] private Image _bonusImage;
        [SerializeField] private Image _blockImage;
        [SerializeField] private TextMeshProUGUI _countBonusText;

        public void SetRotation(Vector3 rotation)
        {
            _image.rectTransform.eulerAngles = rotation;
        }

        public void SetIcon(Sprite sprite)
        {
            _bonusImage.sprite = sprite;
        }

        internal void SetContent(string count, Sprite sprite)
        {
            _countBonusText.SetText(count);
            _bonusImage.sprite = sprite;

            _bonusImage.transform.localPosition = GetCenter(_imageTopPadding);
            _countBonusText.transform.localPosition = GetCenter(_textTopPadding);

            var contentAngle = GetAngle();
            _bonusImage.transform.localEulerAngles = contentAngle;
            _countBonusText.transform.localEulerAngles = contentAngle;

        }

        internal void SetFillAmount(float value)
        {
            _image.fillAmount = value;
        }

        private Vector2 GetCenter(float topPadding)
        {
            var angleCenter = _image.fillAmount * 360f / 2f;
            var radCenter = angleCenter * Mathf.Deg2Rad;
            var radius = _image.rectTransform.rect.width / 2f - topPadding;
            var x = radius * Mathf.Sin(radCenter);
            var y = radius * Mathf.Cos(radCenter);

            return new Vector2(x, y);
        }

        private Vector3 GetAngle()
        {
            var angeleZ = _image.fillAmount * 360f / -2;
            return new Vector3(0, 0, angeleZ);
        }

        internal void Block()
        {
            _blockImage.gameObject.SetActive(true);
        }
    }
}
