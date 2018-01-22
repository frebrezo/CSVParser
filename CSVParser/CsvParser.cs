using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSVParser
{
	/// <summary>
	/// CSV parser. CsvParser follows Microsoft Excel parsing conventions.
	/// </summary>
	public class CsvParser
	{
		/// <summary>
		/// Parse line into CSV record array
		/// </summary>
		/// <param name="line"></param>
		/// <param name="delimiter"></param>
		/// <returns></returns>
		public IList<string> ParseCsvLine(string line, char delimiter)
		{
			StringBuilder fieldVal = new StringBuilder();
			IList<string> lineArr = new List<string>();
			bool startsWithDQuote = false;
			bool unexpectedDQuote = false;
			int cInt = 0;
			char c = (char)0;
			int quotedCInt = 0;
			char quotedC = (char)0;
			int nextQuotedCInt = 0;
			char nextQuotedC = (char)0;
			using (System.IO.StringReader strBuffer = new System.IO.StringReader(line))
			{
				// Loop over each character in string line.
				while ((cInt = strBuffer.Read()) > 0)
				{
					c = (char)cInt;
					// If the field value starts with a doublequote (") then set startsWithDQuote = true.
					if (fieldVal.Length == 0 && c == '"')
					{
						startsWithDQuote = true;
					}
					// If a delimiter has been reached, add the field value to lineArr.
					//		Otherwise, continue looking for the end of the field value.
					if (c == delimiter)
					{
						lineArr.Add(startsWithDQuote ? fieldVal.ToString() : fieldVal.ToString().Trim());
						fieldVal.Length = 0;
						startsWithDQuote = false;
						unexpectedDQuote = false;
					}
					else
					{
						// If c == doublequote ("), scan for a matching doublequote (") character.
						if (c == '"')
						{
							// If the field value length > 0 and the field doesn't start with a
							//		doublequote (") then a doublequote (") was encountered in the
							//		middle of the field value. Append it to field value.
							if (fieldVal.Length > 0 && !startsWithDQuote)
							{
								fieldVal.Append(c);
							}
							// Scan for end of the field.
							while ((quotedCInt = strBuffer.Read()) > 0)
							{
								quotedC = (char)quotedCInt;
								// If field value does NOT start with a doublequote (") and the
								//		delimiter character has been encoutered, then the
								//		end of the field value has been found.
								//		Add the field to lineArr and break out of the loop.
								//		Otherwise, a delimiter was found in the middle of a
								//		field value surrounded with doublequotes ("), so add the
								//		delimiter to the field value.
								//		If doublequote (") character is encoutered in the middle
								//		of the field value and the field value starts with
								//		a doublequote (") character then doublequote (") must be
								//		escaped.
								if (!startsWithDQuote && quotedC == delimiter)
								{
									lineArr.Add(startsWithDQuote ? fieldVal.ToString() : fieldVal.ToString().Trim());
									fieldVal.Length = 0;
									startsWithDQuote = false;
									unexpectedDQuote = false;
									break;
								}
								else if (quotedC == '"')
								{
									if (fieldVal.Length > 0 && !startsWithDQuote)
									{
										fieldVal.Append(quotedC);
									}
									// Read the immediate next character and add it to
									//		the field value. If the immediate next character
									//		is a delimiter then add field value to lineArr and
									//		break out of the loop.
									if ((nextQuotedCInt = strBuffer.Read()) > 0)
									{
										nextQuotedC = (char)nextQuotedCInt;
										// The field value starts with a doublequote (") character,
										//		but a doublequote (") was encountered without
										//		a matching doublequote ("). If a doublequote (")
										//		is encountered at the end of a field value,
										//		then add the final doublequote (") character.
										if (startsWithDQuote && quotedC == '"' && nextQuotedC != '"' && nextQuotedC != delimiter)
										{
											unexpectedDQuote = true;
										}
										if (nextQuotedC == delimiter)
										{
											if (unexpectedDQuote && quotedC == '"')
											{
												fieldVal.Append(quotedC);
											}
											lineArr.Add(startsWithDQuote ? fieldVal.ToString() : fieldVal.ToString().Trim());
											fieldVal.Length = 0;
											startsWithDQuote = false;
											unexpectedDQuote = false;
											break;
										}
										else
										{
											fieldVal.Append(nextQuotedC);
										}
									}
									else if (unexpectedDQuote && quotedC == '"')
									{
										// If a final doublequote (") is encountered,
										//		then add the final doublequote (") character.
										fieldVal.Append(quotedC);
									}
								}
								else
								{
									fieldVal.Append(quotedC);
								}
							}
						}
						else
						{
							fieldVal.Append(c);
						}
					}
				}
				// After looping all the characters in string line, if field value contains
				//		a value, add it to lineArr.
				if (fieldVal.Length > 0)
				{
					lineArr.Add(startsWithDQuote ? fieldVal.ToString() : fieldVal.ToString().Trim());
					fieldVal.Length = 0;
					startsWithDQuote = false;
					unexpectedDQuote = false;
				}
			}
			return lineArr;
		}
	}
}
