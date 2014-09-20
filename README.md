MVC-Honeypot
============

Honeypot implementation in ASP.NET MVC

###What does it do
This mechanism allows you to detect bot posts from forms on website without using CAPTCHA and bother visitors to eneter weird letter and numbers. 
In short, it more elegant and user frendly approach in detecting bot form posts. 
It is based on masking the real field fith field that has some illogical name. When form is posted illogical named field holds actual data, and meaningfull named field is a trap field. If meniningfull named field value is set, that is proof that bot has filled out the form (this field should be not visible on the page, so that only bots can find it inspecting docuemnt structure)

###How does it work
The solution contains of three elements:
* HtmlHelper for rendering out the input text control with honeypot trap
* ActionFilterAttribute which validates request and marks request trap field
* Extension method HasHoneypotTrapped for HttpRequestBase returning boolean value whether honeypot trap is triggered

###How to use it
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
