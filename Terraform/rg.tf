# A reference to the Azure Resource Group for this environment
data "azurerm_resource_group" "rg" {
  name = var.rg_name[terraform.workspace]
  #name = "s185d01-childrens-social-care-cpd-rg"
  # location = "westeurope"

  # tags = {
  #   "Environment"      = terraform.workspace
  #   "Parent Business"  = "Children’s Care"
  #   "Service Offering" = "Social Workforce"
  #   "Portfolio"        = "Newly Onboarded"
  #   "Service Line"     = "Newly Onboarded"
  #   "Service"          = "Newly Onboarded"
  #   # "Portfolio"        = "Vulnerable Children and Families"
  #   # "Service Line"     = "Children and Social care"
  #   # "Service"          = "Children and Social care"
  #   "Product" = "Social Workforce"
  # }
}