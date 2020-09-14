using System;

namespace LFE.MMD
{
    public class Header
    {
        private const string ExpectedSignature = "Vocaloid Motion Data 0002";

        public string ModelName { get; private set; }
        public string Signature { get; private set; }
        public int Version { get; private set; }

        public static Header Parse(BytesReader reader)
        {
            // signature
            string signature = reader.ReadBytes(30).GetStringTrimNulls();
            if (!signature.Equals(Header.ExpectedSignature))
            {
                throw new FormatException($"Invalid header: expecting signature '{Header.ExpectedSignature}', got '{signature}'");
            }

            // version from signature string
            int version = int.Parse(signature.Substring(signature.Length - 1));
            if (version != 2)
            {
                throw new FormatException($"Invalid header: expecting version '2', got '{version}'");
            }

            // model name
            string modelName = reader.ReadBytes(20).GetStringTrimNulls();

            return new Header
            {
                ModelName = modelName,
                Signature = signature,
                Version = version
            };
        }
    }

}
