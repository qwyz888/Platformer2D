using System;
using Plugins.Zenject.OptionalExtras.SampleGame1__Beginner_.Scripts.Misc;
using Plugins.Zenject.OptionalExtras.SampleGame1__Beginner_.Scripts.Util;
using Plugins.Zenject.Source.Factories;
using Plugins.Zenject.Source.Internal;
using UnityEngine;
using Zenject;

namespace Plugins.Zenject.OptionalExtras.SampleGame1__Beginner_.Scripts.Asteroid
{
    public class Asteroid : MonoBehaviour
    {
        LevelHelper _level;
        Rigidbody _rigidBody;
        Settings _settings;

        // We could just add [Inject] to the field declarations but
        // it's often better practice to use PostInject methods
        // Note that we can't use Constructors here because this is
        // a MonoBehaviour
        [Inject]
        public void Construct(LevelHelper level, Settings settings)
        {
            _level = level;
            _settings = settings;
            _rigidBody = GetComponent<Rigidbody>();
        }

        public Vector3 Position
        {
            get { return transform.position; }
            set { transform.position = value; }
        }

        public float Mass
        {
            get { return _rigidBody.mass; }
            set { _rigidBody.mass = value; }
        }

        public float Scale
        {
            get
            {
                var scale = transform.localScale;
                // We assume scale is uniform
                Assert.That(scale[0] == scale[1] && scale[1] == scale[2]);

                return scale[0];
            }
            set
            {
                transform.localScale = new Vector3(value, value, value);
                _rigidBody.mass = value;
            }
        }

        public Vector3 Velocity
        {
            get { return _rigidBody.linearVelocity; }
            set { _rigidBody.linearVelocity = value; }
        }

        public void FixedTick()
        {
            // Limit speed to a maximum
            var speed = _rigidBody.linearVelocity.magnitude;

            if (speed > _settings.maxSpeed)
            {
                var dir = _rigidBody.linearVelocity / speed;
                _rigidBody.linearVelocity = dir * _settings.maxSpeed;
            }
        }

        public void Tick()
        {
            CheckForTeleport();
        }

        void CheckForTeleport()
        {
            if (Position.x > _level.Right + Scale && IsMovingInDirection(Vector3.right))
            {
                transform.SetX(_level.Left - Scale);
            }
            else if (Position.x < _level.Left - Scale && IsMovingInDirection(-Vector3.right))
            {
                transform.SetX(_level.Right + Scale);
            }
            else if (Position.y < _level.Bottom - Scale && IsMovingInDirection(-Vector3.up))
            {
                transform.SetY(_level.Top + Scale);
            }
            else if (Position.y > _level.Top + Scale && IsMovingInDirection(Vector3.up))
            {
                transform.SetY(_level.Bottom - Scale);
            }

            transform.RotateAround(transform.position, Vector3.up, 30 * Time.deltaTime);
        }

        bool IsMovingInDirection(Vector3 dir)
        {
            return Vector3.Dot(dir, _rigidBody.linearVelocity) > 0;
        }

        [Serializable]
        public class Settings
        {
            public float massScaleFactor;
            public float maxSpeed;
        }

        public class Factory : PlaceholderFactory<Asteroid>
        {
        }
    }
}
