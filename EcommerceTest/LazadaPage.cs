using EcommerceTest;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using System.Globalization;

class LazadaPage
{
    private readonly IWebDriver _driver;
    private readonly WebDriverWait _wait;

    // Locator nên khai báo ở đây
    private readonly By productListLocator = By.CssSelector("div.Ms6aG");
    private readonly By productNameLocator = By.CssSelector("div.RfADt > a");
    private readonly By productPriceLocator = By.CssSelector("div.aBrP0 > span.ooOxS");

    public LazadaPage(IWebDriver driver)
    {
        _driver = driver;
        _wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
    }

    public void Search(string keyword)
    {
        string url = $"https://www.lazada.vn/catalog/?q={Uri.EscapeDataString(keyword)}";
        _driver.Navigate().GoToUrl(url);
        _wait.Until(d => d.FindElements(productListLocator).Count > 0);
    }

    public List<Product> GetProducts()
    {
        var products = new List<Product>();

        var results = _driver.FindElements(productListLocator);

        foreach (var result in results.Take(5))
        {
            try
            {
                var nameElement = result.FindElement(productNameLocator);
                var name = nameElement.Text;
                var href = nameElement.GetAttribute("href");
                var link = href.StartsWith("http") ? href : "https:" + href;

                var priceElement = result.FindElement(productPriceLocator);
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
