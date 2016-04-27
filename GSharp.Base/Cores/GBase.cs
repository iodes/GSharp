using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace GSharp.Base.Cores
{
    [Serializable]
    public abstract class GBase
    {
        /// <summary>
        /// 객체를 네이티브 소스로 변환합니다.
        /// </summary>
        /// <returns></returns>
        public abstract string ToSource();

        /// <summary>
        /// 객체를 직렬화하여 바이너리 형태로 저장합니다.
        /// </summary>
        /// <param name="path"></param>
        public void Serialize(string path)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                formatter.Serialize(stream, this);
            }
        }
    }
}
