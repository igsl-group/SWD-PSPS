using DocxGenerator.Library;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace WordDocumentGenerator.Library
{
    public class RunDocumentGenerator<T> where T : class
    {
        private string _FilePath;
        private string _FileName;
        private string _outputFilePath;

        public RunDocumentGenerator(string FilePath, string FileName)
        {
            _FilePath = FilePath;
            _FileName = FileName;
        }

        public void generateDoc(T t)
        {
            var inputFilePath = Path.Combine(_FilePath, _FileName);
            _outputFilePath = Path.Combine(@"MailMerge\Templates", "Test_Template - 1_out.docx");

            SimpleDocumentGenerator<T> docGenerator = new SimpleDocumentGenerator<T>(new DocumentGenerationInfo
            {
                DataContext = t,
                TemplateData = File.ReadAllBytes(inputFilePath)
            });

            byte[] fileContents = docGenerator.GenerateDocument();

            if (fileContents != null)
            {
                File.WriteAllBytes(_outputFilePath, fileContents);
            }
        }
    }
}