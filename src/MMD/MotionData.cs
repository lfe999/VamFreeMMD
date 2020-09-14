using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using LFE;

namespace LFE.MMD
{
    public class MotionData
    {
        public string Name { get; set; }
        public string EnglishName { get; set; }
        public string VamBoneName { get; set; }
        public uint FrameId { get; set; }
        public Vector3 Position { get; set; }
        public Quaternion Rotation { get; set; }
        public byte[][][] Interpolation { get; set; }

        public float VamTimestamp => FrameId / 30f;

        public static MotionData Parse(BytesReader reader)
        {
            // name
            var name = reader.ReadBytes(15).GetStringTrimNulls();
            var englishName = name.ToEnglishName();
            if (string.IsNullOrEmpty(englishName))
            {
                englishName = name;
            }
            var vamBoneName = englishName.ToVamBoneName();

            // frame
            uint frameId = BitConverter.ToUInt32(reader.ReadBytes(4), 0);

            // position
            float posX = BitConverter.ToSingle(reader.ReadBytes(4), 0);
            float posY = BitConverter.ToSingle(reader.ReadBytes(4), 0);
            float posZ = BitConverter.ToSingle(reader.ReadBytes(4), 0);

            // rotation
            float rotX = BitConverter.ToSingle(reader.ReadBytes(4), 0);
            float rotY = BitConverter.ToSingle(reader.ReadBytes(4), 0);
            float rotZ = BitConverter.ToSingle(reader.ReadBytes(4), 0);
            float rotW = BitConverter.ToSingle(reader.ReadBytes(4), 0);

            // interpolation - https://mikumikudance.fandom.com/wiki/VMD_file_format
            var interpolation = new byte[4][][];
            for (int i = 0; i < 4; i++) { interpolation[i] = new byte[4][]; for (int j = 0; j < 4; j++) { interpolation[i][j] = new byte[4]; } }
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                    for (int k = 0; k < 4; k++)
                        interpolation[i][j][k] = reader.ReadByte();

            return new MotionData
            {
                Name = name,
                EnglishName = englishName,
                VamBoneName = vamBoneName,
                FrameId = frameId,
                Position = new Vector3(posX, posY, posZ),
                Rotation = new Quaternion(rotX, rotY, rotZ, rotW),
                Interpolation = interpolation
            };
        }

        public override string ToString()
        {
            return $"MotionData(i={FrameId}, name={Name} ({EnglishName}), p={Position}, r={Rotation}";
        }
    }
}
