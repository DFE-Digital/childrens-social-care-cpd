terraform {
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "3.29.1"
    }
  }

  backend "azurerm" {}
}

provider "azurerm" {
  features {}
  tenant_id = var.tenant_id
}

data "azurerm_client_config" "current" {}
