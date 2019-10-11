imagetyperzapi - Imagetyperz API wrapper
=========================================
imagetyperzapi is a super easy to use bypass C# captcha API wrapper for imagetyperz.com captcha service

## Installation

    Install-Package imagetyperz-api-latest

or

    git clone https://github.com/imagetyperz-api/imagetyperz-api-csharp

## How to use?

Add the library as a reference to your project, and use the namespace

``` csharp
using ImageTypers;
```
Set access_token or username and password (legacy) for authentication

``` csharp
string access_key = "your_access_key";
ImageTyperzAPI i = new ImageTyperzAPI(access_token);
```
legacy authentication, will get deprecated at some point
``` csharp
i.set_user_and_password("your_username", "your_password");
```
Once you've set your authentication details, you can start using the API

**Get balance**

``` csharp
string balance = i.account_balance();
Console.WriteLine(string.format("Balance: {0}", balance));
```

## Image captcha

### Submit image captcha

``` csharp
// optional parameters
Dictionary<string, string> image_params = new Dictionary<string, string>();
image_params.Add("iscase", "true");         // case sensitive captcha
image_params.Add("isphrase", "true");       // text contains at least one space (phrase)
image_params.Add("ismath", "true");         // instructs worker that a math captcha has to be solved
image_params.Add("alphanumeric", "1");      // 1 - digits only, 2 - letters only
image_params.Add("minlength", "2");         // captcha text length (minimum)
image_params.Add("maxlength", "5");         // captcha text length (maximum)

string captcha_text = i.solve_captcha("captcha.jpg", image_params);
Console.WriteLine(string.format("Captcha text: {0}", captcha_text));
```

**URL instead of captcha image**
``` csharp
string captcha_text = i.solve_captcha("http://abc.com/captcha.jpg", image_params);
```
**OBS:** URL instead of image file path works when you're authenticated with access_key.
 For those that are still using username & password, retrieve your access_key from 
 imagetyperz.com

## reCAPTCHA
 
### Submit recaptcha details

For recaptcha submission there are two things that are required, and some optional parameters
- page_url
- site_key
- type - can be one of this 3 values: `1` - normal, `2` - invisible, `3` - v3 (it's optional, defaults to `1`)
- v3_min_score - minimum score to target for v3 recaptcha `- optional`
- v3_action - action parameter to use for v3 recaptcha `- optional`
- proxy - proxy to use when solving recaptcha, eg. `12.34.56.78:1234` or `12.34.56.78:1234:user:password` `- optional`
- user_agent - User-Agent to use when solving recaptcha `- optional` 

``` csharp
Dictionary<string, string> d = new Dictionary<string, string>();
d.Add("page_url", "example.com");
d.Add("sitekey", "6FDD-s34g3321-3234fgfh23rv32fgtrrsv3c");
d.Add("type", "3");                
d.Add("v3_min_score", "0.1");       
d.Add("v3_action", "homepage");    
d.Add("user_agent", "Your user agent");

string captcha_id = i.submit_recaptcha(d);
```
This method returns a captchaID. This ID will be used next, to retrieve the g-response, once workers have
completed the captcha. This takes somewhere between 10-80 seconds.

### Retrieve captcha response

Once you have the captchaID, you check for it's progress, and later on retrieve the gresponse.

The ***in_progress(captcha_id)*** method will tell you if captcha is still being decoded by workers.
Once it's no longer in progress, you can retrieve the gresponse with ***retrieve_recaptcha(captcha_id)***

``` csharp
while(i.in_progress(captcha_id))
{
    System.Threading.Thread.Sleep(10000);      // sleep for 10 seconds
}
// completed at this point
string recaptcha_response = i.retrieve_captcha(captcha_id);
```

## GeeTest

GeeTest is a captcha that requires 3 parameters to be solved:
- domain
- challenge
- gt

The response of this captcha after completion are 3 codes:
- challenge
- validate
- seccode

### Submit GeeTest
```csharp
Dictionary<string, string> dg = new Dictionary<string, string>();
dg.Add("domain", "geetest captcha domain");
dg.Add("challenge", "geetest captcha challenge");
dg.Add("gt", "geetest captcha gt");
//dg.Add("proxy", "126.45.34.53:123"); // or with auth 126.45.34.53:123:user:pass - optional
//dg.Add("user_agent", "Your user agent"); // optional
string geetest_id = i.submit_geetest(dg);
```

Just like reCAPTCHA, you'll receive a captchaID.
Using the ID, you'll be able to retrieve 3 codes after completion.

Optionally, you can send proxy and user_agent along.

### Retrieve GeeTest codes
```csharp
Console.WriteLine(string.Format("Geetest captcha id: {0}", geetest_id));
Console.WriteLine("Waiting for geetest captcha to be solved ...");

// check for completion
while (i.in_progress(geetest_id)) System.Threading.Thread.Sleep(10000);      // sleep for 10 seconds and retry

// we got a response at this point
Dictionary<string, string> geetest_response = i.retrieve_geetest(geetest_id);     // get the response
Console.WriteLine(string.Format("Geetest response: {0} - {1} - {2}", geetest_response["challenge"], 
	geetest_response["validate"], geetest_response["seccode"]));
```

Response will be a string dictionary, which looks like this: `{'challenge': '...', 'validate': '...', 'seccode': '...'}`

## Capy & hCaptcha

This are two different captcha types, but both are similar to reCAPTCHA. They require a `pageurl` and `sitekey` for solving. hCaptcha is the newest one.

### IMPORTANT
For this two captcha types, the reCAPTCHA methods are used (explained above), except that there's one small difference.

The `pageurl` parameter should have at the end of it `--capy` added for Capy captcha and `--hcaptcha` for the hCaptcha. This instructs our system it's a capy or hCaptcha. It will be changed in the future, to have it's own endpoints.

For example, if you were to have the `pageurl` = `https://mysite.com` you would send it as `https://mysite.com--capy` if it's capy or `https://mysite.com--hcaptcha` for hCaptcha. Both require a sitekey too, which is sent as reCAPTCHA sitekey, and response is received as reCAPTCHA response, once again using the reCAPTCHA method.

#### Example
``` csharp
Dictionary<string, string> d = new Dictionary<string, string>();
d.Add("page_url", "example.com--capy");		   // or `domain.com--hcaptcha`
d.Add("sitekey", "sitekey_here");

// submit
string captcha_id = i.submit_recaptcha(d);

// retrieve capy
while(i.in_progress(captcha_id))
{
    System.Threading.Thread.Sleep(10000);      // sleep for 10 seconds
}
// completed at this point
string solution = i.retrieve_captcha(captcha_id);
```

## Other methods/variables

**Affiliate id**

The constructor accepts a 2nd parameter, as the affiliate id.
``` csharp
ImageTyperzAPI i = new ImageTyperzAPI("your_token", "123");
```

**Get details of proxy for recaptcha**

In case you submitted the recaptcha with proxy, you can check the status of the proxy, if it was used or not,
and if not, what the reason was with the following:

``` csharp
Console.WriteLine(i.was_proxy_used(captcha_id));
```

**Set captcha bad**

When a captcha was solved wrong by our workers, you can notify the server with it's ID,
so we know something went wrong

``` csharp
i.set_captcha_bad(captcha_id);
```

## Examples
Check the com.example package

## Command-line client
For those that are looking for a command-line client for windows, check the cli project in the solution

## Binary
If you don't want to compile your own library, you can check the binary folder for a compiled version.
**Keep in mind** that this might not be the latest version with every release.
Binary folder contains both library and command line tool for windows

## License
API library is licensed under the MIT License

## More information
More details about the server-side API can be found [here](http://imagetyperz.com)


<sup><sub>captcha, bypasscaptcha, decaptcher, decaptcha, 2captcha, deathbycaptcha, anticaptcha, 
bypassrecaptchav2, bypassnocaptcharecaptcha, bypassinvisiblerecaptcha, captchaservicesforrecaptchav2, 
recaptchav2captchasolver, googlerecaptchasolver, recaptchasolverpython, recaptchabypassscript</sup></sub>

