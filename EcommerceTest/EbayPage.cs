using EcommerceTest;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using System.Globalization;
using SeleniumExtras.WaitHelpers;

public class EbayPage
{
    private readonly IWebDriver _driver;
    private readonly WebDriverWait _wait;

    private const string Url = "https://www.ebay.com/sch/i.html?_nkw=iphone+16";

    private readonly By searchBoxLocator = By.Id("gh-ac");
    private readonly By productListLocator = By.CssSelector("li.s-item");
    private readonly By productNameLocator = By.CssSelector("div.s-item__title > span[role='heading']");
    private readonly By productLinkLocator = By.CssSelector("a.s-item__link");
    private readonly By productPriceLocator = By.CssSelector("span.s-item__price");

    public EbayPage(IWebDriver driver)
    {
        _driver = driver;
        _wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
    }

    public void Open()
    {
        _driver.Navigate().GoToUrl(Url);
        _wait.Until(ExpectedConditions.ElementIsVisible(searchBoxLocator));
    }

    public void Search(string keyword)
    {
        var searchBox = _wait.Until(d => d.FindElement(searchBoxLocator));
        searchBox.Clear();
        searchBox.SendKeys(keyword);
        searchBox.Submit();
        _wait.Until(d => d.FindElements(productListLocator).Count > 0);
    }

    public List<Product> GetProducts()
    {
        var products = new List<Product>();
        var results = _driver.FindElements(productListLocator);

        foreach (var result in results)
        {
            try
            {
                var nameElement = result.FindElement(productNameLocator);
                var name = nameElement.Text.Trim();

                if (string.IsNullOrWhiteSpace(name) || name.ToLower().Contains("shop on ebay"))
                    continue;

                var link = result.FindElement(productLinkLocator).GetAttribute("href");
                var priceText = result.FindElement(productPriceLocator).Text.Trim();

                var price = ParsePrice(priceText);
                if (price.HasValue)
                {
                    products.Add(new Product
                    {
                        Website = "eBay",
                        Name = name,
                        Price = price.Value,
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

    private decimal? ParsePrice(string priceText)
    {
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
            return price;

        return null;
    }
}
