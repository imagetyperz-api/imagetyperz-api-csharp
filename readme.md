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
string captcha_text = i.solve_captcha("captcha.jpg");
Console.WriteLine(string.format("Captcha text: {0}", captcha_text));
```
2nd argument is a boolean which represents if captcha is case sensitive

**URL instead of captcha image**
``` csharp
string captcha_text = i.solve_captcha("http://abc.com/captcha.jpg", false);
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
//d.Add("proxy", "126.45.34.53:123"); // or with auth 126.45.34.53:123:user:pass - optional
//d.Add("user_agent", "Your user agent"); // optional
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

