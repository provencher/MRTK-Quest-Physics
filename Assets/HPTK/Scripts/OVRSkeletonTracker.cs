using System.Collections.Generic;
using UnityEngine;

namespace HPTK.Input
{
    public class OVRSkeletonTracker : InputDataProvider
    {
        [SerializeField]
        OVRSkeleton.SkeletonType skeletonType;

        public OVRHand handData;
        public OVRSkeleton boneData;

        IList<OVRBone> providedBones;

        bool FindHand()
        {
            if (handData != null && boneData != null)
                return true;

            foreach (var skeleton in FindObjectsOfType<OVRSkeleton>())
            {
                if (skeleton.GetSkeletonType() != skeletonType)
                    continue;

                boneData = skeleton;
                handData = skeleton.GetComponent<OVRHand>();
                return true;
            }

            return false;
        }

        public override void UpdateData()
        {
            base.UpdateData();

            if (!FindHand())
                return;

            if (handData.IsTracked)
            {
                providedBones = boneData.Bones;

                for (int i = 0; i < providedBones.Count; i++)
                {
                    bones[i].space = Space.World;
                    bones[i].position = providedBones[i].Transform.position;
                    bones[i].rotation = providedBones[i].Transform.rotation;
                }

                UpdateFingers();
            }
        }
    }
}