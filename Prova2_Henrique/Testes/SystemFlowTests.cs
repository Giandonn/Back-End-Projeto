using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using Xunit;
using System;
using System.Data.SqlClient;
using Microsoft.Data.SqlClient;

namespace Testes
{
    public class SystemFlowTests : IDisposable
    {
        private readonly IWebDriver driver;
        private readonly string connectionString = "Server=localhost;Database=YourDatabase;User Id=YourUser;Password=YourPassword;";

        public SystemFlowTests()
        {
            driver = new ChromeDriver();
        }

        [Fact]
        public void CompleteUserFlowTest()
        {
            string testEmail = $"emaildetest@gmail.com";
            string testPassword = "Senha123!";
            string newName = "Jorjinho do tenis";

            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            driver.Navigate().GoToUrl("http://localhost/register");
            wait.Until(driver => driver.FindElement(By.Id("email"))).SendKeys(testEmail);
            driver.FindElement(By.Id("password")).SendKeys(testPassword);
            driver.FindElement(By.Id("confirmPassword")).SendKeys(testPassword);
            driver.FindElement(By.Id("registerButton")).Click();

            driver.Navigate().GoToUrl("http://localhost/login");
            wait.Until(driver => driver.FindElement(By.Id("email"))).SendKeys(testEmail);
            driver.FindElement(By.Id("password")).SendKeys(testPassword);
            driver.FindElement(By.Id("loginButton")).Click();

            driver.Navigate().GoToUrl("http://localhost/profile");
            wait.Until(driver => driver.FindElement(By.Id("name"))).Clear();
            driver.FindElement(By.Id("name")).SendKeys(newName);
            driver.FindElement(By.Id("saveProfileButton")).Click();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT Name FROM Users WHERE Email = @Email", connection);
                command.Parameters.AddWithValue("@Email", testEmail);
                string dbName = command.ExecuteScalar()?.ToString();

                Assert.Equal(newName, dbName);
            }
        }

        public void Dispose()
        {
            driver.Quit();
        }
    }
}
