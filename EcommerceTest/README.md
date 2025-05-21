# Hướng dẫn cài đặt Selenium với Firefox (C#)

Dự án sử dụng **Selenium WebDriver** để tự động hóa trình duyệt **Firefox**, viết bằng ngôn ngữ **C# (.NET 6 trở lên)**.

---

##  Yêu cầu hệ thống

- Hệ điều hành: Windows 10+, macOS, hoặc Linux
- .NET SDK: .NET 6 trở lên
- Trình duyệt: Firefox
- Công cụ phát triển: Visual Studio 2022 hoặc Visual Studio Code

---

##  Cài đặt Selenium IDE (tùy chọn để record test)

Link cài đặt:  
https://addons.mozilla.org/en-US/firefox/addon/selenium-ide/

**Cách cài:**
1. Truy cập link trên bằng Firefox
2. Nhấn **Add to Firefox**
3. Cho phép cài đặt tiện ích

 ** Selenium IDE dùng để ghi lại thao tác, không phải để chạy test code .NET. Dùng trong mục đích hỗ trợ ghi test nhanh.

---

##  Các package cần thiết (qua NuGet)

Cài các package sau vào project C# của bạn:

 Selenium.WebDriver
 Selenium.Support
 Selenium.WebDriver.GeckoDriver
