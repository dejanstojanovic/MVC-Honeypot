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
There are few staps you need to to do in order to enable honeypot trap on your form page.
* Add reference to Mvc.Hoheypot
* Add hobeypot field for the form field which will be used for the trap (usually it'n an email field)
```razor
@Html.HoneyPotField("Email", Model.Email)
```
* Add a filter with honeypot fields on the controller action
```cs
[HttpPost]
[HoneypotFilter("Email")]
public ActionResult PostForm(FormModel model)
{
    //Action logic
}
```
###How to know if trap was triggered
In your post form action you should do a check similar to the following
```cs
if (ModelState.IsValid && Request.HasHoneypotTrapped())
{
    //Pretend you sent something
}
else if (ModelState.IsValid)
{
    //Regular user, valid fields
}
else
{
    //Regular user, invalid fields
}
```
