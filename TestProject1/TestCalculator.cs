using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using NUnit.Framework;
using System;

namespace TestProject1
{
    [TestFixture]
    public class TestCalculator
    {
        private IWebDriver driver;
        private WebDriverWait wait;

        [SetUp]
        public void SetUp()
        {
            var options = new ChromeOptions();

            options.AddArgument("--headless=new");
            options.AddArgument("--window-size=1920,1080");
            options.AddArgument("--disable-gpu");

            driver = new ChromeDriver(options);
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            driver.Navigate().GoToUrl("https://testsheepnz.github.io/BasicCalculator.html");

            wait.Until(d => d.FindElement(By.Id("number1Field")).Displayed);
        }

        [TearDown]
        public void TearDown()
        {
            if (driver != null)
            {
                driver.Quit();
                driver.Dispose();
            }
        }

        public void PerformCalculation(
            string firstNumber,
            string operation,
            string secondNumber,
            string expectedResult,
            string expectedError)
        {
            var textBoxFirstNum = driver.FindElement(By.Id("number1Field"));
            var textBoxSecondNum = driver.FindElement(By.Id("number2Field"));
            var dropDownOperation = driver.FindElement(By.Id("selectOperationDropdown"));
            var calcBtn = driver.FindElement(By.Id("calculateButton"));
            var clearBtn = driver.FindElement(By.Id("clearButton"));

            clearBtn.Click();

            textBoxFirstNum.Clear();
            textBoxSecondNum.Clear();

            if (!string.IsNullOrEmpty(firstNumber))
            {
                textBoxFirstNum.SendKeys(firstNumber);
            }

            if (!string.IsNullOrEmpty(secondNumber))
            {
                textBoxSecondNum.SendKeys(secondNumber);
            }

            if (!string.IsNullOrEmpty(operation))
            {
                new SelectElement(dropDownOperation).SelectByText(operation);
            }

            calcBtn.Click();

            if (!string.IsNullOrEmpty(expectedError))
            {
                wait.Until(d => d.FindElement(By.Id("errorMsgField")).Text.Contains(expectedError));

                var actualError = driver.FindElement(By.Id("errorMsgField")).Text;

                Assert.That(actualError, Does.Contain(expectedError));
            }
            else
            {
                wait.Until(d => d.FindElement(By.Id("numberAnswerField")).GetAttribute("value") == expectedResult);

                var actualResult = driver.FindElement(By.Id("numberAnswerField")).GetAttribute("value");

                Assert.That(actualResult, Is.EqualTo(expectedResult));
            }
        }

        [Test]
        [TestCase("5", "Add", "10", "15", "")]
        [TestCase("3.5", "Subtract", "1.2", "2.3", "")]
        [TestCase("2e2", "Multiply", "1.5", "300", "")]
        [TestCase("5", "Divide", "0", "", "Divide by zero error!")]
        [TestCase("invalid", "Add", "10", "", "Number 1 is not a number")]
        public void TestNumberCalculator(
            string firstNumber,
            string operation,
            string secondNumber,
            string expectedResult,
            string expectedError)
        {
            PerformCalculation(firstNumber, operation, secondNumber, expectedResult, expectedError);
        }
    }
}