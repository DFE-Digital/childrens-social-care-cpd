terraform {
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "3.21.1"
    }
  }

  backend "azurerm" {
    resource_group_name  = "s185d01-tfstate-rg"
    storage_account_name = "s185d01tfstate"
    container_name       = "tfstate"
    key                  = "terraform.tfstate"
  }
}

provider "azurerm" {
  features {}
  tenant_id = var.tenant_id
}