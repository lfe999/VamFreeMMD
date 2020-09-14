using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using LFE.MMD;

namespace LFE
{
    public class TimelineAnimation
    {
        public Atom Person;
        public VmdFile VmdFile;
        public TimelineAnimation(Atom person, VmdFile vmdFile)
        {
            Person = person;
            VmdFile = vmdFile;
        }

        public IEnumerable<VamFrameData> GetVamFrameData()
        {
            var motionTrack = new MotionTrack(VmdFile, Person, 0.05f, 0f);

            for (uint i = 0; i <= motionTrack.MaxFrame; i++)
            {
                var boneDataForFrame = motionTrack.InterpolatedBonesAtFrame(i);
                var boneDataByVamNameForFrame = new Dictionary<string, MotionData>();
                foreach (var b in boneDataForFrame)
                {
                    if (string.IsNullOrEmpty(b.Value.VamBoneName))
                    {
                        continue;
                    }
                    boneDataByVamNameForFrame[b.Value.VamBoneName] = b.Value;
                }
                var orderedBones = VmdFile.PartsEnglish;
                foreach (var boneName in orderedBones)
                {
                    var frame = boneDataForFrame[boneName];
                    var controllerName = frame.VamBoneName;
                    if (string.IsNullOrEmpty(controllerName))
                    {
                        continue;
                    }

                    var controller = Person.freeControllers.FirstOrDefault(fc => fc.name == controllerName);
                    if (controller == null)
                    {
                        continue;
                    }

                    controller.currentPositionState = FreeControllerV3.PositionState.Off;
                    controller.currentRotationState = FreeControllerV3.RotationState.Off;

                    if (controllerName == "hipControl")
                    {
                        controller.currentPositionState = FreeControllerV3.PositionState.On;
                        controller.currentRotationState = FreeControllerV3.RotationState.On;
                    }
                    else if (VmdFile.UsesIK && (controllerName == "lFootControl" || controllerName == "rFootControl"))
                    {
                        controller.currentPositionState = FreeControllerV3.PositionState.On;
                    }
                    else
                    {
                        controller.currentRotationState = FreeControllerV3.RotationState.On;
                    }

                    var initialPosition = controller.transform.position;
                    var initialRotation = controller.transform.rotation;

                    Vector3 position = new Vector3(
                        (initialPosition.x + (frame.Position.x * 0.08f * -1)),
                        (initialPosition.y + (frame.Position.y * 0.08f)),
                        (initialPosition.z + (frame.Position.z * 0.08f * -1))
                    );
                    if (controllerName == "hipControl")
                    {
                        position.y = position.y + -0.05f;
                        position.z = position.z + 0f;
                    }

                    Quaternion rotation = new Quaternion(
                        frame.Rotation.x,
                        frame.Rotation.y,
                        frame.Rotation.z,
                        frame.Rotation.w
                    );

                    var parentBoneName = VmdFile.ParentVamBone(controllerName);
                    SuperController.LogMessage($"{controllerName} has a parent of {parentBoneName}");
                    if (!string.IsNullOrEmpty(parentBoneName))
                    {
                        if (boneDataByVamNameForFrame.ContainsKey(parentBoneName))
                        {
                            var parentFrame = boneDataByVamNameForFrame[parentBoneName];
                            rotation = parentFrame.Rotation * rotation;
                            boneDataByVamNameForFrame[controllerName].Rotation = rotation;
                            // SuperController.LogMessage($"adding parent rotation {parentFrame.EnglishName} ({frame.Rotation}) to {frame.EnglishName} ({parentFrame.Rotation})");
                        }
                    }

                    switch (controllerName)
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

                    yield return new VamFrameData
                    {
                        Frame = frame.FrameId,
                        ControllerName = controllerName,
                        Timestamp = frame.VamTimestamp,
                        Position = position,
                        Rotation = rotation
                    };
                }
            }

            yield break;
        }

        // public IEnumerable<VamFrameData> GetVamFrameData()
        // {
        //     var vmdFramesByVamBoneName = VmdFile.Motions.Where(f => f.VamBoneName != null).ToLookup(f => f.VamBoneName);
        //     var previousFramesBone = VmdFile.Motions
        //         .Where(f => f.FrameId == 0)
        //         .Where(f => f.VamBoneName != null)
        //         .GroupBy(f => f.VamBoneName)
        //         .ToDictionary(g => g.Key, g => g.FirstOrDefault());

        //     foreach (var controllerName in VmdFile.Parts)
        //     {
        //         int frameCounter = 0;
        //         var controllerFrames = vmdFramesByVamBoneName[controllerName].ToList();
        //         if (controllerFrames.Count == 0)
        //         {
        //             continue;
        //         }

        //         var controller = Person.freeControllers.FirstOrDefault(fc => fc.name == controllerName);
        //         if (controller == null)
        //         {
        //             continue;
        //         }

        //         controller.currentPositionState = FreeControllerV3.PositionState.Off;
        //         controller.currentRotationState = FreeControllerV3.RotationState.Off;

        //         if (controllerName == "hipControl")
        //         {
        //             controller.currentPositionState = FreeControllerV3.PositionState.On;
        //             controller.currentRotationState = FreeControllerV3.RotationState.On;
        //         }
        //         else if (VmdFile.UsesIK && (controllerName == "lFootControl" || controllerName == "rFootControl"))
        //         {
        //             controller.currentPositionState = FreeControllerV3.PositionState.On;
        //         }
        //         else if (controllerFrames.Any())
        //         {
        //             // controller.currentPositionState = FreeControllerV3.PositionState.On;
        //             controller.currentRotationState = FreeControllerV3.RotationState.On;
        //         }

        //         var initialPosition = controller.transform.position;
        //         var initialRotation = controller.transform.rotation;

        //         foreach (var frame in controllerFrames)
        //         {
        //             float timestamp = frame.VamTimestamp;

        //             if (frameCounter == 0)
        //             {
        //                 yield return new VamFrameData
        //                 {
        //                     Frame = frameCounter,
        //                     ControllerName = controllerName,
        //                     Timestamp = 0f,
        //                     Position = new Vector3(initialPosition.x, initialPosition.y, initialPosition.z),
        //                     Rotation = new Quaternion(0, 0, 0, 0)
        //                 };
        //                 frameCounter++;
        //             }


        //             Vector3 position = new Vector3(
        //                 (initialPosition.x + (frame.Position.x * 0.08f * -1)),
        //                 (initialPosition.y + (frame.Position.y * 0.08f)),
        //                 (initialPosition.z + (frame.Position.z * 0.08f * -1))
        //             );
        //             if (controllerName == "hipControl")
        //             {
        //                 position.y = position.y + -0.05f;
        //                 position.z = position.z + 0f;
        //             }

        //             Quaternion rotation = new Quaternion(
        //                 frame.Rotation.x,
        //                 frame.Rotation.y,
        //                 frame.Rotation.z,
        //                 frame.Rotation.w
        //             );

        //             // if(frame.FrameNumber == 0) {
        //             var parentBone = VmdFile.ParentVamBone(frame.VamBoneName);
        //             if (previousFramesBone.ContainsKey(parentBone ?? ""))
        //             {
        //                 rotation = rotation * previousFramesBone[parentBone].Rotation;
        //             }
        //             // }

        //             switch (controllerName)
        //             {
        //                 case "rArmControl":
        //                 case "rElbowControl":
        //                 case "rHandControl":
        //                     rotation = rotation * Quaternion.Euler(Vector3.forward * 30);
        //                     // SuperController.LogMessage($"adjusting {controllerName}");
        //                     break;
        //                 case "lArmControl":
        //                 case "lElbowControl":
        //                 case "lHandControl":
        //                     rotation = rotation * Quaternion.Euler(Vector3.forward * -30);
        //                     // SuperController.LogMessage($"adjusting {controllerName}");
        //                     break;
        //             }

        //             rotation.x = rotation.x * -1;
        //             rotation.z = rotation.z * -1;

        //             yield return new VamFrameData
        //             {
        //                 Frame = frameCounter,
        //                 ControllerName = controllerName,
        //                 Timestamp = timestamp,
        //                 Position = position,
        //                 Rotation = rotation
        //             };
        //             frameCounter++;

        //             previousFramesBone[controllerName] = frame;
        //         }
        //     }
        // }

        public SimpleJSON.JSONClass ToJSON()
        {
            var atomName = Person.uid;

            var animationJSON = new SimpleJSON.JSONClass();
            animationJSON["AtomType"] = atomName;
            animationJSON["Clips"] = new SimpleJSON.JSONArray();

            var clipJSON = new SimpleJSON.JSONClass();
            clipJSON["AnimationName"] = "MMD";
            clipJSON["AnimationLength"] = VmdFile.Motions.Select(f => f.VamTimestamp).Max().ToString();
            clipJSON["BlendDuration"] = "0.75";
            clipJSON["Loop"] = "1";
            clipJSON["Transition"] = "0";
            clipJSON["EnsureQuaternionContinuity"] = "1";
            clipJSON["AminationLayer"] = "Main Layer";
            clipJSON["Speed"] = "1";
            clipJSON["Weight"] = "1";

            var controllersJSON = new SimpleJSON.JSONArray();
            clipJSON["Controllers"] = controllersJSON;

            var vamFrames = GetVamFrameData()?.ToLookup(f => f.ControllerName);
            foreach (var item in vamFrames)
            {
                var controllerName = item.Key;
                var frames = item.ToList();

                var controllerJSON = BuildTimelineControllerNode(controllerName);
                var i = 0;
                foreach (var frame in frames)
                {
                    var t = frame.Timestamp;

                    if (new string[] { "hipControl", "lFootControl", "rFootControl" }.Contains(frame.ControllerName))
                    {
                        controllerJSON["X"].Add(BuildTimelineControllerValueNode(t, frame.Position.x));
                        controllerJSON["Y"].Add(BuildTimelineControllerValueNode(t, frame.Position.y));
                        controllerJSON["Z"].Add(BuildTimelineControllerValueNode(t, frame.Position.z));
                    }

                    if (i == 0 && frame.Rotation.x == 0 && frame.Rotation.y == 0 && frame.Rotation.z == 0)
                    {
                        // skip rotation if first frame and all 0s
                    }
                    else
                    {
                        controllerJSON["RotX"].Add(BuildTimelineControllerValueNode(t, frame.Rotation.x));
                        controllerJSON["RotY"].Add(BuildTimelineControllerValueNode(t, frame.Rotation.y));
                        controllerJSON["RotZ"].Add(BuildTimelineControllerValueNode(t, frame.Rotation.z));
                        controllerJSON["RotW"].Add(BuildTimelineControllerValueNode(t, frame.Rotation.w));
                    }
                    i++;
                }
                controllersJSON.Add(controllerJSON);
            }

            animationJSON["Clips"].Add(clipJSON);
            return animationJSON;
        }

        private static SimpleJSON.JSONClass BuildTimelineControllerNode(string controller)
        {
            var controllerJSON = new SimpleJSON.JSONClass();
            controllerJSON["Controller"] = controller;
            controllerJSON["X"] = new SimpleJSON.JSONArray();
            controllerJSON["Y"] = new SimpleJSON.JSONArray();
            controllerJSON["Z"] = new SimpleJSON.JSONArray();
            controllerJSON["RotX"] = new SimpleJSON.JSONArray();
            controllerJSON["RotY"] = new SimpleJSON.JSONArray();
            controllerJSON["RotZ"] = new SimpleJSON.JSONArray();
            controllerJSON["RotW"] = new SimpleJSON.JSONArray();
            return controllerJSON;
        }

        private static SimpleJSON.JSONClass BuildTimelineControllerValueNode(float t, float v)
        {
            var node = new SimpleJSON.JSONClass();
            node["t"] = t.ToString();
            node["v"] = v.ToString();
            // { LeaveAsIs, Flat, Linear, Smooth, Bounce, LinearFlat, FlatLinear, CopyPrevious, Constant, FlatLong };
            node["c"] = "3";
            node["ti"] = "0";
            node["to"] = "0";
            return node;
        }
    }

    public class VamFrameData
    {
        public long Frame { get; set; }
        public float Timestamp { get; set; }
        public string ControllerName { get; set; }
        public Vector3 Position { get; set; }
        public Quaternion Rotation { get; set; }

        public override string ToString()
        {
            return $"VamFrameData(t={Timestamp} i={Frame} name={ControllerName}, p={Position}, r={Rotation}";
        }
    }

}
