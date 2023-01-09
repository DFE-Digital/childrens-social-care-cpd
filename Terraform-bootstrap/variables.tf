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
