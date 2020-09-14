using System.Collections.Generic;
using System;
using System.Text;
using System.Linq;

namespace LFE
{
    public static class Extensions
    {
        // VaM is not compiled with I18N with shift_jis encoding. This makes things harder and hacky here.
        // This is a nasty mapping in iso-8859-1 (which should be shift_jis) to turn things into english names
        public static List<KeyValuePair<string, string>> ToEnglishMap = new List<KeyValuePair<string, string>>() {
            new KeyValuePair<string, string>("SÄÌe", "ParentNode"),
            new KeyValuePair<string, string>("ìS", "ControlNode"),
            new KeyValuePair<string, string>("Z^[", "Center"),
            new KeyValuePair<string, string>("¾ÝÀ°", "Center"),
            new KeyValuePair<string, string>("O[v", "Group"),
            new KeyValuePair<string, string>("O[u", "Groove"),
            new KeyValuePair<string, string>("LZ", "Cancel"),
            new KeyValuePair<string, string>("ã¼g", "UpperBody"),
            new KeyValuePair<string, string>("º¼g", "LowerBody"),
            new KeyValuePair<string, string>("èñ", "Wrist"),
            new KeyValuePair<string, string>("«ñ", "Ankle"),
            new KeyValuePair<string, string>("ñ", "Neck"),
            new KeyValuePair<string, string>("ª", "Head"),
            new KeyValuePair<string, string>("ç", "Face"),
            new KeyValuePair<string, string>("º{", "Chin"),
            new KeyValuePair<string, string>("º ²", "Chin"),
            new KeyValuePair<string, string>(" ²", "Jaw"),
            new KeyValuePair<string, string>("{", "Jaw"),
            new KeyValuePair<string, string>("¼Ú", "Eyes"),
            new KeyValuePair<string, string>("Ú", "Eye"),
            new KeyValuePair<string, string>("û", "Eyebrow"),
            new KeyValuePair<string, string>("ã", "Tongue"),
            new KeyValuePair<string, string>("Ü", "Tears"),
            new KeyValuePair<string, string>("«", "Cry"),
            new KeyValuePair<string, string>("", "Teeth"),
            new KeyValuePair<string, string>("Æê", "Blush"),
            new KeyValuePair<string, string>("Â´ß", "Pale"),
            new KeyValuePair<string, string>("K[", "Gloom"),
            new KeyValuePair<string, string>("¾", "Sweat"),
            new KeyValuePair<string, string>("{", "Anger"),
            new KeyValuePair<string, string>("´î", "Emotion"),
            new KeyValuePair<string, string>("", "Marks"),
            new KeyValuePair<string, string>("Ã¢", "Dark"),
            new KeyValuePair<string, string>("", "Waist"),
            new KeyValuePair<string, string>("¯", "Hair"),
            new KeyValuePair<string, string>("OÂÒÝ", "Braid"),
            new KeyValuePair<string, string>("¹", "Breast"),
            new KeyValuePair<string, string>("û", "Boob"),
            new KeyValuePair<string, string>("¨ÁÏ¢", "Tits"),
            new KeyValuePair<string, string>("Ø", "Muscle"),
            new KeyValuePair<string, string>(" ", "Belly"),
            new KeyValuePair<string, string>("½", "Clavicle"),
            new KeyValuePair<string, string>("¨", "Shoulder"),
            new KeyValuePair<string, string>("r", "Arm"),
            new KeyValuePair<string, string>("¤Å", "Arm"),
            new KeyValuePair<string, string>("Ð¶", "Elbow"),
            new KeyValuePair<string, string>("I", "Elbow"),
            new KeyValuePair<string, string>("è", "Hand"),
            new KeyValuePair<string, string>("ew", "ThumbFinger"),
            new KeyValuePair<string, string>("lw", "IndexFinger"),
            new KeyValuePair<string, string>("l·w", "IndexFinger"),
            new KeyValuePair<string, string>("w", "MiddleFinger"),
            new KeyValuePair<string, string>("òw", "RingFinger"),
            new KeyValuePair<string, string>("¬w", "LittleFinger"),
            new KeyValuePair<string, string>("«", "Leg"),
            new KeyValuePair<string, string>("Ð´", "Knee"),
            new KeyValuePair<string, string>("ÂÜ", "Toe"),
            new KeyValuePair<string, string>("³", "Sleeve"),
            new KeyValuePair<string, string>("VK", "New"),
            new KeyValuePair<string, string>("{[", "Bone"),
            new KeyValuePair<string, string>("", "Twist"),
            new KeyValuePair<string, string>("ñ]", "Rotation"),
            new KeyValuePair<string, string>("²", "Axis"),
            new KeyValuePair<string, string>("È¸À²", "Necktie"),
            new KeyValuePair<string, string>("lN^C", "Necktie"),
            new KeyValuePair<string, string>("wbhZbg", "Headset"),
            new KeyValuePair<string, string>("üè", "Accessory"),
            new KeyValuePair<string, string>("{", "Ribbon"),
            new KeyValuePair<string, string>("Ý", "Collar"),
            new KeyValuePair<string, string>("R", "String"),
            new KeyValuePair<string, string>("R[h", "Cord"),
            new KeyValuePair<string, string>("CO", "Earring"),
            new KeyValuePair<string, string>("Kl", "Eyeglasses"),
            new KeyValuePair<string, string>("á¾", "Glasses"),
            new KeyValuePair<string, string>("Xq", "Hat"),
            new KeyValuePair<string, string>("½¶°Ä", "Skirt"),
            new KeyValuePair<string, string>("XJ[g", "Skirt"),
            new KeyValuePair<string, string>("pc", "Pantsu"),
            new KeyValuePair<string, string>("Vc", "Shirt"),
            new KeyValuePair<string, string>("t", "Frill"),
            new KeyValuePair<string, string>("}t[", "Muffler"),
            new KeyValuePair<string, string>("ÏÌ×°", "Muffler"),
            new KeyValuePair<string, string>("", "Clothes"),
            new KeyValuePair<string, string>("u[c", "Boots"),
            new KeyValuePair<string, string>("Ë±ÝÝ", "CatEars"),
            new KeyValuePair<string, string>("Wbv", "Zip"),
            new KeyValuePair<string, string>("¼Þ¯Ìß", "Zip"),
            new KeyValuePair<string, string>("_~[", "Dummy"),
            new KeyValuePair<string, string>("ÀÞÐ°", "Dummy"),
            new KeyValuePair<string, string>("î", "Category"),
            new KeyValuePair<string, string>(" ÙÑ", "Antenna"),
            new KeyValuePair<string, string>("AzÑ", "Antenna"),
            new KeyValuePair<string, string>("~AQ", "Sideburn"),
            new KeyValuePair<string, string>("àÝ °", "Sideburn"),
            new KeyValuePair<string, string>("cCe", "Twintail"),
            new KeyValuePair<string, string>("¨³°", "Pigtail"),
            new KeyValuePair<string, string>("ÐçÐç", "Flutter"),
            new KeyValuePair<string, string>("²®", "Adjustment"),
            new KeyValuePair<string, string>("â", "Aux"),
            new KeyValuePair<string, string>("E", "Right"),
            new KeyValuePair<string, string>("¶", "Left"),
            new KeyValuePair<string, string>("# O", "Front"),
            new KeyValuePair<string, string>("ãë", "Behind"),
            new KeyValuePair<string, string>("ã", "Back"),
            new KeyValuePair<string, string>("¡", "Side"),
            new KeyValuePair<string, string>("", "Middle"),
            new KeyValuePair<string, string>("ã", "Upper"),
            new KeyValuePair<string, string>("º", "Lower"),
            new KeyValuePair<string, string>("e", "Parent"),
            new KeyValuePair<string, string>("æ", "Tip"),
            new KeyValuePair<string, string>("p[c", "Part"),
            new KeyValuePair<string, string>("õ", "Light"),
            new KeyValuePair<string, string>("ß", "Return"),
            new KeyValuePair<string, string>("H", "Wing"),
            new KeyValuePair<string, string>("ª", "Base"),
            new KeyValuePair<string, string>("Ñ", "Strand"),
            new KeyValuePair<string, string>("ö", "Tail"),
            new KeyValuePair<string, string>("K", "Butt"),
            new KeyValuePair<string, string>("ü", "Ornament"),
            new KeyValuePair<string, string>("O", "0"),
            new KeyValuePair<string, string>("P", "1"),
            new KeyValuePair<string, string>("Q", "2"),
            new KeyValuePair<string, string>("R", "3"),
            new KeyValuePair<string, string>("S", "4"),
            new KeyValuePair<string, string>("T", "5"),
            new KeyValuePair<string, string>("U", "6"),
            new KeyValuePair<string, string>("V", "7"),
            new KeyValuePair<string, string>("W", "8"),
            new KeyValuePair<string, string>("X", "9"),
            new KeyValuePair<string, string>("`", "A"),
            new KeyValuePair<string, string>("a", "B"),
            new KeyValuePair<string, string>("b", "C"),
            new KeyValuePair<string, string>("c", "D"),
            new KeyValuePair<string, string>("d", "E"),
            new KeyValuePair<string, string>("e", "F"),
            new KeyValuePair<string, string>("f", "G"),
            new KeyValuePair<string, string>("g", "H"),
            new KeyValuePair<string, string>("h", "I"),
            new KeyValuePair<string, string>("i", "J"),
            new KeyValuePair<string, string>("j", "K"),
            new KeyValuePair<string, string>("k", "L"),
            new KeyValuePair<string, string>("l", "M"),
            new KeyValuePair<string, string>("m", "N"),
            new KeyValuePair<string, string>("n", "O"),
            new KeyValuePair<string, string>("o", "P"),
            new KeyValuePair<string, string>("p", "Q"),
            new KeyValuePair<string, string>("q", "R"),
            new KeyValuePair<string, string>("r", "S"),
            new KeyValuePair<string, string>("s", "T"),
            new KeyValuePair<string, string>("t", "U"),
            new KeyValuePair<string, string>("u", "V"),
            new KeyValuePair<string, string>("v", "W"),
            new KeyValuePair<string, string>("w", "X"),
            new KeyValuePair<string, string>("x", "Y"),
            new KeyValuePair<string, string>("y", "Z"),
            new KeyValuePair<string, string>("{", "+"),
            new KeyValuePair<string, string>("|", "-"),
            new KeyValuePair<string, string>("Q", "_"),
            new KeyValuePair<string, string>("^", "/"),
            new KeyValuePair<string, string>(".", "_")
        };

        public static string ToEnglishName(this string name)
        {
            var newName = name;
            foreach (var replacement in ToEnglishMap)
            {
                // SuperController.LogMessage($"{replacement.Key.ToByteString()}");
                newName = newName.Replace(replacement.Key, replacement.Value);
            }

            return newName;
        }

        private static Dictionary<string, string> BoneNameMap = new Dictionary<string, string>() {
            // Body
            {"Head", "headControl"},
            {"RightElbow", "rElbowControl"},
            {"LeftElbow", "lElbowControl"},
            {"RightArm", "rArmControl"},
            {"LeftArm", "lArmControl"},
            {"RightShoulder", "rShoulderControl"},
            {"LeftShoulder", "lShoulderControl"},
            {"RightShoulderP", "rShoulderControl"},
            {"LeftShoulderP", "lShoulderControl"},
            {"RightWrist", "rHandControl"},
            {"LeftWrist", "lHandControl"},
            {"RightLegIK", "rFootControl"},
            {"LeftLegIK", "lFootControl"},
            {"RightAnkle", "rFootControl"},
            {"LeftAnkle", "lFootControl"},
            {"RightToeTipIK", "rToeControl"},
            {"LeftToeTipIK", "lToeControl"},
            {"UpperBody", "abdomen2Control"},
            {"UpperBody2", "abdomen2Control"},
            {"LowerBody", "pelvisControl"},
            {"LeftKnee", "lKneeControl"},
            {"RightKnee", "rKneeControl"},
            {"Center", "hipControl"},
            {"Neck", "neckControl"},
            {"LeftLeg", "lThighControl"},
            {"RightLeg", "rThighControl"},

            // Fingers
            {"LeftRingFinger1", "lRing1"},
            {"LeftRingFinger2", "lRing2"},
            {"LeftRingFinger3", "lRing3"},
            {"RightRingFinger1", "rRing1"},
            {"RightRingFinger2", "rRing2"},
            {"RightRingFinger3", "rRing3"},

            {"LeftIndexFinger1", "lIndex1"},
            {"LeftIndexFinger2", "lIndex2"},
            {"LeftIndexFinger3", "lIndex3"},
            {"RightIndexFinger1", "rIndex1"},
            {"RightIndexFinger2", "rIndex2"},
            {"RightIndexFinger3", "rIndex3"},

            {"LeftMiddleFinger1", "lMid1"},
            {"LeftMiddleFinger2", "lMid2"},
            {"LeftMiddleFinger3", "lMid3"},
            {"RightMiddleFinger1", "rMid1"},
            {"RightMiddleFinger2", "rMid2"},
            {"RightMiddleFinger3", "rMid3"},

            {"LeftLittleFinger1", "lPinky1"},
            {"LeftLittleFinger2", "lPinky2"},
            {"LeftLittleFinger3", "lPinky3"},
            {"RightLittleFinger1", "rPinky1"},
            {"RightLittleFinger2", "rPinky2"},
            {"RightLittleFinger3", "rPinky3"},

            {"LeftThumbFinger1", "lThumb1"},
            {"LeftThumbFinger2", "lThumb2"},
            {"LeftThumbFinger3", "lThumb3"},
            {"RightThumbFinger1", "rThumb1"},
            {"RightThumbFinger2", "rThumb2"},
            {"RightThumbFinger3", "rThumb3"},
        };

        public static string ToVamBoneName(this string name)
        {
            if (BoneNameMap.ContainsKey(name))
            {
                return BoneNameMap[name];
            }
            return null;
        }

        public static string ToByteString(this byte[] bytes, int startIdx = -1, int endIdx = -1)
        {
            startIdx = startIdx < 0 ? 0 : startIdx;
            startIdx = endIdx < 0 ? bytes.Length - 1 : startIdx;

            var sb = new StringBuilder();
            sb.Append("[");
            for (int i = startIdx; i < endIdx && i < bytes.Length; i++)
            {
                sb.Append(bytes[i].ToString());
                if (i + 1 < endIdx && i + 1 < bytes.Length)
                {
                    sb.Append(",");
                }
            }
            sb.Append("]");
            return sb.ToString();
        }

        public static string GetStringTrimNulls(this byte[] data)
        {
            // if you find a null, set everything else after it to null
            var firstNull = Array.IndexOf(data, (byte)0);
            if (firstNull >= 0)
            {
                for (var i = firstNull; i < data.Length; i++)
                {
                    data[i] = 0;
                }
            }
            return Encoding.GetEncoding("iso-8859-1").GetString(data).TrimEnd(new char[] { (char)0 });
        }
    }

}
