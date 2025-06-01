using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOTNETAreaMap
{
    public interface IProcessor
    {
        void Process(Type input, StreamWriter writer);

        TypeLink[] GetReferencedTypes();
    }
}
