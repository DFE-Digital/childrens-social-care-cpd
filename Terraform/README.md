## Requirements

| Name | Version |
|------|---------|
| <a name="requirement_azurerm"></a> [azurerm](#requirement\_azurerm) | 3.29.1 |

## Providers

| Name | Version |
|------|---------|
| <a name="provider_azurerm"></a> [azurerm](#provider\_azurerm) | 3.29.1 |

## Modules

No modules.

## Resources

| Name | Type |
|------|------|
| [azurerm_application_gateway.appgw](https://registry.terraform.io/providers/hashicorp/azurerm/3.29.1/docs/resources/application_gateway) | resource |
| [azurerm_linux_web_app.linux-web-app](https://registry.terraform.io/providers/hashicorp/azurerm/3.29.1/docs/resources/linux_web_app) | resource |
| [azurerm_network_interface.nic](https://registry.terraform.io/providers/hashicorp/azurerm/3.29.1/docs/resources/network_interface) | resource |
| [azurerm_network_interface_application_gateway_backend_address_pool_association.nic-assoc01](https://registry.terraform.io/providers/hashicorp/azurerm/3.29.1/docs/resources/network_interface_application_gateway_backend_address_pool_association) | resource |
| [azurerm_network_security_group.nsg](https://registry.terraform.io/providers/hashicorp/azurerm/3.29.1/docs/resources/network_security_group) | resource |
| [azurerm_network_security_rule.dev-whitelist-rules](https://registry.terraform.io/providers/hashicorp/azurerm/3.29.1/docs/resources/network_security_rule) | resource |
| [azurerm_network_security_rule.nsg-rule-03](https://registry.terraform.io/providers/hashicorp/azurerm/3.29.1/docs/resources/network_security_rule) | resource |
| [azurerm_network_security_rule.whitelist-rules](https://registry.terraform.io/providers/hashicorp/azurerm/3.29.1/docs/resources/network_security_rule) | resource |
| [azurerm_service_plan.service-plan](https://registry.terraform.io/providers/hashicorp/azurerm/3.29.1/docs/resources/service_plan) | resource |
| [azurerm_subnet.backend](https://registry.terraform.io/providers/hashicorp/azurerm/3.29.1/docs/resources/subnet) | resource |
| [azurerm_subnet.frontend](https://registry.terraform.io/providers/hashicorp/azurerm/3.29.1/docs/resources/subnet) | resource |
| [azurerm_subnet_network_security_group_association.blockall](https://registry.terraform.io/providers/hashicorp/azurerm/3.29.1/docs/resources/subnet_network_security_group_association) | resource |
| [azurerm_virtual_network.vnet1](https://registry.terraform.io/providers/hashicorp/azurerm/3.29.1/docs/resources/virtual_network) | resource |
| [azurerm_client_config.current](https://registry.terraform.io/providers/hashicorp/azurerm/3.29.1/docs/data-sources/client_config) | data source |
| [azurerm_key_vault.kv](https://registry.terraform.io/providers/hashicorp/azurerm/3.29.1/docs/data-sources/key_vault) | data source |
| [azurerm_key_vault_secret.dev-ips](https://registry.terraform.io/providers/hashicorp/azurerm/3.29.1/docs/data-sources/key_vault_secret) | data source |
| [azurerm_key_vault_secret.ips](https://registry.terraform.io/providers/hashicorp/azurerm/3.29.1/docs/data-sources/key_vault_secret) | data source |
| [azurerm_public_ip.pip1](https://registry.terraform.io/providers/hashicorp/azurerm/3.29.1/docs/data-sources/public_ip) | data source |
| [azurerm_resource_group.rg](https://registry.terraform.io/providers/hashicorp/azurerm/3.29.1/docs/data-sources/resource_group) | data source |

## Inputs

| Name | Description | Type | Default | Required |
|------|-------------|------|---------|:--------:|
| <a name="input_appgw_name"></a> [appgw\_name](#input\_appgw\_name) | Name of the Application Gateway | `map(string)` | <pre>{<br>  "Dev": "s185d01-csc-cpd-app-gateway",<br>  "Pre-Prod": "s185t01-csc-cpd-app-gateway",<br>  "Prod": "s185p01-csc-cpd-app-gateway",<br>  "Test": "s185d02-csc-cpd-app-gateway"<br>}</pre> | no |
| <a name="input_appgw_probe"></a> [appgw\_probe](#input\_appgw\_probe) | Name of App Gateway Health Probe | `map(string)` | <pre>{<br>  "Dev": "s185d01AGProbe",<br>  "Pre-Prod": "s185t01AGProbe",<br>  "Prod": "s185p01AGProbe",<br>  "Test": "s185d02AGProbe"<br>}</pre> | no |
| <a name="input_appgw_tier"></a> [appgw\_tier](#input\_appgw\_tier) | SKU for Application Gateway Tier | `map(string)` | <pre>{<br>  "Dev": "Standard_v2",<br>  "Pre-Prod": "Standard_v2",<br>  "Prod": "WAF_v2",<br>  "Test": "Standard_v2"<br>}</pre> | no |
| <a name="input_backend_address_pool_name"></a> [backend\_address\_pool\_name](#input\_backend\_address\_pool\_name) | Name of Backend Address Pool | `map(string)` | <pre>{<br>  "Dev": "s185d01BackendPool",<br>  "Pre-Prod": "s185t01BackendPool",<br>  "Prod": "s185p01BackendPool",<br>  "Test": "s185d02BackendPool"<br>}</pre> | no |
| <a name="input_cpd_client_id"></a> [cpd\_client\_id](#input\_cpd\_client\_id) | Client ID used by the application to access Key Vault in Azure | `string` | n/a | yes |
| <a name="input_cpd_client_secret"></a> [cpd\_client\_secret](#input\_cpd\_client\_secret) | Secret used by the application to access Key Vault in Azure | `string` | n/a | yes |
| <a name="input_cpd_keyvaultendpoint"></a> [cpd\_keyvaultendpoint](#input\_cpd\_keyvaultendpoint) | URL Endpoitn for Key Vault in Azure | `string` | n/a | yes |
| <a name="input_frontend_ip_configuration_name"></a> [frontend\_ip\_configuration\_name](#input\_frontend\_ip\_configuration\_name) | Name of Frontend IP Configuration | `map(string)` | <pre>{<br>  "Dev": "s185d01AGIPConfig",<br>  "Pre-Prod": "s185t01AGIPConfig",<br>  "Prod": "s185p01AGIPConfig",<br>  "Test": "s185d02AGIPConfig"<br>}</pre> | no |
| <a name="input_frontend_port_name"></a> [frontend\_port\_name](#input\_frontend\_port\_name) | Name of Frontend Port | `map(string)` | <pre>{<br>  "Dev": "s185d01FrontendPort",<br>  "Pre-Prod": "s185t01FrontendPort",<br>  "Prod": "s185p01FrontendPort",<br>  "Test": "s185d02FrontendPort"<br>}</pre> | no |
| <a name="input_gateway_ip_configuration"></a> [gateway\_ip\_configuration](#input\_gateway\_ip\_configuration) | Name of Gateway IP Configuration | `map(string)` | <pre>{<br>  "Dev": "s185d01-gateway-ip-configuration",<br>  "Pre-Prod": "s185t01-gateway-ip-configuration",<br>  "Prod": "s185p01-gateway-ip-configuration",<br>  "Test": "s185d02-gateway-ip-configuration"<br>}</pre> | no |
| <a name="input_http_setting_name"></a> [http\_setting\_name](#input\_http\_setting\_name) | Name of HTTP Setting | `map(string)` | <pre>{<br>  "Dev": "s185d01HTTPsetting",<br>  "Pre-Prod": "s185t01HTTPsetting",<br>  "Prod": "s185p01HTTPsetting",<br>  "Test": "s185d02HTTPsetting"<br>}</pre> | no |
| <a name="input_listener_name"></a> [listener\_name](#input\_listener\_name) | Name of HTTP Listener | `map(string)` | <pre>{<br>  "Dev": "s185d01Listener",<br>  "Pre-Prod": "s185t01Listener",<br>  "Prod": "s185p01Listener",<br>  "Test": "s185d02Listener"<br>}</pre> | no |
| <a name="input_network_nic_ip_conf_name"></a> [network\_nic\_ip\_conf\_name](#input\_network\_nic\_ip\_conf\_name) | Name of NIC IP Configuration | `map(string)` | <pre>{<br>  "Dev": "s185d01nic-ipconfig-1",<br>  "Pre-Prod": "s185t01nic-ipconfig-1",<br>  "Prod": "s185p01nic-ipconfig-1",<br>  "Test": "s185d02nic-ipconfig-1"<br>}</pre> | no |
| <a name="input_nic_name"></a> [nic\_name](#input\_nic\_name) | Name of Network Interface | `map(string)` | <pre>{<br>  "Dev": "s185d01nic-1",<br>  "Pre-Prod": "s185t01nic-1",<br>  "Prod": "s185p01nic-1",<br>  "Test": "s185d02nic-1"<br>}</pre> | no |
| <a name="input_nsg_name"></a> [nsg\_name](#input\_nsg\_name) | Name of Network Security Group | `map(string)` | <pre>{<br>  "Dev": "s185d01-chidrens-social-care-cpd-sn01-nsg",<br>  "Pre-Prod": "s185t01-chidrens-social-care-cpd-sn01-nsg",<br>  "Prod": "s185p01-chidrens-social-care-cpd-sn01-nsg",<br>  "Test": "s185d02-chidrens-social-care-cpd-sn01-nsg"<br>}</pre> | no |
| <a name="input_pip_name"></a> [pip\_name](#input\_pip\_name) | Name of Public IP address | `map(string)` | <pre>{<br>  "Dev": "s185d01AGPublicIPAddress",<br>  "Pre-Prod": "s185t01AGPublicIPAddress",<br>  "Prod": "s185p01AGPublicIPAddress",<br>  "Test": "s185d02AGPublicIPAddress"<br>}</pre> | no |
| <a name="input_private_link_ip_conf_name"></a> [private\_link\_ip\_conf\_name](#input\_private\_link\_ip\_conf\_name) | Name of Private Link IP Configuration | `map(string)` | <pre>{<br>  "Dev": "s185d01-csc-cpd-app-gateway-private-link-ip-conf",<br>  "Pre-Prod": "s185t01-csc-cpd-app-gateway-private-link-ip-conf",<br>  "Prod": "s185p01-csc-cpd-app-gateway-private-link-ip-conf",<br>  "Test": "s185d02-csc-cpd-app-gateway-private-link-ip-conf"<br>}</pre> | no |
| <a name="input_private_link_name"></a> [private\_link\_name](#input\_private\_link\_name) | Name of Private Link | `map(string)` | <pre>{<br>  "Dev": "s185d01-csc-cpd-app-gateway-private-link",<br>  "Pre-Prod": "s185t01-csc-cpd-app-gateway-private-link",<br>  "Prod": "s185p01-csc-cpd-app-gateway-private-link",<br>  "Test": "s185d02-csc-cpd-app-gateway-private-link"<br>}</pre> | no |
| <a name="input_request_routing_rule_name"></a> [request\_routing\_rule\_name](#input\_request\_routing\_rule\_name) | Name of Request Routing Rule | `map(string)` | <pre>{<br>  "Dev": "s185d01RoutingRule",<br>  "Pre-Prod": "s185t01RoutingRule",<br>  "Prod": "s185p01RoutingRule",<br>  "Test": "s185d02RoutingRule"<br>}</pre> | no |
| <a name="input_rg_name"></a> [rg\_name](#input\_rg\_name) | Name of Resource Group | `map(string)` | <pre>{<br>  "Dev": "s185d01-childrens-social-care-cpd-rg",<br>  "Pre-Prod": "s185t01-childrens-social-care-cpd-rg",<br>  "Prod": "s185p01-childrens-social-care-cpd-rg",<br>  "Test": "s185d02-childrens-social-care-cpd-rg"<br>}</pre> | no |
| <a name="input_service_plan_name"></a> [service\_plan\_name](#input\_service\_plan\_name) | Name of Service Plan | `map(string)` | <pre>{<br>  "Dev": "s185d01-csc-cpd-app-service-plan",<br>  "Pre-Prod": "s185t01-csc-cpd-app-service-plan",<br>  "Prod": "s185p01-csc-cpd-app-service-plan",<br>  "Test": "s185d02-csc-cpd-app-service-plan"<br>}</pre> | no |
| <a name="input_service_plan_sku"></a> [service\_plan\_sku](#input\_service\_plan\_sku) | SKU for Service Plan | `map(string)` | <pre>{<br>  "Dev": "B1",<br>  "Pre-Prod": "B1",<br>  "Prod": "P1v2",<br>  "Test": "B1"<br>}</pre> | no |
| <a name="input_tenant_id"></a> [tenant\_id](#input\_tenant\_id) | The Tenant ID of the subscription being used | `string` | n/a | yes |
| <a name="input_vnet_address_space"></a> [vnet\_address\_space](#input\_vnet\_address\_space) | Subnets used for Address Space | `map(string)` | <pre>{<br>  "Dev": "10.0.0.0/16",<br>  "Pre-Prod": "10.0.0.0/16",<br>  "Prod": "10.0.0.0/16",<br>  "Test": "10.1.0.0/16"<br>}</pre> | no |
| <a name="input_vnet_backend_name"></a> [vnet\_backend\_name](#input\_vnet\_backend\_name) | Name of Backend VNET | `map(string)` | <pre>{<br>  "Dev": "s185d01-chidrens-social-care-cpd-sn02",<br>  "Pre-Prod": "s185t01-chidrens-social-care-cpd-sn02",<br>  "Prod": "s185p01-chidrens-social-care-cpd-sn02",<br>  "Test": "s185d02-chidrens-social-care-cpd-sn02"<br>}</pre> | no |
| <a name="input_vnet_backend_prefixes"></a> [vnet\_backend\_prefixes](#input\_vnet\_backend\_prefixes) | Subnets used for Backend VNET | `map(string)` | <pre>{<br>  "Dev": "10.0.0.64/26",<br>  "Pre-Prod": "10.0.0.64/26",<br>  "Prod": "10.0.0.64/26",<br>  "Test": "10.1.0.64/26"<br>}</pre> | no |
| <a name="input_vnet_frontend_name"></a> [vnet\_frontend\_name](#input\_vnet\_frontend\_name) | Name of Frontend VNET | `map(string)` | <pre>{<br>  "Dev": "s185d01-chidrens-social-care-cpd-sn01",<br>  "Pre-Prod": "s185t01-chidrens-social-care-cpd-sn01",<br>  "Prod": "s185p01-chidrens-social-care-cpd-sn01",<br>  "Test": "s185d02-chidrens-social-care-cpd-sn01"<br>}</pre> | no |
| <a name="input_vnet_frontend_prefixes"></a> [vnet\_frontend\_prefixes](#input\_vnet\_frontend\_prefixes) | Subnets used for Frontend VENT | `map(string)` | <pre>{<br>  "Dev": "10.0.0.0/26",<br>  "Pre-Prod": "10.0.0.0/26",<br>  "Prod": "10.0.0.0/26",<br>  "Test": "10.1.0.0/26"<br>}</pre> | no |
| <a name="input_vnet_name"></a> [vnet\_name](#input\_vnet\_name) | Name of Address Space | `map(string)` | <pre>{<br>  "Dev": "s185d01-chidrens-social-care-cpd-vn01",<br>  "Pre-Prod": "s185t01-chidrens-social-care-cpd-vn01",<br>  "Prod": "s185p01-chidrens-social-care-cpd-vn01",<br>  "Test": "s185d02-chidrens-social-care-cpd-vn01"<br>}</pre> | no |
| <a name="input_web_app_name"></a> [web\_app\_name](#input\_web\_app\_name) | Name of Web Application | `map(string)` | <pre>{<br>  "Dev": "s185d01-chidrens-social-care-cpd-app-service",<br>  "Pre-Prod": "s185t01-chidrens-social-care-cpd-app-service",<br>  "Prod": "s185p01-chidrens-social-care-cpd-app-service",<br>  "Test": "s185d02-chidrens-social-care-cpd-app-service"<br>}</pre> | no |

## Outputs

| Name | Description |
|------|-------------|
| <a name="output_public_ip_address"></a> [public\_ip\_address](#output\_public\_ip\_address) | n/a |
