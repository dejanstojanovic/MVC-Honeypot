MVC-Honeypot
============

Honeypot implementation in ASP.NET MVC

###What does it do
This mechanism allows you to detect bot posts from forms on website without using CAPTCHA and bother visitors to eneter weird letter and numbers. 
In short, it more elegant and user frendly approach in detecting bot form posts.

###How does it work
The solution contains of three elements:
* HtmlHelper for rendering out the input text control with honeypot trap
* ActionFilterAttribute which validates request and marks request trap field
* Extension method HasHoneypotTrapped for HttpRequestBase returning boolean value whether honeypot trap is triggered

###How to use it
