# Terraform provider using Azure - also for the backend
terraform {
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "3.90.0"
    }
  }
  required_version = ">= 1.0"
  backend "azurerm" {}
}

provider "azurerm" {
  features {}
  tenant_id = var.tenant_id
}
