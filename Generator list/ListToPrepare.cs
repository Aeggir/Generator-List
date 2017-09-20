using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Generator_list
{
    class ListToPrepare
    {
        private string[] listToPreparation;
        List<SplitLinesList> listToMake;        

        public ListToPrepare(string[] listToPreparation)
        {
            this.listToPreparation = listToPreparation;
            makeList();
        }

        private void makeList()
        {
            listToMake = new List<SplitLinesList>();
            int i = 0;
            foreach (string lineToPrepare in listToPreparation)
            {
                int count = spacesToCount(lineToPrepare);
                listToMake.Add(new SplitLinesList(lineToPrepare, i, count));
                i++;
            }
        }

        private int spacesToCount(string lineToPrepare)
        {
            string[] spaces = lineToPrepare.Split(new char[] {' '});
            return spaces.Length;
        }

        public IEnumerable<SplitLinesList> sortListByCount()
        {
            var list = from lines in listToMake
                       orderby lines.SpacesCount ascending
                       select lines;
            return list;
        }
    }
}
