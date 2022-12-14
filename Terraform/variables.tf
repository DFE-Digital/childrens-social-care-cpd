variable "rg_name" {
  type = map(string)
  default = {
    Dev      = "s185d01-childrens-social-care-cpd-rg"
    Test     = "s185d02-childrens-social-care-cpd-rg"
    Pre-Prod = "s185t01-childrens-social-care-cpd-rg"
    Prod     = "s185p01-childrens-social-care-cpd-rg"
  }
  description = "Name of Resource Group"
}

variable "nsg_name" {
  type = map(string)
  default = {
    Dev      = "s185d01-chidrens-social-care-cpd-sn01-nsg"
    Test     = "s185d02-chidrens-social-care-cpd-sn01-nsg"
    Pre-Prod = "s185t01-chidrens-social-care-cpd-sn01-nsg"
    Prod     = "s185p01-chidrens-social-care-cpd-sn01-nsg"
  }
  description = "Name of Network Security Group"
}

variable "service_plan_name" {
  type = map(string)
  default = {
    Dev      = "s185d01-csc-cpd-app-service-plan"
    Test     = "s185d02-csc-cpd-app-service-plan"
    Pre-Prod = "s185t01-csc-cpd-app-service-plan"
    Prod     = "s185p01-csc-cpd-app-service-plan"
  }
  description = "Name of Service Plan"
}

variable "service_plan_sku" {
  type = map(string)
  default = {
    Dev      = "B1"
    Test     = "B1"
    Pre-Prod = "B1"
    Prod     = "P1v2"
  }
  description = "SKU for Service Plan"
}

variable "web_app_name" {
  type = map(string)
  default = {
    Dev      = "s185d01-chidrens-social-care-cpd-app-service"
    Test     = "s185d02-chidrens-social-care-cpd-app-service"
    Pre-Prod = "s185t01-chidrens-social-care-cpd-app-service"
    Prod     = "s185p01-chidrens-social-care-cpd-app-service"
  }
  description = "Name of Web Application"
}

variable "network_nic_ip_conf_name" {
  type = map(string)
  default = {
    Dev      = "s185d01nic-ipconfig-1"
    Test     = "s185d02nic-ipconfig-1"
    Pre-Prod = "s185t01nic-ipconfig-1"
    Prod     = "s185p01nic-ipconfig-1"
  }
  description = "Name of NIC IP Configuration"
}

variable "vnet_name" {
  type = map(string)
  default = {
    Dev      = "s185d01-chidrens-social-care-cpd-vn01"
    Test     = "s185d02-chidrens-social-care-cpd-vn01"
    Pre-Prod = "s185t01-chidrens-social-care-cpd-vn01"
    Prod     = "s185p01-chidrens-social-care-cpd-vn01"
  }
  description = "Name of Address Space"
}

variable "vnet_address_space" {
  type = map(string)
  default = {
    Dev      = "10.0.0.0/16"
    Test     = "10.1.0.0/16"
    Pre-Prod = "10.0.0.0/16"
    Prod     = "10.0.0.0/16"
  }
  description = "Subnets used for Address Space"
}

variable "vnet_frontend_name" {
  type = map(string)
  default = {
    Dev      = "s185d01-chidrens-social-care-cpd-sn01"
    Test     = "s185d02-chidrens-social-care-cpd-sn01"
    Pre-Prod = "s185t01-chidrens-social-care-cpd-sn01"
    Prod     = "s185p01-chidrens-social-care-cpd-sn01"
  }
  description = "Name of Frontend VNET"
}

variable "vnet_frontend_prefixes" {
  type = map(string)
  default = {
    Dev      = "10.0.0.0/26"
    Test     = "10.1.0.0/26"
    Pre-Prod = "10.0.0.0/26"
    Prod     = "10.0.0.0/26"
  }
  description = "Subnets used for Frontend VENT"
}

variable "vnet_backend_name" {
  type = map(string)
  default = {
    Dev      = "s185d01-chidrens-social-care-cpd-sn02"
    Test     = "s185d02-chidrens-social-care-cpd-sn02"
    Pre-Prod = "s185t01-chidrens-social-care-cpd-sn02"
    Prod     = "s185p01-chidrens-social-care-cpd-sn02"
  }
  description = "Name of Backend VNET"
}

variable "vnet_backend_prefixes" {
  type = map(string)
  default = {
    Dev      = "10.0.0.64/26"
    Test     = "10.1.0.64/26"
    Pre-Prod = "10.0.0.64/26"
    Prod     = "10.0.0.64/26"
  }
  description = "Subnets used for Backend VNET"
}

variable "pip_name" {
  type = map(string)
  default = {
    Dev      = "s185d01AGPublicIPAddress"
    Test     = "s185d02AGPublicIPAddress"
    Pre-Prod = "s185t01AGPublicIPAddress"
    Prod     = "s185p01AGPublicIPAddress"
  }
  description = "Name of Public IP address"
}

variable "nic_name" {
  type = map(string)
  default = {
    Dev      = "s185d01nic-1"
    Test     = "s185d02nic-1"
    Pre-Prod = "s185t01nic-1"
    Prod     = "s185p01nic-1"
  }
  description = "Name of Network Interface"
}

variable "appgw_name" {
  type = map(string)
  default = {
    Dev      = "s185d01-csc-cpd-app-gateway"
    Test     = "s185d02-csc-cpd-app-gateway"
    Pre-Prod = "s185t01-csc-cpd-app-gateway"
    Prod     = "s185p01-csc-cpd-app-gateway"
  }
  description = "Name of the Application Gateway"
}

variable "appgw_tier" {
  type = map(string)
  default = {
    Dev      = "Standard_v2"
    Test     = "Standard_v2"
    Pre-Prod = "Standard_v2"
    Prod     = "WAF_v2"
  }
  description = "SKU for Application Gateway Tier"
}

variable "appgw_probe" {
  type = map(string)
  default = {
    Dev      = "s185d01-chidrens-social-care-cpd-hp"
    Test     = "s185d02-chidrens-social-care-cpd-hp"
    Pre-Prod = "s185t01-chidrens-social-care-cpd-hp"
    Prod     = "s185p01-chidrens-social-care-cpd-hp"
  }
  description = "Name of App Gateway Health Probe"
}

variable "appgw_ssl_probe" {
  type = map(string)
  default = {
    Dev      = "s185d01-chidrens-social-care-cpd-ssl-hp"
    Test     = "s185d02-chidrens-social-care-cpd-ssl-hp"
    Pre-Prod = "s185t01-chidrens-social-care-cpd-ssl-hp"
    Prod     = "s185p01-chidrens-social-care-cpd-ssl-hp"
  }
  description = "Name of App Gateway SSL Health Probe"
}

variable "private_link_ip_conf_name" {
  type = map(string)
  default = {
    Dev      = "s185d01-csc-cpd-app-gateway-private-link-ip-conf"
    Test     = "s185d02-csc-cpd-app-gateway-private-link-ip-conf"
    Pre-Prod = "s185t01-csc-cpd-app-gateway-private-link-ip-conf"
    Prod     = "s185p01-csc-cpd-app-gateway-private-link-ip-conf"
  }
  description = "Name of Private Link IP Configuration"
}

variable "private_link_name" {
  type = map(string)
  default = {
    Dev      = "s185d01-csc-cpd-app-gateway-private-link"
    Test     = "s185d02-csc-cpd-app-gateway-private-link"
    Pre-Prod = "s185t01-csc-cpd-app-gateway-private-link"
    Prod     = "s185p01-csc-cpd-app-gateway-private-link"
  }
  description = "Name of Private Link"
}

variable "gateway_ip_configuration" {
  type = map(string)
  default = {
    Dev      = "s185d01-gateway-ip-configuration"
    Test     = "s185d02-gateway-ip-configuration"
    Pre-Prod = "s185t01-gateway-ip-configuration"
    Prod     = "s185p01-gateway-ip-configuration"
  }
  description = "Name of Gateway IP Configuration"
}

variable "backend_address_pool_name" {
  type = map(string)
  default = {
    Dev      = "s185d01-chidrens-social-care-cpd-bep"
    Test     = "s185d02-chidrens-social-care-cpd-bep"
    Pre-Prod = "s185t01-chidrens-social-care-cpd-bep"
    Prod     = "s185p01-chidrens-social-care-cpd-bep"
  }
  description = "Name of Backend Address Pool"
}

variable "frontend_port_name" {
  type = map(string)
  default = {
    Dev      = "s185d01FrontendPort"
    Test     = "s185d02FrontendPort"
    Pre-Prod = "s185t01FrontendPort"
    Prod     = "s185p01FrontendPort"
  }
  description = "Name of Frontend Port"
}

variable "frontend_ssl_port_name" {
  type = map(string)
  default = {
    Dev      = "s185d01FrontendSSLPort"
    Test     = "s185d02FrontendSSLPort"
    Pre-Prod = "s185t01FrontendSSLPort"
    Prod     = "s185p01FrontendSSLPort"
  }
  description = "Name of Frontend SSL Port"
}

variable "frontend_ip_configuration_name" {
  type = map(string)
  default = {
    Dev      = "s185d01AGIPConfig"
    Test     = "s185d02AGIPConfig"
    Pre-Prod = "s185t01AGIPConfig"
    Prod     = "s185p01AGIPConfig"
  }
  description = "Name of Frontend IP Configuration"
}

variable "http_setting_name" {
  type = map(string)
  default = {
    Dev      = "s185d01-chidrens-social-care-cpd-bes-http"
    Test     = "s185d02-chidrens-social-care-cpd-bes-http"
    Pre-Prod = "s185t01-chidrens-social-care-cpd-bes-http"
    Prod     = "s185p01-chidrens-social-care-cpd-bes-http"
  }
  description = "Name of HTTP Setting"
}

variable "https_setting_name" {
  type = map(string)
  default = {
    Dev      = "s185d01-chidrens-social-care-cpd-bes-https"
    Test     = "s185d02-chidrens-social-care-cpd-bes-https"
    Pre-Prod = "s185t01-chidrens-social-care-cpd-bes-https"
    Prod     = "s185p01-chidrens-social-care-cpd-bes-https"
  }
  description = "Name of HTTPS Setting"
}

variable "listener_name" {
  type = map(string)
  default = {
    Dev      = "s185d01-chidrens-social-care-cpd-listener-http"
    Test     = "s185d02-chidrens-social-care-cpd-listener-http"
    Pre-Prod = "s185t01-chidrens-social-care-cpd-listener-http"
    Prod     = "s185p01-chidrens-social-care-cpd-listener-http"
  }
  description = "Name of HTTP Listener"
}

variable "redirect_config_name" {
  type = map(string)
  default = {
    Dev      = "s185d01Redirect"
    Test     = "s185d02Redirect"
    Pre-Prod = "s185t01Redirect"
    Prod     = "s185p01Redirect"
  }
  description = "Name of the redirect configuration"
}

variable "ssl_listener_name" {
  type = map(string)
  default = {
    Dev      = "s185d01-chidrens-social-care-cpd-listener-https"
    Test     = "s185d02-chidrens-social-care-cpd-listener-https"
    Pre-Prod = "s185t01-chidrens-social-care-cpd-listener-https"
    Prod     = "s185p01-chidrens-social-care-cpd-listener-https"
  }
  description = "Name of SSL HTTPS Listener"
}

variable "request_routing_rule_name" {
  type = map(string)
  default = {
    Dev      = "s185d01-chidrens-social-care-cpd-rule-http"
    Test     = "s185d02-chidrens-social-care-cpd-rule-http"
    Pre-Prod = "s185t01-chidrens-social-care-cpd-rule-http"
    Prod     = "s185p01-chidrens-social-care-cpd-rule-http"
  }
  description = "Name of Request Routing Rule"
}

variable "request_ssl_routing_rule_name" {
  type = map(string)
  default = {
    Dev      = "s185d01-chidrens-social-care-cpd-rule-https"
    Test     = "s185d02-chidrens-social-care-cpd-rule-https"
    Pre-Prod = "s185t01-chidrens-social-care-cpd-rule-https"
    Prod     = "s185p01-chidrens-social-care-cpd-rule-https"
  }
  description = "Name of Request SSL Routing Rule"
}

variable "appinsights_name" {
  type = map(string)
  default = {
    Dev      = "s185d01-chidrens-social-care-cpd-ai"
    Test     = "s185d02-chidrens-social-care-cpd-ai"
    Pre-Prod = "s185t01-chidrens-social-care-cpd-ai"
    Prod     = "s185p01-chidrens-social-care-cpd-ai"
  }
  description = "Name of Appplication Insights"
}

variable "log_analytics_ws_name" {
  type = map(string)
  default = {
    Dev      = "s185d01-chidrens-social-care-cpd-la"
    Test     = "s185d02-chidrens-social-care-cpd-la"
    Pre-Prod = "s185t01-chidrens-social-care-cpd-la"
    Prod     = "s185p01-chidrens-social-care-cpd-la"
  }
  description = "Name of Log Analytics Workspace"
}

variable "key_vault_rg" {
  type = map(string)
  default = {
    Dev      = "s185d01-childrens-social-care-shared-rg"
    Test     = "s185d01-childrens-social-care-shared-rg"
    Pre-Prod = "s185t01-childrens-social-care-shared-rg"
    Prod     = "s185p01-childrens-social-care-shared-rg"
  }
  description = "Name of Key Vault Resource Group"
}

variable "key_vault_name" {
  type = map(string)
  default = {
    Dev      = "cpd-key-vault"
    Test     = "cpd-key-vault"
    Pre-Prod = "s185t-CPD-Key-Vault"
    Prod     = "s185p-CPD-Key-Vault"
  }
  description = "Name of Key Vault"
}

variable "key_vault_url" {
  type = map(string)
  default = {
    Dev      = "https://cpd-key-vault.vault.azure.net/"
    Test     = "https://cpd-key-vault.vault.azure.net/"
    Pre-Prod = "https://s185t-cpd-key-vault.vault.azure.net/"
    Prod     = "https://s185p-cpd-key-vault.vault.azure.net/"
  }
  description = "URL of Key Vault"
}

variable "autoscale_name" {
  type = map(string)
  default = {
    Dev      = "s185d01-app-autoscale"
    Test     = "s185d02-app-autoscale"
    Pre-Prod = "s185t01-app-autoscale"
    Prod     = "s185p01-app-autoscale"
  }
  description = "Name of Key Vault"
}


variable "autoscale_min" {
  type = map(string)
  default = {
    Dev      = 1
    Test     = 1
    Pre-Prod = 1
    Prod     = 3
  }
  description = "Minimum Autoscale Value"
}

variable "autoscale_max" {
  type = map(string)
  default = {
    Dev      = 2
    Test     = 2
    Pre-Prod = 2
    Prod     = 10
  }
  description = "Maximum Autoscale Value"
}

variable "private_dns_zone_vn_link_name" {
  type = map(string)
  default = {
    Dev      = "s185d01-dnszonelink"
    Test     = "s185d02-dnszonelink"
    Pre-Prod = "s185t01-dnszonelink"
    Prod     = "s185p01-dnszonelink"
  }
  description = "Name of Private DNS Zone Virtual Network Link"
}

variable "private_endpoint_name" {
  type = map(string)
  default = {
    Dev      = "s185d01-backwebappprivateendpoint"
    Test     = "s185d02-backwebappprivateendpoint"
    Pre-Prod = "s185t01-backwebappprivateendpoint"
    Prod     = "s185p01-backwebappprivateendpoint"
  }
  description = "Name of Private Endpoint"
}

variable "private_dns_zone_group_name" {
  type = map(string)
  default = {
    Dev      = "s185d01-privatednszonegroup"
    Test     = "s185d02-privatednszonegroup"
    Pre-Prod = "s185t01-privatednszonegroup"
    Prod     = "s185p01-privatednszonegroup"
  }
  description = "Name of Private DNS Zone Group"
}

variable "private_endpoint_conn_name" {
  type = map(string)
  default = {
    Dev      = "s185d01-privateendpointconnection"
    Test     = "s185d02-privateendpointconnection"
    Pre-Prod = "s185t01-privateendpointconnection"
    Prod     = "s185p01-privateendpointconnection"
  }
  description = "Name of Private Endpoint Connection"
}

variable "tenant_id" {
  type        = string
  sensitive   = true
  description = "The Tenant ID of the subscription being used"
}

variable "cpd_client_id" {
  type        = string
  sensitive   = true
  description = "Client ID used by the application to access Key Vault in Azure"
}

variable "cpd_client_secret" {
  type        = string
  sensitive   = true
  description = "Secret used by the application to access Key Vault in Azure"
}

variable "cpd_keyvaultendpoint" {
  type        = string
  sensitive   = true
  description = "URL Endpoint for Key Vault in Azure"
}
