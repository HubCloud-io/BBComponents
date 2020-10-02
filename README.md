# BBComponents
Free components for Blazor created with Bootstrap css framework. [WASM Demo.](https://bbcomponentsdemo.z6.web.core.windows.net/)

# Disclaimer
This project is experimental and can be deeply refactored in future.

# Dependencies
This project depends on 
   * [Bootstrap  v4.3.1](https://getbootstrap.com/)
   * [Font Awesome 5.13.0](https://fontawesome.com) 

# Installation
You can install the package via the NuGet package manager just search for *HubCloud.BBComponents*. You can also install via powershell using the following command.

```powershell
Install-Package HubCloud.BBComponents
```

# Setup
You need to refer Bootstrap, Font Awesome and BBComponents CSS in your index.html (Blazor WebAssembly) or _Host.cshtml (Blazor Server).

```html
    <!-- Bootstrap styles -->
    <link href="css/bootstrap/bootstrap.min.css" rel="stylesheet" />
    <!-- Fontawesome -->
    <link href="lib/font-awesome/css/all.css" rel="stylesheet" />
    <!-- BBComponents styles -->
    <link href="/_content/HubCloud.BBComponents/styles.css" rel="stylesheet" />
```

# Available components
Right now there are available following components:
* Alert hub
* Confirm
* Date picker
* Number input
* Navs
* Select
* List group

