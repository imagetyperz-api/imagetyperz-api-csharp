imagetyperz-api-csharp - Imagetyperz API wrapper
=========================================

imagetyperzapi is a super easy to use bypass captcha API wrapper for imagetyperz.com captcha service

## Installation
    git clone https://github.com/imagetyperz-api/imagetyperz-api-csharp

## Usage

Simply require the module, set the auth details and start using the captcha service:

``` csharp
using ImageTypers;
```
Set access_token for authentication:

``` csharp
ImageTypersAPI i = new ImageTypersAPI('your_access_token');
```

Once you've set your authentication details, you can start using the API.

**Get balance**

``` csharp
string balance = i.account_balance();
Console.WriteLine(string.Format("Balance: {0}", balance));
```

## Solving
For solving a captcha, it's a two step process:
- **submit captcha** details - returns an ID
- use ID to check it's progress - and **get solution** when solved.

Each captcha type has it's own submission method.

For getting the response, same method is used for all types.


### Image captcha

``` csharp
Dictionary<string, string> image_params = new Dictionary<string, string>();
// image_params.Add("iscase", "true");         // case sensitive captcha
// image_params.Add("isphrase", "true");       // text contains at least one space (phrase)
// image_params.Add("ismath", "true");         // instructs worker that a math captcha has to be solved
// image_params.Add("alphanumeric", "1");      // 1 - digits only, 2 - letters only
// image_params.Add("minlength", "2");         // captcha text length (minimum)
// image_params.Add("maxlength", "5");         // captcha text length (maximum)
string captcha_id = i.submit_image("captcha.jpg", image_params);
```
ID received is used to retrieve solution when solved.

**Observation**
It works with URL instead of image file too, but authentication has to be done using token.

### reCAPTCHA

For recaptcha submission there are two things that are required.
- page_url (**required**)
- site_key (**required**)
- type (optional, defaults to 1 if not given)
  - `1` - v2
  - `2` - invisible
  - `3` - v3
  - `4` - enterprise v2
  - `5` - enterprise v3
- domain - used in loading of reCAPTCHA interface, default: `www.google.com` (alternative: `recaptcha.net`) - `optional`
- v3_min_score - minimum score to target for v3 recaptcha `- optional`
- v3_action - action parameter to use for v3 recaptcha `- optional`
- proxy - proxy to use when solving recaptcha, eg. `12.34.56.78:1234` or `12.34.56.78:1234:user:password` `- optional`
- user_agent - useragent to use when solve recaptcha `- optional` 
- data-s - extra parameter used in solving recaptcha `- optional`
- cookie_input - cookies used in solving reCAPTCHA - `- optional`

``` csharp
Dictionary<string, string> d = new Dictionary<string, string>();
d.Add("page_url", "https://your-site.com");
d.Add("sitekey", "7LrGJmcUABBAALFtIb_FxC0LXm_GwOLyJAfbbUCL");
// d.Add("type", "1");                 // optional
// d.Add("domain", "www.google.com");  // used in loading reCAPTCHA interface, default: www.google.com (alternative: recaptcha.net) - optional
// d.Add("v3_min_score", "0.1");       // optional
// d.Add("v3_action", "homepage");     // optional
// d.Add("proxy", "126.45.34.53:123"); // or with auth 126.45.34.53:123:user:pass - optional
// d.Add("user_agent", "Your user agent"); // optional
// d.Add("data-s", "recaptcha data-s value"); // optional
// d.Add("cookie_input", "a=b;c=d"); // optional
string captcha_id = i.submit_recaptcha(d);
```
ID will be used to retrieve the g-response, once workers have 
completed the captcha. This takes somewhere between 10-80 seconds. 

Check **Retrieve response** 

### GeeTest

GeeTest is a captcha that requires 3 parameters to be solved:
- domain
- challenge
- gt
- api_server (optional)

The response of this captcha after completion are 3 codes:
- challenge
- validate
- seccode

**Important**
This captcha requires a **unique** challenge to be sent along with each captcha.

```csharp
Dictionary<string, string> d = new Dictionary<string, string>();
d.Add("domain", "https://your-site.com");
d.Add("challenge", "eea8d7d1bd1a933d72a9eda8af6d15d3");
d.Add("gt", "1a761081b1114c388092c8e2fd7f58bc");
// d.Add("api_server", "api.geetest.com"); // optional
// d.Add("proxy", "126.45.34.53:123"); // or with auth 126.45.34.53:123:user:pass - optional
// d.Add("user_agent", "Your user agent"); // optional
string captcha_id = i.submit_geetest(d);
```

Optionally, you can send proxy and user_agent along.


### GeeTestV4

GeeTesV4 is a new version of captcha from geetest that requires 2 parameters to be solved:

- domain
- geetestid (captchaID) - gather this from HTML source of page with captcha, inside the `<script>` tag you'll find a link that looks like this: https://i.imgur.com/XcZd47y.png

The response of this captcha after completion are 5 parameters:

- captcha_id
- lot_number
- pass_token
- gen_time
- captcha_output

```csharp
Dictionary<string, string> d = new Dictionary<string, string>();
d.Add("domain", "https://example.com");
d.Add("geetestid", "647f5ed2ed8acb4be36784e01556bb71");
//d.Add("proxy", "126.45.34.53:123"); // or with auth 126.45.34.53:123:user:pass - optional
//d.Add("user_agent", "Your user agent"); // optional

string captcha_id = i.submit_geetest_v4(d);
```

Optionally, you can send proxy and user_agent along.


### hCaptcha

Requires page_url and sitekey

```csharp
Dictionary<string, string> d = new Dictionary<string, string>();
d.Add("page_url", "https://your-site.com");
d.Add("sitekey", "8c7062c7-cae6-4e12-96fb-303fbec7fe4f");

// d.Add("invisible", "1");              // if captcha is invisible - optional

// domain used in loading of hcaptcha interface, default: hcaptcha.com - optional
// d.Add("domain", "hcaptcha.com");

// extra parameters, useful for enterprise
// submit userAgent from requests too, when this is used
// d.Add("HcaptchaEnterprise", "{\"rqdata\": \"value taken from web requests\"}");

// d.Add("proxy", "126.45.34.53:123"); // or with auth 126.45.34.53:123:user:pass - optional
// d.Add("user_agent", "Your user agent"); // optional
string captcha_id = i.submit_hcaptcha(d);
```

### Capy

Requires page_url and sitekey

```csharp
Dictionary<string, string> d = new Dictionary<string, string>();
d.Add("page_url", "https://your-site.com");
d.Add("sitekey", "Fme6hZLjuCRMMC3uh15F52D3uNms5c");
// d.Add("proxy", "126.45.34.53:123"); // or with auth 126.45.34.53:123:user:pass - optional
// d.Add("user_agent", "Your user agent"); // optional
string captcha_id = i.submit_capy(d);
```

### Tiktok

Requires page_url cookie_input

```csharp
Dictionary<string, string> d = new Dictionary<string, string>();
d.Add("page_url", "https://tiktok.com");
// make sure `s_v_web_id` cookie is present
d.Add("cookie_input", "s_v_web_id:verify_kd6243o_fd449FX_FDGG_1x8E_8NiQ_fgrg9FEIJ3f;tt_webid:612465623570154;tt_webid_v2:7679206562717014313;SLARDAR_WEB_ID:d0314f-ce16-5e16-a066-71f19df1545f;");
// d.Add("proxy", "126.45.34.53:123"); // or with auth 126.45.34.53:123:user:pass - optional
// d.Add("user_agent", "Your user agent"); // optional
string captcha_id = i.submit_tiktok(d);
```

### FunCaptcha

Requires page_url, sitekey and s_url

```csharp
Dictionary<string, string> d = new Dictionary<string, string>();
d.Add("page_url", "https://your-site.com");
d.Add("sitekey", "11111111-1111-1111-1111-111111111111");
d.Add("s_url", "https://api.arkoselabs.com");
//d.Add("data", "{\"a\": \"b\"}");   // optional, extra funcaptcha data in JSON format
//d.Add("proxy", "126.45.34.53:123"); // or with auth 126.45.34.53:123:user:pass - optional
//d.Add("user_agent", "Your user agent"); // optional
string captcha_id = i.submit_funcaptcha(d);
```

### Turnstile (Cloudflare)

Requires page_url, sitekey

```csharp
Dictionary<string, string> d = new Dictionary<string, string>();
d.Add("page_url", "https://your-site.com");
d.Add("sitekey", "0x4ABBBBAABrfvW5vKbx11FZ");
//d.Add("domain", "challenges.cloudflare.com"); // domain used in loading turnstile interface, default: challenges.cloudflare.com - optional
//d.Add("action", "homepage");                  // used in loading turnstile interface, similar to reCAPTCHA - optional
//d.Add("cdata", "your cdata");                 // used in loading turnstile interface - optional
//d.Add("proxy", "126.45.34.53:123"); // or with auth 126.45.34.53:123:user:pass - optional
//d.Add("user_agent", "Your user agent"); // optional

string captcha_id = i.submit_turnstile(d);
```

### Task

Requires template_name, page_url and usually variables

```csharp
Dictionary<string, string> d = new Dictionary<string, string>();
d.Add("template_name", "Login test page");
d.Add("page_url", "https://imagetyperz.net/automation/login");
d.Add("variables", "{\"username\": \"abc\", \"password\": \"paZZW0rd\"}");
//d.Add("proxy", "126.45.34.53:123"); // or with auth 126.45.34.53:123:user:pass - optional
//d.Add("user_agent", "Your user agent"); // optional
string captcha_id = i.submit_task(d);
```

#### Task pushVariable
Update a variable value while task is running. Useful when dealing with 2FA authentication.

When template reaches an action that uses a variable which wasn't provided with the submission of the task,
task (while running on worker machine) will wait for variable to be updated through push.

You can use the pushVariables method as many times as you need, even overwriting previously set variables.
```java
String code = "24323";
i.task_push_variables(captcha_id, "{\"twofactor_code\": \"" + code + "\"}");
```

## Retrieve response

Regardless of the captcha type (and method) used in submission of the captcha, this method is used
right after to check for it's solving status and also get the response once solved.

It requires one parameter, that's the **captcha ID** gathered from first step.

```python
response = i.retrieve_response(captcha_id);
```

```csharp
string captcha_id = i.submit_recaptcha(d); // works with any captcha type submitted
Console.WriteLine("Waiting for captcha to be solved...");
Dictionary<string, string> response = null;
while (response == null)
{
    System.Threading.Thread.Sleep(10000);       // sleep for 10 secons before checking for response
    response = i.retrieve_response(captcha_id);
}
ImageTypers.Utils.print_response(response);
```
The response is a JSON object that looks like this:
```json
{
  "CaptchaId": 176707908, 
  "Response": "03AGdBq24PBCbwiDRaS_MJ7Z...mYXMPiDwWUyEOsYpo97CZ3tVmWzrB", 
  "Cookie_OutPut": "", 
  "Proxy_reason": "",
  "Status": "Solved"
}
```

## Other methods/variables

**Affiliate id**

The constructor accepts a 2nd parameter, as the affiliate id. 
``` csharp
ImagetypersAPI i = new ImagetypersAPI(username, password, 123);     // init with affiliate id
```

**Requests timeout**

Specify timeout for requests made to API
``` csharp
i.set_timeout(10);    // set timeout to 10 seconds
```

**Set captcha bad**

When a captcha was solved wrong by our workers, you can notify the server with it's ID,
so we know something went wrong.

``` python
i.set_captcha_bad(captcha_id);
```

## Examples
Check `example/captcha` folder for examples, for each type of captcha.

## License
API library is licensed under the MIT License

## More information
More details about the server-side API can be found [here](http://imagetyperz.com)

<sup><sub>captcha, bypasscaptcha, decaptcher, decaptcha, 2captcha, deathbycaptcha, anticaptcha, 
bypassrecaptchav2, bypassnocaptcharecaptcha, bypassinvisiblerecaptcha, captchaservicesforrecaptchav2, 
recaptchav2captchasolver, googlerecaptchasolver, recaptchasolver-csharp, recaptchabypassscript</sup></sub>

