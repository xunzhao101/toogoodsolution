using AccountDataTransform.Library;
using AccountDataTransform.Library.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;

namespace AccountDataTransform.App
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Dictionary<string, DataTransformer> transformList = new Dictionary<string, DataTransformer>();
        private Dictionary<string, bool> dataFileSpecifications = new Dictionary<string, bool>();
        public MainWindow()
        {

            InitializeComponent();
        }
        /// <summary>
        /// Sample code for handle one data file format
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnTest1_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                IList<IFieldProcessor> processorList = new List<IFieldProcessor>();
                processorList.Add(new IdAccountNoProcessor(TargetFieldConstant.AccountCode, 0));
                processorList.Add(new RequiredFieldProcessor(TargetFieldConstant.Name, 1));
                IList<string> srdRRSPList = new List<string>() { "1", "2", "3", "4" };
                IList<string> targetRRSPList = new List<string>() { "Trading", "RRSP", "RESP", "Fund" };
                processorList.Add(new EnumFieldProcessor(srdRRSPList, targetRRSPList, TargetFieldConstant.Type, 2));
                processorList.Add(new OptionFieldProcessor(TargetFieldConstant.OpenDate, 3, (int)EnumDataTypes.DateTime));
                IList<string> srdCurrentcyList = new List<string>() { "CD", "US" };
                IList<string> targetCurrencyList = new List<string>() { "CAD", "USD" };
                processorList.Add(new EnumFieldProcessor(srdCurrentcyList, targetCurrencyList, TargetFieldConstant.Currency, 4));
                string[] targetFieldArray = new string[] { TargetFieldConstant.AccountCode, TargetFieldConstant.Name, TargetFieldConstant.Type, TargetFieldConstant.OpenDate, TargetFieldConstant.Currency };
                DataTransformer transform = new DataTransformer(processorList, targetFieldArray);
                string filePath = @"test\testfile1.txt";
                DataTransformRunner runner = new DataTransformRunner(transform, filePath, true);
                runner.Execute();
                var validationResult = runner.ValidationResult;
                var transformResult = runner.TransformResult;
                SetValicationResult(validationResult);
                SetTransformationResult(transformResult);
                lsbTransformResult.ItemsSource = runner.GetResult();
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
            }
        }
        /// <summary>
        /// Sample code for handle one data file format
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnTest2_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                IList<IFieldProcessor> processorList = new List<IFieldProcessor>();
                processorList.Add(new RequiredFieldProcessor(TargetFieldConstant.Name, 0));
                processorList.Add(new RequiredFieldProcessor(TargetFieldConstant.Type, 1));
                IList<string> srdCurrentcyList = new List<string>() { "C", "U" };
                IList<string> targetCurrencyList = new List<string>() { "CAD", "USD" };
                processorList.Add(new EnumFieldProcessor(srdCurrentcyList, targetCurrencyList, TargetFieldConstant.Currency, 2));
                processorList.Add(new RequiredFieldProcessor(TargetFieldConstant.AccountCode, 3));
                string[] targetFieldArray = new string[] { TargetFieldConstant.AccountCode, TargetFieldConstant.Name, TargetFieldConstant.Type, TargetFieldConstant.Currency };
                DataTransformer transform = new DataTransformer(processorList, targetFieldArray);
                string filePath = @"test\testfile2.txt";
                DataTransformRunner runner = new DataTransformRunner(transform, filePath);
                runner.Execute();
                var validationResult = runner.ValidationResult;
                var transformResult = runner.TransformResult;
                SetValicationResult(validationResult);
                SetTransformationResult(transformResult);
                lsbTransformResult.ItemsSource = runner.GetResult();
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
            }
        }
        /// <summary>
        /// Sample code for using configuration to handle data file transformation. 
        /// The transformation and test file are chosen using file format combobox(cmbFileFormats)
        /// and test file combobox(cmbTestFile).
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnTest3_Click(object sender, RoutedEventArgs e)
        {
            string fileFormat = cmbFileFormats.SelectedItem as string;
            string testFile = cmbTestFile.SelectedItem as string;
            if (!string.IsNullOrEmpty(fileFormat) && !string.IsNullOrEmpty(testFile))
            {
                DataTransformer transform = transformList[fileFormat];
                DataTransformRunner runner = new DataTransformRunner(transform, testFile, dataFileSpecifications[fileFormat]);
                runner.Execute();
                var validationResult = runner.ValidationResult;
                var transformResult = runner.TransformResult;
                SetValicationResult(validationResult);
                SetTransformationResult(transformResult);
                lsbTransformResult.ItemsSource = runner.GetResult();
            }
        }
        /// <summary>
        /// Read configuration from an xml file.
        /// </summary>
        private void ReadConfigFile()
        {
            string configFile = @"test/config.xml";
            XmlDocument doc = new XmlDocument();
            doc.Load(configFile);
            var nodes = doc.SelectNodes("FileFormats/FileFormat");
            for (int i = 0; i < nodes.Count; i++)
            {
                XmlNode node = nodes[i];
                string formatName = node.Attributes["Name"].Value;
                bool withHeader = node.Attributes["WithHeader"].Value == "true";
               
                IList<IFieldProcessor> processorList = new List<IFieldProcessor>();
                for (int j = 0; j < node.ChildNodes.Count; j++)
                {
                    XmlNode columnNode = node.ChildNodes[j];
                    if (columnNode.Name == "Column")
                    {
                        string target = columnNode.Attributes["Target"].Value;
                        int srcIndex = Convert.ToInt32(columnNode.Attributes["SrcIndex"].Value);
                        int typeIndex = Convert.ToInt32(columnNode.Attributes["TypeIndex"].Value);
                        int? dataType = null;
                        XmlAttribute dataTypeAttr = columnNode.Attributes["DataType"];
                        if (dataTypeAttr != null)
                        {
                            int ret;
                            if (Int32.TryParse(dataTypeAttr.Value, out ret))
                            {
                                dataType = ret;
                            }
                        }
                        switch (typeIndex)
                        {
                            case 1:
                                IFieldProcessor required = new RequiredFieldProcessor(target, srcIndex,dataType);
                                processorList.Add(required);
                                break;
                            case 2:
                                IFieldProcessor option = new OptionFieldProcessor(target, srcIndex,dataType);
                                processorList.Add(option);
                                break;
                            case 3:
                                IFieldProcessor pair = new IdAccountNoProcessor(target, srcIndex, dataType);
                                processorList.Add(pair);
                                break;
                            case 4:
                                string matchList = columnNode.Attributes["MatchData"].Value;
                                string[] pairList = matchList.Split(new char[] { ',' });
                                IList<string> srcList = new List<string>();
                                IList<string> targetList = new List<string>();
                                for (int k = 0; k < pairList.Length; k++)
                                {
                                    string[] valuePair = pairList[k].Split(new char[] { '|' });
                                    if (valuePair.Length == 2)
                                    {
                                        srcList.Add(valuePair[0]);
                                        targetList.Add(valuePair[1]);
                                    }
                                }
                                processorList.Add(new EnumFieldProcessor(srcList, targetList, target, srcIndex, dataType));
                                break;
                        }
                    }

                }
                IList<string> targetFieldList = new List<string>();
                foreach (IFieldProcessor p in processorList)
                {
                    targetFieldList.Add(p.TargetField);
                }
                DataTransformer trans = new DataTransformer(processorList, targetFieldList.ToArray());
                transformList.Add(formatName, trans);
                dataFileSpecifications.Add(formatName, withHeader);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ReadConfigFile();
            cmbFileFormats.ItemsSource = transformList.Keys.ToList();
            cmbTestFile.ItemsSource = new List<string>() { @"test\testfile1.txt", @"test\testfile2.txt", @"test\testfile3.txt" };
        }
        private void SetValicationResult(DataValidationResult result)
        {
            if (result != null)
            {
                lbValidationTotalLines.Content = "Total Lines:" + result.TotalLineCount;
                lbValidationErrorLines.Content = "Invalid Lines:" + result.ErrorLineCount;
                lbValidationValidLines.Content = "Valid Lines:" + result.SucceedLineCount;
                lsbValidationErrorLines.ItemsSource = result.ErrorLines;
                lsbValidationValidLines.ItemsSource = result.ValidatedLines;
            }
        }

        private void SetTransformationResult(DataTransformResult result)
        {
            if (result!=null)
            {
                lbTransformTotalLines.Content = "Total Lines:" + result.TotalLineCount;
                lbTransformErrorLines.Content = "Invalid Lines:" + result.InvalidLineCount;
                lbTransformValidLines.Content = "Valid Lines:" + result.SucceedLineCount;
                lsbTransformErrorLines.ItemsSource = result.ErrorLines;
            }
        }
    }
}
