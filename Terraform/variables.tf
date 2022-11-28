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
    Dev      = "s185d01AGProbe"
    Test     = "s185d02AGProbe"
    Pre-Prod = "s185t01AGProbe"
    Prod     = "s185p01AGProbe"
  }
  description = "Name of App Gateway Health Probe"
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
    Dev      = "s185d01BackendPool"
    Test     = "s185d02BackendPool"
    Pre-Prod = "s185t01BackendPool"
    Prod     = "s185p01BackendPool"
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
    Dev      = "s185d01HTTPsetting"
    Test     = "s185d02HTTPsetting"
    Pre-Prod = "s185t01HTTPsetting"
    Prod     = "s185p01HTTPsetting"
  }
  description = "Name of HTTP Setting"
}

variable "listener_name" {
  type = map(string)
  default = {
    Dev      = "s185d01Listener"
    Test     = "s185d02Listener"
    Pre-Prod = "s185t01Listener"
    Prod     = "s185p01Listener"
  }
  description = "Name of HTTP Listener"
}

variable "request_routing_rule_name" {
  type = map(string)
  default = {
    Dev      = "s185d01RoutingRule"
    Test     = "s185d02RoutingRule"
    Pre-Prod = "s185t01RoutingRule"
    Prod     = "s185p01RoutingRule"
  }
  description = "Name of Request Routing Rule"
}

variable "appinsights_name" {
  type = map(string)
  default = {
    Dev      = "s185d01AppInsights"
    Test     = "s185d02AppInsights"
    Pre-Prod = "s185t01AppInsights"
    Prod     = "s185p01AppInsights"
  }
  description = "Name of Appplication Insights"
}

variable "log_analytics_ws_name" {
  type = map(string)
  default = {
    Dev      = "s185d01LogAnalyticsWS"
    Test     = "s185d02LogAnalyticsWS"
    Pre-Prod = "s185t01LogAnalyticsWS"
    Prod     = "s185p01LogAnalyticsWS"
  }
  description = "Name of Log Analytics Workspace"
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
  description = "URL Endpoitn for Key Vault in Azure"
}
