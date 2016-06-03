// Copyright 2014 Tokarev Mikhail (also known as Deepscorn)
// http://www.apache.org/licenses/LICENSE-2.0
using Assets.Main.Sources.Util;
using Assets.Main.Sources.Util.Extensions;
using Assets.Main.Sources.Util.Navigation;
using Assets.Main.Sources.Util.Types;
using UnityEngine;

namespace Assets.Main.Sources.Scripting.Basic
{
    // Set destination, then navigationagent follows path. Set destination to null and agent
    // will stop navigating. But if it was jumping, it will finish jump.
    // Also, if you change destination while agent was jumping it will first finish jump
    // and only when start navigating to new destination
    public class NavigationAgent : MonoBehaviourExt
    {
        public Vector3 NextPosition { get; set; }
        public bool IsOnOffMeshLink { get; private set; }
        public NavigationLinkData CurrentOffMeshLinkData { get; private set; }
        public bool Stopped { get { return !movingToDestination; } }
        public Vector3 Destination
        {
            get
            {
                return destination;
            }
            set
            {
                if (!destination.EpsilonEquals(value))
                {
                    destination = value;
                    OnDestinationChanged();
                }
            }
        }
        public float ScanWidth { set { scanWidth = value; } get { return scanWidth; } }

        // todo: consider (when needed) bool variable movingFromStart to be able to
        // set agent navigating to destination from start of the scene
        // otherwise agent starts navigating only on destination change

        [SerializeField] private float jumpHeight = 10F;
        [SerializeField] private float jumpDistance = 5F;
        [SerializeField] private float fallDownHeight = 20F;
        // Maximum movement speed when following a path
        [SerializeField] private float speed = 0.5F;
        [SerializeField] private Vector3 destination;
        [SerializeField] private float scanWidth = 3.2F;
        // Note: setting that to false must be done only when we finished moving to destination or
        // we may freeze in air during jump :)
        private bool movingToDestination;
        private NavigationPlane navigationPlane;

        private void OnDestinationChanged()
        {
            movingToDestination = true;
        }

        private void Start()
        {
            NextPosition = transform.position;
        }

        public void OnDrawGizmos()
        {
            if (IsOnOffMeshLink)
            {
                var rad = 1F;
                var oldColor = Gizmos.color;
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(CurrentOffMeshLinkData.StartPosition, rad);
                Gizmos.color = Color.blue;
                Gizmos.DrawSphere(CurrentOffMeshLinkData.EndPosition, rad);
                Gizmos.color = oldColor;
            }
        }

        private void Update()
        {
            if (movingToDestination && !IsOnOffMeshLink)
            {
                TryJump();
            }
            if (IsOnOffMeshLink)
            {
                StepJumping();
            }
            else if (movingToDestination)
            {
                StepGoing();
            }
        }

        private void TryJump()
        {
            var rayDistance = jumpHeight + fallDownHeight;
            var distanceLeftVector = Destination - transform.position;
            var limitedJumpDistance = Mathf.Min(jumpDistance, distanceLeftVector.magnitude);
            var rayStart = transform.position + Vector3.Normalize(distanceLeftVector) * limitedJumpDistance;
            rayStart.y += jumpHeight;

            if (!TryJump(rayStart, rayDistance))
            {
                var scanDelta = Vector3.Normalize(distanceLeftVector).LeftOrthogonalXZ() * 0.5F * ScanWidth;
                if (!TryJump(rayStart + scanDelta, rayDistance))
                {
                    TryJump(rayStart - scanDelta, rayDistance);
                }
            }
        }

        private bool TryJump(Vector3 rayFrom, float rayDistance)
        {
            var rayDirection = Vector3.down;
            //PersistentGizmos.Instance.AddRaycast(rayFrom, rayDirection * rayDistance);
            RaycastHit rayHit;
            if (Physics.Raycast(rayFrom, rayDirection, out rayHit, rayDistance))
            {
                var hitNavigationPlane = rayHit.transform.GetComponentInChildren<NavigationPlane>();
                if (hitNavigationPlane != null && hitNavigationPlane != navigationPlane)
                {
                    StartJump(hitNavigationPlane, rayHit.point);
                    return true;
                }
            }
            return false;
        }

        private void StepGoing()
        {
            var distanceLeft2 = Vector3.SqrMagnitude(Destination - transform.position);
            var speed2 = speed * speed;
            if (distanceLeft2 < speed2)
            {
                NextPosition = Destination;
                movingToDestination = false;
            }
            else
            {
                var step = Vector3.Normalize(Destination - transform.position) * speed;
                NextPosition = new Vector3(transform.position.x + step.x,
                    transform.position.y, // we can't fly :)
                    transform.position.z + step.z);
            }
        }

        private void StepJumping()
        {
            var distanceLeft2 = Vector3.SqrMagnitude(CurrentOffMeshLinkData.EndPosition - transform.position);
            var speed2 = speed * speed;
            if (distanceLeft2 < speed2)
            {
                NextPosition = CurrentOffMeshLinkData.EndPosition;
                FinishJump();
            }
            else
            {
                NextPosition = transform.position + Vector3.Normalize(CurrentOffMeshLinkData.EndPosition - transform.position) * speed;
            }
        }

        private void StartJump(NavigationPlane destinationPlane, Vector3 destinationPoint)
        {
            navigationPlane = destinationPlane;
            IsOnOffMeshLink = true;
            CurrentOffMeshLinkData = new NavigationLinkData(transform.position, destinationPoint);
        }

        private void FinishJump()
        {
            IsOnOffMeshLink = false;
            CurrentOffMeshLinkData = null;
        }
    }
}
