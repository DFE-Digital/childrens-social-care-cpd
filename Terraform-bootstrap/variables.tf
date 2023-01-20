variable "rg_name" {
  type = map(string)
  default = {
    Dev       = "s185d01-childrens-social-care-cpd-rg"
    Test      = "s185d02-childrens-social-care-cpd-rg"
    Load-Test = "s185d03-childrens-social-care-cpd-rg"
    Pre-Prod  = "s185t01-childrens-social-care-cpd-rg"
    Prod      = "s185p01-childrens-social-care-cpd-rg"
  }
  description = "Name of Resource Group"
}

variable "env_name" {
  type = map(string)
  default = {
    Dev       = "Dev"
    Test      = "Test"
    Load-Test = "Dev"
    Pre-Prod  = "Pre-Pro"
    Prod      = "Prod"
  }
  description = "Name of Environment"
}

variable "pip_name" {
  type = map(string)
  default = {
    Dev       = "s185d01AGPublicIPAddress"
    Test      = "s185d02AGPublicIPAddress"
    Load-Test = "s185d03AGPublicIPAddress"
    Pre-Prod  = "s185t01AGPublicIPAddress"
    Prod      = "s185p01AGPublicIPAddress"
  }
  description = "Name of Public IP address"
}

variable "vnet_name" {
  type = map(string)
  default = {
    Dev       = "s185d01-chidrens-social-care-cpd-vn01"
    Test      = "s185d02-chidrens-social-care-cpd-vn01"
    Load-Test = "s185d03-chidrens-social-care-cpd-vn01"
    Pre-Prod  = "s185t01-chidrens-social-care-cpd-vn01"
    Prod      = "s185p01-chidrens-social-care-cpd-vn01"
  }
  description = "Name of Address Space"
}

variable "vnet_address_space" {
  type = map(string)
  default = {
    Dev       = "10.0.0.0/16"
    Test      = "10.1.0.0/16"
    Load-Test = "10.2.0.0/16"
    Pre-Prod  = "10.0.0.0/16"
    Prod      = "10.0.0.0/16"
  }
  description = "Subnets used for Address Space"
}

variable "appinsights_name" {
  type = map(string)
  default = {
    Dev       = "s185d01-chidrens-social-care-cpd-ai"
    Test      = "s185d02-chidrens-social-care-cpd-ai"
    Load-Test = "s185d03-chidrens-social-care-cpd-ai"
    Pre-Prod  = "s185t01-chidrens-social-care-cpd-ai"
    Prod      = "s185p01-chidrens-social-care-cpd-ai"
  }
  description = "Name of Application Insights"
}

variable "log_analytics_ws_name" {
  type = map(string)
  default = {
    Dev       = "s185d01-chidrens-social-care-cpd-la"
    Test      = "s185d02-chidrens-social-care-cpd-la"
    Load-Test = "s185d03-chidrens-social-care-cpd-la"
    Pre-Prod  = "s185t01-chidrens-social-care-cpd-la"
    Prod      = "s185p01-chidrens-social-care-cpd-la"
  }
  description = "Name of Log Analytics Workspace"
}

variable "key_vault_rg" {
  type = map(string)
  default = {
    Dev       = "s185d01-childrens-social-care-shared-rg"
    Test      = "s185d01-childrens-social-care-shared-rg"
    Load-Test = "s185d01-childrens-social-care-shared-rg"
    Pre-Prod  = "s185t01-childrens-social-care-shared-rg"
    Prod      = "s185p01-childrens-social-care-shared-rg"
  }
  description = "Name of Key Vault Resource Group"
}

variable "key_vault_name" {
  type = map(string)
  default = {
    Dev       = "s185d-CPD-Key-Vault"
    Test      = "s185d-CPD-Key-Vault"
    Load-Test = "s185d-CPD-Key-Vault"
    Pre-Prod  = "s185t-CPD-Key-Vault"
    Prod      = "s185p-CPD-Key-Vault"
  }
  description = "Name of Key Vault"
}

variable "dfe-ips" {
  type        = string
  sensitive   = true
  description = "List of DfE IP addresses for whitelisting"
}

variable "dfe-dev-ips" {
  type        = string
  sensitive   = true
  description = "List of developer IP addresses for whitelisting"
}
