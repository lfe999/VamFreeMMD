using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MVR.FileManagementSecure;
using UnityEngine;

// https://github.com/mmd-for-unity-proj/mmd-for-unity/blob/de6d0978c207be3b03a28677ae34a1202146f7d9/LICENSE

namespace LFE.MMD
{
    public class VmdFile
    {
        public Header Header { get; private set; }
        public Dictionary<string, List<MotionData>> MotionsByBone { get; private set; }
        public IEnumerable<MotionData> Motions => MotionsByBone.SelectMany(kvp => kvp.Value);
        public Dictionary<string, List<FaceMotionData>> FaceMotionsByBone { get; private set; }
        public IEnumerable<FaceMotionData> FaceMotions => FaceMotionsByBone.SelectMany(kvp => kvp.Value);
        public bool UsesIK { get; private set; }

        public static float Fps = 30;

        private VmdFile(byte[] data)
        {

            var reader = new BytesReader(data);

            // header
            Header = Header.Parse(reader);

            // bone motions
            long motionCount = BitConverter.ToInt32(reader.ReadBytes(4), 0);
            var motions = new List<MotionData>();
            for (var i = 0; i < motionCount; i++)
            {
                var motion = MotionData.Parse(reader);
                motions.Add(motion);
            }
            MotionsByBone = motions
                .OrderBy(f => f.FrameId)
                .GroupBy(g => g.EnglishName)
                .ToDictionary(g => g.Key, g => g.ToList());

            // face motion
            long faceMotionCount = BitConverter.ToInt32(reader.ReadBytes(4), 0);
            var faceMotions = new List<FaceMotionData>();
            for (var i = 0; i < faceMotionCount; i++)
            {
                faceMotions.Add(FaceMotionData.Parse(reader));
            }
            FaceMotionsByBone = faceMotions
                .OrderBy(f => f.FrameId)
                .GroupBy(g => g.Name)
                .ToDictionary(g => g.Key, g => g.ToList());

            // TODO: camera motion

            // TODO: light motion

            UsesIK = Motions.Count(f => f.EnglishName == "LeftLegIK") > 1 || Motions.Count(f => f.EnglishName == "RightLegIK") > 1;
        }


        public static VmdFile Read(string fileName)
        {
            var data = FileManagerSecure.ReadAllBytes(fileName);
            return new VmdFile(data);
        }

        public Dictionary<string, string> Dependencies = new Dictionary<string, string>() {
            { "abdomen2Control", "hipControl" },
            { "pelvisControl", "hipControl" },
            { "rShoulderControl", "abdomen2Control" },
            { "rArmControl", "rShoulderControl" },
            { "rElbowControl", "rArmControl" },
            { "rHandControl", "rElbowControl" },
            { "lShoulderControl", "abdomen2Control" },
            { "lArmControl", "lShoulderControl" },
            { "lElbowControl", "lArmControl" },
            { "lHandControl", "lElbowControl" },
            { "neckControl", "abdomen2Control" },
            { "headControl", "neckControl" },
            { "lThighControl", "pelvisControl" },
            { "rThighControl", "pelvisControl" },
            { "lKneeControl", "lThighControl" },
            { "rKneeControl", "rThighControl" },
            { "lThumb3", "lThumb2"},
            { "lThumb2", "lThumb1"},
            { "rThumb3", "rThumb2"},
            { "rThumb2", "rThumb1"}
        };

        private List<string> _partsEnglish = null;
        public List<string> PartsEnglish
        {
            get
            {
                if (_partsEnglish == null)
                {
                    // order matters
                    _partsEnglish = new List<string>() {
                        "Center", // center
                        "UpperBody", "UpperBody2", "Neck", "Head", // upper body
                        "LowerBody", "LeftLeg", "RightLeg", "RightKnee", "LeftKnee", // lower body
                        "RightShoulder", "LeftShoulder", "LeftArm", "RightArm", // arms center
                        "LeftElbow", "RightElbow", "RightWrist", "LeftWrist" // arms out
                    };

                    if (UsesIK)
                    {
                        _partsEnglish.AddRange(new string[] { "LeftLegIK", "RightLegIK" });
                    }
                    else
                    {
                        _partsEnglish.AddRange(new string[] { "LeftAnkle", "RightAnkle" });
                        Dependencies["rFootControl"] = "rKneeControl";
                        Dependencies["lFootControl"] = "lKneeControl";
                    }
                }
                return _partsEnglish;
            }
        }

        private List<string> _parts = null;
        public List<string> Parts
        {
            get
            {
                if (_parts == null)
                {
                    // order matters
                    _parts = new List<string>() {
                        "Center", // center
                        "UpperBody", "UpperBody2", "Neck", "Head", // upper body
                        "LowerBody", "LeftLeg", "RightLeg", "RightKnee", "LeftKnee", // lower body
                        "RightShoulder", "LeftShoulder", "LeftArm", "RightArm", // arms center
                        "LeftElbow", "RightElbow", "RightWrist", "LeftWrist" // arms out
                    };

                    if (UsesIK)
                    {
                        _parts.AddRange(new string[] { "LeftLegIK", "RightLegIK" });
                    }
                    else
                    {
                        _parts.AddRange(new string[] { "LeftAnkle", "RightAnkle" });
                        Dependencies["rFootControl"] = "rKneeControl";
                        Dependencies["lFootControl"] = "lKneeControl";
                    }

                    _parts = _parts.Select(p => p.ToVamBoneName()).ToList();
                }
                return _parts;
            }
        }

        private Dictionary<string, string> _parentVamBone = new Dictionary<string, string>();
        public string ParentVamBone(string child)
        {
            if (!_parentVamBone.ContainsKey(child))
            {
                if (Dependencies.ContainsKey(child ?? ""))
                {
                    var parent = Dependencies[child];
                    var parentFrames = Motions.Where(f => f.VamBoneName == parent).ToList();
                    while (parent != null && parentFrames.Count <= 1)
                    {
                        if (parent == "hipControl")
                        {
                            break;
                        }
                        if (Dependencies.ContainsKey(parent ?? ""))
                        {
                            parent = Dependencies[parent];
                            parentFrames = Motions.Where(f => f.VamBoneName == parent).ToList();
                        }
                    }
                    // SuperController.LogMessage($"{child} has the parent {parent}");
                    _parentVamBone[child] = parent;
                }
                else
                {
                    _parentVamBone[child] = null;
                }
                // SuperController.LogMessage($"{child} has the parent null");

            }
            return _parentVamBone[child];
        }
    }

    public class BytesReader
    {
        private byte[] _data;
        private int _idx;
        public BytesReader(byte[] data)
        {
            _idx = 0;
            _data = data;
        }

        public byte[] ReadBytes(int count)
        {
            if (_idx + count > _data.Length)
            {
                throw new IndexOutOfRangeException("read too many bytes");
            }

            byte[] slice = new byte[count];
            Array.Copy(_data, _idx, slice, 0, count);
            _idx = _idx + count;
            return slice;
        }

        public byte ReadByte()
        {
            _idx = _idx + 1;
            return _data[_idx - 1];
        }
    }

}
