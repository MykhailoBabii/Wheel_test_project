using System;
using System.Collections.Generic;
using DG.Tweening;
using Game.Settings;
using UnityEngine;

namespace Game.UI
{
    public class WheelView : MonoBehaviour
    {
        [SerializeField] private SectionView _sectionImage;
        [SerializeField] private RectTransform _wheelRect;
        private readonly Dictionary<int, float> _sectionAngleDict = new();
        private readonly Dictionary<int, SectionView> _sectionViewDict = new();


        [ContextMenu("Draw")]
        public void PrepareWheel(List<SpinData> spinDatas)
        {
            var sectionCount = spinDatas.Count;
            float angleZ = 360f / (float)sectionCount;
            var rotation = new Vector3();

            for (int i = 0; i < spinDatas.Count; i++)
            {
                rotation.z = angleZ * (float)i;
                var sprite = spinDatas[i].Sprite;
                var count = $"{spinDatas[i].Count}X";
                var sectionImage = Instantiate(_sectionImage, transform);

                sectionImage.SetFillAmount(1f / (float)sectionCount);
                sectionImage.SetRotation(rotation);
                sectionImage.SetContent(count, sprite);

                _sectionAngleDict[spinDatas[i].ListIndex] = angleZ / 2 - angleZ * i;
                _sectionViewDict[spinDatas[i].ListIndex] = sectionImage;
            }
        }

        public void SpinToSection(int sectionIndex, float spinTime, int spinCount, Action onComplete)
        {
            var angleZ = _sectionAngleDict[sectionIndex] + 360f * (float)spinCount;
            var eulerAngles = new Vector3(0, 0, angleZ);
            DOVirtual.Vector3(_wheelRect.eulerAngles, eulerAngles, spinTime, value =>
            {
                _wheelRect.eulerAngles = value;
            })
            .OnComplete(() => onComplete?.Invoke());
        }

        public void BlockSection(int sectionIndex)
        {
            _sectionViewDict[sectionIndex].Block();
        }

        public SectionView GetSection(int sectionIndex)
        {
            return _sectionViewDict[sectionIndex];
        }
    }
}
