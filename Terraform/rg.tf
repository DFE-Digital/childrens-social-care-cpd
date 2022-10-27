resource "azurerm_resource_group" "rg" {
  name     = "s185d01-childrens-social-care-rg"
  location = "westeurope"

  tags = {
    "Environment"  = "Dev",
    "Portfolio"    = "Newly Onboarded",
    "Service Line" = "Childrens Social Care Improvement and Learning",
    "Service"      = "Newly Onboarded"
  }
}
