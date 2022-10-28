resource "azurerm_resource_group" "rg" {
  name     = "s185d01-childrens-social-care-rg"
  location = "northeurope"

  tags = {
    "Environment"      = "Dev",
    "Parent Business"  = "Children’s Care",
    "Service Offering" = "Social Workforce",
    "Portfolio"        = "Vulnerable Children and Families",
    "Service Line"     = "Children and Social care",
    "Service"          = "Children and Social care",
    "Product"          = "Social Workforce"
  }
}
