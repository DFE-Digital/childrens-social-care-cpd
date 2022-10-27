resource "azurerm_resource_group" "rg" {
  name     = "s185d01-childrens-social-care-rg"
  location = "westeurope"

  tags = {
    "Environment"  = "Dev",
    "Portfolio"    = "Newly Onboarded",
    "Service Line" = "Newly Onboarded",
    "Service"      = "Newly Onboarded"
  }
}
