
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


###How Generate MachineKey?
from https://support.microsoft.com/en-us/kb/2915218#AppendixA
>Security warning

>There are many web sites that will generate a <machineKey> element for you with the click of a button. Never use a <machineKey> element that you obtained from one of these sites. It is impossible to know whether these keys were created securely or if they are being recorded to a secret database. You should only ever use <machineKey> configuration elements that you created yourself.

```
# Generates a <machineKey> element that can be copied + pasted into a Web.config file.
function Generate-MachineKey {
  [CmdletBinding()]
  param (
    [ValidateSet("AES", "DES", "3DES")]
    [string]$decryptionAlgorithm = 'AES',
    [ValidateSet("MD5", "SHA1", "HMACSHA256", "HMACSHA384", "HMACSHA512")]
    [string]$validationAlgorithm = 'HMACSHA256'
  )
  process {
    function BinaryToHex {
        [CmdLetBinding()]
        param($bytes)
        process {
            $builder = new-object System.Text.StringBuilder
            foreach ($b in $bytes) {
              $builder = $builder.AppendFormat([System.Globalization.CultureInfo]::InvariantCulture, "{0:X2}", $b)
            }
            $builder
        }
    }
    switch ($decryptionAlgorithm) {
      "AES" { $decryptionObject = new-object System.Security.Cryptography.AesCryptoServiceProvider }
      "DES" { $decryptionObject = new-object System.Security.Cryptography.DESCryptoServiceProvider }
      "3DES" { $decryptionObject = new-object System.Security.Cryptography.TripleDESCryptoServiceProvider }
    }
    $decryptionObject.GenerateKey()
    $decryptionKey = BinaryToHex($decryptionObject.Key)
    $decryptionObject.Dispose()
    switch ($validationAlgorithm) {
      "MD5" { $validationObject = new-object System.Security.Cryptography.HMACMD5 }
      "SHA1" { $validationObject = new-object System.Security.Cryptography.HMACSHA1 }
      "HMACSHA256" { $validationObject = new-object System.Security.Cryptography.HMACSHA256 }
      "HMACSHA385" { $validationObject = new-object System.Security.Cryptography.HMACSHA384 }
      "HMACSHA512" { $validationObject = new-object System.Security.Cryptography.HMACSHA512 }
    }
    $validationKey = BinaryToHex($validationObject.Key)
    $validationObject.Dispose()
    [string]::Format([System.Globalization.CultureInfo]::InvariantCulture,
      "<machineKey decryption=`"{0}`" decryptionKey=`"{1}`" validation=`"{2}`" validationKey=`"{3}`" />",
      $decryptionAlgorithm.ToUpperInvariant(), $decryptionKey,
      $validationAlgorithm.ToUpperInvariant(), $validationKey)
  }
}
```
or `ASP.NET 4.0` applications, you can just call Generate-MachineKey without parameters to generate a <machineKey> element as follows:
```
PS> Generate-MachineKey
<machineKey decryption="AES" decryptionKey="..." validation="HMACSHA256" validationKey="..." />
```
`ASP.NET 2.0 and 3.5` applications do not support HMACSHA256. Instead, you can specify SHA1 to generate a compatible <machineKey> element as follows:
```
PS> Generate-MachineKey -validation sha1
<machineKey decryption="AES" decryptionKey="..." validation="SHA1" validationKey="..." />
```
As soon as you have a <machineKey> element, you can put it in the Web.config file. The <machineKey> element is only valid in the Web.config file at the root of your application and is not valid at the subfolder level.
```
<configuration>
  <system.web>
    <machineKey ... />
  </system.web>
</configuration>
```
For a full list of supported algorithms, run help Generate-MachineKey from the Windows PowerShell prompt.

-----------------------------
Here I Just use the MachineKey:``


