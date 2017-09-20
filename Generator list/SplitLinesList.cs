using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Generator_list
{
    class SplitLinesList
    {
        private string splitLines;
        private int id, spacesCount;

        public string SplitLines { get { return splitLines; } }
        public int Id { get { return id; } }
        public int SpacesCount { get { return spacesCount; } }

        public SplitLinesList(string splitLines, int id, int spacesCount)
        {
            this.splitLines = splitLines;
            this.id = id;
            this.spacesCount = spacesCount;
        }
    }
}
