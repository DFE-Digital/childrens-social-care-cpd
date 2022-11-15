variable "rg_name" {
  type = map(string)
  default = {
    Dev      = "s185d01-childrens-social-care-rg"
    Test     = "s185d01-childrens-social-care-rg"
    Pre-Prod = "s185d01-childrens-social-care-rg"
    Prod     = "s185d01-childrens-social-care-rg"
  }
}

variable "nsg_name" {
  type = map(string)
  default = {
    Dev      = "s185d01-chidrens-social-care-cpd-sn01-nsg"
    Test     = "s185d01-chidrens-social-care-cpd-sn01-nsg"
    Pre-Prod = "s185d01-chidrens-social-care-cpd-sn01-nsg"
    Prod     = "s185d01-chidrens-social-care-cpd-sn01-nsg"
  }
}

variable "service_plan_name" {
  type = map(string)
  default = {
    Dev      = "s185d01-csc-cpd-app-service-plan"
    Test     = "s185d01-csc-cpd-app-service-plan"
    Pre-Prod = "s185d01-csc-cpd-app-service-plan"
    Prod     = "s185d01-csc-cpd-app-service-plan"
  }
}

variable "service_plan_sku" {
  type = map(string)
  default = {
    Dev      = "B1"
    Test     = "B1"
    Pre-Prod = "B1"
    Prod     = "B1"
  }
}

variable "web_app_name" {
  type = map(string)
  default = {
    Dev      = "s185d01-chidrens-social-care-cpd-app-service"
    Test     = "s185d01-chidrens-social-care-cpd-app-service"
    Pre-Prod = "s185d01-chidrens-social-care-cpd-app-service"
    Prod     = "s185d01-chidrens-social-care-cpd-app-service"
  }
}

variable "network_nic_ip_conf_name" {
  type = map(string)
  default = {
    Dev      = "s185d01nic-ipconfig-1"
    Test     = "s185d01nic-ipconfig-1"
    Pre-Prod = "s185d01nic-ipconfig-1"
    Prod     = "s185d01nic-ipconfig-1"
  }
}

variable "vnet_name" {
  type = map(string)
  default = {
    Dev      = "s185d01-chidrens-social-care-cpd-vn01"
    Test     = "s185d01-chidrens-social-care-cpd-vn01"
    Pre-Prod = "s185d01-chidrens-social-care-cpd-vn01"
    Prod     = "s185d01-chidrens-social-care-cpd-vn01"
  }
}

variable "vnet_address_space" {
  type = map(string)
  default = {
    Dev      = "10.0.0.0/16"
    Test     = "10.0.0.0/16"
    Pre-Prod = "10.0.0.0/16"
    Prod     = "10.0.0.0/16"
  }
}

variable "vnet_frontend_name" {
  type = map(string)
  default = {
    Dev      = "s185d01-chidrens-social-care-cpd-sn01"
    Test     = "s185d01-chidrens-social-care-cpd-sn01"
    Pre-Prod = "s185d01-chidrens-social-care-cpd-sn01"
    Prod     = "s185d01-chidrens-social-care-cpd-sn01"
  }
}

variable "vnet_frontend_prefixes" {
  type = map(string)
  default = {
    Dev      = "10.0.0.0/26"
    Test     = "10.0.0.0/26"
    Pre-Prod = "10.0.0.0/26"
    Prod     = "10.0.0.0/26"
  }
}

variable "vnet_backend_name" {
  type = map(string)
  default = {
    Dev      = "s185d01-chidrens-social-care-cpd-sn02"
    Test     = "s185d01-chidrens-social-care-cpd-sn02"
    Pre-Prod = "s185d01-chidrens-social-care-cpd-sn02"
    Prod     = "s185d01-chidrens-social-care-cpd-sn02"
  }
}

variable "vnet_backend_prefixes" {
  type = map(string)
  default = {
    Dev      = "10.0.0.128/26"
    Test     = "10.0.0.128/26"
    Pre-Prod = "10.0.0.128/26"
    Prod     = "10.0.0.128/26"
  }
}

variable "pip_name" {
  type = map(string)
  default = {
    Dev      = "s185d01AGPublicIPAddress"
    Test     = "s185d01AGPublicIPAddress"
    Pre-Prod = "s185d01AGPublicIPAddress"
    Prod     = "s185d01AGPublicIPAddress"
  }
}

variable "nic_name" {
  type = map(string)
  default = {
    Dev      = "s185d01nic-1"
    Test     = "s185d01nic-1"
    Pre-Prod = "s185d01nic-1"
    Prod     = "s185d01nic-1"
  }
}

variable "appgw_name" {
  type = map(string)
  default = {
    Dev      = "s185d01-csc-cpd-app-gateway"
    Test     = "s185d01-csc-cpd-app-gateway"
    Pre-Prod = "s185d01-csc-cpd-app-gateway"
    Prod     = "s185d01-csc-cpd-app-gateway"
  }
}

variable "appgw_tier" {
  type = map(string)
  default = {
    Dev      = "Standard_v2"
    Test     = "Standard_v2"
    Pre-Prod = "Standard_v2"
    Prod     = "Standard_v2"
  }
}

variable "appgw_probe" {
  type = map(string)
  default = {
    Dev      = "s185d01AGProbe"
    Test     = "s185d01AGProbe"
    Pre-Prod = "s185d01AGProbe"
    Prod     = "s185d01AGProbe"
  }
}

variable "private_link_ip_conf_name" {
  type = map(string)
  default = {
    Dev      = "s185d01-csc-cpd-app-gateway-private-link-ip-conf"
    Test     = "s185d01-csc-cpd-app-gateway-private-link-ip-conf"
    Pre-Prod = "s185d01-csc-cpd-app-gateway-private-link-ip-conf"
    Prod     = "s185d01-csc-cpd-app-gateway-private-link-ip-conf"
  }
}

variable "private_link_name" {
  type = map(string)
  default = {
    Dev      = "s185d01-csc-cpd-app-gateway-private-link"
    Test     = "s185d01-csc-cpd-app-gateway-private-link"
    Pre-Prod = "s185d01-csc-cpd-app-gateway-private-link"
    Prod     = "s185d01-csc-cpd-app-gateway-private-link"
  }
}

variable "gateway_ip_configuration" {
  type = map(string)
  default = {
    Dev      = "s185d01-gateway-ip-configuration"
    Test     = "s185d01-gateway-ip-configuration"
    Pre-Prod = "s185d01-gateway-ip-configuration"
    Prod     = "s185d01-gateway-ip-configuration"
  }
}


variable "backend_address_pool_name" {
  type = map(string)
  default = {
    Dev      = "s185d01BackendPool"
    Test     = "s185d01BackendPool"
    Pre-Prod = "s185d01BackendPool"
    Prod     = "s185d01BackendPool"
  }
}

variable "frontend_port_name" {
  type = map(string)
  default = {
    Dev      = "s185d01FrontendPort"
    Test     = "s185d01FrontendPort"
    Pre-Prod = "s185d01FrontendPort"
    Prod     = "s185d01FrontendPort"
  }
}

variable "frontend_ip_configuration_name" {
  type = map(string)
  default = {
    Dev      = "s185d01AGIPConfig"
    Test     = "s185d01AGIPConfig"
    Pre-Prod = "s185d01AGIPConfig"
    Prod     = "s185d01AGIPConfig"
  }
}

variable "http_setting_name" {
  type = map(string)
  default = {
    Dev      = "s185d01HTTPsetting"
    Test     = "s185d01HTTPsetting"
    Pre-Prod = "s185d01HTTPsetting"
    Prod     = "s185d01HTTPsetting"
  }
}

variable "listener_name" {
  type = map(string)
  default = {
    Dev      = "s185d01Listener"
    Test     = "s185d01Listener"
    Pre-Prod = "s185d01Listener"
    Prod     = "s185d01Listener"
  }
}

variable "request_routing_rule_name" {
  type = map(string)
  default = {
    Dev      = "s185d01RoutingRule"
    Test     = "s185d01RoutingRule"
    Pre-Prod = "s185d01RoutingRule"
    Prod     = "s185d01RoutingRule"
  }
}

variable "redirect_configuration_name" {
  type = map(string)
  default = {
    Dev      = "s185d01RedirectConfig"
    Test     = "s185d01RedirectConfig"
    Pre-Prod = "s185d01RedirectConfig"
    Prod     = "s185d01RedirectConfig"
  }
}

variable "tenant_id" {
  type      = string
  sensitive = true
}

variable "cpd_client_id" {
  type      = string
  sensitive = true
}

variable "cpd_client_secret" {
  type      = string
  sensitive = true
}

variable "cpd_keyvaultendpoint" {
  type      = string
  sensitive = true
}

variable "acr_password" {
  type      = string
  sensitive = true
}
