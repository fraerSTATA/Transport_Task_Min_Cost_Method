using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Fogel
{
    public class MatrixN : IMatrixXML
    {
        private int[,] matrix;

        public int[,] getMatrix() => this.matrix;
      

        public void setMatrix(int[,] par)
        {
            this.matrix = par;
        }

        public void PutMatrixToXML()
        {
           

            var  doc = new XDocument(
            new XDeclaration("1.0", "UTF-8", null),
            new XElement("matrix",
            Enumerable.Range(0, matrix.GetLength(0)).Select(row => new XElement("row",
            Enumerable.Range(0, matrix.GetLength(1)).Select(column => new XElement("cell",
                matrix[row, column]))))));
            doc.Root.SetAttributeValue("dataType", "System.Int32");
            var type = Type.GetType(doc.Root.Attribute("dataType").Value.ToString());

            using (var streamWriter = new StreamWriter("C:/Users/Admin-PC/Desktop/Математическое моделирование/Fogel/Matrix.xml",
            false, Encoding.UTF8))
            {
                XmlWriterSettings settings = new XmlWriterSettings()
                {
                    Indent = true,
                    IndentChars = " ",
                    Encoding = Encoding.UTF8,
                    NewLineChars = Environment.NewLine
                };

                using (XmlWriter xmlWriter = XmlWriter.Create(streamWriter, settings))
                {
                    doc.Save(xmlWriter);
                    xmlWriter.Flush();
                }
            }
        }

        public void CopyMatrix(MatrixN copy)
        {
            matrix = new int[copy.getMatrix().GetLength(0),copy.getMatrix().GetLength(1)];
            for (int i = 0; i < copy.getMatrix().GetLength(0); i++)
            {
                for (int j = 0; j < copy.getMatrix().GetLength(1); j++)
                {
                    this.matrix[i, j] = copy.getMatrix()[i, j];
                }
            }
        }
        public void TakeMatrixFromXML(string path)
        {
            XmlDocument doc = new XmlDocument();
            using (var reader = XmlReader.Create(path))
            {
                doc.Load(reader);
                var element = doc.DocumentElement;
                matrix = new int[element.ChildNodes.Count, element.FirstChild.ChildNodes.Count];
                int j = 0;
                int i = 0;
                foreach (XmlNode node in element)
                {
                    j = 0;
                    foreach (XmlNode cnode in node.ChildNodes)
                    {
                        string a = cnode.InnerText;
                        matrix[i, j] = Convert.ToInt32(a);
                        j++;
                    }
                    i++;
                }
            }

            int potreb = 0;
            for(int i = 0; i < matrix.GetLength(1);i++)
            {
                potreb += matrix[0, i];
            }
            int post = 0;
                 for (int j = 0; j < matrix.GetLength(0); j++)
            {
                post += matrix[j,0];
            }
         
            if(potreb > post)
            {
                makeRow(potreb - post);
            }
            else if (post > potreb)
            {
                makeColumn( post - potreb);
            }
        }


        protected void makeRow(int raz)
        {
            int[,] copy = new int[matrix.GetLength(0) + 1, matrix.GetLength(1)];
            for (int i = 0; i < copy.GetLength(0)-1; i++)
            {
                for (int j = 0; j < copy.GetLength(1); j++)
                {
                    copy[i,j] = matrix[i, j];
                }
            }
            copy[copy.GetLength(0)-1, 0] = raz;
            int row = matrix.GetLength(0) + 1;
            int column = matrix.GetLength(1);
            matrix = new int[row, column];
            matrix = copy;
        }

        protected void makeColumn(int raz)
        {
            int[,] copy = new int[matrix.GetLength(0), matrix.GetLength(1) + 1 ];
            for (int i = 0; i < copy.GetLength(0); i++)
            {
                for (int j = 0; j < copy.GetLength(1)-1; j++)
                {
                    copy[i, j] = matrix[i, j];
                }
            }
            copy[0, copy.GetLength(1) - 1] = raz;
            int row = matrix.GetLength(0);
            int column = matrix.GetLength(1) + 1;
            matrix = new int[row, column];
            matrix = copy;
        }

    }

    
}
