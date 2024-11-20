# InvoiceDesigner
With InvoiceDesigner, customizing your invoice print form is super easy. You can add, move around, and tweak elements in the layout, all without touching any code. Want to include extra info or some custom fields? No problem! 

## Technical Features
 - No JavaScript/TypeScript/React/Angular/Vuejs and other frontend framework
 - No npm dependencies
 - C#
 - MudBlazor UI
 
## Screenshots
<p align="center">
  <img src="screenshot1.png"  />
</p>
<p align="center">
  <img src="screenshot2.png"  />
</p>
<p align="center">
  <img src="screenshot3.png"  />
</p>

## Features
- works locally/lan
- Customizable PDF invoice
- Manage your Clients
- Invoice Management
- Customer Management
- User access control for companies
- Built-in authorization system

## Development Environment
- [Microsoft Visual Studio 2022 Community Edition](https://visualstudio.microsoft.com/vs/community/)
- [QuestPDF](https://github.com/QuestPDF/QuestPDF)

## Config
### Change SecretKey!
**InvoiceDesigner\InvoiceDesigner.API\appsettings.json**
```json
  "JWTOption": {
    "SecretKey": "b3£O(27z4e=p^JRf(-&vm]k5f7$WRjpcAz{N&S", // Change this!
    "ExpiresHours": 8
  }
```

### Base Url REST API
**InvoiceDesigner\InvoiceDesigner.WebApp\appsettings.json**

```json
  "ApiSettings": {
    "BaseUrl": "https://localhost:7048/"
  }
``` 
## Launching
- create migration
```bash
dotnet ef migrations add AddInitialMigration --startup-project InvoiceDesigner.API --project InvoiceDesigner.Infrastructure
``` 
- update database   
```bash
dotnet ef database update --project InvoiceDesigner.Infrastructure --startup-project InvoiceDesigner.Api

``` 
Set multiple startup projects 

<p align="center">
  <img src="screenshot4.png"  />
</p>

- default login
  - user: admin
  - pass: admin

## Author
[Karel Kalata](https://github.com/karelkalata)


## License
InvoiceDesigner is licensed under the MIT license, an open-source software license.
Please read the license [QuestPDF](https://github.com/QuestPDF/QuestPDF/blob/main/LICENSE.md)