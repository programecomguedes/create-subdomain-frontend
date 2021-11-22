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

Código responsável por criar um subdomínio a partir do Azure Automation:
```
Param($subdomain)

$connectionName = "AzureRunAsConnection"

# Get the connection "AzureRunAsConnection "
$servicePrincipalConnection=Get-AutomationConnection -Name $connectionName         

Connect-AzAccount `
    -ServicePrincipal `
    -TenantId $servicePrincipalConnection.TenantId `
    -ApplicationId $servicePrincipalConnection.ApplicationId `
    -CertificateThumbprint $servicePrincipalConnection.CertificateThumbprint

# Set DNS TXT Value
New-AzDnsRecordSet -ZoneName {{DNS_ZONE_NAME_HERE}} -ResourceGroupName {{RESOURCE_GROUP_HERE}} `
 -Name "asuid.$($subdomain)" -RecordType "txt" -Ttl 3600 `
 -DnsRecords (New-AzDnsRecordConfig -Value  "{{TXT_VALUE_HERE}}")
 
# Set DNS CNAME Value
New-AzDnsRecordSet -ZoneName {{DNS_ZONE_NAME_HERE}} -ResourceGroupName "{{RESOURCE_GROUP_HERE}}" `
 -Name $subdomain -RecordType "CNAME" -Ttl 3600 `
 -DnsRecords (New-AzDnsRecordConfig -cname "{{WEB_APP_NAME}}.azurewebsites.net")

# Get All custom domains of the web app
$webApp = Get-AzWebApp -ResourceGroupName {{RESOURCE_GROUP_HERE}} -Name {{WEB_APP_NAME}}
$hostNames = $webApp.HostNames 

# Add a new custom domain
$hostNames.Add("$subdomain.{{DNS_ZONE_NAME_HERE}}")

# Set custom domains
Set-AzWebApp -Name {{WEB_APP_NAME}} `
 -ResourceGroupName {{RESOURCE_GROUP_HERE}} `
 -HostNames @($hostNames)
```

## Azure Logic App

<img src="https://user-images.githubusercontent.com/15362349/142265258-3601834f-22d3-4f81-af1d-bdd71a771d9f.png" width="500">

## Assista no YouTube:
- [Criando domínios customizados no Azure com .NET Core (Parte 1/2)](https://youtu.be/VJp3mKLPe8k)
- [Criando domínios customizados no Azure com .NET Core (Parte 2/2)](https://youtu.be/xzbxJHMiD2Y)
