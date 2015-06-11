MVC-Honeypot
============

Honeypot anti bot mechanism implementation in ASP.NET MVC

###What does it do
This mechanism allows you to detect bot posts from forms on website without using CAPTCHA and bother visitors to enter weird letter and numbers. 

In short, it more elegant and user friendly approach in detecting bot form posts. It is based on masking the real field with field that has some illogical name. 

When form is posted illogical named field holds actual data, and meaningful named field is a trap field. If meaningful named field value is set, that is proof that bot has filled out the form (this field should be not visible on the page, so that only bots can find it inspecting document structure)

###How does it work
The solution contains of three elements:
* HtmlHelper for rendering out the input text control with honeypot trap
* ActionFilterAttribute which validates request and marks request trap field
* Extension method HasHoneypotTrapped for HttpRequestBase returning boolean value whether honeypot trap is triggered

###How to use it
You can download the project and include in your solution as project or compiled dll. 
Another option is to install it with NuGet package manager.

[![ScreenShot](http://dejanstojanovic.net/media/23565/nuget-small.png)](https://www.nuget.org/packages/Mvc.Honeypot/)

```
PM> Install-Package Mvc.Honeypot
```

There are few staps you need to to do in order to enable honeypot trap on your form page.
* Add reference to Mvc.Hoheypot
* Add hobeypot field for the form field which will be used for the trap (usually it's an email field)
```cs
@Html.HoneyPotField("Email", Model.Email)
```
By default, helper will generate text field for user and hidden field for bot. 
```html
<input name="6D9A89AAA95B1B3BFD6C7C5A6D5535FF" type="text" id="6D9A89AAA95B1B3BFD6C7C5A6D5535FF" />
<input name="Email" type="hidden" id="Email" />
```
As bots are getting smarter and smarter they can start checking input type of the field. Helper enables you to change input types of both value field and honey pot field.
```cs
<style type="text/css">
    .masked
    {
        display:none;
    }
</style>
@Html.HoneyPotField("Email", Model.Email, null, HtmlHelpers.InputType.Text, "masked", HtmlHelpers.InputType.Email)
```
This will produce more confusing html for the bot but as you see you will have to use some css to hide trap field from the normal user
```html
<style type="text/css">
    .masked
    {
        display:none;
    }
</style>
<input name="6D9A89AAA95B1B3BFD6C7C5A6D5535FF" type="text" id="6D9A89AAA95B1B3BFD6C7C5A6D5535FF" />
<input name="Email" type="email" id="Email" class="masked" />
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
[HttpPost]
[HoneypotFilter("Email")]
public ActionResult PostForm(FormModel model)
{
    if (ModelState.IsValid && Request.HasHoneypotTrapped())
    {
        //Honeypot trap triggered, possible bot
    }
    else if (ModelState.IsValid)
    {
        //Regular user, valid fields
    }
    else
    {
        //Regular user, invalid fields
    }
}
```
###What to do whan you detect that honeypot is triggered
Usually when something is posted you show some thank you message and do something with posted data. In case of bot detection with honeypot you should not return any message different than normal post in your action. This will keep deceiving bot that data is successfully sent.

The only difference is that you will treat posed data differently than normal, ignore the data, log it somewhere, or mark as a bot post when storing.
