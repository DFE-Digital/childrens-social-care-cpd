resource "azurerm_resource_group" "rg" {
  name     = var.rg_name[terraform.workspace]
  location = "westeurope"
  tags = {
    "Environment"      = var.env_name[terraform.workspace]
    "Parent Business"  = "Children’s Care"
    "Service Offering" = "Social Workforce"
    "Portfolio"        = "Vulnerable Children and Families"
    "Service Line"     = "Children and Social care"
    "Service"          = "Children and Social care"
    "Product"          = "Social Workforce"
  }
}