using System.Globalization;
using EcommerceTest;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

class LazadaPage
{
    private readonly IWebDriver _driver;

    public LazadaPage(IWebDriver driver) => _driver = driver;

    public void Search(string keyword)
    {
        string url = $"https://www.lazada.vn/catalog/?q={Uri.EscapeDataString(keyword)}";
        _driver.Navigate().GoToUrl(url);
        Thread.Sleep(3000);
    }

    public List<Product> GetProducts()
    {
        var products = new List<Product>();
        var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));

        wait.Until(d => d.FindElements(By.CssSelector("div.Ms6aG")).Count > 0);
        var results = _driver.FindElements(By.CssSelector("div.Ms6aG"));

        //Console.WriteLine($"Số sản phẩm tìm thấy: {results.Count}");

        foreach (var result in results.Take(5))
        {
            try
            {
                var nameElement = result.FindElement(By.CssSelector("div.RfADt > a"));
                var name = nameElement.Text;

                var href = nameElement.GetAttribute("href");
                var link = href.StartsWith("http") ? href : "https:" + href;

                var priceElement = result.FindElement(By.CssSelector("div.aBrP0 > span.ooOxS"));
                var priceText = priceElement.Text.Replace("₫", "").Replace(".", "").Trim();

                if (decimal.TryParse(priceText, NumberStyles.Any, CultureInfo.InvariantCulture, out var price))
                {
                    products.Add(new Product
                    {
                        Website = "Lazada",
                        Name = name,
                        Price = price,
                        Link = link
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi khi lấy sản phẩm: " + ex.Message);
            }
        }

        return products;
    }

}
