using UnityEngine;
using Zenject;

namespace Plugins.Zenject.OptionalExtras.SampleGame1__Beginner_.Scripts.Misc
{
    public class LevelHelper
    {
        readonly Camera _camera;

        public LevelHelper(
            [Inject(Id = "Main")]
            Camera camera)
        {
            _camera = camera;
        }

        public float Bottom
        {
            get { return -ExtentHeight; }
        }

        public float Top
        {
            get { return ExtentHeight; }
        }

        public float Left
        {
            get { return -ExtentWidth; }
        }

        public float Right
        {
            get { return ExtentWidth; }
        }

        public float ExtentHeight
        {
            get { return _camera.orthographicSize; }
        }

        public float Height
        {
            get { return ExtentHeight * 2.0f; }
        }

        public float ExtentWidth
        {
            get { return _camera.aspect * _camera.orthographicSize; }
        }

        public float Width
        {
            get { return ExtentWidth * 2.0f; }
        }
    }
}

