# Invoice Designer
With Invoice Designer, customizing your invoice print form is super easy. You can add, move around, and tweak elements in the layout, all without touching any code. Want to include extra info or some custom fields? No problem! 

## Technical Features
- No JavaScript/TypeScript/React/Angular/Vuejs and other frontend framework
- Completely free of npm dependencies
- Built with the power and simplicity of C#
- Enhanced UI/UX powered by [MudBlazor](https://github.com/MudBlazor/MudBlazor)
 
## Features
### Flexible Invoice Design
- Fully customizable PDF invoice templates using a drag-and-drop system.
### Multicomponent and Multi-Currency Support
- Support for managing unlimited companies, each operating with different currencies.
### Accounting Integration
- Built on a comprehensive accounting platform following GAAP principles.
- Editable chart of accounts for complete customization.
- Editable templates for double-entry bookkeeping of each accounting document.
- Double-entry bookkeeping system with configurable data export to third-party accounting systems.
- Tax management capabilities.
### Reports
- Trial Balance
- Customer Debit
### Security and Access Management
- Advanced user access control for managing permissions.
### Connectivity
- Works locally, over LAN, or via the internet.
### Localization
- Supported languages: English, Čeština, España, Deutsch, Français, Italiano, Português, Polski, Русский.


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
<p align="center">
  <img src="screenshot5.png"  />
</p>
<p align="center">
  <img src="screenshot6.png"  />
</p>
<p align="center">
  <img src="screenshot7.png"  />
</p>

- Trial Balance: A report that shows the final balances of all general ledger accounts over a specific period. These balances reflect the cumulative impact of transactions based on the account type (e.g., assets, liabilities, revenue, expenses, equity)
<p align="center">
  <img src="screenshot8.png"  />
</p>

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