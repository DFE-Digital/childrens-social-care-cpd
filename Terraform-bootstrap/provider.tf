terraform {
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "3.21.1"
    }
  }
  required_version = ">= 1.0"
}

provider "azurerm" {
  features {}
}

data "azurerm_client_config" "current" {}
