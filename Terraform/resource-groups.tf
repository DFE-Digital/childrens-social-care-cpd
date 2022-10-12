resource "azurerm_resource_group" "rg" {
  name     = "s185d01-devcontentful-rg"
  location = "westeurope"
  tags = {
    "Environment"  = "Dev",
    "Portfolio"    = "Digital and Technology",
    "Service Line" = "Childrens Social Care Improvement and Learning",
    "Service"      = "Azure Platform"
  }
}
