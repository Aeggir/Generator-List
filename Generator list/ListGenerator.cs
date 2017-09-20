using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Generator_list
{
    public class ListGenerator
    {
        public ListGenerator(TextBox inputTextBox, string className, string listName, string categoryName)
        {
            this.inputTextBox = inputTextBox;
            this.className = className;
            this.listName = listName;
            this.categoryName = categoryName;
            CreateList();
        }

        private TextBox inputTextBox;
        private string className, listName, categoryName, listTextGenerator;
        private string[] splitLines, splitedInputText, splitedCategoryNames;
        private int lineError = 1, categoryIndex, numberOfCategoriesToJoin, breakIndex;
        private bool lineErrorBool = true;
        private List<SplitLinesList> listAfterAscending;

        public string ListTextGenerator { get { return listTextGenerator; } }

        private void CreateList()
        {
            splitLines = inputTextBox.Text.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            splitedCategoryNames = categoryName.Split(new char[] { ' ' });
            makeLineSplit();
            listTextGenerator = "List<" + className + "> " + listName + " = new List<" + className + ">() {" + Environment.NewLine;
            
            foreach (string splitLine in splitLines)
            {
                listTextGenerator += "new " + className + Environment.NewLine + " { ";
                
                splitedInputText = splitLine.Split(new char[] { ' ' });
                categoryIndex = 0;

                CheckIfCorrect();

                if (lineErrorBool)
                {
                    for (int i = 0; i < splitedCategoryNames.Length; i++)
                    {
                    
                        categoryWriter(i);

                        if (i < splitedCategoryNames.Length - 1)
                        {
                            listTextGenerator += ", ";
                        }                                                
                    }                    
                    listTextGenerator += "}," + Environment.NewLine;
                }
            }

            listTextGenerator += " };";

            if (lineErrorBool)                        
                inputTextBox.Text = listTextGenerator;          

        }

        private void CheckIfCorrect()
        {          

            int lineLengt = splitedInputText.Length;            
            bool haveBreak = false;
            int agregatteJoinNameNumber = 0;
            for (int i = 0; i < splitedCategoryNames.Length; i++)
            {
                agregatteJoinNameNumber += joinNameNumber(i);
            }
            lineLengt -= agregatteJoinNameNumber;

            for (int i = 0; i < splitedInputText.Length; i++)
            {
                if (createJoinText(i, true))
                {
                    haveBreak = true;                    
                }                
            }


            if (haveBreak)
            {
                if (lineLengt > splitedCategoryNames.Length)
                {
                    MessageBox.Show("O " + (lineLengt - splitedCategoryNames.Length) + " wyrażeń więcej w linii: " + lineError);
                    lineErrorBool = false;
                }
                else if (lineLengt < (splitedCategoryNames.Length - agregatteJoinNameNumber))
                {
                    MessageBox.Show("O " + (splitedCategoryNames.Length - lineLengt) + " wyrażeń mniej w linii: " + lineError);
                    lineErrorBool = false;
                }
            }
            else
            {

                if (lineLengt > splitedCategoryNames.Length)
                {
                    MessageBox.Show("O " + (lineLengt - splitedCategoryNames.Length) + " wyrażeń więcej w linii: " + lineError);
                    lineErrorBool = false;
                }
                else if (lineLengt < splitedCategoryNames.Length)
                {
                    MessageBox.Show("O " + (splitedCategoryNames.Length - lineLengt) + " wyrażeń mniej w linii: " + lineError);
                    lineErrorBool = false;
                }
            }
            
            lineError++;
            
        }

        private void categoryWriter(int i)
        {
            
            if (splitedCategoryNames[i].Contains("`@"))
            {
                categoryTextJoin(i);
            }
            else if (splitedCategoryNames[i].Contains("`"))
            {
                listTextGenerator += splitedCategoryNames[i].TrimEnd('`') + " = " + splitedInputText[categoryIndex];
            }
            else
            {
                listTextGenerator += splitedCategoryNames[i] + " = \"" + splitedInputText[categoryIndex] + "\"";
            }
            categoryIndex++;
        }

        private void joinNameNumber(int i, out int index, out int result)
        {
            index = splitedCategoryNames[i].IndexOf("`@");
            result = int.Parse(splitedCategoryNames[i].Substring(index + 2));            
        }

        private int joinNameNumber(int i)
        {
            int result = 0;
            int index = 0;
            if (splitedCategoryNames[i].Contains("`@"))
            {
                joinNameNumber(i, out index, out result);
                result--;
                return result;
            }
            return 0;
        }

        private void categoryTextJoin(int i)
        {
            int index = 0;
            numberOfCategoriesToJoin = 0;
            joinNameNumber(i, out index, out numberOfCategoriesToJoin);
            int newCategoryIndex = numberOfCategoriesToJoin + categoryIndex;
            string joinCategoryText = createJoinText(newCategoryIndex);
            joinCategoryText = joinCategoryText.TrimStart(' ');
            if (breakIndex > 0)
            {
                newCategoryIndex -= breakIndex;
                joinCategoryText = joinCategoryText.TrimEnd('`');
            }

            listTextGenerator += splitedCategoryNames[i].Remove(index) + " = \"" + joinCategoryText + "\"";
            newCategoryIndex--;
            categoryIndex = newCategoryIndex;
        }

        private void createJoinText(int newCategoryIndex, out bool splitCategoryContains, out string result)
        {
            breakIndex = 0;
            result = "";
            int doWhile = 0; 
            for (int i = categoryIndex; i < newCategoryIndex; i++)
            {
                doWhile++;
                result += " " + splitedInputText[i];
                if (splitedInputText[i].Contains("`"))
                {
                    splitCategoryContains = true;
                    breakIndex += numberOfCategoriesToJoin - doWhile;
                    return;
                }
                
            }
            splitCategoryContains = false;
        }

        private string createJoinText(int newCategoryIndex)
        {
            bool splitCategoryContains = true;
            string result;
            createJoinText(newCategoryIndex, out splitCategoryContains, out result);
            return result;
        }

        private bool createJoinText(int newCategoryIndex, bool boolVersion)
        {            
            string result;
            bool splitCategoryContains;            
            createJoinText(newCategoryIndex, out splitCategoryContains, out result);
            if (splitCategoryContains)
            {
                return true;
            }
            return false;
        }

        public void prepareList()
        {
            makeLineSplit();
            ListToPrepare listToMake = new ListToPrepare(splitLines);
            listAfterAscending = new List<SplitLinesList>();
            listAfterAscending.AddRange(listToMake.sortListByCount());
            inputTextBox.Text = "";           
            foreach (SplitLinesList line in listAfterAscending)
            {
                inputTextBox.Text += line.SplitLines + Environment.NewLine;
            }
            makeLineSplit();
        }

        private void makeLineSplit()
        {
            splitLines = inputTextBox.Text.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            splitedCategoryNames = categoryName.Split(new char[] { ' ' });
        }

        public void compareLines()
        {
            string[] splitNewLines = inputTextBox.Text.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            int listIndex = 0;
            int wordIndex = 0;
            List<string> sentenceList = new List<string>();
            List<string> newSentenceList = new List<string>();
            List<string> splitList = new List<string>(splitNewLines);
            string joinWords = "";

            for (int i = 0; i < splitNewLines.Length; i++)
            {
                if (!(splitNewLines[i] == splitLines[i]))
                {
                    listIndex = i;
                }
            }


                    for (int i = 0; i < 2; i++)
                    {
                int index = listIndex + i + 1;
                sentenceList.Add(splitNewLines[index]);
                    }

                    string[] splitWords = splitNewLines[listIndex].Split(new char[] { ' ' });

                    for (int id = 0; id < splitWords.Length; id++)
                    {
                        if (splitWords[id].Contains("`"))
                        {
                            wordIndex = id;
                        }
                    }

                    foreach (string sentence in sentenceList)
                    {
                joinWords = "";
                string[] splitWordsSentence = sentence.Split(new char[] { ' ' });
                        splitWordsSentence[wordIndex] += "`";
                        foreach (string joinWordsFromSentence in splitWordsSentence)
                        {
                            joinWords += joinWordsFromSentence + " ";
                        }
                        joinWords = joinWords.Trim();
                        newSentenceList.Add(joinWords);
                    }

                    

            for (int i = 0; i < 2; i++)
            {
                int index = listIndex + i + 1;
                splitList.RemoveAt(index);
                splitList.Insert(index, newSentenceList[i]);
            }

            foreach (string wordsInList in splitList)
            {
                inputTextBox.Text += wordsInList + Environment.NewLine;
            }


        }
    }
}
