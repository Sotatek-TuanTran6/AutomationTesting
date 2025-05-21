using System.Globalization;
using OpenQA.Selenium;
using EcommerceTest;

public class EbayPage
{
    private readonly IWebDriver _driver;
    private const string Url = "https://www.ebay.com/sch/i.html?_nkw=iphone+16";

    public EbayPage(IWebDriver driver) => _driver = driver;

    public void Open() => _driver.Navigate().GoToUrl(Url);

    public void Search(string keyword)
    {
        var searchBox = _driver.FindElement(By.Id("gh-ac"));
        searchBox.Clear();
        searchBox.SendKeys(keyword);
        searchBox.Submit();
    }

    public List<Product> GetProducts()
    {
        var products = new List<Product>();
        var results = _driver.FindElements(By.CssSelector("li.s-item"));

        foreach (var result in results)
        {
            try
            {
                var nameElement = result.FindElement(By.CssSelector("div.s-item__title > span[role='heading']"));
                var name = nameElement.Text.Trim();

                if (string.IsNullOrWhiteSpace(name) || name.ToLower().Contains("shop on ebay"))
                    continue;

                var link = result.FindElement(By.CssSelector("a.s-item__link")).GetAttribute("href");

                var priceText = result.FindElement(By.CssSelector("span.s-item__price")).Text.Trim();

                var priceCleanChars = new List<char>();
                foreach (var c in priceText)
                {
                    if (char.IsDigit(c) || c == '.' || c == ',')
                        priceCleanChars.Add(c);
                    else
                        break;
                }
                var priceClean = new string(priceCleanChars.ToArray()).Replace(",", "");

                if (decimal.TryParse(priceClean, NumberStyles.Any, CultureInfo.InvariantCulture, out var price))
                {
                    products.Add(new Product
                    {
                        Website = "eBay",
                        Name = name,
                        Price = price,
                        Link = link
                    });
                }

                if (products.Count == 5)
                    break;
            }
            catch (NoSuchElementException)
            {
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error parsing product: " + ex.Message);
            }
        }

        return products;
    }


}
