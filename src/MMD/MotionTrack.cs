using UnityEngine;
using System.Collections.Generic;
using System.Linq;


namespace LFE.MMD
{
    public class MotionTrack
    {
        private VmdFile _vmdFile;
        private Dictionary<string, Vector3> _initialBonePositions;

        public Atom Person;
        public Dictionary<string, FreeControllerV3> ControllersByName;
        public Dictionary<string, DAZBone> BonesByName;

        public uint MaxFrame { get; private set; }

        public float HipOffsetY { get; set; }
        public float HipOffsetZ { get; set; }

        public MotionTrack(VmdFile vmdFile, Atom person, float hipOffsetY, float hipOffsetZ)
        {
            _vmdFile = vmdFile;
            Person = person;
            MaxFrame = _vmdFile.Motions.Max(m => m.FrameId);
            HipOffsetY = hipOffsetY;
            HipOffsetZ = hipOffsetZ;

            _initialBonePositions = new Dictionary<string, Vector3>();
            ControllersByName = new Dictionary<string, FreeControllerV3>();
            foreach (var controller in Person.freeControllers)
            {
                ControllersByName[controller.name] = controller;
                _initialBonePositions[controller.name] = controller.transform.position;
            }

            BonesByName = new Dictionary<string, DAZBone>();
            foreach (var bone in Person.GetComponentsInChildren<DAZBone>())
            {
                BonesByName[bone.id] = bone;
            }

            // InitControllers();
        }

        private void InitController(FreeControllerV3 controller) {
            var name = controller.name;

            controller.currentPositionState = FreeControllerV3.PositionState.Off;
            controller.currentRotationState = FreeControllerV3.RotationState.Off;

            switch(name) {
                case "headControl":
                    controller.currentRotationState = FreeControllerV3.RotationState.Off;

                    // controller.RBHoldPositionSpring = 454.0f;
                    // controller.RBHoldPositionDamper = 5.92f;
                    // controller.RBHoldRotationSpring = 30.0f;
                    // controller.RBHoldRotationDamper = 12.66f;
                    // controller.jointRotationDriveSpring = 11.72f;
                    // controller.jointRotationDriveDamper = 1.0f;
                    break;
                case "neckControl":
                    controller.currentRotationState = FreeControllerV3.RotationState.On;

                    controller.RBHoldPositionSpring = 16.0f;
                    controller.RBHoldPositionDamper = 7.72f;
                    controller.RBHoldRotationSpring = 30.0f;
                    controller.RBHoldRotationDamper = 3.86f;
                    controller.jointRotationDriveSpring = 23.0f;
                    controller.jointRotationDriveDamper = 40.0f;
                    break;
                case "chestControl":
                    controller.currentRotationState = FreeControllerV3.RotationState.On;

                    controller.RBHoldPositionSpring = 232.0f;
                    controller.RBHoldPositionDamper = 75.0f;
                    controller.RBHoldRotationSpring = 574.0f;
                    controller.RBHoldRotationDamper = 1.0f;
                    controller.jointRotationDriveSpring = 410.0f;
                    controller.jointRotationDriveDamper = 235.0f;
                    break;
                case "abdomen2Control":
                    controller.currentRotationState = FreeControllerV3.RotationState.On;

                    controller.RBHoldPositionSpring = 334.0f;
                    controller.RBHoldPositionDamper = 135.0f;
                    controller.RBHoldRotationSpring = 500.0f;
                    controller.RBHoldRotationDamper = 1.0f;
                    controller.jointRotationDriveSpring = 271.0f;
                    controller.jointRotationDriveDamper = 1.0f;
                    break;
                case "abdomenControl":
                    controller.currentRotationState = FreeControllerV3.RotationState.On;

                    controller.RBHoldPositionSpring = 334.0f;
                    controller.RBHoldPositionDamper = 135.0f;
                    controller.RBHoldRotationSpring = 500.0f;
                    controller.RBHoldRotationDamper = 1.0f;
                    controller.jointRotationDriveSpring = 0.0f;
                    controller.jointRotationDriveDamper = 0.5f;
                    break;
                case "pelvisControl":
                    controller.currentRotationState = FreeControllerV3.RotationState.On;

                    controller.RBHoldPositionSpring = 734.0f;
                    controller.RBHoldPositionDamper = 135.0f;
                    controller.RBHoldRotationSpring = 700.0f;
                    controller.RBHoldRotationDamper = 1.0f;
                    controller.jointRotationDriveSpring = 0.0f;
                    controller.jointRotationDriveDamper = 0.5f;
                    break;
                case "hipControl":
                    controller.currentPositionState = FreeControllerV3.PositionState.On;
                    controller.currentRotationState = FreeControllerV3.RotationState.On;

                    controller.RBHoldPositionSpring = 1500.0f;
                    controller.RBHoldPositionDamper = 650.0f;
                    controller.RBHoldRotationSpring = 500.0f;
                    controller.RBHoldRotationDamper = 1.0f;
                    controller.jointRotationDriveSpring = 0.0f;
                    controller.jointRotationDriveDamper = 0.5f;
                    break;
                case "lShoulderControl":
                case "rShoulderControl":
                    controller.currentRotationState = FreeControllerV3.RotationState.On;

                    controller.RBHoldPositionSpring = 45.0f;
                    controller.RBHoldPositionDamper = 35.0f;
                    controller.RBHoldRotationSpring = 500.0f;
                    controller.RBHoldRotationDamper = 1.0f;
                    controller.jointRotationDriveSpring = 102.5f;
                    controller.jointRotationDriveDamper = 28.75f;
                    break;
                case "lArmControl":
                case "rArmControl":
                    controller.currentRotationState = FreeControllerV3.RotationState.On;

                    // controller.RBHoldPositionSpring = 182.0f;
                    // controller.RBHoldPositionDamper = 35.0f;
                    // controller.RBHoldRotationSpring = 700.0f;
                    // controller.RBHoldRotationDamper = 1.0f;
                    // controller.jointRotationDriveSpring = 262.5f;
                    // controller.jointRotationDriveDamper = 2.0f;
                    break;
                case "lElbowControl":
                case "rElbowControl":
                    controller.currentRotationState = FreeControllerV3.RotationState.On;

                    // controller.RBHoldPositionSpring = 100.0f;
                    // controller.RBHoldPositionDamper = 35.0f;
                    // controller.RBHoldRotationSpring = 280.0f;
                    // controller.RBHoldRotationDamper = 1.0f;
                    // controller.jointRotationDriveSpring = 0.0f;
                    // controller.jointRotationDriveDamper = 0.5f;
                    break;
                case "lHandControl":
                case "rHandControl":
                    controller.currentRotationState = FreeControllerV3.RotationState.On;

                    // controller.RBHoldPositionSpring = 250.0f;
                    // controller.RBHoldPositionDamper = 1.0f;
                    // controller.RBHoldRotationSpring = 450.0f;
                    // controller.RBHoldRotationDamper = 0.5f;
                    // controller.jointRotationDriveSpring = 0.0f;
                    // controller.jointRotationDriveDamper = 0.0f;
                    break;
                case "lHipControl":
                case "rHipControl":
                    controller.currentRotationState = FreeControllerV3.RotationState.On;

                    controller.RBHoldPositionSpring = 2000.0f;
                    controller.RBHoldPositionDamper = 435.0f;
                    controller.RBHoldRotationSpring = 500.0f;
                    controller.RBHoldRotationDamper = 1.0f;
                    controller.jointRotationDriveSpring = 0.0f;
                    controller.jointRotationDriveDamper = 0.5f;
                    break;
                case "lKneeControl":
                case "rKneeControl":
                    controller.currentRotationState = FreeControllerV3.RotationState.On;

                    controller.RBHoldPositionSpring = 500.0f;
                    controller.RBHoldPositionDamper = 3.0f;
                    controller.RBHoldRotationSpring = 500.0f;
                    controller.RBHoldRotationDamper = 1.0f;
                    controller.jointRotationDriveSpring = 0.0f;
                    controller.jointRotationDriveDamper = 0.5f;
                    break;
                case "lFootControl":
                case "rFootControl":
                    if(_vmdFile.UsesIK) {
                        controller.currentPositionState = FreeControllerV3.PositionState.On;
                    }
                    controller.RBHoldPositionSpring = 10000.0f;
                    controller.RBHoldPositionDamper = 2.0f;
                    controller.RBHoldRotationSpring = 450.0f;
                    controller.RBHoldRotationDamper = 1.0f;
                    controller.jointRotationDriveSpring = 0.0f;
                    controller.jointRotationDriveDamper = 0.5f;
                    break;
                default:
                    controller.currentRotationState = FreeControllerV3.RotationState.On;
                    break;

            }
        }

        private void InitControllers()
        {
            foreach(var controller in Person.freeControllers) {
                InitController(controller);
            }
        }

        public List<VamFrameData> VamPoseAtTime(float time)
        {
            uint frameId = (uint)(time * VmdFile.Fps);
            return VamPoseAtFrame(frameId);
        }

        public List<VamFrameData> VamPoseAtFrame(uint frameId)
        {
            var boneDataForFrame = InterpolatedBonesAtFrame(frameId);
            var boneDataByVamNameForFrame = new Dictionary<string, MotionData>();
            foreach (var b in boneDataForFrame)
            {
                if (string.IsNullOrEmpty(b.Value.VamBoneName))
                {
                    continue;
                }
                boneDataByVamNameForFrame[b.Value.VamBoneName] = b.Value;
            }

            var data = new List<VamFrameData>();

            // body bones
            var orderedBones = _vmdFile.PartsEnglish;
            foreach (var boneName in orderedBones)
            {
                var frame = boneDataForFrame[boneName];
                var vamBoneName = frame.VamBoneName;
                if (string.IsNullOrEmpty(vamBoneName))
                {
                    continue;
                }

                var isController = ControllersByName.ContainsKey(vamBoneName);

                if (isController)
                {
                    var controller = ControllersByName[vamBoneName];

                    InitController(controller);

                    // controller.currentPositionState = FreeControllerV3.PositionState.Off;
                    // controller.currentRotationState = FreeControllerV3.RotationState.Off;

                    // if (vamBoneName == "hipControl")
                    // {
                    //     controller.currentPositionState = FreeControllerV3.PositionState.On;
                    //     controller.currentRotationState = FreeControllerV3.RotationState.On;
                    // }
                    // else if (_vmdFile.UsesIK && (vamBoneName == "lFootControl" || vamBoneName == "rFootControl"))
                    // {
                    //     controller.currentPositionState = FreeControllerV3.PositionState.On;
                    // }
                    // else
                    // {
                    //     controller.currentRotationState = FreeControllerV3.RotationState.On;
                    // }
                }
                else
                {
                    continue;
                }

                var initialPosition = _initialBonePositions[vamBoneName];
                Vector3 position = new Vector3(
                    (initialPosition.x + (frame.Position.x * 0.08f * -1)),
                    (initialPosition.y + (frame.Position.y * 0.08f)),
                    (initialPosition.z + (frame.Position.z * 0.08f * -1))
                );
                if (vamBoneName == "hipControl")
                {
                    position.y = position.y + HipOffsetY;
                    position.z = position.z + HipOffsetZ;
                }

                Quaternion rotation = new Quaternion(
                    frame.Rotation.x,
                    frame.Rotation.y,
                    frame.Rotation.z,
                    frame.Rotation.w
                );

                var parentBoneName = _vmdFile.ParentVamBone(vamBoneName);
                // SuperController.LogError($"{controllerName} has a parent of {parentBoneName}");
                if (!string.IsNullOrEmpty(parentBoneName))
                {
                    if (boneDataByVamNameForFrame.ContainsKey(parentBoneName))
                    {
                        var parentFrame = boneDataByVamNameForFrame[parentBoneName];
                        rotation = parentFrame.Rotation * rotation;
                        boneDataByVamNameForFrame[vamBoneName].Rotation = rotation;
                        // SuperController.LogMessage($"adding parent rotation {parentFrame.EnglishName} ({frame.Rotation}) to {frame.EnglishName} ({parentFrame.Rotation})");
                    }
                }

                switch (vamBoneName)
                {
                    case "rArmControl":
                    case "rElbowControl":
                    case "rHandControl":
                        rotation = rotation * Quaternion.Euler(Vector3.forward * 30);
                        // SuperController.LogMessage($"adjusting {controllerName}");
                        break;
                    case "lArmControl":
                    case "lElbowControl":
                    case "lHandControl":
                        rotation = rotation * Quaternion.Euler(Vector3.forward * -30);
                        // SuperController.LogMessage($"adjusting {controllerName}");
                        break;
                }

                rotation.x = rotation.x * -1;
                rotation.z = rotation.z * -1;

                data.Add(new VamFrameData
                {
                    Frame = frame.FrameId,
                    ControllerName = vamBoneName,
                    Timestamp = frame.VamTimestamp,
                    Position = position,
                    Rotation = rotation
                });
            }

            // fingers
            foreach (var item in boneDataByVamNameForFrame)
            {
                var name = item.Key;
                var frame = item.Value;

                var bone = BonesByName.ContainsKey(name) ? BonesByName[name] : null;
                if (bone == null)
                {
                    continue;
                }

                Vector3 position = Vector3.zero;
                Quaternion rotation = new Quaternion(
                    frame.Rotation.x,
                    frame.Rotation.y,
                    frame.Rotation.z,
                    frame.Rotation.w
                );

                var parentBoneName = _vmdFile.ParentVamBone(name);
                if (!string.IsNullOrEmpty(parentBoneName))
                {
                    if (boneDataByVamNameForFrame.ContainsKey(parentBoneName))
                    {
                        var parentFrame = boneDataByVamNameForFrame[parentBoneName];
                        rotation = parentFrame.Rotation * rotation;
                    }
                }

                switch (name)
                {
                    case "rThumb1":
                        // case "rThumb2":
                        rotation = rotation * Quaternion.Euler(Vector3.up * -55);
                        break;
                    case "lThumb1":
                        // case "lThumb2":
                        rotation = rotation * Quaternion.Euler(Vector3.up * 55);
                        break;
                }

                rotation.x = rotation.x * -1;
                rotation.z = rotation.z * -1;

                data.Add(new VamFrameData
                {
                    Frame = frame.FrameId,
                    ControllerName = name,
                    Timestamp = frame.VamTimestamp,
                    Position = position,
                    Rotation = rotation
                });

            }
            // foreach(var fingerName in new string[] {"lIndex1", "lIndex2", "lIndex3"}) {
            //     var frame = boneDataForFrame[fingerName];
            //     var controllerName = frame.VamBoneName;
            //     if(string.IsNullOrEmpty(controllerName)) {
            //         continue;
            //     }

            //     var bone = BonesByName.ContainsKey(fingerName) ? BonesByName[fingerName] : null;
            //     if(bone == null) {
            //         continue;
            //     }

            //     SuperController.LogMessage($"TODO: handle {fingerName}");
            // }


            return data;
        }


        public Dictionary<string, MotionData> InterpolatedBonesAtFrame(uint frameId)
        {
            var interpolatedFrameBones = new Dictionary<string, MotionData>();
            foreach (var boneName in _vmdFile.MotionsByBone.Keys.ToList())
            {
                var interpolated = InterpolatedBoneAtFrame(boneName, frameId);
                if (interpolated != null)
                {
                    interpolatedFrameBones[boneName] = interpolated;
                }
            }
            return interpolatedFrameBones;
        }

        public MotionData InterpolatedBoneAtFrame(string boneName, uint targetFrameId)
        {
            if (targetFrameId > MaxFrame) { targetFrameId = MaxFrame; }
            else if (targetFrameId < 0) { targetFrameId = 0; }

            var motions = _vmdFile.MotionsByBone[boneName];
            var lastMotion = motions.Last();

            if (targetFrameId >= lastMotion.FrameId)
            {
                return new MotionData
                {
                    FrameId = targetFrameId,
                    Name = lastMotion.Name,
                    EnglishName = lastMotion.EnglishName,
                    VamBoneName = lastMotion.VamBoneName,
                    Position = lastMotion.Position,
                    Rotation = lastMotion.Rotation
                };
            }

            for (var i = 0; i < motions.Count; i++)
            {
                var motion = motions[i];
                if (motion.FrameId == targetFrameId)
                {
                    // no interpolation - found exact frame
                    return new MotionData
                    {
                        FrameId = targetFrameId,
                        Name = motion.Name,
                        EnglishName = motion.EnglishName,
                        VamBoneName = motion.VamBoneName,
                        Position = motion.Position,
                        Rotation = motion.Rotation
                    };
                }
                else if (motion.FrameId > targetFrameId)
                {
                    // interpolation -- just went past target frame
                    var start = motions[i - 1];
                    var end = motions[i];
                    float progress = (float)(targetFrameId - start.FrameId) / (float)(end.FrameId - start.FrameId);
                    // TODO: honor mmd interpolation bezier curves
                    return new MotionData
                    {
                        FrameId = targetFrameId,
                        Name = motion.Name,
                        EnglishName = motion.EnglishName,
                        VamBoneName = motion.VamBoneName,
                        Position = Vector3.Lerp(start.Position, end.Position, progress),
                        Rotation = Quaternion.Lerp(start.Rotation, end.Rotation, progress)
                    };
                }
            }

            return null;
        }

        public Dictionary<string, FaceMotionData> VamFacesAtTime(float time)
        {
            // TODO: map to vam morphs
            uint frameId = (uint)(time * VmdFile.Fps);
            return InterpolatedFacesAtFrame(frameId);
        }

        public Dictionary<string, FaceMotionData> InterpolatedFacesAtFrame(uint frameId)
        {
            var interpolatedFrameFaces = new Dictionary<string, FaceMotionData>();
            foreach (var faceName in _vmdFile.FaceMotionsByBone.Keys.ToList())
            {
                var interpolated = InterpolatedFaceAtFrame(faceName, frameId);
                if (interpolated != null)
                {
                    interpolatedFrameFaces[faceName] = interpolated;
                }
            }
            return interpolatedFrameFaces;
        }

        public FaceMotionData InterpolatedFaceAtFrame(string boneName, uint targetFrameId)
        {
            if (targetFrameId > MaxFrame) { targetFrameId = MaxFrame; }
            else if (targetFrameId < 0) { targetFrameId = 0; }

            var motions = _vmdFile.FaceMotionsByBone[boneName];
            var lastMotion = motions.Last();

            if (targetFrameId >= lastMotion.FrameId)
            {
                return new FaceMotionData
                {
                    Name = lastMotion.Name,
                    FrameId = targetFrameId,
                    Rate = lastMotion.Rate
                };
            }

            for (var i = 0; i < motions.Count; i++)
            {
                var motion = motions[i];
                if (motion.FrameId == targetFrameId)
                {
                    // no interpolation - found exact frame
                    return new FaceMotionData
                    {
                        FrameId = targetFrameId,
                        Name = motion.Name,
                        Rate = motion.Rate
                    };
                }
                else if (motion.FrameId > targetFrameId)
                {
                    // interpolation -- just went past target frame
                    var start = motions[i - 1];
                    var end = motions[i];
                    float progress = (float)(targetFrameId - start.FrameId) / (float)(end.FrameId - start.FrameId);
                    return new FaceMotionData
                    {
                        FrameId = targetFrameId,
                        Name = motion.Name,
                        Rate = Mathf.Lerp(start.Rate, end.Rate, progress)
                    };
                }
            }

            return null;
        }

    }
}
