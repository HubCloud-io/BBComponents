using NUnit.Framework;
using BBComponents.Components;

namespace BbComponents.UnitTests
{
    [TestFixture]
    public class Tests
    {
     

        [Test]
        public void GetFirstCalendarDate_WeekDay_FirstDayMonday_ReturnFirstMonday()
        {
            var date = BbDatePicker.GetFirstCalendarDate(new System.DateTime(2020, 04, 2), BBComponents.Enums.FirstWeekDays.Monday);

            Assert.AreEqual(3, date.Month);
            Assert.AreEqual(30, date.Day);

        }

        [Test]
        public void GetFirstCalendarDate_Sunday_FirstDayMonday_ReturnFirstMonday()
        {
            var date = BbDatePicker.GetFirstCalendarDate(new System.DateTime(2020, 4, 5), BBComponents.Enums.FirstWeekDays.Monday);

            Assert.AreEqual(3, date.Month);
            Assert.AreEqual(30, date.Day);

        }

        [Test]
        public void GetFirstCalendarDate_Monday_FirstDayMonday_ReturnFirstMonday()
        {
            var date = BbDatePicker.GetFirstCalendarDate(new System.DateTime(2020, 6, 1), BBComponents.Enums.FirstWeekDays.Monday);

            Assert.AreEqual(6, date.Month);
            Assert.AreEqual(1, date.Day);

        }

    }
}