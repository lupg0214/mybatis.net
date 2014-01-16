using System;
using System.Collections.Specialized;
using MyBatis.Common.Logging;
using MyBatis.Common.Logging.Impl;
using MyBatis.DataMapper.Sqlite.Test.Domain.ComplexStructure;
using NUnit.Framework;

namespace MyBatis.DataMapper.Sqlite.Test.Fixtures
{
    [TestFixture]
    public class ComplexBindNestedIterateTestFixture : BaseTest
    {
        [Test]
        public void TestIterateWithPrepend()
        {
            LogManager.Adapter = new ConsoleOutLoggerFA(new NameValueCollection());

            InitScript(sessionFactory.DataSource, "../../Scripts/nested-iterate.sql");

            var rules = new Filter
            {
                Rules = new Rules
                {
                    CustomFields = new CustomField[]
                    {
                        new CustomField { ColumnName = "Column1" },
                        new CustomField { ColumnName = "Column2" },
                        new CustomField { ColumnName = "Column3" },
                        new CustomField { ColumnName = "Column4" },
                        new CustomField { ColumnName = "Column5" },
                    }
                }
            };

            var items = dataMapper.QueryForDataTable("Campaign.IterateWithPrepend", rules);
        }

        [Test]
        public void TestTwoDifferentItemsInArrayUsingTwoDifferentComparisonMechanismsWithVariableBinding()
        {
            LogManager.Adapter = new ConsoleOutLoggerFA(new NameValueCollection());

            InitScript(sessionFactory.DataSource, "../../Scripts/nested-iterate.sql");

            var firstName = "Muppet 3";
            var field2 = 1;

            var rules = new Filter
            {
                Rules = new Rules
                {
                    CustomFields = new CustomField[]
                    {
                        new CustomField
                        {
                            ColumnName = "Firstname",
                            DataType = ColumnDataTypeEnum.String,
                            Name = "First Name",
                            Comparisons = new Compare[]
                            {
                                new Compare
                                {
                                    Contains = firstName
                                }
                            },
                        },
                        new CustomField
                        {
                            ColumnName = "Field2",
                            DataType = ColumnDataTypeEnum.Integer,
                            Name = "Age",
                            Comparisons = new Compare[]
                            {
                                new Compare
                                {
                                    Equal = field2.ToString()
                                }
                            },
                        }
                    }
                }
            };

            var items = dataMapper.QueryForDataTable("Campaign.FilterPeople", rules);
            var foundFirstName = false;
            var foundField2 = false;

            for (var rowIndex = 0; rowIndex < items.Rows.Count; rowIndex++)
            {
                var row = items.Rows[rowIndex];

                for (var columnIndex = 0; columnIndex < row.ItemArray.Length; columnIndex++)
                {
                    var column = items.Columns[columnIndex];
                    var rowItem = row.ItemArray[columnIndex];

                    switch (column.ColumnName)
                    {
                        case "Firstname":
                            foundFirstName = true;
                            Assert.IsTrue(Convert.ToString(rowItem).ToLower().Contains(firstName.ToLower()));
                            break;

                        case "Field2":
                            foundField2 = true;
                            Assert.IsTrue(Convert.ToInt32(rowItem) == field2);
                            break;
                    }

                    System.Console.WriteLine("{0}\t{1}: {2}", row, column, rowItem);
                }

                System.Console.WriteLine(Environment.NewLine);
            }

            Assert.That(items.Rows.Count == 1, String.Format("Expected a single row to be returned, instead {0} rows were returned", items.Rows.Count));
            Assert.That(foundFirstName, "First name column was not returned");
            Assert.That(foundField2, "Field2 column was not returned");
        }
    }
}