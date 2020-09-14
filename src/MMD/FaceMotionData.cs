using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using LFE;

namespace LFE.MMD
{
    public class FaceMotionData
    {
        public string Name { get; set; }
        public uint FrameId { get; set; }
        public float Rate { get; set; }

        public static FaceMotionData Parse(BytesReader reader)
        {
            var name = reader.ReadBytes(15).GetStringTrimNulls();
            uint frameNumber = BitConverter.ToUInt32(reader.ReadBytes(4), 0);
            float rate = BitConverter.ToSingle(reader.ReadBytes(4), 0);

            return new FaceMotionData
            {
                Name = name,
                FrameId = frameNumber,
                Rate = rate
            };
        }

        public override string ToString()
        {
            return $"FaceMotionData(i={FrameId}, name={Name}, r={Rate})";
        }

    }
}
