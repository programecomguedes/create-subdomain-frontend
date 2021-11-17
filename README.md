# Criação de Domínios Customizados no Azure + Registro.BR
### Azure Web App .NET Core + Azure Logic App + Azure Automation

Front-end para criação de subdomínios customizados no Portal Azure.

Pré-requisitos para utilização:
- Criar um DNS Zone no Azure
- Apontar DNZ Zone para o Registro.BR
- Criação de Web App no Azure
- Configuração de Web App para exibição a partir do domínio registrado em Registro.BR
- Criar um Azure Automation
- Criar um Logic App
- Editar método "CreateSubdomain" no "HomeController" com o endpoint responsável pela criação do subdomínio.

Gerar um Service Principal para autenticação no Azure Automation:
```
az ad sp create-for-rbac --name SubdomainServicePrincipalName
```

A execução do código acima deverá gerar um resultado semelhante ao que segue abaixo:
```
{
  "appId": "xxxxf1d7-xxxx-4e75-9dxx-xxxc6cd6xxxx",
  "displayName": "SubdomainServicePrincipalName",
  "name": "xxxxf1d7-xx5f-xx75-xxxx-0afc6cd6xxxx",
  "password": "XXXXBSkDAtX8.XxH7-XXXTnX.Rp7fXxXX",
  "tenant": "Xxxcf1XX-46XX-40e5-acXX-f23b4501XXxx"
}
```

Código responsável por criar um subdomínio a partir do Azure Automation:
```
Param($subdomain)

az login --service-principal -u "{{APP_ID_HERE}}" -p "{{PASSWORD_HERE}}" --tenant "{{TENANT_HERE}}"

az network dns record-set txt add-record --resource-group "{{RESOURCE_GROUP_HERE}}" --zone-name {{DOMAIN_HERE}} --record-set-name "asuid.$($subdomain)"--value "{{TXT_VALUE_HERE}}"

az network dns record-set cname set-record --resource-group "{{RESOURCE_GROUP_HERE}}" --zone-name {{DOMAIN_HERE}} --record-set-name $subdomain --cname {{WEBAPP_NAME_HERE}}.azurewebsites.net

az webapp config hostname add --webapp-name {{WEBAPP_NAME_HERE}} --resource-group "{{RESOURCE_GROUP_HERE}}" --hostname "$($subdomain).{{DOMAIN_HERE}}"
```

## Azure Logic App
Request Body JSON Schema do Http Request:
```
{
    "subdomain": "www"
}
```

Schema do Parse JSON:
```
{
    "properties": {
        "subdomain": {
            "type": "string"
        }
    },
    "type": "object"
}
```

Name do Initialize variable: **subdomain**

Type do do Initialize variable: **String**

Value do Initialize variable: **subdomain**
