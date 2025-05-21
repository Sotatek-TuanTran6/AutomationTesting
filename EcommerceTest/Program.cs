using OpenQA.Selenium.Firefox;

class TestRunner
{
    static void Main()
    {
        var options = new FirefoxOptions();

        using var driver = new FirefoxDriver(options);
        driver.Manage().Window.Maximize();

        var ebay = new EbayPage(driver);
        ebay.Open();
        ebay.Search("iPhone 16");
        var ebayProducts = ebay.GetProducts();

        var lazada = new LazadaPage(driver);
        lazada.Search("iPhone 16");
        var lazadaProducts = lazada.GetProducts();

        var combined = lazadaProducts.Concat(ebayProducts)
            .OrderBy(p => p.Price)
            .ToList();

        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.WriteLine("Kết quả tìm kiếm iPhone 16 trên Lazada và eBay (sắp xếp theo giá tăng dần):");
        //Console.WriteLine("Website\nTên sản phẩm\nGiá\nLink\n");

        foreach (var p in combined)
        {
            Console.WriteLine($"{p.Website}\n{p.Name}\n{p.Price:N0}₫\n{p.Link}\n");
        }

        driver.Quit();
    }
}
