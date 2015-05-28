using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ManagementSoftware.Helper;
using Common.DataTransferObjects;
using System.Globalization;
using System.Windows;
using Common.Util;
using System.Windows.Media;
using System.Windows.Controls;
using ManagementSoftware.Model;
using System.Collections.Generic;
using System.Collections;

namespace ManagementSoftware.Tests.Helper
{
    [TestClass]
    public class Helper
    {
        [TestMethod]
        public void ColumnVisibilityConverterTest()
        {
            ColumnVisibilityConverter testee = new ColumnVisibilityConverter();

            object retValue = testee.Convert((object) 31, typeof(Visibility), "30", default(CultureInfo));
            Assert.AreEqual(retValue, Visibility.Visible);

            retValue = testee.Convert((object)30, typeof(Visibility), "30", default(CultureInfo));
            Assert.AreEqual(retValue, Visibility.Hidden);
        }

        [TestMethod]
        public void EmployeeTypeConverterTest()
        {
            EmployeeTypeConverter testee = new EmployeeTypeConverter();
            object retValue;
            string expectedRetValue;

            
            expectedRetValue = EmployeeTypeConverter.TYPE_NULL;
            retValue = testee.Convert(null, typeof(string), null, default(CultureInfo));
            Assert.AreEqual(retValue, expectedRetValue);

            Employee e = new Driver();
            expectedRetValue = Util.CreateValuePair<EEmployeeType>(e.EmployeeType).Value;
            retValue = testee.Convert(e, typeof(string), null, default(CultureInfo));
            Assert.AreEqual(retValue, expectedRetValue);

            retValue = testee.Convert((Int32)32, typeof(string), null, default(CultureInfo));
            Assert.AreEqual(retValue, "Int32");
        }

        [TestMethod]
        public void GridColorConverterTest()
        {
            GridColorConverter testee = new GridColorConverter();
            object retValue;
            
            retValue = Convert(testee, "", -1);
            Assert.AreEqual(retValue, GridColorConverter.DEFAULT_BRUSH);
            
            retValue = Convert(testee, "", 30);
            Assert.AreEqual(retValue, GridColorConverter.DEFAULT_BRUSH);
          
            retValue = Convert(testee, "asdf", 10);
            Assert.AreEqual(retValue, GridColorConverter.DEFAULT_BRUSH);

            retValue = Convert(testee, ShiftScheduleMonthEntry.AM_SHIFT, 10);
            Assert.AreEqual(retValue, GridColorConverter.AM_SHIFT_BRUSH);

            retValue = Convert(testee, ShiftScheduleMonthEntry.PM_SHIFT, 10);
            Assert.AreEqual(retValue, GridColorConverter.PM_SHIFT_BRUSH);

            retValue = Convert(testee, ShiftScheduleMonthEntry.NO_SHIFT, 10);
            Assert.AreEqual(retValue, GridColorConverter.NO_SHIFT_BRUSH);
        }

        private object Convert(GridColorConverter testee, string shift, int index)
        {
            ShiftScheduleMonthEntry entry = new ShiftScheduleMonthEntry(null, 30);
    
            if (index >= 0 && index < 30)
            {
                entry.Days[index] = shift;
            }

            return testee.Convert(new object[]{index+1, entry.Days}, typeof(SolidColorBrush), null, default(CultureInfo));
        }

        [TestMethod]
        public void MyListBoxTest()
        {
            MyListBox box = new MyListBox();
            box.SelectionMode = SelectionMode.Multiple;
           
            List<string> all = new List<string>();
            all.Add("1");
            all.Add("2");
            all.Add("3");
            all.Add("4");

            List<string> select = new List<string>(all);
            select.RemoveAt(0);

            box.ItemsSource = all;

            box.Select(select);

            CollectionAssert.AreEqual(box.SelectedItems, new List<string>(select));
        }
    }
}
