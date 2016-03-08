
##HQF.Tutorial.OAuth2.ResourceServer

```
Install-Package Microsoft.AspNet.WebApi -Version 5.2.2
Install-Package Microsoft.AspNet.WebApi.Owin -Version 5.2.2
Install-Package Microsoft.Owin.Host.SystemWeb -Version 3.0.0
Install-Package Microsoft.Owin.Cors -Version 3.0.0
Install-Package Microsoft.Owin.Security.OAuth -Version 2.1.0
```

Important note:
> In the initial post I was using package ¡°Microsoft.Owin.Security.OAuth¡± version ¡°3.0.0¡± which differs from the version used in the Authorization Server version ¡°2.1.0¡±, there was a bug in my solution when I was using 2 different versions, basically the bug happens when we send an expired token to the Resource Server, the result for this that Resource Server accepts this token even if it is expired. I¡¯ve noticed that the properties for the Authentication ticket are null and there is no expiry date. To fix this issue we need to unify the assembly version ¡°Microsoft.Owin.Security.OAuth¡± between the Authorization Server and the Resource Server, I believe there is breaking changes between those 2 versions that¡¯s why ticket properties are not de-crypted and de-serialized correctly.  Thanks for Ashish Verma for notifying me about this.