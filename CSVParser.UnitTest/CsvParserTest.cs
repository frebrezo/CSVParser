using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace CSVParser.UnitTest
{
	/// <summary>
	/// Unit tests for CsvParser.
	/// </summary>
	[TestClass]
	public class CsvParserTest
	{
		[TestMethod]
		public void ParseCsvLine_HappyPathDoubleQuotedTest()
		{
			// "field1","field2","field3"
			string csvLine = @"""field1"",""field2"",""field3""";
			CsvParser csvParser = new CsvParser();
			IList<string> fieldList = csvParser.ParseCsvLine(csvLine, ',');
			Assert.AreEqual(3, fieldList.Count);
			Assert.AreEqual("field1", fieldList[0]);
			Assert.AreEqual("field2", fieldList[1]);
			Assert.AreEqual("field3", fieldList[2]);
		}

		[TestMethod]
		public void ParseCsvLine_HappyPathDoubleQuotedWithSpacesTest()
		{
			// " field1  ","  field2  ","field3   "
			string csvLine = @""" field1  "",""  field2  "",""field3   """;
			CsvParser csvParser = new CsvParser();
			IList<string> fieldList = csvParser.ParseCsvLine(csvLine, ',');
			Assert.AreEqual(3, fieldList.Count);
			Assert.AreEqual(" field1  ", fieldList[0]);
			Assert.AreEqual("  field2  ", fieldList[1]);
			Assert.AreEqual("field3   ", fieldList[2]);
		}

		[TestMethod]
		public void ParseCsvLine_HappyPathTest()
		{
			// field1,field2,field3
			string csvLine = @"field1,field2,field3";
			CsvParser csvParser = new CsvParser();
			IList<string> fieldList = csvParser.ParseCsvLine(csvLine, ',');
			Assert.AreEqual(3, fieldList.Count);
			Assert.AreEqual("field1", fieldList[0]);
			Assert.AreEqual("field2", fieldList[1]);
			Assert.AreEqual("field3", fieldList[2]);
		}

		[TestMethod]
		public void ParseCsvLine_HappyPathExtraSpacesTest()
		{
			//  field1 , field2 , field3   
			string csvLine = @" field1 , field2 , field3   ";
			CsvParser csvParser = new CsvParser();
			IList<string> fieldList = csvParser.ParseCsvLine(csvLine, ',');
			Assert.AreEqual(3, fieldList.Count);
			Assert.AreEqual("field1", fieldList[0]);
			Assert.AreEqual("field2", fieldList[1]);
			Assert.AreEqual("field3", fieldList[2]);
		}

		[TestMethod]
		public void ParseCsvLine_EscapedDoubleQuotesTest()
		{
			// "field1 with ""doublequote""","field2","field3"
			string csvLine = @"""field1 with """"doublequote"""""",""field2"",""field3""";
			CsvParser csvParser = new CsvParser();
			IList<string> fieldList = csvParser.ParseCsvLine(csvLine, ',');
			Assert.AreEqual(3, fieldList.Count);
			Assert.AreEqual("field1 with \"doublequote\"", fieldList[0]);
			Assert.AreEqual("field2", fieldList[1]);
			Assert.AreEqual("field3", fieldList[2]);
		}

		[TestMethod]
		public void ParseCsvLine_EscapedDoubleAndCommaTest()
		{
			// "field1 with ""doublequote""","field2.1,field2.2","field3"
			string csvLine = @"""field1 with """"doublequote"""""",""field2.1,field2.2"",""field3""";
			CsvParser csvParser = new CsvParser();
			IList<string> fieldList = csvParser.ParseCsvLine(csvLine, ',');
			Assert.AreEqual(3, fieldList.Count);
			Assert.AreEqual("field1 with \"doublequote\"", fieldList[0]);
			Assert.AreEqual("field2.1,field2.2", fieldList[1]);
			Assert.AreEqual("field3", fieldList[2]);
		}

		[TestMethod]
		public void ParseCsvLine_MissingDoubleQuoteAtEnd()
		{
			// "field1","field2","field3"
			string csvLine = @"""field1"",""field2"",""field3";
			CsvParser csvParser = new CsvParser();
			IList<string> fieldList = csvParser.ParseCsvLine(csvLine, ',');
			Assert.AreEqual(3, fieldList.Count);
			Assert.AreEqual("field1", fieldList[0]);
			Assert.AreEqual("field2", fieldList[1]);
			Assert.AreEqual("field3", fieldList[2]);
		}

		[TestMethod]
		public void ParseCsvLine_MissingDoubleQuoteInMiddle()
		{
			// "field1",field2","field3"
			string csvLine = @"""field1"",field2"",""field3""";
			CsvParser csvParser = new CsvParser();
			IList<string> fieldList = csvParser.ParseCsvLine(csvLine, ',');
			Assert.AreEqual(3, fieldList.Count);
			Assert.AreEqual("field1", fieldList[0]);
			Assert.AreEqual("field2\"", fieldList[1]);
			Assert.AreEqual("field3", fieldList[2]);
		}

		[TestMethod]
		public void ParseCsvLine_ExtraDoubleQuoteInMiddle()
		{
			// "field1",field2"","field3"
			string csvLine = @"""field1"",field2"""",""field3""";
			CsvParser csvParser = new CsvParser();
			IList<string> fieldList = csvParser.ParseCsvLine(csvLine, ',');
			Assert.AreEqual(3, fieldList.Count);
			Assert.AreEqual("field1", fieldList[0]);
			Assert.AreEqual("field2\"\"", fieldList[1]);
			Assert.AreEqual("field3", fieldList[2]);
		}

		[TestMethod]
		public void ParseCsvLine_EmptyFieldDoubleQuoted()
		{
			// "","field1","field2","field3"
			string csvLine = @""""",""field1"",""field2"",""field3""";
			CsvParser csvParser = new CsvParser();
			IList<string> fieldList = csvParser.ParseCsvLine(csvLine, ',');
			Assert.AreEqual(4, fieldList.Count);
			Assert.AreEqual("", fieldList[0]);
			Assert.AreEqual("field1", fieldList[1]);
			Assert.AreEqual("field2", fieldList[2]);
			Assert.AreEqual("field3", fieldList[3]);
		}

		[TestMethod]
		public void ParseCsvLine_EmptyField()
		{
			// ,field1,field2,field3
			string csvLine = @",field1,field2,field3";
			CsvParser csvParser = new CsvParser();
			IList<string> fieldList = csvParser.ParseCsvLine(csvLine, ',');
			Assert.AreEqual(4, fieldList.Count);
			Assert.AreEqual("", fieldList[0]);
			Assert.AreEqual("field1", fieldList[1]);
			Assert.AreEqual("field2", fieldList[2]);
			Assert.AreEqual("field3", fieldList[3]);
		}

		[TestMethod]
		public void ParseCsvLine_UnescapedDoubleQuote()
		{
			// field1,"field2",field3 "random doublequote", field4 with space
			string csvLine = @"field1,""field2"",field3 ""random doublequote"", field4 with space";
			CsvParser csvParser = new CsvParser();
			IList<string> fieldList = csvParser.ParseCsvLine(csvLine, ',');
			Assert.AreEqual(4, fieldList.Count);
			Assert.AreEqual("field1", fieldList[0]);
			Assert.AreEqual("field2", fieldList[1]);
			Assert.AreEqual("field3 \"random doublequote\"", fieldList[2]);
			Assert.AreEqual("field4 with space", fieldList[3]);
		}

		[TestMethod]
		public void ParseCsvLine_UnescapedDoubleQuoteInDoubleQuotedField()
		{
			// "field1","field2 "","field3"
			string csvLine = @"""field1"",""field2 """",""field3""";
			CsvParser csvParser = new CsvParser();
			IList<string> fieldList = csvParser.ParseCsvLine(csvLine, ',');
			Assert.AreEqual(2, fieldList.Count);
			Assert.AreEqual("field1", fieldList[0]);
			Assert.AreEqual("field2 \",field3\"", fieldList[1]);
		}

		[TestMethod]
		public void ParseCsvLine_UnescapedDoubleQuoteInDoubleQuotedFieldInMiddle()
		{
			// "field1","field2 "","field3",field 4
			string csvLine = @"""field1"",""field2 """",""field3"",field 4";
			CsvParser csvParser = new CsvParser();
			IList<string> fieldList = csvParser.ParseCsvLine(csvLine, ',');
			Assert.AreEqual(3, fieldList.Count);
			Assert.AreEqual("field1", fieldList[0]);
			Assert.AreEqual("field2 \",field3\"", fieldList[1]);
			Assert.AreEqual("field 4", fieldList[2]);
		}

		[TestMethod]
		public void ParseCsvLine_UnescapedDoubleQuoteInDoubleQuotedFieldInMiddleAndTripleDoubleQuoteAtEnd()
		{
			// "field1","field2 "","field3",field """4
			string csvLine = @"""field1"",""field2 """",""field3"",field """"""4";
			CsvParser csvParser = new CsvParser();
			IList<string> fieldList = csvParser.ParseCsvLine(csvLine, ',');
			Assert.AreEqual(3, fieldList.Count);
			Assert.AreEqual("field1", fieldList[0]);
			Assert.AreEqual("field2 \",field3\"", fieldList[1]);
			Assert.AreEqual("field \"\"\"4", fieldList[2]);
		}

		[TestMethod]
		public void ParseCsvLine_UnescapedDoubleQuoteInDoubleQuotedFieldInBeginningAndMiddleAndTripleDoubleQuoteAtEnd()
		{
			// ""field1","field2 "","field3",field """4
			string csvLine = @"""""field1"",""field2 """",""field3"",field """"""4";
			CsvParser csvParser = new CsvParser();
			IList<string> fieldList = csvParser.ParseCsvLine(csvLine, ',');
			Assert.AreEqual(3, fieldList.Count);
			Assert.AreEqual("field1\"", fieldList[0]);
			Assert.AreEqual("field2 \",field3\"", fieldList[1]);
			Assert.AreEqual("field \"\"\"4", fieldList[2]);
		}
	}
}
