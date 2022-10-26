resource "azurerm_resource_group" "rg" {
  name     = "s185d01-childrens-social-care-rg"
  location = "westeurope"

  tags = {
    "Environment"  = "Dev",
    "Portfolio"    = "Children’s and families",
    "Service Line" = "Childrens Social Care Improvement and Learning",
    "Service"      = "Social Worker Career Progression"
  }
}
