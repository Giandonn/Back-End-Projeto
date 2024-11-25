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
        private readonly string connectionString = "Server=(localdb)\\mssqllocaldb;Database=UsuarioDB;ConnectRetryCount=0;";

        public SystemFlowTests()
        {
            driver = new ChromeDriver();
        }

        [Fact]
        public void CompleteUserFlowTest()
        {
            string testEmail = "emaildetest@gmail.com";
            string testPassword = "Senha123!";
            string testNome = "Jorjinho";
            string testSobrenome = "do tenis";
            string testIdade = "2000-05-15"; 
            string testTelefone = "11987654321"; 
            string testCep = "12345678"; 
            string testEstado = "São Paulo";
            string testCidade = "São Paulo"; 
            string newName = "Jorjinho"; 

            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            driver.Navigate().GoToUrl("http://127.0.0.1:5502/cadastro.html");

            wait.Until(driver => driver.FindElement(By.Id("inputNome"))).SendKeys(testNome);
            wait.Until(driver => driver.FindElement(By.Id("inputSobrenome"))).SendKeys(testSobrenome);
            wait.Until(driver => driver.FindElement(By.Id("inputIdade"))).SendKeys(testIdade);
            wait.Until(driver => driver.FindElement(By.Id("inputTelefone"))).SendKeys(testTelefone);
            wait.Until(driver => driver.FindElement(By.Id("inputEmail"))).SendKeys(testEmail);
            wait.Until(driver => driver.FindElement(By.Id("inputPassword"))).SendKeys(testPassword);
            wait.Until(driver => driver.FindElement(By.Id("inputConfirmPassword"))).SendKeys(testPassword);
            wait.Until(driver => driver.FindElement(By.Id("inputCep"))).SendKeys(testCep);

            wait.Until(driver => driver.FindElement(By.Id("inputEstado"))).SendKeys(testEstado);
            wait.Until(driver => driver.FindElement(By.Id("inputCidade"))).SendKeys(testCidade);

            driver.FindElement(By.CssSelector("button[type='submit']")).Click();


            driver.Navigate().GoToUrl("http://127.0.0.1:5502/login.html");

            wait.Until(driver => driver.FindElement(By.Id("inputEmail"))).SendKeys(testEmail);
            wait.Until(driver => driver.FindElement(By.Id("inputPassword"))).SendKeys(testPassword);
            driver.FindElement(By.Id("loginbtn")).Click();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("SELECT Email FROM Usuarios WHERE Email = @Email", connection);
                    command.Parameters.AddWithValue("@Email", testEmail);
                    string dbName = command.ExecuteScalar()?.ToString();

                    Assert.Equal(testEmail, dbName);
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Erro ao executar a consulta: {ex.Message}");
                throw;  
            }

        }


        public void Dispose()
        {
            driver.Quit();
        }
    }
}
