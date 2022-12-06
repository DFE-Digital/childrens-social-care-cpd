terraform {
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "3.21.1"
    }
  }

  backend "azurerm" {}
}

provider "azurerm" {
  features {}
  tenant_id                  = var.tenant_id
  skip_provider_registration = true
}

data "azurerm_client_config" "current" {}
