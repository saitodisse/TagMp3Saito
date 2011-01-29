using System;

namespace TagMp3Saito
{
    [Serializable]
    public class Mp3Field
    {
        public int Index { get; set; }
        public bool Active { get; set; }

        public string FrameTypeString { get; set; }
        public string TagName { get; set; }
        public string Description { get; set; }
        public string DetailedDescription { get; set; }
        public object Valor { get; set; }
        public bool IdV1_Compatible { get; set; }

        public override string ToString()
        {
            return string.Format("[{0}] - {1}", TagName, DetailedDescription);
        }
    }
}
